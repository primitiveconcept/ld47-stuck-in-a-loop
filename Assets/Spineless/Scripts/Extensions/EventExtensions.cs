using System;
using UnityEngine;


namespace Spineless.Extensions.Events
{
    /// <summary>
    /// Extension methods for various event delegate types.
    /// </summary>
    public static class EventExtensions
    {
        #region Actions
        /// <summary>
        /// Raise an Action event.
        /// </summary>
        /// <param name="action"></param>
        public static void Raise(
            this Action action,
            string log = null)
        {
            if (log != null)
                Debug.Log(log);
            if (action != null)
                action();
        }


        /// <summary>
        /// Raise an Action event with an argument.
        /// </summary>
        /// <typeparam name="T">Action argument type.</typeparam>
        /// <param name="action"></param>
        /// <param name="arg">Action argument.</param>
        public static void Raise<T>(
            this Action<T> action,
            T arg,
            string log = null)
        {
            if (log != null)
                Debug.Log(log);
            if (action != null)
                action(arg);
        }


        /// <summary>
        /// Raise an Action event with arguments.
        /// </summary>
        /// <typeparam name="T1">First Action argument type.</typeparam>
        /// <typeparam name="T2">Second Action argument type.</typeparam>
        /// <param name="action"></param>
        /// <param name="arg1">Action argument.</param>
        /// <param name="arg2">Action argument.</param>
        public static void Raise<T1, T2>(
            this Action<T1,
                T2> action,
            T1 arg1,
            T2 arg2,
            string log = null)
        {
            if (log != null)
                Debug.Log(log);
            if (action != null)
                action(arg1, arg2);
        }


        /// <summary>
        /// Raise an Action event with arguments.
        /// </summary>
        /// <typeparam name="T1">First Action argument type.</typeparam>
        /// <typeparam name="T2">Second Action argument type.</typeparam>
        /// <typeparam name="T3">Third Action argument type.</typeparam>
        /// <param name="action"></param>
        /// <param name="arg1">Action argument.</param>
        /// <param name="arg2">Action argument.</param>
        /// <param name="arg3">Action argument.</param>
        public static void Raise<T1, T2, T3>(
            this Action<T1, T2, T3> action,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            string log = null)
        {
            if (action != null)
                action(arg1, arg2, arg3);
            if (log != null)
                Debug.Log(log);
        }


        /// <summary>
        /// Raise an Action event with arguments.
        /// </summary>
        /// <typeparam name="T1">First Action argument type.</typeparam>
        /// <typeparam name="T2">Second Action argument type.</typeparam>
        /// <typeparam name="T3">Third Action argument type.</typeparam>
        /// <typeparam name="T4">Fourth Action argument type.</typeparam>
        /// <param name="action"></param>
        /// <param name="arg1">Action argument.</param>
        /// <param name="arg2">Action argument.</param>
        /// <param name="arg3">Action argument.</param>
        /// <param name="arg4">Action argument.</param>
        public static void Raise<T1, T2, T3, T4>(
            this Action<T1, T2, T3, T4> action,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            string log = null)
        {
            if (log != null)
                Debug.Log(log);
            if (action != null)
                action(arg1, arg2, arg3, arg4);
        }
        #endregion


        #region Event Handler
        /// <summary>
        /// Raise an EventHandler event.
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <param name="sender">Object raising event.</param>
        /// <param name="e">Event arguments.</param>
        public static void Raise(
            this EventHandler eventHandler,
            object sender,
            EventArgs e,
            string log = null)
        {
            if (log != null)
                Debug.Log(log);
            if (eventHandler != null)
                eventHandler(sender, e);
        }


        /// <summary>
        /// Raise an EventHandler event.
        /// </summary>
        /// <typeparam name="TEventArgs">Type of event arguments.</typeparam>
        /// <param name="eventHandler"></param>
        /// <param name="sender">Object raising event.</param>
        /// <param name="e">Event arguments.</param>
        public static void Raise<TEventArgs>(
            this EventHandler<TEventArgs> eventHandler,
            object sender,
            TEventArgs e,
            string log = null)
            where TEventArgs : EventArgs
        {
            if (log != null)
                Debug.Log(log);
            if (eventHandler != null)
                eventHandler(sender, e);
        }
        #endregion
    }
}