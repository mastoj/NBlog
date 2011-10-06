using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJ.Mvc.Filter
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class AllowAnonymousAttribute : Attribute
    {
    }
}
