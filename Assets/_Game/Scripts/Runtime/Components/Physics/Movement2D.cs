namespace StuckInALoop
{
    using PrimitiveEngine.Unity;
    using UnityEngine;


    public class Movement2D : EntityMonoBehaviour
    {
        public enum Axis
        {
            All,
            Horizontal,
            Vertical
        }
        
        public float Speed;
        public float IntertiaStrength; // Between 0 and 1.
        public Vector2 MoveAxis;
        public Vector2 AxisMultipliers = Vector2.one;
        public Axis ClampAxisTo;
        public bool BinaryMovement;
    }
}