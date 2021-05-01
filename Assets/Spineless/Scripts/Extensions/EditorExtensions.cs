#if UNITY_EDITOR

namespace Spineless.Extensions.Editor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;


    public static class EditorExtensions
    {
        public static T FromSerializedProperty<T>(
            this FieldInfo fieldInfo,
            SerializedProperty property)
            where T : class
        {
            object targetObject = fieldInfo.GetValue(property.serializedObject.targetObject);
            if (targetObject == null)
                return null;

            T actualObject = null;
            if (targetObject.GetType().IsArray)
            {
                int index = Convert.ToInt32(new string(property.propertyPath.Where(c => char.IsDigit(c)).ToArray()));
                actualObject = ((T[])targetObject)[index];
            }
            else
            {
                actualObject = targetObject as T;
            }

            return actualObject;
        }


        public static string GenerateAssetPath(string folderPath, string fileName = null)
        {
            // Format path to use proper directory separators.
            if (folderPath.StartsWith("Assets"))
                folderPath = folderPath.Substring(6);

            folderPath = (Application.dataPath + folderPath)
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar)
                .TrimEnd(Path.DirectorySeparatorChar);

            if (string.IsNullOrEmpty(fileName))
                return folderPath;

            string filePath = string.Concat(
                folderPath,
                Path.DirectorySeparatorChar,
                fileName);

            return filePath;
        }


        public static void InvokeEditorFunction(string assemblyName, string className, string methodName)
        {
            Assembly editorAssembly =
                AppDomain.CurrentDomain.GetAssemblies().First(a => a.FullName.StartsWith(assemblyName));
            Type utilityType = editorAssembly.GetTypes().FirstOrDefault(t => t.FullName.Contains(className));
            utilityType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static)
                .Invoke(obj: null, parameters: null);
        }
    }
}

#endif