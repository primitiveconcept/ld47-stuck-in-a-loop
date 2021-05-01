namespace Spineless
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using Random = System.Random;


    public class Coroutines : MonoBehaviour
    {
        private static Coroutines _instance;


        #region Properties
        public static Coroutines Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("Global Coroutines").AddComponent<Coroutines>();
                    _instance.gameObject.hideFlags = HideFlags.HideInHierarchy;
                }

                return _instance;
            }
        }
        #endregion

        
        /// <summary>
        /// Start a coroutine from global coroutine GameObject instance.
        /// Use when a Coroutine's executions shouldn't be tied to a specific object.
        /// </summary>
        /// <param name="routine">Coroutine to run.</param>
        /// <returns>Coroutine</returns>
        public static Coroutine StartGlobal(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }


        /// <summary>
        /// Fades the specified image to the target opacity and duration.
        /// </summary>
        /// <param name="target">Target Image.</param>
        /// <param name="color">Color to ease to.</param>
        /// <param name="duration">Duration over which to ease.</param>
        public static Coroutine EaseImageColor(
            Image target,
            Color color,
            float duration,
            Action callback = null)
        {
            return Instance.StartCoroutine(EaseImageColorCoroutine(target, color, duration, callback));
        }
        
        
        /// <summary>
        /// Fades the specified sprite to the target opacity and duration.
        /// </summary>
        /// <param name="target">Target SpriteRenderer.</param>
        /// <param name="color">Color to ease to.</param>
        /// <param name="duration">Duration over which to ease.</param>
        public static Coroutine EaseSpriteColor(
            SpriteRenderer target,
            Color color,
            float duration,
            Action callback = null)
        {
            return Instance.StartCoroutine(EaseSpriteColorCoroutine(target, color, duration, callback));
        }


        /// <summary>
        /// Shake the specified transform for some duration.
        /// </summary>
        /// <param name="transform">Transform to shake.</param>
        /// <param name="duration">Total shake duration.</param>
        /// <param name="magnitude">Amount of shake.</param>
        /// <param name="decayRate">How quickly to reduce shake amount.</param>
        public static Coroutine ShakeTransform(
            Transform transform,
            float duration,
            float magnitude = 0.7f,
            Action callback = null)
        {
            return Instance.StartCoroutine(ShakeCoroutine(transform, duration, magnitude, callback));
        }
        

        public static IEnumerator WaitForSeconds(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            if (callback != null)
                callback();
        }


        /// <summary>
        /// Fades the specified image to the target opacity and duration.
        /// </summary>
        /// <param name="target">Target.</param>
        /// <param name="duration">Duration.</param>
        /// <param name="color">Target color.</param>
        /// <param name="callback">Optional callback.</param>
        public static IEnumerator FadeImage(Image target, float duration, Color color, Action callback = null)
        {
            if (target == null)
                yield break;

            float alpha = target.color.a;

            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
            {
                if (target == null)
                    yield break;
                Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
                target.color = newColor;
                yield return null;
            }

            target.color = color;
            if (callback != null)
                callback();
        }


        public static IEnumerator FadeImageInOut(Image target, float fadeTime, float stayTime)
        {
            target.color = Color.clear;
            yield return FadeImage(target, fadeTime, Color.white);
            yield return new WaitForSeconds(stayTime);
            yield return FadeImage(target, fadeTime, Color.clear);
        }


        public static IEnumerator FlickerRenderers(Renderer[] renderers, float duration, float speed)
        {
            int iterations = (int)((duration / 3) / speed);
            for (int i = 0; i < iterations; i++)
            {
                foreach (Renderer renderer in renderers)
                {
                    renderer.enabled = false;
                }

                yield return new WaitForSeconds(speed);
                foreach (Renderer renderer in renderers)
                {
                    renderer.enabled = true;
                }

                yield return new WaitForSeconds(speed);
            }
        }


        public static IEnumerator OnNextFrame(Action callback)
        {
            yield return null;
            if (callback != null)
                callback();
        }
        

        private static IEnumerator EaseImageColorCoroutine(
            Image target,
            Color color,
            float duration,
            Action callback = null)
        {
            if (target == null)
                yield break;

            float red = target.color.r;
            float green = target.color.g;
            float blue = target.color.b;
            float alpha = target.color.a;

            for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / duration))
            {
                if (target == null)
                    yield break;
                Color easedColor = new Color(
                    Mathf.SmoothStep(red, color.r, t),
                    Mathf.SmoothStep(green, color.g, t),
                    Mathf.SmoothStep(blue, color.b, t),
                    Mathf.SmoothStep(alpha, color.a, t));
                target.color = easedColor;
                yield return null;
            }
            target.color = color;

            if (callback != null)
                callback();
        }
        
        
        private static IEnumerator EaseSpriteColorCoroutine(
            SpriteRenderer target,
            Color color,
            float duration,
            Action callback = null)
        {
            if (target == null)
                yield break;

            float red = target.color.r;
            float green = target.color.g;
            float blue = target.color.b;
            float alpha = target.color.a;

            for (float t = 0.0f; t < 1.0f; t += (Time.deltaTime / duration))
            {
                if (target == null)
                    yield break;
                Color easedColor = new Color(
                    Mathf.SmoothStep(red, color.r, t),
                    Mathf.SmoothStep(green, color.g, t),
                    Mathf.SmoothStep(blue, color.b, t),
                    Mathf.SmoothStep(alpha, color.a, t));
                target.color = easedColor;
                yield return null;
            }
            target.color = color;

            if (callback != null)
                callback();
        }


        private static IEnumerator ShakeCoroutine(
            Transform transform,
            float duration,
            float magnitude = 0.7f,
            Action callback = null)
        {
            Vector3 originalPosition = transform.localPosition;
            float counter = duration;

            while (counter > 0)
            {
                counter -= Time.deltaTime;
                float shakeAdjustment = counter / duration;
                transform.localPosition =
                    originalPosition 
                    + UnityEngine.Random.insideUnitSphere * (magnitude * shakeAdjustment);
                yield return null;
            }

            transform.localPosition = originalPosition;
            if (callback != null)
                callback();
        }
        
    }
}