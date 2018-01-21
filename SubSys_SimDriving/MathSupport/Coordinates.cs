using System;
using System.Drawing;
using SubSys_SimDriving.SysSimContext;

namespace SubSys_SimDriving.MathSupport
{
    /// <summary>
    /// 内部路段的相对坐标系统转化绝对元胞坐标系系统
    /// </summary>
    internal static class Coordinates
    {
        /// <summary>
        /// 输入新坐标系的点，返回旧坐标系的点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="a"></param>
        internal static Point Rotate(Point mpNew,SinCos a)
        {
            Point mp=new Point(0,0);
            mp.X = mpNew.X * a.iCos - mpNew.Y * a.iSin;
            mp.Y = mpNew.X * a.iSin + mpNew.Y * a.iCos;
            return mp;
        }

        /// <summary>
        /// 坐标系平移
        /// </summary>
        /// <param name="old"></param>
        /// <param name="panVector">平移向量</param>
        internal static void Offset(ref Point old,Point panVector)
        {
            old.Y += panVector.Y;
            old.X += panVector.X;
        }
        internal static void OffsetYAxis(ref Point old, int iYSpan)
        {
            Point panVector = new Point(0, -iYSpan);
            Coordinates.Offset(ref old,panVector);
        }

        /// <summary>
        /// 相对元胞坐标系转换到绝对坐标系,平移，然后旋转变换
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        internal static Point RltXYToRealXY(ref Point point,MyPoint vPoiner)
        {
            //坐标系平移变换,将中心x和底部y坐标系平移到中心坐标系
            Coordinates.OffsetYAxis(ref point, SimSettings.iMaxLanes);
            Point indexOld = Coordinates.GetRealXY(point, vPoiner);
            
            return point;
        }

        /// <summary>
        /// 将路段端点坐标系转化为交叉口中央坐标系
        /// </summary>
        /// <param name="old"></param>
        internal static void ToCenterXY(ref Point old)
        {
            Coordinates.Offset(ref old,new Point(0, -SysSimContext.SimSettings.iMaxLanes));
        }

        /// <summary>
        /// 两个点的欧式距离
        /// </summary>
        internal static int Distance(Point p1, Point p2)
        {
            int iX = Math.Abs(p1.X - p2.X);
            int iY = Math.Abs(p1.Y - p2.Y);
            return (int)Math.Round(Math.Sqrt(iX * iX + iY * iY));
        }
        
        //internal static double FloatDistance(Point p1)
        //{
        //    return (int)Math.Round(Math.Sqrt(iX * iX + iY * iY));
        //}
        
        private static MyPoint mpBaseVector = new MyPoint(1.0f, 0);//使用x轴做基向量
        /// <summary>
        /// 按照参数2指定的向量与基向量的夹角计算参数1对应的绝对元胞坐标
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        internal static Point GetRealXY(Point point, MyPoint newVector)
        {
            if (newVector == null)
            {
                ThrowHelper.ThrowArgumentNullException("输入的参数不能为零");
            }
            //获取正弦和余弦值并且进行旋转变换
            SinCos sc = VectorTools.getSinCos(mpBaseVector, newVector);
            return Coordinates.Rotate(point, sc);
        }
    }
}
 
