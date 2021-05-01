namespace Spineless.Extensions.GameObjects
{
    using UnityEngine;


    public static class GameObjectExtensions
    {
        /// <summary>
        /// Get hierarchy path of a gameobject as a string.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="includeRoot">Whether the root of the hierarchy should be included in path (Default: True).</param>
        /// <returns></returns>
        public static string GetHierarchyPath(this GameObject gameObject, bool includeRoot = false)
        {
            string path = "/" + gameObject.name;
            while (gameObject.transform.parent != null)
            {
                gameObject = gameObject.transform.parent.gameObject;

                if (gameObject.transform.parent != null
                    || includeRoot)
                {
                    path = "/" + gameObject.name + path;
                }
            }

            // Remove leading separator character.
            path = path.Remove(0, 1);

            return path;
        }
    }
}