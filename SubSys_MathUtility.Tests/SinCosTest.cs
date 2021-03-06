using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubSys_MathUtility;

namespace SubSys_MathUtility.Tests
{
    /// <summary>此类包含 SinCos 的参数化单元测试</summary>
    [TestClass]
    [PexClass(typeof(SinCos))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class SinCosTest
    {
    }
}
