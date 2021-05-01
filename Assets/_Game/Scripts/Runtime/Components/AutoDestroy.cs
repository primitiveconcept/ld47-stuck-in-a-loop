namespace StuckInALoop
{
    using PrimitiveEngine.Unity;
    using Spineless;
    using UnityEngine;


    public class AutoDestroy : EntityMonoBehaviour
    {
        public float TimeToLive;

        private float counter;


        public void OnSpawn()
        {
            this.counter = 0;
        }

        public void Update()
        {
            if (this.TimeToLive > 0)
            {
                if (this.counter >= this.TimeToLive)
                {
                    if (!PoolManager.Despawn(this.gameObject))
                        Destroy(this.gameObject);
                }
                    
                else
                    this.counter += Time.deltaTime;
            }
        }
    }
}