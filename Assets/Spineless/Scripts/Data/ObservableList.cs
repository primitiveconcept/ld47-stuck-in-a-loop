namespace Spineless
{
    using System;
    using System.Collections.Generic;


    [Serializable]
    public class ObservableList<T> : List<T>
    {
        public event Action<T> Added;
        public event Action<int> Changed;
        public event Action<T> Removed;


        #region Properties
        public new T this[int index]
        {
            get { return base[index]; }
            set
            {
                base[index] = value;
                RaiseEvent(Changed, index);
            }
        }
        #endregion


        public new void Add(T item)
        {
            base.Add(item);
            RaiseEvent(Added, item);
        }


        public new void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);
            foreach (T item in collection)
            {
                RaiseEvent(Added, item);
            }
        }


        public new void Clear()
        {
            foreach (T item in this)
            {
                Remove(item);
            }
        }


        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            RaiseEvent(Added, item);
        }


        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            foreach (T item in collection)
            {
                RaiseEvent(Added, item);
            }
        }


        public new void Remove(T item)
        {
            base.Remove(item);
            RaiseEvent(Removed, item);
        }


        public new void RemoveAll(Predicate<T> match)
        {
            base.RemoveAll(match);
            foreach (T item in this)
            {
                if (match(item))
                    Remove(item);
            }
        }


        public new void RemoveRange(int index, int count)
        {
            for (int i = index; i < count; i++)
            {
                T item = this[i];
                Remove(item);
            }
        }


        /// <summary>
        /// Raise an Action event with an argument.
        /// </summary>
        /// <typeparam name="T">Action argument type.</typeparam>
        /// <param name="action"></param>
        /// <param name="arg">Action argument.</param>
        private static void RaiseEvent<T>(
            Action<T> action,
            T arg)
        {
            if (action != null)
                action(arg);
        }
    }
}