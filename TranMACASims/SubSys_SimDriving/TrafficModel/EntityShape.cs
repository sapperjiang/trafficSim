using System.Drawing;
using System.Collections.Generic;
using SubSys_MathUtility;

namespace SubSys_SimDriving.TrafficModel
{
	public class EntityShape:List<MyPoint>
	{
        private List<MyPoint> ShapePoints;

        /// <summary>
        /// 交通元素如交叉口\路段\车道的外形,用于画图.这些元素的该属性,为画图提供素材
        /// </summary>
        internal EntityShape()
        {
            ShapePoints = new List<MyPoint>();
        }
        //EntityShape
	}
	 
}
 
