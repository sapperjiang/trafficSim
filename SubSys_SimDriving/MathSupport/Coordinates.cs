using System;
using System.Drawing;
using SubSys_SimDriving.SysSimContext;

namespace SubSys_SimDriving.MathSupport
{
    /// <summary>
    /// �ڲ�·�ε��������ϵͳת������Ԫ������ϵϵͳ
    /// </summary>
    internal static class Coordinates
    {
        /// <summary>
        /// ����������ϵ�ĵ㣬���ؾ�����ϵ�ĵ�
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
        /// ����ϵƽ��
        /// </summary>
        /// <param name="old"></param>
        /// <param name="panVector">ƽ������</param>
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
        /// ���Ԫ������ϵת������������ϵ,ƽ�ƣ�Ȼ����ת�任
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        internal static Point RltXYToRealXY(ref Point point,MyPoint vPoiner)
        {
            //����ϵƽ�Ʊ任,������x�͵ײ�y����ϵƽ�Ƶ���������ϵ
            Coordinates.OffsetYAxis(ref point, SimSettings.iMaxLanes);
            Point indexOld = Coordinates.GetRealXY(point, vPoiner);
            
            return point;
        }

        /// <summary>
        /// ��·�ζ˵�����ϵת��Ϊ�������������ϵ
        /// </summary>
        /// <param name="old"></param>
        internal static void ToCenterXY(ref Point old)
        {
            Coordinates.Offset(ref old,new Point(0, -SysSimContext.SimSettings.iMaxLanes));
        }

        /// <summary>
        /// �������ŷʽ����
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
        
        private static MyPoint mpBaseVector = new MyPoint(1.0f, 0);//ʹ��x����������
        /// <summary>
        /// ���ղ���2ָ����������������ļнǼ������1��Ӧ�ľ���Ԫ������
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        internal static Point GetRealXY(Point point, MyPoint newVector)
        {
            if (newVector == null)
            {
                ThrowHelper.ThrowArgumentNullException("����Ĳ�������Ϊ��");
            }
            //��ȡ���Һ�����ֵ���ҽ�����ת�任
            SinCos sc = VectorTools.getSinCos(mpBaseVector, newVector);
            return Coordinates.Rotate(point, sc);
        }
    }
}
 
