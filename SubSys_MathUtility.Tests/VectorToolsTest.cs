using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubSys_MathUtility;

namespace SubSys_MathUtility.Tests
{
    /// <summary>此类包含 VectorTools 的参数化单元测试</summary>
    [TestClass]
    [PexClass(typeof(VectorTool))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class VectorToolsTest
    {
    }
}
