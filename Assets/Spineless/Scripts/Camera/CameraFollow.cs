namespace Spineless
{
    using UnityEngine;


    public class CameraFollow : MonoBehaviour
    {
        public Transform FollowTarget;
        public Vector3 Offset;
        public float SmoothTime = 0.3F;

        private Camera camera;
        private Vector3 originalOffest;
        private float originalOrthographicSize;
        private float originalZ;
        private Vector3 velocity = Vector3.zero;


        public void Awake()
        {
            this.originalZ = this.transform.position.z;
            this.originalOffest = this.Offset;
            this.camera = GetComponent<Camera>();
            if (this.camera != null)
            {
                this.originalOrthographicSize = Camera.main.orthographicSize;    
            }
        }


        public void Update()
        {
            if (this.camera != null)
            {
                float scaleFactor = this.transform.position.z / this.originalZ;
                this.camera.orthographicSize = this.originalOrthographicSize * scaleFactor;
                this.Offset = this.originalOffest * scaleFactor;
            }
            
            Vector3 targetPosition = this.FollowTarget.TransformPoint(new Vector3(
                0, 
                0, 
                0 + this.originalZ));
            this.transform.position = Vector3.SmoothDamp(
                current: this.transform.position, 
                target: targetPosition + this.Offset, 
                currentVelocity: ref this.velocity, 
                smoothTime: this.SmoothTime);
        }


        public void ChangeFollowTarget(Transform newTarget)
        {
            this.FollowTarget = newTarget;
        }


        public void ChangeOffset(Vector3 newOffset)
        {
            this.Offset = newOffset;
        }
    }
}