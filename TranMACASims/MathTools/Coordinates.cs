using System;
using System.Drawing;

namespace SubSys_MathUtility
{


	//public  struct MyInt:int
	
	/// <summary>
	/// �ڲ�·�ε��������ϵͳת������Ԫ������ϵϵͳ
	/// </summary>
	public static class Coordinates
	{

		/// <summary>
		/// Ԫ������ϵ��ͼ������ϵͳ֮���ƫ������
		/// ��ʼ��������ϵͳ��ԭ�㶼��ͼ������ϵԭ�㴦,����ƽ��XΪ��
		/// ����ƽ��YΪ��
		/// </summary>
		public static Point GraphicsOffset = new Point(0, 0);
		///// <summary>
		///// convert cartesian coordinates to screen coordinates and enlarge it ת��Ϊ��Ļ����ϵ;
		///// </summary>
		///// <param name="rltPos"></param>
		///// <param name="iScaleFactor"></param>
		public static PointF Project(PointF p, int iScaleFactor)
		{
			return Coordinates.Project(new OxyzPointF(p.X, p.Y), iScaleFactor).ToPointF();
		}
		
		public static PointF Project(OxyzPoint p, int iScaleFactor)
		{
			return Coordinates.Project(p.ToOxyzPointF(), iScaleFactor).ToPointF();
		}
//
		/// <summary>
		/// screen coordinates scalaor
		/// </summary>
		/// <param name="mp">cartesian coordinates</param>
		/// <param name="iScaleFactor">һ��Ԫ�����ȶ�Ӧ����Ļ���ص���</param>
		/// <returns></returns>
		public static OxyzPointF Project(OxyzPointF mp, int iScaleFactor)
		{
			OxyzPointF scrnPoint = new OxyzPointF();
			scrnPoint._X = (float)Math.Round(iScaleFactor * mp._X);
			scrnPoint._Y = (float)Math.Round(iScaleFactor * mp._Y);
//
//			//����ƽ��(ƫ��)
//			scrnPoint.X -= Coordinates.GraphicsOffset.X;
//			scrnPoint.Y -= Coordinates.GraphicsOffset.Y;
			return scrnPoint;//����Ǹ��ṹ�������ƣ�Ȼ�󷵻��µĽ��
		}
		
		
		/// <summary>
		/// offset��x��y��ֵ������ƫ�ƣ���xy��Ϊ��ֵ
		/// </summary>
		/// <param name="scrnPoint">ԭ����ϵ</param>
		/// <param name="offset">ƫ������</param>
		/// <returns>����ֵΪ�²���</returns>
		public static PointF Offset(PointF scrnPoint, PointF offset)
		{
			//����ƽ��(ƫ��)
//			scrnPoint.X += offset.X;
//			scrnPoint.Y += offset.Y;
//			return scrnPoint;//����Ǹ��ṹ�������ƣ�Ȼ�󷵻��µĽ��
			
			return Coordinates.Offset(new OxyzPointF(scrnPoint.X,scrnPoint.Y,0f),new OxyzPointF(offset.X,offset.Y,0f)).ToPointF();
			
		}

		/// <summary>
		/// vector use math coordinates .while offset servers screen coordinates,Y is reversed .so .Y needs to be reverse
		/// </summary>
		/// <param name="scrnPoint"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		public static OxyzPointF Offset(OxyzPointF scrnPoint, OxyzPointF offset)
		{
			OxyzPointF mp = new OxyzPointF(scrnPoint._X, scrnPoint._Y);
			//����ƽ��(ƫ��)
			mp._X += offset._X;
			mp._Y -= offset._Y;
			return mp;
		}
		
		public static OxyzPointF Offset(OxyzPoint scrnPoint, OxyzPointF offset)
		{
			return Coordinates.Offset(scrnPoint.ToOxyzPointF(),offset);
		}
		
		/// <summary>
		/// to enlarge a point for drawing.use screen coordinates
		/// </summary>
		/// <param name="mp"></param>
		/// <param name="iWidth"></param>
		/// <returns>new points</returns>
		public static OxyzPointF Offset(OxyzPointF mOld, int iWidth)
		{
			double dWidth = -1*iWidth;
			mOld._X += dWidth;
			mOld._Y += dWidth;
			mOld._Z += dWidth;
			
			return  mOld;
		}
		
		
			/// <summary>
		/// to enlarge a point for drawing.use screen coordinates
		/// </summary>
		/// <param name="mp"></param>
		/// <param name="iWidth"></param>
		/// <returns>new points</returns>
		public static OxyzPointF Offset(OxyzPointF mOld, int iWidth,OxyzPointF vector)
		{
			double dWidth = iWidth;
			mOld._X += dWidth*vector._X;
			mOld._Y += dWidth*vector._Y;
			mOld._Z += dWidth*vector._Z;
			
			return  mOld;
		}
		

		/// <summary>
		/// ����������ϵ�ĵ㣬���ؾ�����ϵ�ĵ�
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="a"></param>
		public static Point Rotate(Point mpNew,SinCos a)
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
		public static void Offset(ref Point old,Point panVector)
		{
			old.Y += panVector.Y;
			old.X += panVector.X;
		}
		public static void OffsetYAxis(ref Point old, int iYSpan)
		{
			Point panVector = new Point(0, -iYSpan);
			Coordinates.Offset(ref old,panVector);
		}

		

		public static OxyzPointF mpBaseVector = new OxyzPointF(0, 1.0f);//ʹ��x����������
		/// <summary>
		/// ���ղ���2ָ����������������ļнǼ������1��ľ���Ԫ������
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
//		public static Point GetRealXY(Point point, OxyzPointF newVector)
//		{
//			if (newVector == null)
//			{
//				throw new ArgumentException("����Ĳ�������Ϊ��");
//			}
//			//��ȡ���Һ�����ֵ���ҽ�����ת�任
//			SinCos sc = VectorTools.GetSinCos(mpBaseVector, newVector);
//			return Coordinates.Rotate(point, sc);
//		}
		
		/// <summary>
		/// �������ŷʽ����
		/// </summary>
		public static int Distance(Point p1, Point p2)
		{
			int iX = Math.Abs(p1.X - p2.X);
			int iY = Math.Abs(p1.Y - p2.Y);
			return (int)Math.Round(Math.Sqrt(iX * iX + iY * iY));
		}
		
		
		//------------------------20160202-------------------------�����������ŷʽ����
		
		public static double Distance(OxyzPointF p1, OxyzPointF p2)
		{
			double iX = Math.Abs(p1._X - p2._X);
			double iY = Math.Abs(p1._Y - p2._Y);
			double iZ = Math.Abs(p1._Z - p2._Z);
			
			return Math.Sqrt(iX * iX + iY * iY+iZ * iZ);
			
		}
		
	}
}

