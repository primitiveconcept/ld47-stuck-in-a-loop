namespace StuckInALoop
{
    using System.Collections.Generic;
    using System.Linq;
    using PrimitiveEngine;
    using PrimitiveEngine.Unity;
    using UnityEngine;


    public class PlayerInteractionInputSystem : EntityComponentProcessingSystem<PlayerInput, EntityBoxCollider2D, PhysicsBody2D>
    {
        public override void Process(
            Entity entity, 
            PlayerInput playerInput, 
            EntityBoxCollider2D entityBoxCollider2D, 
            PhysicsBody2D physicsBody2D)
        {
            Transform transform = physicsBody2D.transform;
            BoxCollider2D boxCollider2D = entityBoxCollider2D.BoxCollider2D;
            
            // ReSharper disable once Unity.PreferNonAllocApi
            Collider2D[] hits = Physics2D.OverlapBoxAll(
                point: boxCollider2D.bounds.center, 
                size: boxCollider2D.size + new Vector2(physicsBody2D.SkinWidth, physicsBody2D.SkinWidth), 
                angle: transform.rotation.eulerAngles.z);

            List<Interactable> interactables = new List<Interactable>();
            foreach (Collider2D hit in hits)
            {
                Interactable interactable = hit.GetComponent<Interactable>();
                if (interactable == null)
                    continue;
                
                interactables.Add(interactable);

                // Entered proximity of new interactable.
                if (playerInput.CurrentInteractable != interactable
                    && interactable.OnProximityEntered != null)
                {
                    interactable.OnProximityEntered.Invoke();
                }
                    
                // Player activate current interactable. 
                bool triggeredInteract = playerInput.Controls.MouseSecondaryPressed.triggered
                                         || playerInput.Controls.Interact.triggered;
                if (triggeredInteract
                    && interactable.OnInteract != null)
                {
                    interactable.OnInteract.Invoke();
                }
            }

            // Player left proximity of previous interactable.
            // NOTE: Don't have interactables be too close together.
            if (playerInput.CurrentInteractable != null
                && !interactables.Contains(playerInput.CurrentInteractable))
            {
                if (playerInput.CurrentInteractable.OnProximityLeft != null)
                    playerInput.CurrentInteractable.OnProximityLeft.Invoke();
            }

            // Set player's new interactable.
            if (interactables.Count > 0)
                playerInput.CurrentInteractable = interactables[0];
            else
                playerInput.CurrentInteractable = null;
        }
    }
}