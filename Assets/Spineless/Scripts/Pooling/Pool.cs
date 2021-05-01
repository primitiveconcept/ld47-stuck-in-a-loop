namespace Spineless
{
    using System.Collections.Generic;
    using UnityEngine;


    public class Pool : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private int preloadAmount = 0;

        [SerializeField]
        private bool limit = false;

        [SerializeField]
        private int maxCount;

        [SerializeField]
        private List<GameObject> active = new List<GameObject>();

        [SerializeField]
        private List<GameObject> inactive = new List<GameObject>();


        #region Properties
        public List<GameObject> Active
        {
            get { return this.active; }
            set { this.active = value; }
        }


        public List<GameObject> Inactive
        {
            get { return this.inactive; }
            set { this.inactive = value; }
        }


        public bool Limit
        {
            get { return this.limit; }
            set { this.limit = value; }
        }


        public int MaxCount
        {
            get { return this.maxCount; }
            set { this.maxCount = value; }
        }


        public GameObject Prefab
        {
            get { return this.prefab; }
            set { this.prefab = value; }
        }


        public int PreloadAmount
        {
            get { return this.preloadAmount; }
            set { this.preloadAmount = value; }
        }


        private int TotalCount
        {
            get
            {
                int count = 0;

                count += this.active.Count;
                count += this.inactive.Count;

                return count;
            }
        }
        #endregion


        public void Awake()
        {
            if (this.prefab == null)
                return;

            PoolManager.Add(this);
            Preload();
        }


        public bool Despawn(GameObject instance)
        {
            if (!this.active.Contains(instance))
                return false;

            if (instance.transform.parent != this.transform)
                instance.transform.parent = this.transform;

            this.active.Remove(instance);
            this.inactive.Add(instance);

            instance.BroadcastMessage("OnDespawn", SendMessageOptions.DontRequireReceiver);

            instance.SetActive(false);
            
            return true;
        }


        public void DespawnAll()
        {
            PoolManager.DeactivatePool(this.prefab);
        }


        public void DestroyCount(int count)
        {
            if (count > this.inactive.Count)
            {
                Debug.LogWarning(
                    "Destroy Count value: " + count + " is greater than inactive Count: "
                    + this.inactive.Count + ". Destroying all available inactive objects of type: " +
                    this.prefab.name + "." +
                    "Use DestroyUnused(false) instead to achieve the same.");
                DestroyUnused(false);
                return;
            }

            for (int i = this.inactive.Count - 1; i >= this.inactive.Count - count; i--)
            {
                Destroy(this.inactive[i]);
            }

            this.inactive.RemoveRange(this.inactive.Count - count, count);
        }


        public void DestroyUnused(bool preloadLimit)
        {
            if (preloadLimit)
            {
                for (int i = this.inactive.Count - 1; i >= this.PreloadAmount; i--)
                {
                    Destroy(this.inactive[i]);
                }

                if (this.inactive.Count > this.PreloadAmount)
                    this.inactive.RemoveRange(this.PreloadAmount, this.inactive.Count - this.PreloadAmount);
            }
            else
            {
                for (int i = 0; i < this.inactive.Count; i++)
                {
                    Destroy(this.inactive[i]);
                }

                this.inactive.Clear();
            }
        }


        public void OnDestroy()
        {
            this.active.Clear();
            this.inactive.Clear();
        }


        public void Preload()
        {
            if (this.prefab == null)
            {
                Debug.LogWarning("Cannot preload empty prefab.");
                return;
            }

            for (int i = this.TotalCount; i < this.PreloadAmount; i++)
            {
                GameObject poolObject = Instantiate(this.prefab, Vector3.zero, Quaternion.identity);
                poolObject.transform.SetParent(this.transform);

                Rename(poolObject.transform);
                poolObject.SetActive(false);
                this.inactive.Add(poolObject);
            }
        }


        public GameObject Spawn(Vector3 position, Quaternion rotation)
        {
            GameObject poolObject;
            Transform poolTransform;

            if (this.inactive.Count > 0)
            {
                poolObject = this.inactive[0];
                this.inactive.RemoveAt(0);

                poolTransform = poolObject.transform;
            }
            else
            {
                if (this.Limit
                    && this.active.Count >= this.MaxCount)
                {
                    return null;
                }

                poolObject = Instantiate(this.prefab);
                poolTransform = poolObject.transform;
                Rename(poolTransform);
            }

            poolTransform.position = position;
            poolTransform.rotation = rotation;

            if (poolTransform.parent != this.transform)
                poolTransform.parent = this.transform;

            this.active.Add(poolObject);

            poolObject.SetActive(true);

            poolObject.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

            return poolObject;
        }


        private void Rename(Transform instance)
        {
            instance.name += (this.TotalCount + 1).ToString("#000");
        }
    }
}