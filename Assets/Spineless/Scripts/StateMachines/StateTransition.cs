namespace Spineless
{
    using System;


    /// <summary>
    /// StateTransition class
    /// </summary>
    /// <typeparam name="E">Enumeration of possible states.</typeparam>
    public class StateTransition<E> : IEquatable<StateTransition<E>>
    {
        #region Constructors
        /// <summary>
        /// Instantiates a new instance of the StateTransition class.
        /// </summary>
        public StateTransition()
        {
        }


        /// <summary>
        /// Instantiates a new instance of the StateTransition class.
        /// </summary>
        /// <param name="initialState">Starting state.</param>
        /// <param name="endState">Ending state.</param>
        public StateTransition(E initialState, E endState)
        {
            // Null check.
            if (initialState == null
                || endState == null)
                throw new ArgumentNullException("States in a StateTransition must not be null.");

            this.initialState = initialState;
            this.endState = endState;
        }
        #endregion


        #region Properties
        /// <summary>
        /// Get the ending state of this StateTransition.
        /// </summary>
        public E EndState
        {
            get { return this.endState; }
        }


        /// <summary>
        /// Get the initial state of this StateTransition.
        /// </summary>
        public E InitialState
        {
            get { return this.initialState; }
        }
        #endregion


        /// <summary>
        /// Check if this StateTransition is the same as another.
        /// </summary>
        /// <param name="stateTransition">StateTransition to check against.</param>
        /// <returns>true if StateTransitions are the same.</returns>
        public bool Equals(StateTransition<E> stateTransition)
        {
            if (ReferenceEquals(stateTransition, this))
            {
                return true;
            }

            else if (ReferenceEquals(stateTransition, null))
            {
                throw new ArgumentNullException("stateTransition argument must not be null.");
            }

            else
            {
                bool isEqual =
                    this.initialState.Equals(stateTransition.InitialState) &&
                    this.endState.Equals(stateTransition.EndState);
                return isEqual;
            }
        }


        /// <summary>
        /// Gets the hash number of this state transition.
        /// </summary>
        /// <returns>int hash number.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + this.initialState.GetHashCode();
                hash = hash * 31 + this.endState.GetHashCode();
                return hash;
            }
        }


        #region Fields
        protected E initialState;
        protected E endState;
        #endregion
    }
}