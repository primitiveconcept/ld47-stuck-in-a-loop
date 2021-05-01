namespace Spineless
{
    using UnityEngine;


    public class ParallaxElement : MonoBehaviour
    {
        public Camera Camera;
        public float horizontalSpeed;
        public float verticalSpeed;
        public bool moveInOppositeDirection;
        public bool scrollTexture;

        private Vector3 previousCameraPosition;
        private Material material;


        public void OnEnable()
        {
            this.previousCameraPosition = this.Camera.transform.position;
            this.material = GetComponent<Renderer>().material;
        }


        public void Update()
        {
            Vector3 position = this.Camera.transform.position;
            Vector3 distance = position - this.previousCameraPosition;
            float direction = (this.moveInOppositeDirection)
                                  ? -1f
                                  : 1f;
            var translation = Vector3.Scale(
                                  distance,
                                  new Vector3(this.horizontalSpeed, this.verticalSpeed)) * direction;

            if (this.scrollTexture)
            {
                this.material.mainTextureOffset += (Vector2)translation;
            }
            else
            {
                this.transform.Translate(translation);
            }
            
            this.previousCameraPosition = position;
        }
    }
}