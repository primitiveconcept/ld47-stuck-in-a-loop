namespace StuckInALoop
{
    using UnityEngine;
    using UnityEngine.Events;


    public class Timer : MonoBehaviour
    {
        public float Interval;
        public float Variance;

        public bool Active { get; set; }
        
        public UnityEvent OnInterval;
        
        
        public float CurrentInterval;
        public float Counter;


        public void ResetTimer()
        {
            this.Counter = 0;
            this.CurrentInterval = this.Interval + UnityEngine.Random.Range(-this.Variance, this.Variance);
        }

        public void Update()
        {
            if (!this.Active)
                return;

            this.Counter += Time.deltaTime;
            if (this.Counter > this.CurrentInterval)
            {
                if (this.OnInterval != null)
                    this.OnInterval.Invoke();
                ResetTimer();
            }
        }
    }
}