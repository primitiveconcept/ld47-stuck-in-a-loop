# if UNITY_EDITOR

namespace Spineless
{
    using UnityEditor;
    using UnityEditorInternal;


    public static class TagsGenerator
    {
        [MenuItem("Spineless/Create or Update Tags Class")]
        public static void CreateTagsClass()
        {
            string[] tags = InternalEditorUtility.tags;
            
            CodeGenerator codeGenerator = new CodeGenerator("Tags");
            codeGenerator.ClassNamespace = "";
            codeGenerator.IsStatic = true;
            codeGenerator.OutputDirectory = "";
            codeGenerator.ClassDescription = "List of all Unity Tags in the project.";

            for (int i = 0; i < tags.Length; i++)
            {
                string tag = tags[i];
                codeGenerator.AddConstant(
                    tag.Replace(" ", ""),
                    tag);
            }

            codeGenerator.WriteCodeToFile();
        }
    }
}

#endif