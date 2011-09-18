using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJ.Extensions
{
    public static class BoolExtensions
    {
        public static bool IsTrue(this bool valueToCheck)
        {
            return valueToCheck;
        }
        public static bool IsFalse(this bool valueToCheck)
        {
            return !valueToCheck;
        }
    }
}
