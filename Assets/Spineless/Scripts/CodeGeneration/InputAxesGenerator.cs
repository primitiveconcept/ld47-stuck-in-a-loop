#if UNITY_EDITOR

namespace Spineless
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;


    public static class InputAxesGenerator
    {
        [MenuItem("Spineless/Create or Update InputAxes Class")]
        public static void CreateInputAxesClass()
        {
            string[] axes = GetAxesNames();
            if (axes == null)
            {
                Debug.LogWarning("No input axes defined in InputManager.");
                return;
            }
                

            CodeGenerator codeGenerator = new CodeGenerator("InputAxes");
            codeGenerator.ClassNamespace = "";
            codeGenerator.IsStatic = true;
            codeGenerator.OutputDirectory = "";
            codeGenerator.ClassDescription = "List of all Unity Input Axes (from the InputManager) in the project.";

            for (int i = 0; i < axes.Length; i++)
            {
                string axis = axes[i];
                codeGenerator.AddConstant(
                    axis.Replace(" ", ""),
                    axis);
            }

            codeGenerator.WriteCodeToFile();
        }


        private static string[] GetAxesNames()
        {
            Object inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
            SerializedProperty axisArray = new SerializedObject(inputManager).FindProperty("m_Axes");

            if (axisArray.arraySize == 0)
                return null;

            List<string> axes = new List<string>();

            for (int i = 0; i < axisArray.arraySize; ++i)
            {
                SerializedProperty axis = axisArray.GetArrayElementAtIndex(i);

                string name = axis.FindPropertyRelative("m_Name").stringValue;
                if (!axes.Contains(name))
                    axes.Add(name);
            }

            return axes.ToArray();
        }
    }
}

#endif