namespace Spineless
{
    using UnityEngine;


    public class HiddenAttribute : PropertyAttribute
    {
    }
}


#region Property Drawer
#if UNITY_EDITOR
namespace Spineless
{
    using UnityEditor;
    using UnityEngine;


    [CustomPropertyDrawer(typeof(HiddenAttribute))]
    public class HiddenAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0f;
        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
        }
    }
}
#endif
#endregion