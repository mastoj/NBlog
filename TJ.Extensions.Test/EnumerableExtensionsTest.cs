using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace TJ.Extensions.Test
{
    [TestFixture]
    public class EnumerableExtensionsTest
    {
        [Test]
        public void SimpleTest_AreEqual()
        {
            // arrange
            var list = CreateSimpleList(Enumerable.Range(1, 10));
            var listToCompare = CreateSimpleList(Enumerable.Range(1, 10));

            // act
            var comparer = list.Comparer(listToCompare);
            comparer.AddExpression(y => y.StringProp);
            comparer.AddExpression(y => y.IntProp);
            var areEqual = comparer.AreEqual();

            // assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void SimpleTest_AreNotEqual()
        {
            // arrange
            var list = CreateSimpleList(Enumerable.Range(1, 10));
            var listToCompare = CreateSimpleList(Enumerable.Range(1, 11));

            // act
            var comparer = list.Comparer(listToCompare);
            comparer.AddExpression(y => y.StringProp);
            comparer.AddExpression(y => y.IntProp);
            var areEqual = comparer.AreEqual();

            // assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void SimpleTest_OrShouldBeEqual()
        {
            // arrange
            var list = CreateSimpleList(Enumerable.Range(1, 10), setStringFunc: y => "bla");
            var listToCompare = CreateSimpleList(Enumerable.Range(1, 10));

            // act
            var comparer = list.Comparer(listToCompare);
            comparer.AddExpression(y => y.StringProp);
            comparer.AddOrExpression(y => y.IntProp);
            var areEqual = comparer.AreEqual();

            // assert
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void SimpleTest_AndShouldNotBeEqualIfStringIsNotMatching()
        {
            // arrange
            var list = CreateSimpleList(Enumerable.Range(1, 10), setStringFunc: y => "bla");
            var listToCompare = CreateSimpleList(Enumerable.Range(1, 10));

            // act
            var comparer = list.Comparer(listToCompare);
            comparer.AddExpression(y => y.StringProp);
            comparer.AddExpression(y => y.IntProp);
            var areEqual = comparer.AreEqual();

            // assert
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void SimpleTest_AndShouldBeEqualForNestedPropertyAccess()
        {
            // arrange
            var list = CreateComplexList(Enumerable.Range(1, 10), setStringFunc: y => "bla");
            var listToCompare = CreateComplexList(Enumerable.Range(1, 10));

            // act
            var comparer = list.Comparer(listToCompare);
            comparer.AddExpression(y => y.StringProp);
            comparer.AddOrExpression(y => y.SimpleComplexProp.IntProp);
            var areEqual = comparer.AreEqual();

            // assert
            Assert.IsTrue(areEqual);
        }

        private IEnumerable<Simple> CreateSimpleList(IEnumerable<int> range, Func<int, string> setStringFunc = null, Func<int, int> setIntFunc = null)
        {
            setStringFunc = setStringFunc ?? ((y) => "stringprop" + y.ToString());
            setIntFunc = setIntFunc ?? ((y) => y);
            foreach (var i in range)
            {
                yield return new Simple() {StringProp = setStringFunc(i), IntProp = setIntFunc(i)};
            }
        }

        private IEnumerable<Complex>  CreateComplexList(IEnumerable<int> range, Func<int, string> setStringFunc = null, Func<int, Simple> setSimpleFunc = null)
        {
            setStringFunc = setStringFunc ?? ((y) => "stringprop" + y.ToString());
            setSimpleFunc = setSimpleFunc ?? ((y) => new Simple() { StringProp = "strrr", IntProp = y});
            foreach (var i in range)
            {
                yield return new Complex { StringProp = setStringFunc(i), SimpleComplexProp = setSimpleFunc(i) };
            }
        }
    }

    public class Simple
    {
        public string StringProp { get; set; }
        public int IntProp { get; set; }
    }

    public class Complex
    {
        public string StringProp { get; set; }
        public Simple SimpleComplexProp { get; set; }
    }

    public static class EnumerableComparer
    {
        public static EnumerableCompareResult<T> Comparer<T>(this IEnumerable<T> list, IEnumerable<T> compareToList)
        {
            return new EnumerableCompareResult<T>(list, compareToList);
        }
    }

    public class EnumerableCompareResult<T>
    {
        private readonly IEnumerable<T> _list;
        private readonly IEnumerable<T> _compareToList;
        private readonly List<List<Expression<Func<T, dynamic>>>> _filter;
        private List<Expression<Func<T, dynamic>>> _currentClause;

        public EnumerableCompareResult(IEnumerable<T> list, IEnumerable<T> compareToList)
        {
            _list = list;
            _compareToList = compareToList;
            _filter = new List<List<Expression<Func<T, dynamic>>>>();
            _currentClause = new List<Expression<Func<T, dynamic>>>();
            _filter.Add(_currentClause);
        }

        public void AddExpression(Expression<Func<T, dynamic>> propertyExpression)
        {
            _currentClause.Add(propertyExpression);
        }

        public void AddOrExpression(Expression<Func<T, dynamic>> propertyExpression)
        {
            _currentClause = new List<Expression<Func<T, dynamic>>>();
            _filter.Add(_currentClause);
            AddExpression(propertyExpression);
        }

        public bool AreEqual()
        {
            var listCount = _list.Count();
            if (listCount != _compareToList.Count())
            {
                return false;
            }
            var orExpressions = BuildOrExpressions();
            var equalList = _list.Where(y => _compareToList.Any(x => orExpressions.Any(z => z(y, x))));
            var areEqual = equalList.Count() == listCount;
            return areEqual;
        }

        private List<Func<T, T, bool>> BuildOrExpressions()
        {
            var orListExpressions = new List<List<Func<T, T, bool>>>();
            foreach (var filterList in _filter)
            {
                var listExpression = new List<Func<T, T, bool>>();
                foreach (var expression in filterList)
                {
                    var lambda = expression.Compile();
                    Func<T, T, bool> orFunc = (y, x) =>
                                                  {
                                                      var propertyValue1 = lambda(y);
                                                      var propertyValue2 = lambda(x);
                                                      return Equals(propertyValue1, propertyValue2);
                                                  };
                    listExpression.Add(orFunc);
                }
                orListExpressions.Add(listExpression);
            }
            var orExpressions = new List<Func<T, T, bool>>();
            foreach (var orListExpression in orListExpressions)
            {
                Func<T, T, bool> orExpression = (y, x) => orListExpression.All(z => z(y, x));
                orExpressions.Add(orExpression);
            }
            return orExpressions;
        }

        //private PropertyInfo GetPropertyInfo(Expression<Func<T, dynamic>> expression)
        //{
        //    var memberExpression = expression.Body as MemberExpression;
        //    if (memberExpression.IsNull())
        //    {
        //        memberExpression = ((UnaryExpression) expression.Body).Operand as MemberExpression;
        //    }
        //    var propertyInfo = memberExpression.Member as PropertyInfo;
        //    return propertyInfo;
        //}

        public IEnumerable<T> Diff()
        {
            throw new NotImplementedException();
        }
    }

    static class EnumerableCompareResultExtensions
    {
        static EnumerableCompareResult<T> By<T>(this EnumerableCompareResult<T> current, Expression<Func<T, dynamic>> propertyExpression)
        {
            //            var bla = propertyExpression.
            current.AddExpression(propertyExpression);
            return current;
        }

        static EnumerableCompareResult<T> And<T>(this EnumerableCompareResult<T> current, Expression<Func<T, dynamic>> propertyExpression)
        {
            //            current.AddExpression();
            return current;
        }

        static EnumerableCompareResult<T> Or<T>(this EnumerableCompareResult<T> current, Expression<Func<T, dynamic>> propertyExpression)
        {
            //            var bla = propertyExpression.
            return current;
        }
    }

}
