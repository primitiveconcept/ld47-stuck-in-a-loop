namespace Spineless.Extensions.Objects
{
    using System;
    using System.Reflection;
    using UnityEngine;


    public static class ObjectExtensions
    {
        public static Action GetMethod(this object obj, string methodName, params object[] parameters)
        {
            MethodInfo method = obj.GetType().GetMethod(methodName);
            if (method == null)
            {
                Debug.Log("No \"" + methodName + "\" method found on " + obj.GetType().Name + " to invoke.");
                return null;
            }

            return (Action)Delegate.CreateDelegate(typeof(Action), obj, method);
        }
    }
}