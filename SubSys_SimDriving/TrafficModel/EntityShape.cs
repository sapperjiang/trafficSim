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
	
	public partial class EntityShape:List<OxyzPointF>
	{

        List<Point> _drawShape = new List<Point>();
        /// <summary>
        /// 画图的点集合和模型的点集合的转换，用前者创建后者
        /// </summary>
        /// <param name="drawShape"></param>
        /// <returns></returns>
        public static EntityShape CreateShape(List<Point> drawShape)
        {
            EntityShape newShape = new EntityShape();
            for (int i = 1; i < drawShape.Count; i++)
            {
                var pFirst = new OxyzPointF(drawShape[i-1]);
                //the end point
                var pFEnd = new OxyzPointF(drawShape[i]);
                var newShapePoint = EntityShape.NextPoint( pFirst,   pFEnd);
                while (newShapePoint != OxyzPointF.Default)
                {
                    newShape.Add(newShapePoint);
                    newShapePoint = EntityShape.NextPoint(newShapePoint, pFEnd);
                }
            }
            return newShape;
        }
        public static List<Point> GetReverse(List<Point> drawShape)
        {
            //drawShape.Reverse();
            ////point是结构体，不是引用类型
            List<Point> newShape = new List<Point>(drawShape);
            newShape.Reverse();
            return newShape;
        }

        public static OxyzPointF NextPoint(OxyzPointF iCurrPoint, OxyzPointF To)
        {
            OxyzPointF iNew = iCurrPoint;
            //算法保证每一个时间步长内都向目标终点接近，就是为了让其到终点的距离变小
            int iX = (int)(iCurrPoint._X - To._X);//当前位置减去目的位置
            int iY = (int)(iCurrPoint._Y - To._Y);
            ///////////////////////////////
            //			int iX = iCurrPoint._X - op._X;//当前位置减去目的位置
            //			int iY = iCurrPoint._Y - op._Y;
            if (iX != 0)//等于0的情况什么也不做
            {
                iNew._X = iX > 0 ? --iNew._X : ++iNew._X;
            }
            if (iY != 0)//等于0的情况什么也不做
            {
                iNew._Y = iY > 0 ? --iNew._Y : ++iNew._Y;
            }
            if (iX == 0 && iY == 0)///已经到达了目标地点，两个点的坐标差值为0
			{
                iNew = OxyzPointF.Default;
            }
            return iNew;
        }

        /// <summary>
        ///  For a road with direction like this “----->”  ,start means a point at the end of the narrow .
        /// and end means a point at the narrow
        /// </summary>
        public OxyzPointF Start
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
		
        public List<Point> ToPointArray()
        {
            List<Point> lp = new List<Point>();
            IEnumerator<OxyzPointF> of = this.GetEnumerator();
            while (of.MoveNext())
            {
                lp.Add(of.Current.ToPoint());
            }
            return lp;
        }

        /// <summary>
        ///  a road with direction like this “----->”  .Start means a point at the beginning of that narrow .
        /// and End means a point on that narrow
        /// </summary>
        public OxyzPointF End
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

        public List<Point> DrawShape
        {
            get
            {
                return _drawShape;
            }

            set
            {
                _drawShape = value;
            }
        }

        /// <summary>
        ///points added early have smaller index
        /// </summary>

        private Dictionary<int, int> _dicGrids = new  Dictionary<int, int>();
		
		/// <summary>
		/// 利用点坐标，获取该点在实体点集合中的，例如，如果某点在集合中
		/// </summary>
		/// <param name="op">一个点的点坐标</param>
		/// <returns>返回值为-1，说明该点不在集合中</returns>
		public int GetIndex(OxyzPointF op)
		{
			int iIndex = -1;
//			this.FindIndex(op);
			//当此方法返回值时，如果找到该键，便会返回与指定的键相关联的值；
			//否则，则会返回 value 参数的类型默认值。该参数未经初始化即被传递。
			_dicGrids.TryGetValue(op.GetHashCode(),out iIndex);
          //  return base.get.GetIndex(op)
			
//			int p1=-1 ;
//			_dicGrids.TryGetValue(this[1].GetHashCode(),out p1);
//
//
//			int p2 =-1;
//				_dicGrids.TryGetValue(this[2].GetHashCode(),out p2);
//
//					int p3 =-1;
//				_dicGrids.TryGetValue(this[3].GetHashCode(),out p3);
			//哈希表找不到，默认值是0 这与索引是0的点冲突。所以这里采用一个偏移量，与Add函数一起作用
			return iIndex-1;
		}
		
		
		/// <summary>
		/// 重载一个浮点型的Add方法
		/// </summary>
		/// <param name="op"></param>
		public  void Add(OxyzPointF op)
		{
			int iHashCode= op.GetHashCode();
			this._dicGrids.Add(iHashCode,this.Count+1);
			base.Add(op);
//			System.Diagnostics.Debug.Assert((this.Count-1)==base.FindLastIndex(op));
			
		}
		
		public EntityShape DeepClone()
		{
			var eShape = new EntityShape();
			
			for (int i = 0; i < this.Count; i++) {
				eShape.Add(this[i]);
			}
			return eShape;
			
		}
		
	}
	
}

