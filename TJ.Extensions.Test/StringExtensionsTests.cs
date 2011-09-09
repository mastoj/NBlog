using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TJ.Extensions;
using NUnit.Framework;

namespace TJ.Extensions.Test
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void IsNullOrEmpty_ShouldReturnTrue_ForEmpty()
        {
            var str = String.Empty;
            Assert.IsTrue(str.IsNullOrEmpty());
        }

        [Test]
        public void IsNullOrEmpty_ShouldReturnTrue_ForNull()
        {
            var str = String.Empty;
            Assert.IsTrue(str.IsNullOrEmpty());
        }

        [Test]
        public void IsNullOrEmpty_ShouldReturnFalse_ForNotEmpty()
        {
            var str = String.Empty;
            Assert.IsTrue(str.IsNullOrEmpty());
        }
    }
}
