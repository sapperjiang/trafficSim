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
		
		public double _Z;

//		private static UInt32 ID = 0;
//		private UInt32 _id =0;
//
		public OxyzPointF(double x, double y,double z)
		{
			this._X = x;
			this._Y = y;
			this._Z=z;
		}
		
		public static  OxyzPointF operator - (OxyzPointF a, OxyzPointF b)
		{
			return new OxyzPointF(a._X-b._X,a._Y-b._Y,a._Z-b._Z);
		}
		
		public static  OxyzPointF operator + (OxyzPointF a, OxyzPointF b)
		{
			return new OxyzPointF(b._X+a._X,b._Y+a._Y,b._Z+a._Z);
		}
		
		
		public OxyzPointF Clone()
		{
			return new OxyzPointF(this._X,this._Y,this._Z);
		}
		
		public OxyzPointF(double x, double y):this(x,y,0F){}
		
		public OxyzPointF(OxyzPointF p):this(p._X,p._Y,0F){}
		
		public static OxyzPointF Default
		{	
			get{return new OxyzPointF(0f,0f,0f);}
		}
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
				
				hashCode +=(int)(10000 * _X);
				hashCode +=(int)( 10000 * _Y);
				hashCode +=(int)( 10000 * _Z);
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

}
 
