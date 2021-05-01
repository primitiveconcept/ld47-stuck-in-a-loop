namespace StuckInALoop
{
    using PrimitiveEngine;
    using UnityEngine;


    [EntitySystem(UpdateType = UpdateType.FrameUpdate)]
    public class MoveSystem : EntityComponentProcessingSystem<Movement2D, PhysicsBody2D>
    {
        private const float FudgeFactor = 0.02f;


        public override void Process(Entity entity, Movement2D movement2D, PhysicsBody2D physicsBody2D)
        {
            physicsBody2D.Velocity *= movement2D.IntertiaStrength;

            if (Mathf.Abs(physicsBody2D.Velocity.x) < FudgeFactor)
                physicsBody2D.Velocity.x = 0;
            if (Mathf.Abs(physicsBody2D.Velocity.y) < FudgeFactor)
                physicsBody2D.Velocity.y = 0;
            
            if (movement2D.ClampAxisTo != Movement2D.Axis.Vertical
                && movement2D.MoveAxis.x != 0)
            {
                float adjustedSpeed = movement2D.Speed * movement2D.AxisMultipliers.x;
                physicsBody2D.Velocity.x = movement2D.BinaryMovement
                                               ? Mathf.Sign(movement2D.MoveAxis.x) * adjustedSpeed
                                               : movement2D.MoveAxis.x * adjustedSpeed;
            }
            
            if (movement2D.ClampAxisTo != Movement2D.Axis.Horizontal
                && movement2D.MoveAxis.y != 0)
            {
                float adjustedSpeed = movement2D.Speed * movement2D.AxisMultipliers.y;
                physicsBody2D.Velocity.y = movement2D.BinaryMovement
                                               ? Mathf.Sign(movement2D.MoveAxis.y) * adjustedSpeed
                                               : movement2D.MoveAxis.y * adjustedSpeed;
            }
                
        }
    }
}