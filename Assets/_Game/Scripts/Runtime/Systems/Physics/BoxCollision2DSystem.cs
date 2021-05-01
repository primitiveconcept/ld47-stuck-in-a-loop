namespace StuckInALoop
{
    using System.Collections.Generic;
    using PrimitiveEngine;
    using PrimitiveEngine.Unity;
    using Spineless.Extensions.LayerMasks;
    using UnityEngine;


    [EntitySystem(UpdateType = UpdateType.FixedUpdate)]
    public class BoxCollision2DSystem : EntityComponentProcessingSystem<PhysicsBody2D, EntityBoxCollider2D>
    {
        public override void Process(
            Entity entity, 
            PhysicsBody2D physicsBody2D, 
            EntityBoxCollider2D entityBoxCollider2D)
        {
            physicsBody2D.CollidingUp = false;
            physicsBody2D.CollidingDown = false;
            physicsBody2D.CollidingLeft = false;
            physicsBody2D.CollidingRight = false;
            
            CheckCollision(entityBoxCollider2D, physicsBody2D);
        }


        private static void CheckCollision(
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
            
            List<Collider2D> newTriggers = new List<Collider2D>();
            List<Collider2D> newCollisions = new List<Collider2D>();
            
            foreach (Collider2D hit in hits)
            {
                if (hit == boxCollider2D
                    || Physics2D.GetIgnoreLayerCollision(
                        hit.gameObject.layer, boxCollider2D.gameObject.layer))
                {
                    continue;
                }
                    

                if (!hit.isTrigger
                    && !boxCollider2D.isTrigger)
                {
                    Bounds hitBounds = hit.bounds;
                    Bounds entityBounds = entityBoxCollider2D.BoxCollider2D.bounds;
                    physicsBody2D.CollidingDown = 
                        hitBounds.max.y <= entityBounds.min.y;
                    physicsBody2D.CollidingUp =
                        hitBounds.min.y >= entityBounds.max.y;
                    physicsBody2D.CollidingLeft =
                        hitBounds.max.x <= entityBounds.min.x;
                    physicsBody2D.CollidingRight =
                        hitBounds.min.x >= entityBounds.max.x;    
                }

                // Trigger
                if (boxCollider2D.isTrigger
                    || hit.isTrigger)
                {
                    newTriggers.Add(hit);
                        
                    // Trigger Entered
                    if (!entityBoxCollider2D.TriggerList.Contains(hit)
                        && entityBoxCollider2D.TriggerEntered != null)
                    {
                        entityBoxCollider2D.TriggerEntered.Invoke(hit);
                    }

                    // Trigger Stayed
                    else if (entityBoxCollider2D.TriggerStayed != null)
                        entityBoxCollider2D.TriggerStayed.Invoke(hit);
                }
                    
                // Collision
                else
                {
                    newCollisions.Add(hit);
                        
                    // Collision Entered
                    if (!entityBoxCollider2D.CollisionList.Contains(hit)
                        && entityBoxCollider2D.CollisionEntered != null)
                    {
                        entityBoxCollider2D.CollisionEntered.Invoke(hit);
                    }

                    // Collision Stayed
                    else if (entityBoxCollider2D.CollisionStayed != null)
                        entityBoxCollider2D.CollisionStayed.Invoke(hit);
                }
                
                ColliderDistance2D distance = hit.Distance(boxCollider2D);
                if (distance.isOverlapped
                    && !boxCollider2D.isTrigger 
                    && !hit.isTrigger)
                {
                    // Block movement
                    transform.Translate(distance.pointA - distance.pointB);
                }
            }

            foreach (Collider2D previousTrigger in entityBoxCollider2D.TriggerList)
            {
                // Trigger Exited
                if (!newTriggers.Contains(previousTrigger)
                    && entityBoxCollider2D.TriggerExited != null)
                {
                    entityBoxCollider2D.TriggerExited.Invoke(previousTrigger);
                }
            }

            foreach (Collider2D previousCollision in entityBoxCollider2D.CollisionList)
            {
                // Collision Exited
                if (!newCollisions.Contains(previousCollision)
                    && entityBoxCollider2D.CollisionExited != null)
                {
                    entityBoxCollider2D.CollisionExited.Invoke(previousCollision);
                }
            }

            entityBoxCollider2D.TriggerList = newTriggers;
            entityBoxCollider2D.CollisionList = newCollisions;
        }
    }
}