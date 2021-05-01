namespace Spineless
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;


    public class Health : ObservableRangeInt
    {
        [SerializeField]
        private float flickerInterval = 0.05f;

        [SerializeField]
        private Color damageFlickerColor = Color.red;

        [SerializeField]
        private Color invulnerabilityFlickerColor = Color.clear;

        [SerializeField]
        private float invulnerabilityDuration = 2f;

        [SerializeField]
        private bool invulerableOnSpawn = false;

        [SerializeField]
        private UnityEvent onDepleted;

        private bool isInvulnerable;
        private int originalHealth;


        public override void Start()
        {
            if (this.setToMaxOnStart)
                this.current = this.max;

            this.originalHealth = this.Current;
        }


        public override void OnDestroy()
        {
            StopAllCoroutines();
            base.OnDestroy();
        }


        public void Despawn()
        {
            StopAllCoroutines();
            if (PoolManager.HasPool(this.gameObject))
                PoolManager.Despawn(this.gameObject);
            else
                Destroy(this.gameObject);
        }


        // TODO: Might want to pass duration value, for invulnerability powerups
        public void EnableTemporaryInvulnerability()
        {
            if (this.invulnerabilityDuration <= 0)
                return;
            
            Coroutines.StartGlobal(InvulnerabilityCoroutine());
        }


        public void Restore()
        {
            this.SetCurrent(this.Max);
        }
        

        public void OnSpawn()
        {
            SetCurrent(this.originalHealth);
            if (this.invulerableOnSpawn)
                EnableTemporaryInvulnerability();
        }


        public override void Reduce(int amount, bool forceEvent = false)
        {
            if (this.isInvulnerable)
                return;

            base.Reduce(amount, forceEvent);

            if (this.Current == this.Min)
                this.onDepleted.Invoke();
        }


        public void UpdateAnimatorValue()
        {
            Animator animator = GetComponent<Animator>();
            animator.SetInteger("Health", this.Current);
        }


        private IEnumerator InvulnerabilityCoroutine()
        {
            this.isInvulnerable = true;
            float invulnerabilityCounter = 0;
            yield return null; // Hack, sprite renderer may not be available on this frame.

            if (this == null)
                yield break;
            
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            Color originalColor = spriteRenderer.color;
            while (invulnerabilityCounter < this.invulnerabilityDuration)
            {
                spriteRenderer.color = spriteRenderer.color == originalColor
                                           ? this.invulnerabilityFlickerColor
                                           : originalColor;
                invulnerabilityCounter += Time.deltaTime;
                yield return new WaitForSeconds(this.flickerInterval);
            }

            spriteRenderer.color = originalColor;
            this.isInvulnerable = false;
        }


        #region Properties
        public bool IsInvulnerable
        {
            get { return this.isInvulnerable; }
        }


        public UnityEvent OnDepleted
        {
            get { return this.onDepleted; }
        }
        #endregion
    }
}