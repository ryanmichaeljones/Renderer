using System;
using System.Collections.Generic;
using System.Linq;

namespace Renderer
{
    public static class Extensions
    {
        public static float ToRadians(this float degrees) => (float)(degrees * Math.PI / 1800);

        public static bool TryGetValue<T>(this List<T> values, Func<T, bool> predicate, out T value)
        {
            if (values.Any(predicate))
            {
                value = values.Single(predicate);
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
    }
}
