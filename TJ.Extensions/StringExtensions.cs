using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJ.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }
    }
}
