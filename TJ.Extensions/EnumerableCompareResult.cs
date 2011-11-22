using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TJ.Extensions
{
    public class EnumerableCompareResult<T>
    {
        private readonly IEnumerable<T> _list;
        private readonly IEnumerable<T> _compareToList;
        private readonly List<List<Func<T, dynamic>>> _filter;
        private List<Func<T, dynamic>> _currentClause;

        public EnumerableCompareResult(IEnumerable<T> list, IEnumerable<T> compareToList)
        {
            _list = list;
            _compareToList = compareToList;
            _filter = new List<List<Func<T, dynamic>>>();
            _currentClause = new List<Func<T, dynamic>>();
            _filter.Add(_currentClause);
        }

        internal void AddExpression(Func<T, dynamic> propertyExpression)
        {
            _currentClause.Add(propertyExpression);
        }

        internal void AddOrExpression(Func<T, dynamic> propertyExpression)
        {
            _currentClause = new List<Func<T, dynamic>>();
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
                    Func<T, dynamic> localExpression = expression;
                    Func<T, T, bool> andFunc = (y, x) =>
                                                  {
                                                      var propertyValue1 = localExpression(y);
                                                      var propertyValue2 = localExpression(x);
                                                      return Equals(propertyValue1, propertyValue2);
                                                  };
                    listExpression.Add(andFunc);
                }
                orListExpressions.Add(listExpression);
            }
            var orExpressions = new List<Func<T, T, bool>>();
            foreach (var orListExpression in orListExpressions)
            {
                List<Func<T, T, bool>> expression = orListExpression;
                Func<T, T, bool> orExpression = (y, x) => expression.All(z => z(y, x));
                orExpressions.Add(orExpression);
            }
            return orExpressions;
        }

        public IEnumerable<T> Diff()
        {
            throw new NotImplementedException();
        }
    }
}