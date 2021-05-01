namespace StuckInALoop
{
    using PrimitiveEngine.Unity;
    using UnityEngine;


    [CreateAssetMenu]
    public class VariableSet : ScriptableObject
    {
        [ReorderableList]
        public LevelVariable[] Variables;
    }
}