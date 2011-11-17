using System;
using System.Linq.Expressions;

namespace TJ.Extensions
{
    public static class EnumerableCompareResultExtensions
    {
        public static EnumerableCompareResult<T> By<T>(this EnumerableCompareResult<T> current, Expression<Func<T, dynamic>> propertyExpression)
        {
            current.AddExpression(propertyExpression);
            return current;
        }

        public static EnumerableCompareResult<T> And<T>(this EnumerableCompareResult<T> current, Expression<Func<T, dynamic>> propertyExpression)
        {
            current.AddExpression(propertyExpression);
            return current;
        }

        public static EnumerableCompareResult<T> Or<T>(this EnumerableCompareResult<T> current, Expression<Func<T, dynamic>> propertyExpression)
        {
            current.AddOrExpression(propertyExpression);
            return current;
        }
    }
}