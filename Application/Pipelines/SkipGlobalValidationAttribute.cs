using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Pipelines
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    //[AttributeUsage(AttributeTargets.Property)]
    //public class SkipGlobalValidationAttribute : Attribute
    //{
    //}


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class SkipGlobalValidationAttribute : Attribute { }
}
