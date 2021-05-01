namespace StuckInALoop
{
    using PrimitiveEngine;
    using UnityEngine;
    using UnityEngine.InputSystem;


    public class PlayerAttackInputSystem : EntityComponentProcessingSystem<PlayerInput, AttackSpawner>
    {
        public override void Process(Entity entity, PlayerInput playerInput, AttackSpawner attackSpawner)
        {
            if (!playerInput.Controls.MousePrimaryHeld)
                return;
                
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            attackSpawner.SpawnAttack(worldPosition);
        }
    }
}