namespace StuckInALoop
{
    using PrimitiveEngine.Unity;
    using Spineless;
    using UnityEngine;
    using UnityEngine.InputSystem;


    public class AttackSpawner : EntityMonoBehaviour
    {
        public Attack AttackToSpawn;
        public float CooldownTime;
        public bool RotateTowardTarget = false;
        public bool Nest;
        public bool Maintained;
        public bool AutoAttack;
        public bool TargetsPlayer;
        public bool CanHurtOwner = false;

        public float CooldownTimer { get; set; }
        public Attack SpawnedAttack { get; set; }


        public void Update()
        {
            if (this.AutoAttack)
            {
                // Only auto-attack player if they're within attack reach.
                if (this.TargetsPlayer)
                {
                    AutoDestroy autoDestroy = this.AttackToSpawn.GetComponent<AutoDestroy>();
                    Movement2D attackMovement = this.AttackToSpawn.GetComponent<Movement2D>();
                    if (autoDestroy != null
                        && attackMovement != null)
                    {
                        float timeToLive = autoDestroy.TimeToLive;
                        float speed = attackMovement.Speed;
                        float attackDistance = timeToLive * speed;

                        float distanceToPlayer = Vector2.Distance(
                            this.transform.position,
                            GameManager.Player.transform.position);
                        if (distanceToPlayer > attackDistance)
                            return;
                    }
                }
                
                Vector2 target = this.TargetsPlayer
                                     ? (Vector2)GameManager.Player.transform.position
                                     : Vector2.down;
                SpawnAttack(target);
            }
            
            if (this.CooldownTimer <= 0)
                return;

            this.CooldownTimer -= Time.deltaTime;
        }


        public Attack SpawnAttack(Vector2 target)
        {
            if (this.SpawnedAttack != null
                && this.Maintained)
            {
                this.SpawnedAttack.Target = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                
                if (!this.SpawnedAttack.gameObject.activeSelf)
                    this.SpawnedAttack.gameObject.SetActive(true);
            
                if (this.RotateTowardTarget)
                {
                    Transform entityTransform = this.transform;
                    Vector2 direction =
                        (this.SpawnedAttack.Target 
                         - (Vector2)entityTransform.position)
                        .normalized;
                    Vector3 dir = -direction;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    angle += 90;
                    this.SpawnedAttack.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
                return this.SpawnedAttack;
            }

            if (this.CooldownTimer > 0)
                return null;
            
            this.CooldownTimer = this.CooldownTime;

            GameObject spawnedAttackObject = this.Nest
                ? Instantiate(this.AttackToSpawn.gameObject, this.transform.position, Quaternion.identity)
                : PoolManager.Spawn(this.AttackToSpawn.gameObject, this.transform.position);

            spawnedAttackObject.transform.localScale =
                this.AttackToSpawn.transform.localScale
                * this.transform.localScale.x;
            
            if (this.Nest)
                spawnedAttackObject.transform.SetParent(this.transform);
            

            Attack spawnedAttack = spawnedAttackObject.GetComponent<Attack>();
            spawnedAttack.Target = target;
            if (!this.CanHurtOwner)
                spawnedAttack.IgnoreObjects.Add(this.gameObject);

            Movement2D movement2D = spawnedAttackObject.GetComponent<Movement2D>();
            if (movement2D != null)
            {
                Transform entityTransform = this.transform;
                Vector2 direction =
                    (spawnedAttack.Target 
                     - (Vector2)entityTransform.position)
                    .normalized;
                
                if (this.RotateTowardTarget)
                {
                    Vector3 dir = -direction;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    angle += 90;
                    spawnedAttackObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    movement2D.MoveAxis = new Vector2(0f, 1f);
                }
                else
                {
                    movement2D.MoveAxis = direction;    
                }
            }

            if (this.Maintained)
                this.SpawnedAttack = spawnedAttack;

            return spawnedAttack;
        }
    }
    
}