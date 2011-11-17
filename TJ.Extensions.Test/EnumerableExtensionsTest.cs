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
            comparer.By(y => y.StringProp).And(y => y.IntProp);
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
            comparer.By(y => y.StringProp).And(y => y.IntProp);
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
            comparer.By(y => y.StringProp).Or(y => y.IntProp);
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
            comparer.By(y => y.StringProp).And(y => y.IntProp);
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
            comparer.By(y => y.StringProp).Or(y => y.SimpleComplexProp.IntProp);
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
}
