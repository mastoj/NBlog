using System.Collections.Generic;
using System.Text;

namespace TJ.Extensions
{
    public static class EnumerableComparer
    {
        public static EnumerableCompareResult<T> Comparer<T>(this IEnumerable<T> list, IEnumerable<T> compareToList)
        {
            return new EnumerableCompareResult<T>(list, compareToList);
        }
    }
}
