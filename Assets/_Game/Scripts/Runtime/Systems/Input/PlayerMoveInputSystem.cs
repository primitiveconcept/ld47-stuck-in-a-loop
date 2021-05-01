namespace StuckInALoop
{
    using PrimitiveEngine;
    using UnityEngine;


    [EntitySystem(UpdateType = UpdateType.FrameUpdate)]
    public class PlayerMoveInputSystem : EntityComponentProcessingSystem<PlayerInput, Movement2D>
    {
        public override void Process(
            Entity entity, 
            PlayerInput playerInput, 
            Movement2D movement2D)
        {
            movement2D.MoveAxis = playerInput.Controls.PrimaryAxis.ReadValue<Vector2>();
        }
    }
}