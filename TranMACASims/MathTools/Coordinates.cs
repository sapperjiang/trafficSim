using System;
using System.Drawing;

namespace SubSys_MathUtility
{
	/// <summary>
	/// 用于计算双精度浮点数的POINT，这个类原来为2d的mypoint，现在升级为3d的point为了提高精度
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
		/// 重写哈希值，避免两个对象的哈希值相同引起的插入错误
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
	/// 整形structure
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
	/// 内部路段的相对坐标系统转化绝对元胞坐标系系统
	/// </summary>
	public static class Coordinates
	{

		/// <summary>
		/// 元胞坐标系和图像坐标系统之间的偏移量，
		/// 初始两个坐标系统的原点都在图像坐标系原点处,向右平移X为正
		/// 向下平移Y为正
		/// </summary>
		public static Point GraphicsOffset = new Point(0, 0);
		///// <summary>
		///// convert cartesian coordinates to screen coordinates and enlarge it 转换为屏幕坐标系;
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
		/// <param name="iScaleFactor">一个元胞长度对应的屏幕像素点数</param>
		/// <returns></returns>
		public static OxyzPointF Project(OxyzPointF mp, int iScaleFactor)
		{
			OxyzPointF scrnPoint = new OxyzPointF();
			scrnPoint._X = (float)Math.Round(iScaleFactor * mp._X);
			scrnPoint._Y = (float)Math.Round(iScaleFactor * mp._Y);
//
//			//计算平移(偏移)
//			scrnPoint.X -= Coordinates.GraphicsOffset.X;
//			scrnPoint.Y -= Coordinates.GraphicsOffset.Y;
			return scrnPoint;//这个是个结构参数复制，然后返回新的结果
		}
		
		
		/// <summary>
		/// offset中x和y的值向左上偏移，其xy都为负值
		/// </summary>
		/// <param name="scrnPoint">原坐标系</param>
		/// <param name="offset">偏移坐标</param>
		/// <returns>返回值为新参数</returns>
		public static PointF Offset(PointF scrnPoint, PointF offset)
		{
			//计算平移(偏移)
//			scrnPoint.X += offset.X;
//			scrnPoint.Y += offset.Y;
//			return scrnPoint;//这个是个结构参数复制，然后返回新的结果
			
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
			//计算平移(偏移)
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
		/// 输入新坐标系的点，返回旧坐标系的点
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
		/// 坐标系平移
		/// </summary>
		/// <param name="old"></param>
		/// <param name="panVector">平移向量</param>
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

		

		public static OxyzPointF mpBaseVector = new OxyzPointF(0, 1.0f);//使用x轴做基向量
		/// <summary>
		/// 按照参数2指定的向量与基向量的夹角计算参数1点的绝对元胞坐标
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
//		public static Point GetRealXY(Point point, OxyzPointF newVector)
//		{
//			if (newVector == null)
//			{
//				throw new ArgumentException("输入的参数不能为零");
//			}
//			//获取正弦和余弦值并且进行旋转变换
//			SinCos sc = VectorTools.GetSinCos(mpBaseVector, newVector);
//			return Coordinates.Rotate(point, sc);
//		}
		
		/// <summary>
		/// 两个点的欧式距离
		/// </summary>
		public static int Distance(Point p1, Point p2)
		{
			int iX = Math.Abs(p1.X - p2.X);
			int iY = Math.Abs(p1.Y - p2.Y);
			return (int)Math.Round(Math.Sqrt(iX * iX + iY * iY));
		}
		
		
		//------------------------20160202-------------------------计算两个点的欧式距离
		
		public static double Distance(OxyzPointF p1, OxyzPointF p2)
		{
			double iX = Math.Abs(p1._X - p2._X);
			double iY = Math.Abs(p1._Y - p2._Y);
			double iZ = Math.Abs(p1._Z - p2._Z);
			
			return Math.Sqrt(iX * iX + iY * iY+iZ * iZ);
			
		}
		
	}
}

