namespace Spineless
{
    using UnityEngine;


    public class AssetPathAttribute : PropertyAttribute
    {
        public readonly string TargetField;


        #region Constructors
        public AssetPathAttribute(string targetField)
        {
            this.TargetField = targetField;
        }
        #endregion
    }
}


#region Property Drawer
#if UNITY_EDITOR

namespace Spineless
{
    using UnityEditor;
    using UnityEngine;


    [CustomPropertyDrawer(typeof(AssetPathAttribute))]
    public class AssetPathDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.ObjectField(position, property);
            //base.OnGUI(position, property, label);

            if (property != null
                && property.objectReferenceValue != null)
            {
                var assetPathAttribute = this.attribute as AssetPathAttribute;
                string path = AssetDatabase.GetAssetPath(property.objectReferenceValue);
                property.serializedObject.FindProperty(assetPathAttribute.TargetField).stringValue = path;
            }
        }
    }
}

#endif
#endregion