namespace Spineless.Extensions.Animators
{
    using UnityEngine;


    /// <summary>
    /// Various static methods used throughout the Infinite Runner Engine and the Corgi Engine.
    /// </summary>
    public static class AnimatorExtensions
    {
        /// <summary>
        /// Determines if an animator contains a certain parameter, based on a type and a name
        /// </summary>
        /// <returns><c>true</c> if has parameter of type the specified animator name type; otherwise, <c>false</c>.</returns>
        /// <param name="animator">Self.</param>
        /// <param name="name">Name.</param>
        /// <param name="type">Type.</param>
        public static bool HasParameterOfType(this Animator animator, string name, AnimatorControllerParameterType type)
        {
            for (int i = 0; i < animator.parameters.Length; i++)
            {
                AnimatorControllerParameter currParam = animator.parameters[i];
                if (currParam.type == type
                    && currParam.name == name)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Triggers an animator trigger.
        /// </summary>
        /// <param name="animator">Animator.</param>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        public static void SetAnimatorTrigger(this Animator animator, string parameterName)
        {
            if (animator.HasParameterOfType(parameterName, AnimatorControllerParameterType.Trigger))
                animator.SetTrigger(parameterName);
        }


        /// <summary>
        /// Updates the animator bool.
        /// </summary>
        /// <param name="animator">Animator.</param>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        public static void UpdateAnimatorBool(this Animator animator, string parameterName, bool value)
        {
            if (animator.HasParameterOfType(parameterName, AnimatorControllerParameterType.Bool))
                animator.SetBool(parameterName, value);
        }


        /// <summary>
        /// Updates the animator float.
        /// </summary>
        /// <param name="animator">Animator.</param>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="value">Value.</param>
        public static void UpdateAnimatorFloat(this Animator animator, string parameterName, float value)
        {
            if (animator.HasParameterOfType(parameterName, AnimatorControllerParameterType.Float))
                animator.SetFloat(parameterName, value);
        }


        /// <summary>
        /// Updates the animator integer.
        /// </summary>
        /// <param name="animator">Animator.</param>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="value">Value.</param>
        public static void UpdateAnimatorInteger(this Animator animator, string parameterName, int value)
        {
            if (animator.HasParameterOfType(parameterName, AnimatorControllerParameterType.Int))
                animator.SetInteger(parameterName, value);
        }


        public static void UpdateAnimatorTrigger(Animator animator, string parameterName)
        {
            if (animator.HasParameterOfType(parameterName, AnimatorControllerParameterType.Trigger))
                animator.SetTrigger(parameterName);
        }
    }
}