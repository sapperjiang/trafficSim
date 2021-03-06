// <copyright file="LineToolsTest.cs" company="China">Copyright © China 2012</copyright>
using System;
using System.Drawing;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubSys_MathUtility;

namespace SubSys_MathUtility.Tests
{
    /// <summary>此类包含 LineTools 的参数化单元测试</summary>
    [PexClass(typeof(LineTools))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class LineToolsTest
    {
        /// <summary>测试 GetIntersection(PointF, PointF, PointF, PointF) 的存根</summary>
        [PexMethod]
        public PointF GetIntersectionTest(
            PointF lineFirstStar,
            PointF lineFirstEnd,
            PointF lineSecondStar,
            PointF lineSecondEnd
        )
        {
            PointF result = LineTools.GetIntersection
                                (lineFirstStar, lineFirstEnd, lineSecondStar, lineSecondEnd);
            return result;
            // TODO: 将断言添加到 方法 LineToolsTest.GetIntersectionTest(PointF, PointF, PointF, PointF)
        }
    }
}
