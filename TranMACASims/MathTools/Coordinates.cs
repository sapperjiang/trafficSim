using System;
using System.Drawing;

namespace SubSys_MathUtility
{
	/// <summary>
	/// ���ڼ���˫���ȸ�������POINT�������ԭ��Ϊ2d��mypoint����������Ϊ3d��pointΪ����߾���
	/// </summary>
	public partial class OxyzPointF
	{
		public double _X;
		public double _Y;


		public OxyzPoint ToOxyzPoint()
		{
			return new OxyzPoint(this);
		}

		
		
		public PointF ToPointF()
		{
			return new PointF((float)this._X,(float) this._Y);
		}
		public Point ToPoint()
		{
			int x= Convert.ToInt32(this._X);
			int y= Convert.ToInt32(this._Y);
			
			return 	new Point(x,y);
		}
		

	}
	public partial class OxyzPointF
	{
		public double _Z=0F;

//		private static UInt32 ID = 0;
//		private UInt32 _id =0;
//
		public OxyzPointF(double x, double y,double z)
		{
			this._X = x;
			this._Y = y;
			this._Z=z;
			
//			OxyzPointF.ID++;
//			this._id = OxyzPointF.ID;
		}
		public OxyzPointF(double x, double y):this(x,y,0F){}
		
		public OxyzPointF(OxyzPointF p):this(p._X,p._Y,0F){}
		
//		public OxyzPoint ToOxyzPoint()
//		{	return new ox
//		}
//		
		//-----------------------------20160130-----------------------------
		/// <summary>
		/// ��д��ϣֵ��������������Ĺ�ϣֵ��ͬ����Ĳ������
		/// </summary>
		/// <returns></returns>
//		public override int GetHashCode()
//		{
//			return this._id.GetHashCode();
//		}
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				hashCode += 1000000007 * _X.GetHashCode();
				hashCode += 1000000009 * _Y.GetHashCode();
				hashCode += 1000000021 * _Z.GetHashCode();
			}
			return hashCode;
		}

	}
	
	
	/// <summary>
	/// ������
	/// </summary>
	public partial class OxyzPoint
	{
		
		public int _X;
		public int _Y;
		public int _Z=0;
		
		public OxyzPoint(int x,int y,int z)
		{
			this._X = x;
			this._Y = y;
			this._Z=z;
		}
		public OxyzPoint(int x,int y):this(x,y,0){}
		
		public OxyzPoint(OxyzPointF opF)
		{
			this._X = Convert.ToInt32( opF._X);
			this._Y = Convert.ToInt32( opF._Y);
			this._Z = Convert.ToInt32( opF._Z);
		}

		
		public OxyzPointF ToOxyzPointF()
		{
			return new OxyzPointF(this._X,this._Y,this._Z);
		}
		
		public PointF ToPointF()
		{
			return new PointF(this._X, this._Y);
		}
		
		public Point ToPoint()
		{
			return new Point(this._X, this._Y);
		}

		#region Equals and GetHashCode implementation
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				hashCode += 1000000007 * _X.GetHashCode();
				hashCode += 1000000009 * _Y.GetHashCode();
				hashCode += 1000000021 * _Z.GetHashCode();
			}
			return hashCode;
		}

		#endregion
		
		
		public static OxyzPoint Default=new OxyzPoint(-1,-1,-1);
		
		
	}


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
		///// ��Ԫ������ϵת��Ϊ��Ļ����ϵ;
		///// </summary>
		///// <param name="rltPos"></param>
		///// <param name="offset"></param>
		public static Point Project(Point rltPos, int iCellPixels)
		{
			return Coordinates.Project(new OxyzPointF(rltPos.X, rltPos.Y), iCellPixels);
		}
		
		/// <summary>
		/// ��Ԫ������ϵת��Ϊ��Ļ����ϵ
		/// </summary>
		/// <param name="mp"></param>
		/// <param name="iScaleFactor">һ��Ԫ�����ȶ�Ӧ����Ļ���ص���</param>
		/// <returns></returns>
		public static Point Project(OxyzPointF mp, int iScaleFactor)
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

		public static OxyzPointF Offset(OxyzPointF scrnPoint, OxyzPointF offset)
		{
			OxyzPointF mp = new OxyzPointF(scrnPoint._X, scrnPoint._Y);
			//����ƽ��(ƫ��)
			mp._X -= offset._X;
			mp._Y -= offset._Y;
			return mp;
		}
		

		public static OxyzPointF Offset(OxyzPoint scrnPoint, OxyzPointF offset)
		{
			OxyzPointF mp = new OxyzPointF(scrnPoint._X - offset._X, scrnPoint._Y - offset._Y);
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

		

		public static OxyzPointF mpBaseVector = new OxyzPointF(0, 1.0f);//ʹ��x����������
		/// <summary>
		/// ���ղ���2ָ����������������ļнǼ������1��ľ���Ԫ������
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static Point GetRealXY(Point point, OxyzPointF newVector)
		{
			if (newVector == null)
			{
				throw new ArgumentException("����Ĳ�������Ϊ��");
			}
			//��ȡ���Һ�����ֵ���ҽ�����ת�任
			SinCos sc = VectorTools.GetSinCos(mpBaseVector, newVector);
			return Coordinates.Rotate(point, sc);
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

