using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Spineless
{
    [Serializable]
    public abstract class SerializedDictionary<TValue> :
        IDictionary<string, TValue>,
        ISerializationCallbackReceiver
    {
        #region Properties
        public int Count
        {
            get { return this.runtimeDictionary.Count; }
        }


        public bool IsReadOnly
        {
            get { return false; }
        }


        public ICollection<string> Keys
        {
            get { return this.runtimeDictionary.Keys; }
        }


        public ICollection<TValue> Values
        {
            get { return this.runtimeDictionary.Values; }
        }


        #region Indexers
        public TValue this[string key]
        {
            get
            {
                if (this.runtimeDictionary.ContainsKey(key))
                    return this.runtimeDictionary[key];
                else
                    return default(TValue);
            }
            set
            {
                if (this.runtimeDictionary.ContainsKey(key))
                    this.runtimeDictionary[key] = value;
                else
                    this.runtimeDictionary.Add(key, value);
            }
        }
        #endregion
        #endregion


        #region Entry Struct
        [Serializable]
        public struct Entry<TValue>
        {
            private readonly string key;
            private readonly TValue value;


            public Entry(string key, TValue value)
            {
                this.key = key;
                this.value = value;
            }


            public string Key
            {
                get { return this.key; }
            }


            public TValue Value
            {
                get { return this.value; }
            }
        }
        #endregion


        #region Fields
        [SerializeField]
        private readonly List<Entry<TValue>> entries = new List<Entry<TValue>>();

        private Dictionary<string, TValue> runtimeDictionary = new Dictionary<string, TValue>();
        #endregion


        #region ISerializationCallbackReceiver
        public void OnBeforeSerialize()
        {
            this.entries.Clear();
            foreach (KeyValuePair<string, TValue> entry in this)
            {
                this.entries.Add(new Entry<TValue>(entry.Key, entry.Value));
            }
        }


        public void OnAfterDeserialize()
        {
            this.runtimeDictionary.Clear();
            foreach (Entry<TValue> entry in this.entries)
            {
                this.runtimeDictionary.Add(entry.Key, entry.Value);
            }
        }
        #endregion


        #region IDictionary
        public void Add(string key, TValue value)
        {
            this.runtimeDictionary.Add(key, value);
        }


        public void Add(KeyValuePair<string, TValue> item)
        {
            this.runtimeDictionary.Add(item.Key, item.Value);
        }


        public void Clear()
        {
            this.runtimeDictionary.Clear();
        }


        public bool Contains(KeyValuePair<string, TValue> item)
        {
            return ((IDictionary<string, TValue>)this.runtimeDictionary).Contains(item);
        }


        public bool ContainsKey(string key)
        {
            return this.runtimeDictionary.ContainsKey(key);
        }


        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<string, TValue>)this.runtimeDictionary).CopyTo(array, arrayIndex);
        }


        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            return this.runtimeDictionary.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.runtimeDictionary).GetEnumerator();
        }


        public bool Remove(string key)
        {
            return this.runtimeDictionary.Remove(key);
        }


        public bool Remove(KeyValuePair<string, TValue> item)
        {
            return this.runtimeDictionary.Remove(item.Key);
        }


        public bool TryGetValue(string key, out TValue value)
        {
            return this.runtimeDictionary.TryGetValue(key, out value);
        }
        #endregion
    }
}