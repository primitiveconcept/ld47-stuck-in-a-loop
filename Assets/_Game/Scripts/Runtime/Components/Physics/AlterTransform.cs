namespace StuckInALoop
{
    using UnityEngine;


    public class AlterTransform : MonoBehaviour
    {
        [Header("Rotation")]
        public bool AutoRotate;
        public float RotateSpeed;

        [Header("Scale")]
        public bool AutoScale;
        public float ScaleSpeed;
        public float PingPongScaleRate;

        private new Transform transform;


        public void Awake()
        {
            this.transform = this.gameObject.transform;
        }


        public void Update()
        {
            if (this.AutoRotate)
            {
                Vector3 eulerAngles = this.transform.rotation.eulerAngles;
                this.transform.rotation = Quaternion.Euler(0f, 0f, eulerAngles.z + this.RotateSpeed);
            }

            if (this.AutoScale)
            {
                float scaleAdjustment = 
                    Mathf.Sin(Time.realtimeSinceStartup * this.PingPongScaleRate) * this.ScaleSpeed;
                Vector3 localScale = this.transform.localScale;
                localScale = new Vector3(
                    localScale.x + scaleAdjustment,
                    localScale.y + scaleAdjustment,
                    localScale.z);
                this.transform.localScale = localScale;
            }
        }
    }
}