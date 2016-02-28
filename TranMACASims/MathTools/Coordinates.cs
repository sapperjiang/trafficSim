using System;
using System.Drawing;

namespace SubSys_MathUtility
{


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

