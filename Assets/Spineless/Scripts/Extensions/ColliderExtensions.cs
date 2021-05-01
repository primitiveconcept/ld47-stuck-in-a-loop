namespace Spineless.Extensions.Colliders
{
    using UnityEngine;


    public static class ColliderExtensions
    {
        /// <summary>
        /// Return the coordinate of the collider's bottom-middle edge.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns>X/Y coordinate.</returns>
        public static Vector2 GetBottomCenterXY(this Collider2D collider)
        {
            return new Vector2(collider.bounds.center.x, collider.GetBottomY());
        }


        /// <summary>
        /// Return the coordinate of the collider's bottom-left corner.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns>X/Y coordinate.</returns>
        public static Vector2 GetBottomLeftXY(this Collider2D collider)
        {
            return new Vector2(collider.GetLeftX(), collider.GetBottomY());
        }


        /// <summary>
        /// Return the coordinate of the collider's bottom-right corner.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns>X/Y coordinate.</returns>
        public static Vector2 GetBottomRightXY(this Collider2D collider)
        {
            return new Vector2(collider.GetRightX(), collider.GetBottomY());
        }


        /// <summary>
        /// Return the Y coordinate of the bottom edge of Bounds.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns>Y coordinate.</returns>
        public static float GetBottomY(this Bounds bounds)
        {
            return bounds.center.y - bounds.extents.y;
        }


        /// <summary>
        /// Return the Y coordinate of the bottom edge of a Collider2D.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns>Y coordinate.</returns>
        public static float GetBottomY(this Collider2D collider)
        {
            return collider.GetCenterXY().y - collider.bounds.extents.y;
        }


        /// <summary>
        /// Returns the center of a Collider2D.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns>Center as Vector2.</returns>
        public static Vector2 GetCenterXY(this Collider2D collider)
        {
            return collider.bounds.center;
        }


        /// <summary>
        /// Return the height of Bounds.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns>Height of Bounds.</returns>
        public static float GetHeight(this Bounds bounds)
        {
            return bounds.size.y;
        }


        /// <summary>
        /// Return the height of the collider.
        /// </summary>
        /// <param name="boxCollider"></param>
        /// <returns>Height of collider.</returns>
        public static float GetHeight(this Collider2D boxCollider)
        {
            return boxCollider.bounds.size.y;
        }


        /// <summary>
        /// Return the X coordinate of the left edge of a Collider2D.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns>X coordinate.</returns>
        public static float GetLeftX(this Collider2D collider)
        {
            return collider.GetCenterXY().x - collider.bounds.extents.x;
        }


        /// <summary>
        /// Return the X coordinate of the right edge of a Collider2D.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns>X coordinate.</returns>
        public static float GetRightX(this Collider2D collider)
        {
            return collider.GetCenterXY().x + collider.bounds.extents.x;
        }


        /// <summary>
        /// Return the coordinate of the collider's top-middle edge.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns>X/Y coordinate.</returns>
        public static Vector2 GetTopCenterXY(this Collider2D collider)
        {
            return new Vector2(collider.bounds.center.x, collider.GetTopY());
        }


        /// <summary>
        /// Return the coordinate of the collider's top-left corner.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns>X/Y coordinate.</returns>
        public static Vector2 GetTopLeftXY(this Collider2D collider)
        {
            return new Vector2(collider.GetLeftX(), collider.GetTopY());
        }


        /// <summary>
        /// Return the coordinate of the collider's top-right corner.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns>X/Y coordinate.</returns>
        public static Vector2 GetTopRightXY(this Collider2D collider)
        {
            return new Vector2(collider.GetRightX(), collider.GetTopY());
        }


        /// <summary>
        /// Return the Y coordinate of the top edge of a Collider2D.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns>Y coordinate.</returns>
        public static float GetTopY(this Collider2D collider)
        {
            return collider.GetCenterXY().y + collider.bounds.extents.y;
        }


        /// <summary>
        /// Return the width of Bounds.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns>Width of bounds.</returns>
        public static float GetWidth(this Bounds bounds)
        {
            return bounds.size.x;
        }


        /// <summary>
        /// Return the width of the collider.
        /// </summary>
        /// <param name="boxCollider"></param>
        /// <returns>Width of collider.</returns>
        public static float GetWidth(this Collider2D boxCollider)
        {
            return boxCollider.bounds.size.x;
        }
    }
}