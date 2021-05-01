namespace StuckInALoop
{
    using PrimitiveEngine.Unity;
    using UnityEngine.Events;


    public class FollowPlayer : EntityMonoBehaviour
    {
        public float RangeLimit;

        public UnityEvent WhilePlayerInRange;
        public UnityEvent OnPlayerLeavesRange;
        
        public bool PlayerWasInRange { get; set; }
    }
}