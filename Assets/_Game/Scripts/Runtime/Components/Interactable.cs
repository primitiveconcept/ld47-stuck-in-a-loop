namespace StuckInALoop
{
    using System.Collections.Generic;
    using PrimitiveEngine.Unity;
    using UnityEngine;
    using UnityEngine.Events;


    public class Interactable : EntityMonoBehaviour
    {
        public UnityEvent OnProximityEntered;
        public UnityEvent OnProximityLeft;
        public UnityEvent OnInteract;
    }
}