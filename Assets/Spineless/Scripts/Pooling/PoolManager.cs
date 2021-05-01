namespace Spineless
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;


    public class PoolManager : MonoBehaviour
    {
        private static Dictionary<GameObject, Pool> _managedPools =
            new Dictionary<GameObject, Pool>();


        public static Dictionary<GameObject, Pool> ManagedPools
        {
            get
            {
                if (_managedPools == null)
                    _managedPools = new Dictionary<GameObject, Pool>();
                return _managedPools;
            }
        }


        public void OnDestroy()
        {
            ManagedPools.Clear();
        }


        public static void Add(Pool pool)
        {
            if (pool.Prefab == null)
            {
                Debug.LogError(
                    "Prefab of pool: " + pool.gameObject.name + " is empty! " +
                    "Can't add pool to managedPools Dictionary.");
                return;
            }

            if (ManagedPools.ContainsKey(pool.Prefab))
            {
                Debug.LogError(
                    "Pool with prefab " + pool.Prefab.name + " has already been added " +
                    "to managedPools Dictionary.");
                return;
            }

            ManagedPools.Add(pool.Prefab, pool);
        }


        public static Pool CreatePool(
            GameObject prefab,
            int preLoad,
            bool limit,
            int maxCount,
            GameObject poolTarget = null)
        {
            if (ManagedPools.ContainsKey(prefab))
            {
                Debug.LogError("Pool Manager already contains Pool for prefab: " + prefab.name);
                return ManagedPools[prefab];
            }

            if (poolTarget == null)
                poolTarget = new GameObject(prefab.name + " Pool");
            Pool poolComponent = poolTarget.AddComponent<Pool>();

            poolComponent.Prefab = prefab;
            poolComponent.PreloadAmount = preLoad;
            poolComponent.Limit = limit;
            poolComponent.MaxCount = maxCount;

            poolComponent.Awake();

            return poolComponent;
        }


        public static void DeactivatePool(GameObject prefab)
        {
            if (!ManagedPools.ContainsKey(prefab))
            {
                Debug.LogError("PoolManager couldn't find Pool for prefab to deactivate: " + prefab.name);
                return;
            }

            List<GameObject> activeList = ManagedPools[prefab].Active.ToList();
            foreach (GameObject item in activeList)
            {
                ManagedPools[prefab].Despawn(item);
            }
        }


        public static bool Despawn(GameObject instance)
        {
            var pool = GetPool(instance);
            if (pool == null)
                return false;
            
            return pool.Despawn(instance);
        }


        public static void DestroyAllInactive(bool limitToPreLoad)
        {
            foreach (GameObject prefab in ManagedPools.Keys)
                ManagedPools[prefab].DestroyUnused(limitToPreLoad);
        }


        public static void DestroyAllPools()
        {
            foreach (GameObject prefab in ManagedPools.Keys)
                DestroyPool(ManagedPools[prefab].gameObject);
        }


        public static void DestroyPool(GameObject prefab)
        {
            if (!ManagedPools.ContainsKey(prefab))
            {
                Debug.LogError("PoolManager couldn't find Pool for prefab to destroy: " + prefab.name);
                return;
            }

            Destroy(ManagedPools[prefab].gameObject);
            ManagedPools.Remove(prefab);
        }


        public static bool HasPool(GameObject instance)
        {
            foreach (GameObject prefab in ManagedPools.Keys)
            {
                if (ManagedPools[prefab].Active.Contains(instance))
                    return true;

                if (ManagedPools[prefab].Inactive.Contains(instance))
                    return true;
            }

            return false;
        }


        public static Pool GetPool(GameObject instance)
        {
            foreach (GameObject prefab in ManagedPools.Keys)
            {
                if (ManagedPools[prefab].Active.Contains(instance))
                    return ManagedPools[prefab];

                if (ManagedPools[prefab].Inactive.Contains(instance))
                    return ManagedPools[prefab];
            }
            
            return null;
        }


        public static GameObject Spawn(GameObject prefab)
        {
            return Spawn(prefab, Vector3.zero, Quaternion.identity);
        }


        public static GameObject Spawn(GameObject prefab, Vector3 position)
        {
            return Spawn(prefab, position, Quaternion.identity);
        }


        public static GameObject Spawn(
            GameObject prefab,
            Vector3 position,
            Quaternion rotation)
        {
            if (!ManagedPools.ContainsKey(prefab))
            {
                Debug.Log("New pool: " + prefab.name);
                CreatePool(prefab, 0, false, 0);
            }

            return ManagedPools[prefab].Spawn(position, rotation);
        }
    }
}