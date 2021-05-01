namespace Spineless
{
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;


    public class InformationAttribute : PropertyAttribute
    {
        public enum InformationType
        {
            Error,
            Info,
            None,
            Warning
        }


#if UNITY_EDITOR
        public string Message;
        public MessageType Type;
        public bool MessageAfterProperty;

        public InformationAttribute(string message, InformationType type, bool messageAfterProperty)
        {
            this.Message = message;
            if (type == InformationType.Error)
            {
                this.Type = MessageType.Error;
            }

            if (type == InformationType.Info)
            {
                this.Type = MessageType.Info;
            }

            if (type == InformationType.Warning)
            {
                this.Type = MessageType.Warning;
            }

            if (type == InformationType.None)
            {
                this.Type = MessageType.None;
            }

            this.MessageAfterProperty = messageAfterProperty;
        }
#else
		public InformationAttribute(string message, InformationType type, bool messageAfterProperty)
		{

		}
#endif
    }
}


#region Property Drawer
#if UNITY_EDITOR
namespace Spineless
{
    using UnityEditor;
    using UnityEngine;


    /// <summary>
    /// This class allows the display of a message box next to a property.
    /// </summary>
    [CustomPropertyDrawer(typeof(InformationAttribute))]
    public class InformationAttributeDrawer : PropertyDrawer
    {
        const int spaceBeforeTheTextBox = 5;
        const int spaceAfterTheTextBox = 10;
        const int iconWidth = 55;


        #region Properties
        InformationAttribute informationAttribute
        {
            get { return ((InformationAttribute)this.attribute); }
        }
        #endregion


        /// <summary>
        /// Returns the complete height of the whole block (property + help text).
        /// </summary>
        /// <returns>The block height.</returns>
        /// <param name="property">Property.</param>
        /// <param name="label">Label.</param>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property) + DetermineTextboxHeight(this.informationAttribute.Message)
                                                         + spaceAfterTheTextBox + spaceBeforeTheTextBox;
        }


        /// <summary>
        /// OnGUI, displays the property and the textbox in the specified order.
        /// </summary>
        /// <param name="rect">Rect.</param>
        /// <param name="prop">Property.</param>
        /// <param name="label">Label.</param>
        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
        {
            EditorStyles.helpBox.richText = true;
            Rect helpPosition = rect;
            Rect textFieldPosition = rect;

            if (!this.informationAttribute.MessageAfterProperty)
            {
                // we position the message before the property
                helpPosition.height = DetermineTextboxHeight(this.informationAttribute.Message);

                textFieldPosition.y += helpPosition.height + spaceBeforeTheTextBox;
                textFieldPosition.height = GetPropertyHeight(prop, label);
            }
            else
            {
                // we position the property first, then the message
                textFieldPosition.height = GetPropertyHeight(prop, label);

                helpPosition.height = DetermineTextboxHeight(this.informationAttribute.Message);
                // we add the complete property height (property + helpbox, as overridden in this very script), and substract both to get just the property
                helpPosition.y += GetPropertyHeight(prop, label)
                                  - DetermineTextboxHeight(this.informationAttribute.Message)
                                  - spaceAfterTheTextBox;
            }

            EditorGUI.HelpBox(helpPosition, this.informationAttribute.Message, this.informationAttribute.Type);
            EditorGUI.PropertyField(textFieldPosition, prop, label, true);
        }


        /// <summary>
        /// Determines the height of the textbox.
        /// </summary>
        /// <returns>The textbox height.</returns>
        /// <param name="message">Message.</param>
        protected virtual float DetermineTextboxHeight(string message)
        {
            GUIStyle style = new GUIStyle(EditorStyles.helpBox);
            style.richText = true;

            float newHeight = style.CalcHeight(new GUIContent(message), EditorGUIUtility.currentViewWidth - iconWidth);
            return newHeight;
        }
    }
}
#endif
#endregion