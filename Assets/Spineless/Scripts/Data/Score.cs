namespace Spineless
{
    using UnityEngine;
    using UnityEngine.Events;


    public class Score : ObservableRangeInt
    {
        [SerializeField]
        private UnityEvent onMaxedOut;


        #region Properties
        public UnityEvent OnMaxedOut
        {
            get { return this.onMaxedOut; }
        }
        #endregion


        public override void Increase(int amount, bool forceEvent = false)
        {
            base.Increase(amount, forceEvent);

            if (this.Current == this.Max)
                this.onMaxedOut.Invoke();
        }
    }
}