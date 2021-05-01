namespace Spineless
{
    using UnityEngine;


    [ExecuteInEditMode]
    public class StaticBackground : MonoBehaviour
    {
        [SerializeField]
        private bool followCamera = true;

        [SerializeField]
        private bool followCameraEditMode = true;

        private Camera mainCamera;


        public void OnEnable()
        {
            this.mainCamera = Camera.main;
        }


        public void Update()
        {
            if (this.followCamera)
            {
                if ((!this.followCameraEditMode)
                    && (Application.isEditor))
                    return;

                this.transform.position = new Vector3(
                    this.mainCamera.transform.position.x,
                    this.mainCamera.transform.position.y,
                    this.transform.position.z);
            }
        }
    }
}