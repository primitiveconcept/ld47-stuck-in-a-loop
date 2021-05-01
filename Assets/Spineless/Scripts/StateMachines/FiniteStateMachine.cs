namespace Spineless
{
    using System;
    using System.Collections.Generic;
    using System.Linq;


    /// <summary>
    /// Sets up a state machine based on an enumeration of states.
    /// </summary>
    /// <typeparam name="E">Enumeration of possible states.</typeparam>
    public class FiniteStateMachine<E>
    {
        private E currentState;
        private E previousState;
        private bool locked = false;
        private Dictionary<StateTransition<E>, Delegate> stateTransitions;


        #region Constructors
        /// <summary>
        /// Instantiates a new instance of the FiniteStateMachine class.
        /// </summary>
        /// <param name="initialState">Beginning state of the state machine.</param>
        public FiniteStateMachine(E initialState)
        {
            this.currentState = initialState;
            this.stateTransitions = new Dictionary<StateTransition<E>, Delegate>();
        }
        #endregion


        #region Properties
        /// <summary>
        /// Get or the current state of the state machine.
        /// </summary>
        public E CurrentState
        {
            get { return this.currentState; }
        }


        /// <summary>
        /// Gets the last state held by the state machine.
        /// </summary>
        public E PreviousState
        {
            get { return this.previousState; }
        }
        #endregion


        #region Operators
        /// <summary>
        /// Implicit conversion to current state enumeration.
        /// </summary>
        /// <param name="finiteStateMachine"></param>
        public static implicit operator E(FiniteStateMachine<E> finiteStateMachine)
        {
            return finiteStateMachine.CurrentState;
        }
        #endregion


        /// <summary>
        /// Add a new state transition to the state machine.
        /// </summary>
        /// <param name="initialState">Beginning state of transition.</param>
        /// <param name="endState">Ending state of transition.</param>
        /// <param name="action">Callback action method to execute after transition.</param>
        public void AddTransition(E initialState, E endState, Action action)
        {
            StateTransition<E> transition = new StateTransition<E>(initialState, endState);

            // Check if transition already exists.
            if (this.stateTransitions.ContainsKey(transition))
            {
                // Transtion already exists.
                return;
            }

            // Add transition.
            this.stateTransitions.Add(transition, action);
        }


        /// <summary>
        /// Add a new state transition to the state machine.
        /// </summary>
        /// <param name="initialStates">Possible beginning states of transition.</param>
        /// <param name="endState">Ending state of transition.</param>
        /// <param name="action">Callback action method to execute after transition.</param>
        public void AddTransition(IEnumerable<E> initialStates, E endState, Action action)
        {
            foreach (E initialState in initialStates)
            {
                AddTransition(initialState, endState, action);
            }
        }


        /// <summary>
        /// Add a new state transition to the state machine.
        /// </summary>
        /// <param name="initialStates">Possible beginning states of transition.</param>
        /// <param name="endState">Ending state of transition.</param>
        /// <param name="action">Callback action method to execute after transition.</param>
        public void AddTransition(E initialState, IEnumerable<E> endStates, Action action)
        {
            foreach (E endState in endStates)
            {
                AddTransition(initialState, endState, action);
            }
        }


        /// <summary>
        /// Add a new state transition to the state machine for all possible starting states.
        /// </summary>
        /// <param name="endState">Ending state of transition.</param>
        /// <param name="action">Callback action method to execute after transition.</param>
        public void AddTransitionFromAnyState(E endState, Action action, bool disallowTransitionFromSelf = true)
        {
            IEnumerable<E> anyState = Enum.GetValues(typeof(E)).Cast<E>();
            if (disallowTransitionFromSelf)
                anyState = anyState.Where(state => !state.Equals(endState));
            AddTransition(anyState, endState, action);
        }


        /// <summary>
        /// Add a new state transition to the state machine for all possible ending states.
        /// </summary>
        /// <param name="initialState">Beginning state of transition.</param>
        /// <param name="action">Callback action method to execute after transition.</param>
        public void AddTransitionToAnyState(E initialState, Action action, bool disallowTransitionToSelf)
        {
            IEnumerable<E> anyState = Enum.GetValues(typeof(E)).Cast<E>();
            if (disallowTransitionToSelf)
                anyState = anyState.Where(state => !state.Equals(initialState));
            AddTransition(initialState, anyState, action);
        }


        /// <summary>
        /// Attempt to change state.
        /// State will only change if a transition exists for it,
        /// and the FiniteStateMachine is not locked.
        /// </summary>
        /// <param name="newState">State to change to.</param>
        /// <returns>True if state was changed, false if not.</returns>
        public bool ChangeState(E newState)
        {
            // Locked state, abort.
            if (this.locked)
                return false;

            // Advance to transition if valid, indicate success.
            StateTransition<E> transition = new StateTransition<E>(this.currentState, newState);
            Delegate transitionDelegate;
            if (this.stateTransitions.TryGetValue(transition, out transitionDelegate))
            {
                if (transitionDelegate != null)
                {
                    Action transitionAction = transitionDelegate as Action;
                    transitionAction();
                }

                this.previousState = this.currentState;
                this.currentState = newState;

                return true;
            }

            // Invalid transition, indicate no transition was made.
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Forces a state change, even if state transition doesn't exist.
        /// </summary>
        /// <param name="newState">State to change to.</param>
        public void ForceState(E newState)
        {
            this.currentState = newState;
        }


        /// <summary>
        /// Prevent state machine from changing states.
        /// </summary>
        public void Lock()
        {
            this.locked = true;
        }


        /// <summary>
        /// Allow state machine to change states.
        /// </summary>
        public void Unlock()
        {
            this.locked = false;
            ChangeState(this.previousState);
        }
    }
}