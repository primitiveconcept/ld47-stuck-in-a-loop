namespace StuckInALoop
{
    using PrimitiveEngine;
    using PrimitiveEngine.Unity;
    using UnityEngine;


    public class FollowPlayerSystem : EntityComponentProcessingSystem<FollowPlayer, Movement2D>
    {
        public override void Process(
            Entity entity, 
            FollowPlayer followPlayer, 
            Movement2D movement2D)
        {
            Vector3 playerPosition = GameManager.Player.transform.position;
            
            if (followPlayer.RangeLimit > 0)
            {
                float distance = Vector2.Distance(entity.GetTransform().position, playerPosition);
                if (distance > followPlayer.RangeLimit)
                {
                    if (followPlayer.PlayerWasInRange)
                    {
                        if (followPlayer.OnPlayerLeavesRange != null)
                            followPlayer.OnPlayerLeavesRange.Invoke();
                        followPlayer.PlayerWasInRange = false;
                    }
                    
                    movement2D.MoveAxis = Vector2.zero;
                    return;
                }
            }

            followPlayer.PlayerWasInRange = true;
            if (followPlayer.WhilePlayerInRange != null)
                followPlayer.WhilePlayerInRange.Invoke();
            
            Vector3 playerDirection = (playerPosition - entity.GetTransform().position).normalized;
            movement2D.MoveAxis = playerDirection;
        }
    }
}