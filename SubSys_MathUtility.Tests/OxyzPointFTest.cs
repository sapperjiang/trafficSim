using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubSys_MathUtility;

namespace SubSys_MathUtility.Tests
{
    /// <summary>此类包含 OxyzPointF 的参数化单元测试</summary>
    [TestClass]
    [PexClass(typeof(OxyzPointF))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class OxyzPointFTest
    {
        [TestMethod()]
        public void test1()
        {
            OxyzPointF of = new OxyzPointF(0, 0, 0.00001);
            OxyzPointF two = new OxyzPointF(0, 0, 1);
            OxyzPointF three = new OxyzPointF(0, 0, 2);
            if (of==two)
            {
                Assert.Fail();
            }
            if (of == three)
            {
                Assert.Fail();
            }
            if (of != OxyzPointF.Default)
            {
                Assert.Fail();
            }

            //Assert.AreEqual(of, three);
        }
    }
}
