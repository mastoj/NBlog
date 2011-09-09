using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TJ.Extensions.Test
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
        [Test]
        public void IsNull_ShouldReturnTrue_ForNull()
        {
            object obj = null;
            Assert.IsTrue(obj.IsNull());
        }

        [Test]
        public void IsNull_ShouldReturnFalse_ForNotNull()
        {
            object obj = new object();
            Assert.IsFalse(obj.IsNull());
        }
    }
}
