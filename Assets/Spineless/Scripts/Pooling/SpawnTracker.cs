namespace Spineless
{
    using System;
    using System.Collections;
    using UnityEngine;


    public class SpawnTracker : MonoBehaviour
    {
        private Spawner spawner;


        #region Properties
        public Spawner Spawner
        {
            get { return this.spawner; }
            set { this.spawner = value; }
        }
        #endregion


        public void OnDestroy()
        {
            if (this.spawner != null)
                this.spawner.Despawn(this.gameObject);
        }


        public void OnDespawn()
        {
            if (this.spawner != null)
                this.spawner.Despawn(this.gameObject);
        }


        public void Respawn()
        {
            if (this.gameObject.activeSelf)
                this.spawner.Despawn(this.gameObject);
            this.spawner.Spawn();
        }


        public void Respawn(float delay)
        {
            this.Spawner.StartCoroutine(SpawnDelayCoroutine(delay));
        }


        private IEnumerator SpawnDelayCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            Respawn();
        }
    }
}