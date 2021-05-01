namespace StuckInALoop
{
    using PrimitiveEngine;
    using UnityEngine;


    [EntitySystem(UpdateType = UpdateType.FixedUpdate)]
    public class Physics2DSystem : EntityComponentProcessingSystem<PhysicsBody2D>
    {
        public override void Process(Entity entity, PhysicsBody2D physicsBody2D)
        {
            if (physicsBody2D.Velocity == Vector2.zero)
                return;
            
            Transform transform = physicsBody2D.transform;
            transform.Translate(physicsBody2D.Velocity * Time.deltaTime);
        }
    }
}