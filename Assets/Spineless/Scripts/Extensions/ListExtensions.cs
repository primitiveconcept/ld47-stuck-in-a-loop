using System;
using System.Collections.Generic;
using System.Linq;


namespace Spineless.Extensions.Lists
{
    public static class ListExtensions
    {
        /// <summary>
        /// Clone a List, so entries are copies rather than references.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listToClone"></param>
        /// <returns>New List with cloned entries.</returns>
        public static IList<T> Clone<T>(this IList<T> listToClone)
            where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}