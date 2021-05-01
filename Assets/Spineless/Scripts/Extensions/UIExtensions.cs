namespace Spineless.Extensions.UI
{
    using UnityEngine;


    public static class UIExtensions
    {
        public static bool ContainsRect(this Canvas canvas, Transform transform)
        {
            var position = RectTransformUtility.PixelAdjustPoint(transform.position, transform, canvas);
            return canvas.pixelRect.Contains(position);
        }
    }
}