using System;
using System.Drawing;
using System.Collections.Generic;
using SubSys_MathUtility;
using System.Diagnostics;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// 一个用来描述实体形状的一系列点坐标.是个List类,可以用坐标遍历,GIS坐标系的形状
	/// </summary>
	
	public partial class EntityShape:List<OxyzPoint>
	{
		/// <summary>
		/// 交通元素如交叉口\路段\车道的外形,用于画图.这些元素的该属性,为画图提供素材
		/// </summary>
//		internal EntityShape()
//		{
//			_shapePoints = new List<MyPoint>();
//		}
		
		/// <summary>
		/// 重载一个浮点型的Add方法
		/// </summary>
		/// <param name="op"></param>
		public void Add(OxyzPointF op)
		{
			this._dicGrids.Add(op.GetHashCode(),this.Count);
			base.Add(op.ToOxyzPoint());
					
//			System.Diagnostics.Debug.Assert((this.Count-1)==base.FindLastIndex(op));
			
		}
		/// <summary>
		///  For a road with direction like this “----->”  ,start means a point at the end of the narrow .
		/// and end means a point at the narrow
		/// </summary>
		public OxyzPoint Start
		{
			get
			{
				if (this.Count!=0) {
					return this[0];
				} else {
					throw new Exception();
				}
			}
		}
		
		
		/// <summary>
		///  a road with direction like this “----->”  .Start means a point at the end of that narrow .
		/// and End means a point on that narrow
		/// </summary>
		public OxyzPoint End
		{
			get
			{
				int iIndex = this.Count-1;
				if (iIndex>=0) {
					return this[iIndex];
				}
				throw new Exception("mobile is not shaped before called");
			}
		}
	}
	
	/// <summary>
	///points added early have smaller index
	/// </summary>
	public partial class EntityShape:List<OxyzPoint>
	{
		private Dictionary<int, int> _dicGrids = new  Dictionary<int, int>();
		
		/// <summary>
		/// 利用点坐标，获取该点在实体点集合中的，例如，如果某点在集合中
		/// </summary>
		/// <param name="op">一个点的点坐标</param>
		/// <returns>返回值为-1，说明该点不在集合中</returns>
		public int GetIndex(OxyzPoint op)
		{
			int iIndex = -1;
//			this.FindIndex(op);
			//当此方法返回值时，如果找到该键，便会返回与指定的键相关联的值；
			//否则，则会返回 value 参数的类型默认值。该参数未经初始化即被传递。
			_dicGrids.TryGetValue(op.GetHashCode(),out iIndex);
			
			int p1=-1 ;
				_dicGrids.TryGetValue(op.GetHashCode(),out p1);
			
				
			int p2 =-1;
				_dicGrids.TryGetValue(op.GetHashCode(),out p2);
				
				
				
			return iIndex;
		}
	}
	
}

