namespace Spineless
{
    using System;
    using UnityEngine;


    [AttributeUsage(
        AttributeTargets.Field
        | AttributeTargets.Property
        | AttributeTargets.Class
        | AttributeTargets.Struct,
        Inherited = true)]
    public class ConditionalAttribute : PropertyAttribute
    {
        public string ConditionalSourceField = "";
        public bool HideInInspectorIfTrue = false;
        public object[] SpecificValue = null;


        #region Constructors
        public ConditionalAttribute(string conditionalSourceField)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspectorIfTrue = false;
            this.SpecificValue = null;
        }


        public ConditionalAttribute(
            string conditionalSourceField,
            bool hideInInspectorIfTrue)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspectorIfTrue = hideInInspectorIfTrue;
            this.SpecificValue = null;
        }


        public ConditionalAttribute(
            string conditionalSourceField,
            params object[] specificValue)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspectorIfTrue = false;
            this.SpecificValue = specificValue;
        }


        public ConditionalAttribute(
            string conditionalSourceField,
            bool hideInInspectorIfTrue,
            params object[] specificValue)
        {
            this.ConditionalSourceField = conditionalSourceField;
            this.HideInInspectorIfTrue = hideInInspectorIfTrue;
            this.SpecificValue = specificValue;
        }
        #endregion
    }
}


#region Property Drawer
#if UNITY_EDITOR

namespace Spineless
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;


    [CustomPropertyDrawer(typeof(ConditionalAttribute))]
    public class ConditionalDrawer : PropertyDrawer
    {
        public object GetParent(SerializedProperty prop)
        {
            string path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            string[] elements = path.Split('.');
            foreach (string element in elements.Take(elements.Length - 1))
            {
                if (element.Contains("["))
                {
                    string elementName = element.Substring(0, element.IndexOf("["));
                    int index = Convert.ToInt32(
                        element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue(obj, elementName, index);
                }
                else
                {
                    obj = GetValue(obj, element);
                }
            }

            return obj;
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalAttribute condHAtt = (ConditionalAttribute)this.attribute;
            bool showProperty = ShouldShowProperty(condHAtt, property);

            if (showProperty)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            else
            {
                return -EditorGUIUtility.standardVerticalSpacing;
            }
        }


        public object GetValue(object source, string name)
        {
            if (source == null)
                return null;
            Type type = source.GetType();
            FieldInfo f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (f == null)
            {
                PropertyInfo p = type.GetProperty(
                    name,
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p == null)
                    return null;
                return p.GetValue(source, null);
            }

            return f.GetValue(source);
        }


        public object GetValue(object source, string name, int index)
        {
            IEnumerable enumerable = GetValue(source, name) as IEnumerable;
            IEnumerator enm = enumerable.GetEnumerator();
            while (index-- >= 0)
            {
                enm.MoveNext();
            }

            return enm.Current;
        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Debug.Log("Show: ");

            ConditionalAttribute condHAtt = (ConditionalAttribute)this.attribute;
            bool showProperty = ShouldShowProperty(condHAtt, property);

            bool wasEnabled = GUI.enabled;
            GUI.enabled = showProperty;

            if (showProperty)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            GUI.enabled = wasEnabled;
        }


        #region Helper Methods
        private bool ShouldShowProperty(ConditionalAttribute attribute, SerializedProperty property)
        {
            bool isTrue = true;

            SerializedProperty sourcePropertyValue =
                property.serializedObject.FindProperty(attribute.ConditionalSourceField);

            if (sourcePropertyValue != null)
            {
                switch (sourcePropertyValue.propertyType)
                {
                    case SerializedPropertyType.Boolean:
                        isTrue = sourcePropertyValue.boolValue;
                        break;
                    case SerializedPropertyType.ObjectReference:

                        isTrue = sourcePropertyValue.objectReferenceValue != null;
                        break;
                    case SerializedPropertyType.Enum:
                        if (attribute.SpecificValue != null)
                        {
                            isTrue = false;
                            foreach (object value in attribute.SpecificValue)
                            {
                                int enumValue = (int)Convert.ChangeType(value, TypeCode.Int32);
                                isTrue = (sourcePropertyValue.enumValueIndex == enumValue);
                                if (isTrue)
                                    break;
                            }
                        }

                        break;
                }
            }
            else
            {
                object parent = GetParent(property);
                Type eventOwnerType = parent.GetType();
                FieldInfo reflectedProperty = eventOwnerType.GetField(attribute.ConditionalSourceField);
                if (reflectedProperty != null)
                {
                    object reflectedValue = reflectedProperty.GetValue(parent);
                    if (reflectedValue is bool)
                        isTrue = (bool)reflectedValue;
                    else if (attribute.SpecificValue != null)
                    {
                        if (reflectedValue is Enum)
                        {
                            isTrue = false;
                            int reflectedEnumValue = (int)Convert.ChangeType(reflectedValue, TypeCode.Int32);
                            foreach (object value in attribute.SpecificValue)
                            {
                                int enumValue = (int)Convert.ChangeType(value, TypeCode.Int32);
                                isTrue = (reflectedEnumValue == enumValue);
                                if (isTrue)
                                    break;
                            }
                        }
                        else
                        {
                            Debug.Log("SpecificValue must currently be an enum.");
                        }
                    }
                    else
                        isTrue = reflectedValue != null;
                }
                else
                {
                    Debug.LogError(
                        "Attempting to use a ConditionalAttribute, but \""
                        + attribute.ConditionalSourceField
                        + "\" not found in " + GetParent(property).GetType() +
                        ". Is the inspect object nested in an array?");
                }
            }

            if (attribute.HideInInspectorIfTrue)
                return !isTrue;

            return isTrue;
        }
        #endregion
    }
}

#endif
#endregion