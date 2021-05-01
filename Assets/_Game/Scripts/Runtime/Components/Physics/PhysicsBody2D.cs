namespace StuckInALoop
{
    using PrimitiveEngine.Unity;
    using UnityEngine;


    public class PhysicsBody2D : EntityMonoBehaviour
    {
        public Vector2 Velocity;

        public float SkinWidth = 0.07f;
        
        public bool CollidingUp { get; set; }
        public bool CollidingDown { get; set; }
        public bool CollidingLeft { get; set; }
        public bool CollidingRight { get; set; }
    }
}