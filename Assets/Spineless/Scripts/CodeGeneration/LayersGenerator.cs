#if UNITY_EDITOR

namespace Spineless
{
    using System;
    using System.Reflection;
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine;


    public static class LayersGenerator
    {
        [MenuItem("Spineless/Create or Update Layer Classes")]
        public static void CreateLayersClass()
        {
            string[] layers = InternalEditorUtility.layers;
            string[] sortingLayers = GetSortingLayerNames();

            CodeGenerator layersCodeGenerator = new CodeGenerator("Layers");
            CodeGenerator layerMasksCodeGenerator = new CodeGenerator("LayerMasks");
            layersCodeGenerator.ClassNamespace = "";
            layerMasksCodeGenerator.ClassNamespace = "";
            layersCodeGenerator.IsStatic = true;
            layerMasksCodeGenerator.IsStatic = true;
            layersCodeGenerator.OutputDirectory = "";
            layerMasksCodeGenerator.OutputDirectory = "";
            layersCodeGenerator.ClassDescription = "List of all Unity Layers (as strings) in the project.";
            layerMasksCodeGenerator.ClassDescription = "List of all Unity Layers (as masks) in the project.";

            for (int i = 0; i < layers.Length; i++)
            {
                string layer = layers[i];
                layersCodeGenerator.AddConstant(
                    layer.Replace(" ", ""),
                    layer);
                layerMasksCodeGenerator.AddConstant(
                    layer.Replace(" ", ""),
                    LayerMask.GetMask(layer));
            }

            layersCodeGenerator.WriteCodeToFile();
            layerMasksCodeGenerator.WriteCodeToFile();

            if (sortingLayers == null)
                return;
            
            CodeGenerator sortingLayersCodeGenerator = new CodeGenerator("SortingLayers");
            sortingLayersCodeGenerator.ClassNamespace = "";
            sortingLayersCodeGenerator.IsStatic = true;
            sortingLayersCodeGenerator.OutputDirectory = "";
            sortingLayersCodeGenerator.ClassDescription =
                "List of all Unity Sorting Layers (as strings) in the project.";
            for (int i = 0; i < sortingLayers.Length; i++)
            {
                string sortingLayer = sortingLayers[i];
                sortingLayersCodeGenerator.AddConstant(
                    sortingLayer.Replace(" ", ""),
                    sortingLayer);
            }

            sortingLayersCodeGenerator.WriteCodeToFile();
        }


        private static string[] GetSortingLayerNames()
        {
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty(
                "sortingLayerNames",
                BindingFlags.Static | BindingFlags.NonPublic);

            if (sortingLayersProperty != null)
                return (string[])sortingLayersProperty.GetValue(null, new object[0]);

            return null;
        }
    }
}

#endif