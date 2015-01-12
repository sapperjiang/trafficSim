using System;
using System.Drawing;
using System.Collections.Generic;
using SubSys_MathUtility;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// 装饰者
	/// </summary>
	public class EntityShape:List<OxyzPoint>
	{
		//private List<MyPoint> _shapePoints;

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
			base.Add(op.ToOxyzPoint());
		}
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
		public OxyzPoint End
		{
			get
			{
				int iIndex = this.Count-1;
				if (iIndex>=0) {
					return this[iIndex];
				}
				throw new Exception();
//				if (this.Count>=2) {
//						return this[this.Count-1];
//				} else {
//					throw new Exception();
//				}
			}
	
		}
	}
	
}

