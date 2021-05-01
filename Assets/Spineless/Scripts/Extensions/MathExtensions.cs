namespace Spineless.Extensions.Integers
{
    /// <summary>
    /// Contains all extension methods of the Corgi Engine and Infinite Runner Engine.
    /// </summary>
    public static class IntegerExtensions
    {
        /// <summary>
        /// Returns the sum of all the int passed in parameters
        /// </summary>
        /// <param name="thingsToAdd">Things to add.</param>
        public static int Sum(params int[] thingsToAdd)
        {
            int result = 0;
            for (int i = 0; i < thingsToAdd.Length; i++)
            {
                result += thingsToAdd[i];
            }

            return result;
        }
    }
}