namespace StuckInALoop
{
    using UnityEngine;


    public class TriAttack : MonoBehaviour
    {
        public AlterTransform[] ChildTranforms;

        private AlterTransform mainAlterTransform;


        public float CurrentRotationSpeed
        {
            get { return this.mainAlterTransform.RotateSpeed; }
        }


        public void Awake()
        {
            this.mainAlterTransform = GetComponent<AlterTransform>();
        }


        public void SetRotationSpeed(float newSpeed)
        {
            this.mainAlterTransform.RotateSpeed = newSpeed;
            foreach (AlterTransform child in this.ChildTranforms)
            {
                child.RotateSpeed = -newSpeed;
            }
        }
    }
}