using System;
using System.Drawing;

namespace SubSys_MathUtility
{
	/// <summary>
	/// ���ڼ���˫���ȸ�������POINT��Ϊ����߾���
	/// </summary>
	public class MyPoint
	{
		public float _X;
		public float _Y;

		public MyPoint(float x, float y)
		{
			this._X = x;
			this._Y = y;
		}

		public PointF ToPointF()
		{
			return new PointF(this._X, this._Y);
		}
		public  Point ToPoint()
		{
			int x= Convert.ToInt32(this._X);
			int y= Convert.ToInt32(this._Y);
			//        	int y
			return 	new Point(x,y);
		}
	}
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
		///// ��Ԫ������ϵת��Ϊ��Ļ����ϵ;
		///// </summary>
		///// <param name="rltPos"></param>
		///// <param name="offset"></param>
		public static Point Project(Point rltPos, int iCellPixels)
		{
			return Coordinates.Project(new MyPoint(rltPos.X, rltPos.Y), iCellPixels);
		}
		
		/// <summary>
		/// ��Ԫ������ϵת��Ϊ��Ļ����ϵ
		/// </summary>
		/// <param name="mp"></param>
		/// <param name="iScaleFactor">һ��Ԫ�����ȶ�Ӧ����Ļ���ص���</param>
		/// <returns></returns>
		public static Point Project(MyPoint mp, int iScaleFactor)
		{
			Point scrnPoint = new Point();
			scrnPoint.X = (int)Math.Round(iScaleFactor * mp._X);
			scrnPoint.Y = (int)Math.Round(iScaleFactor * mp._Y);
			//����ƽ��(ƫ��)
			scrnPoint.X -= Coordinates.GraphicsOffset.X;
			scrnPoint.Y -= Coordinates.GraphicsOffset.Y;
			return scrnPoint;//����Ǹ��ṹ�������ƣ�Ȼ�󷵻��µĽ��
		}
		/// <summary>
		/// offset��x��y��ֵ������ƫ�ƣ���xy��Ϊ��ֵ
		/// </summary>
		/// <param name="scrnPoint">ԭ����ϵ</param>
		/// <param name="offset">ƫ������</param>
		/// <returns>����ֵΪ�²���</returns>
		public static Point Offset(Point scrnPoint, Point offset)
		{
			//����ƽ��(ƫ��)
			scrnPoint.X -= offset.X;
			scrnPoint.Y -= offset.Y;
			return scrnPoint;//����Ǹ��ṹ�������ƣ�Ȼ�󷵻��µĽ��
		}

		public static MyPoint Offset(MyPoint scrnPoint, MyPoint offset)
		{
			MyPoint mp = new MyPoint(scrnPoint._X, scrnPoint._Y);
			//����ƽ��(ƫ��)
			mp._X -= offset._X;
			mp._Y -= offset._Y;
			return mp;
		}

		public static MyPoint Offset(Point scrnPoint, MyPoint offset)
		{
			MyPoint mp = new MyPoint(scrnPoint.X - offset._X, scrnPoint.Y - offset._Y);
			//����ƽ��(ƫ��)
			//scrnPoint.X -= offset.X;
			//scrnPoint.Y -= offset.Y;
			return mp;//����Ǹ��ṹ�������ƣ�Ȼ�󷵻��µĽ��
			
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

		/// <summary>
		/// �������ŷʽ����
		/// </summary>
		public static int Distance(Point p1, Point p2)
		{
			int iX = Math.Abs(p1.X - p2.X);
			int iY = Math.Abs(p1.Y - p2.Y);
			return (int)Math.Round(Math.Sqrt(iX * iX + iY * iY));
		}

		public static MyPoint mpBaseVector = new MyPoint(0, 1.0f);//ʹ��x����������
		/// <summary>
		/// ���ղ���2ָ����������������ļнǼ������1��ľ���Ԫ������
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static Point GetRealXY(Point point, MyPoint newVector)
		{
			if (newVector == null)
			{
				throw new ArgumentException("����Ĳ�������Ϊ��");
			}
			//��ȡ���Һ�����ֵ���ҽ�����ת�任
			SinCos sc = VectorTools.getSinCos(mpBaseVector, newVector);
			return Coordinates.Rotate(point, sc);
		}
	}
}

