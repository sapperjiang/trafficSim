using System;
using System.Drawing;

namespace SubSys_MathUtility
{
	/// <summary>
	/// ���ڼ���˫���ȸ�������POINT�������ԭ��Ϊ2d��mypoint����������Ϊ3d��pointΪ����߾���
	/// </summary>
	public partial struct OxyzPointF
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
	public partial struct OxyzPointF
	{
		public double _Z;

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
//				hashCode += 1000000007 * _X.GetHashCode();
//				hashCode += 1000000009 * _Y.GetHashCode();
//				hashCode += 1000000021 * _Z.GetHashCode();
				
				hashCode += 1000000007 * (int)_X;
				hashCode += 1000000009 * (int)_Y;
				hashCode += 1000000021 *(int) _Z;
			}
			return hashCode;
		}

	}
	
	
	/// <summary>
	/// ����structure
	/// </summary>
	public partial struct OxyzPoint
	{
		
		public int _X;
		public int _Y;
		public int _Z;
		
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
		
		public OxyzPoint(double dX,double dY,double dZ)
		{
			this._X = Convert.ToInt32(dX);
			this._Y = Convert.ToInt32(dY);
			this._Z = Convert.ToInt32(dZ);
		}
		public OxyzPoint(double dX,double dY):this(dX,dY,0f){}
		
		
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
		public OxyzPoint Clone()
		{
			return new OxyzPoint(this._X,this._Y,this._Z);
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
		public  bool Equals(OxyzPoint obj)
		{
			if (this._X==obj._X&&this._Y==obj._Y&&this._Z==obj._Z) {
				return true;
			}
			return false;
		}

		#endregion
		
		
		public static OxyzPoint Default=new OxyzPoint(-1,-1,-1);
		
		public override string ToString()
		{
			return string.Format("[X={0},Y={1},Z={2}]", _X, _Y, _Z);
		}

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

