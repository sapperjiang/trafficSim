using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubSys_MathUtility;

namespace SubSys_MathUtility.Tests
{
    /// <summary>此类包含 Coordinates 的参数化单元测试</summary>
    [TestClass]
    [PexClass(typeof(Coordinates))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class CoordinatesTest
    {
        [TestMethod()]
        public void ProjectTest()
        {
      //      Assert.Fail();
        }

        [TestMethod()]
        public void OffsetTest()
        {
       //     Assert.Fail();
        }

        [TestMethod()]
        public void OffsetTest1()
        {
      //      Assert.Fail();
        }

        [TestMethod()]
        public void OffsetTest2()
        {
      //      Assert.Fail();
        }

        [TestMethod()]
        public void OffsetTest3()
        {
      //      Assert.Fail();
        }

        [TestMethod()]
        public void RotateTest()
        {
     //       Assert.Fail();
        }

        [TestMethod()]
        public void OffsetTest4()
        {
     //       Assert.Fail();
        }

        [TestMethod()]
        public void OffsetTest5()
        {
  //          Assert.Fail();
        }

        [TestMethod()]
        public void DistanceTest()
        {
   //         Assert.Fail();
        }

        /// <summary>测试 Project(OxyzPointF, Int32) 的存根</summary>
        [PexMethod]
        public OxyzPointF ProjectTest(OxyzPointF mp, int iScaleFactor)
        {
            OxyzPointF result = Coordinates.Project(mp, iScaleFactor);
            return result;
            // TODO: 将断言添加到 方法 CoordinatesTest.ProjectTest(OxyzPointF, Int32)
        }
    }
}
