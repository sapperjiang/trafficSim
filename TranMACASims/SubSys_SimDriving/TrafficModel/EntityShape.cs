using System;
using System.Drawing;
using System.Collections.Generic;
using SubSys_MathUtility;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// 装饰者
	/// </summary>
	public class EntityShape:List<MyPoint>
	{
		private List<MyPoint> _shapePoints;

		/// <summary>
		/// 交通元素如交叉口\路段\车道的外形,用于画图.这些元素的该属性,为画图提供素材
		/// </summary>
		internal EntityShape()
		{
			_shapePoints = new List<MyPoint>();
		}
		//EntityShape
		
		public MyPoint Start
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
		public MyPoint End
		{
			get
			{
				if (this.Count>=2) {
						return this[this.Count-1];
				} else {
					throw new Exception();
				}
			}
	
		}
	}
	
}

