namespace Spineless
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEngine;


    /// <summary>
    /// A very simple state machine that does not include transitions.
    /// Target object must contains method names matching enum value names.
    /// </summary>
    /// <typeparam name="E">Enumeration of states.</typeparam>
    public class SimpleStateMachine<E>
    {
        private Action[] actions;
        private E currentState;
        private E previousState;


        #region Constructors
        /// <summary>
        /// Instantiate a SimpleStateMachine.
        /// </summary>
        /// <param name="targetObject">Object containing methods matching state enumerations.</param>
        public SimpleStateMachine(object targetObject)
        {
            PopulateActions(targetObject);
        }
        #endregion


        #region Properties
        /// <summary>
        /// The current state of the SimpleStateMachine.
        /// </summary>
        public E CurrentState
        {
            get { return this.currentState; }
        }


        /// <summary>
        /// The previous state of the SimpleStateMachine.
        /// </summary>
        public E PreviousState
        {
            get { return this.previousState; }
        }
        #endregion


        #region Operators
        /// <summary>
        /// Get the current state of the SimpleStateMachine.
        /// </summary>
        /// <param name="simpleStateMachine">SimpleStateMachine to get current state from.</param>
        public static implicit operator E(SimpleStateMachine<E> simpleStateMachine)
        {
            return simpleStateMachine.currentState;
        }
        #endregion


        /// <summary>
        /// Execute the Action associated with the current state.
        /// </summary>
        public void Execute()
        {
            int index = (int)(object)this.currentState;
            this.actions[index]();
        }


        public void SetState(E value)
        {
            this.previousState = this.currentState;
            this.currentState = value;
        }


        #region Helper Methods
        private void PopulateActions(object targetObject)
        {
            // Cache update actions automatically from enum values.
            // Methods must have the same names as the enum values.
            IEnumerable<E> loadStateValues =
                Enum.GetValues(typeof(E)).Cast<E>();
            int loadStateValuesCount = loadStateValues.Count();
            this.actions = new Action[loadStateValuesCount];
            Type targetObjectType = targetObject.GetType();

            IEnumerator<E> enumEnumerator = loadStateValues.GetEnumerator();
            enumEnumerator.MoveNext();
            for (int i = 0; i < loadStateValuesCount; i++)
            {
                E stateValue = enumEnumerator.Current;

                MethodInfo enumMethod = targetObjectType.GetMethod(
                    stateValue.ToString(),
                    BindingFlags.Instance | BindingFlags.Public);

                if (enumMethod != null)
                {
                    Action updateAction = Delegate.CreateDelegate(
                                              typeof(Action),
                                              targetObject,
                                              enumMethod) as Action;
                    this.actions[i] = updateAction;
                }

                // Could not find the method.
                else
                {
                    Debug.Log("Could not add state action: " + stateValue.ToString());
                }

                enumEnumerator.MoveNext();
            }
        }
        #endregion
    }
}