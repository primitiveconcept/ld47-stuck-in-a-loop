namespace StuckInALoop
{
    using System.Collections.Generic;
    using PrimitiveEngine.Unity;
    using Spineless;
    using UnityEngine;


    public class Attack : EntityMonoBehaviour
    {
        public int Damage;
        public List<GameObject> IgnoreObjects = new List<GameObject>();
        public List<string> IgnoreTags = new List<string>();
        public bool DestroyOnContact = true;

        public Vector2 Target { get; set; }


        public void ClearIgnoreObjects()
        {
            this.IgnoreObjects.Clear();
        }


        public void OnDespawn()
        {
            ClearIgnoreObjects();
        }


        public void DoDamage(Collider2D other)
        {
            if (this.IgnoreObjects.Contains(other.gameObject))
                return;

            if (this.IgnoreTags.Contains(other.gameObject.tag))
                return;

            Health otherHealth = other.GetComponent<Health>();
            if (otherHealth != null)
            {
                otherHealth.Reduce(this.Damage);
            }

            if (this.DestroyOnContact)
            {
                PoolManager.Despawn(this.gameObject);
            }
        }
    }
}