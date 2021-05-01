namespace Spineless
{
    using System;
    using PrimitiveEngine.Unity;
    using UnityEngine;
    using UnityEngine.Events;


    [Serializable]
    public abstract class ObservableRangeInt : EntityMonoBehaviour
    {
        [SerializeField]
        protected int current;

        [SerializeField]
        protected int min = 0;

        [SerializeField]
        protected int max = 100;

        [SerializeField]
        protected bool setToMaxOnStart = true;

        [SerializeField]
        protected ChangedEvent onChanged;


        #region Properties
        public int Current
        {
            get { return this.current; }
        }


        public int Max
        {
            get { return this.max; }
        }


        public int Min
        {
            get { return this.min; }
        }


        public ChangedEvent OnChanged
        {
            get { return this.onChanged; }
        }


        public bool SetToMaxOnStart
        {
            get { return this.setToMaxOnStart; }
            set { this.setToMaxOnStart = value; }
        }
        #endregion


        public virtual void Increase(int amount, bool forceEvent = false)
        {
            int newValue = this.current + amount;
            if (newValue > this.max)
                newValue = this.max;

            if (newValue == this.current)
            {
                if (forceEvent)
                    InvokeChanged();
                return;
            }

            this.current = newValue;
            InvokeChanged();
        }


        public void InvokeChanged()
        {
            if (this.onChanged != null)
                this.onChanged.Invoke(this);
        }


        public virtual void Reduce(int amount, bool forceEvent = false)
        {
            int newValue = this.current - amount;
            if (newValue < 0)
                newValue = 0;
            if (newValue > this.max)
                newValue = this.max;

            if (newValue == this.current)
            {
                if (forceEvent)
                    InvokeChanged();
                return;
            }

            this.current = newValue;
            InvokeChanged();
        }


        public virtual void SetCurrent(int value, bool forceEvent = false)
        {
            if (value == this.current)
            {
                if (forceEvent)
                    InvokeChanged();
                return;
            }

            this.current = value;
            InvokeChanged();
        }


        public virtual void SetMax(int value, bool forceEvent = false)
        {
            if (value < this.min)
                value = this.min;

            if (value == this.max)
            {
                if (forceEvent)
                    InvokeChanged();
                return;
            }

            this.max = value;
            InvokeChanged();
        }


        public virtual void SetMin(int value, bool forceEvent = false)
        {
            if (value > this.max)
                value = this.max;

            if (value == this.min)
            {
                if (forceEvent)
                    InvokeChanged();
                return;
            }

            this.min = value;
            InvokeChanged();
        }


        public virtual void Start()
        {
            if (this.setToMaxOnStart)
                this.current = this.max;

            InvokeChanged();
        }


        [Serializable]
        public class ChangedEvent : UnityEvent<ObservableRangeInt>
        {
        }
    }
}