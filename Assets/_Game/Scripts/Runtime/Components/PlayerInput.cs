namespace StuckInALoop
{
    using System.Collections.Generic;
    using System.Linq;
    using PrimitiveEngine.Unity;
    using UnityEngine.InputSystem;


    public class PlayerInput : EntityMonoBehaviour
    {
        public InputControls Controls;
        
        public Interactable CurrentInteractable { get; set; }
        public IEnumerable<InputAction> AllControls { get; set; }

        public void Start()
        {
            this.AllControls =
                this.Controls.GetType().GetFields()
                    .Where(info => info.FieldType == typeof(InputAction))
                    .Select(info => info.GetValue(this.Controls))
                    .Cast<InputAction>();
            
            EnableAll();
        }


        public void EnableAll()
        {
            foreach (InputAction inputAction in this.AllControls)
            {
                inputAction.Enable();
            }
        }


        public void DisableAll()
        {
            foreach (InputAction inputAction in this.AllControls)
            {
                inputAction.Disable();
            }
        }
    }
}