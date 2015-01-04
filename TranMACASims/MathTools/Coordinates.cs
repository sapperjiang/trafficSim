using System;
using System.Drawing;

namespace SubSys_MathUtility
{
	/// <summary>
	/// 用于计算双精度浮点数的POINT，为了提高精度
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
		///// 将元胞坐标系转换为屏幕坐标系;
		///// </summary>
		///// <param name="rltPos"></param>
		///// <param name="offset"></param>
		public static Point Project(Point rltPos, int iCellPixels)
		{
			return Coordinates.Project(new MyPoint(rltPos.X, rltPos.Y), iCellPixels);
		}
		
		/// <summary>
		/// 将元胞坐标系转化为屏幕坐标系
		/// </summary>
		/// <param name="mp"></param>
		/// <param name="iScaleFactor">一个元胞长度对应的屏幕像素点数</param>
		/// <returns></returns>
		public static Point Project(MyPoint mp, int iScaleFactor)
		{
			Point scrnPoint = new Point();
			scrnPoint.X = (int)Math.Round(iScaleFactor * mp._X);
			scrnPoint.Y = (int)Math.Round(iScaleFactor * mp._Y);
			//计算平移(偏移)
			scrnPoint.X -= Coordinates.GraphicsOffset.X;
			scrnPoint.Y -= Coordinates.GraphicsOffset.Y;
			return scrnPoint;//这个是个结构参数复制，然后返回新的结果
		}
		/// <summary>
		/// offset中x和y的值向左上偏移，其xy都为负值
		/// </summary>
		/// <param name="scrnPoint">原坐标系</param>
		/// <param name="offset">偏移坐标</param>
		/// <returns>返回值为新参数</returns>
		public static Point Offset(Point scrnPoint, Point offset)
		{
			//计算平移(偏移)
			scrnPoint.X -= offset.X;
			scrnPoint.Y -= offset.Y;
			return scrnPoint;//这个是个结构参数复制，然后返回新的结果
		}

		public static MyPoint Offset(MyPoint scrnPoint, MyPoint offset)
		{
			MyPoint mp = new MyPoint(scrnPoint._X, scrnPoint._Y);
			//计算平移(偏移)
			mp._X -= offset._X;
			mp._Y -= offset._Y;
			return mp;
		}

		public static MyPoint Offset(Point scrnPoint, MyPoint offset)
		{
			MyPoint mp = new MyPoint(scrnPoint.X - offset._X, scrnPoint.Y - offset._Y);
			//计算平移(偏移)
			//scrnPoint.X -= offset.X;
			//scrnPoint.Y -= offset.Y;
			return mp;//这个是个结构参数复制，然后返回新的结果
			
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

		/// <summary>
		/// 两个点的欧式距离
		/// </summary>
		public static int Distance(Point p1, Point p2)
		{
			int iX = Math.Abs(p1.X - p2.X);
			int iY = Math.Abs(p1.Y - p2.Y);
			return (int)Math.Round(Math.Sqrt(iX * iX + iY * iY));
		}

		public static MyPoint mpBaseVector = new MyPoint(0, 1.0f);//使用x轴做基向量
		/// <summary>
		/// 按照参数2指定的向量与基向量的夹角计算参数1点的绝对元胞坐标
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static Point GetRealXY(Point point, MyPoint newVector)
		{
			if (newVector == null)
			{
				throw new ArgumentException("输入的参数不能为零");
			}
			//获取正弦和余弦值并且进行旋转变换
			SinCos sc = VectorTools.getSinCos(mpBaseVector, newVector);
			return Coordinates.Rotate(point, sc);
		}
	}
}

