// ReSharper disable CompareOfFloatsByEqualityOperator
namespace StuckInALoop
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.Serialization;


    [CreateAssetMenu]
    public class InputControls : ScriptableObject
    {
        [Header("Axis")]
        public InputAction PrimaryAxis;

        [Header("Mouse")]
        public InputAction MousePrimaryPressed;
        public InputAction MousePrimaryReleased;
        public InputAction MouseSecondaryPressed;
        public InputAction MouseSecondaryReleased;

        [FormerlySerializedAs("Jump")]
        [Header("Buttons")]
        public InputAction Interact;


        public bool MousePrimaryHeld
        {
            get { return this.MousePrimaryPressed.ReadValue<float>() == 1; }
        }
        
        public bool MouseSecondaryHeld
        {
            get { return this.MouseSecondaryPressed.ReadValue<float>() == 1; }
        }


        public bool InteractHeld
        {
            get { return this.Interact.ReadValue<float>() == 1; }
        }
    }
}