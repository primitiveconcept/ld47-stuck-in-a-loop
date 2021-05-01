namespace Spineless
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;


    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private int maxSpawns = 1;
        
        [SerializeField]
        private bool spawnOnStart = true;

        [SerializeField]
        private UnityEvent onSpawn;

        private List<GameObject> spawnedObjects = new List<GameObject>();
        

        #region Properties
        public GameObject Prefab
        {
            get { return this.prefab; }
            set { this.prefab = value; }
        }


        public bool SpawnOnStart
        {
            get { return this.spawnOnStart; }
            set { this.spawnOnStart = value; }
        }


        public int SpawnCount
        {
            get { return this.spawnedObjects.Count; }
        }
        #endregion


        public void Despawn(GameObject spawnedObject)
        {
            if (spawnedObject.activeSelf)
                Destroy(spawnedObject);

            if (this.spawnedObjects.Contains(spawnedObject))
                this.spawnedObjects.Remove(spawnedObject);
        }


        public void DespawnAll()
        {
            for (int i = 0; i < this.spawnedObjects.Count; i++)
            {
                var spawnedObject = this.spawnedObjects[i];
                if (spawnedObject.activeSelf)
                    Destroy(spawnedObject);
            }

            this.spawnedObjects.Clear();
        }


        public GameObject Spawn()
        {
            if (this.SpawnCount >= this.maxSpawns)
                return null;
            
            GameObject spawnedObject = Instantiate(this.prefab, this.transform.position, this.transform.rotation);
            this.spawnedObjects.Add(spawnedObject);
            SpawnTracker spawnTracker = spawnedObject.GetComponent<SpawnTracker>();
            if (spawnTracker == null)
                spawnTracker = spawnedObject.AddComponent<SpawnTracker>();
            spawnTracker.Spawner = this;

            if (this.onSpawn != null)
                this.onSpawn.Invoke();
            
            return spawnedObject;
        }


        public void Start()
        {
            if (this.spawnOnStart)
                Spawn();
        }
    }
}