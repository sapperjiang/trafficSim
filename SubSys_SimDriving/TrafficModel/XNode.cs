using System;
using System.Collections;
using System.Collections.Generic;

using SubSys_MathUtility;
using SubSys_SimDriving.Agents;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
	
	/// <summary>
	/// 使用矩阵类型data structure
	/// 五路交叉.环路的支持有待讨论，三路交叉,crossings are supported
	/// 表示道路交叉口的类
	/// </summary>
	public class XNode : StaticOBJ
	{
        
		/// <summary>
		/// 新的roadnode的哈希散列值由其中心Position的哈希值和其ID构成
		/// </summary>
		/// <returns></returns>
		private HashMatrix _mobiles; 
		public HashMatrix Mobiles
		{
			get
			{
				if (this._mobiles == null) {
					this._mobiles=new HashMatrix();
				}
				return this._mobiles;
			}
		}

		

		/// <summary>
		/// 存贮本节点所有出边的哈希表，键值是代表边的Way哈希，值是代表Way
		/// </summary>
		private Dictionary<int, Way> _dicEdges = new Dictionary<int,Way>();

		/// <summary>
		/// 提供对哈希矩阵内部元素的遍历
		/// </summary>
		/// <returns></returns>
		public IEnumerator<MobileOBJ> GetEnumerator()
		{
           return this._mobiles.GetEnumerator();
		}

		public ICollection Ways
		{
			get
			{
				return this._dicEdges.Values;
			}
		}
		#region 用来保存邻接矩阵中节点出边的边成员,不应当使用RoadNetwork之外的类访问这些成员
		/// <summary>
		/// 注意在出边表中，保持Way的from字段是this节点，否则函数抛出异常
		/// </summary>
		/// <param name="Way"></param>
		internal void AddWay(Way way)
		{
          //  System.Diagnostics.Debug.Assert(way != null);
            
			if (!Contains(way.GetHashCode()))
			{
                //加入判断是否是当前点的出边的信息防止出错
                System.Diagnostics.Debug.Assert(way.From == this);
                //{
                //                throw new Exception("添加了不属于该顶点的边");
                //}
                var iLaneCount = way.Width;//.Lanes.Count;
                base.Length = iLaneCount > base.Length ? iLaneCount : base.Length;

				_dicEdges.Add(way.GetHashCode(), way);
			}
			else
			{
				throw new ArgumentException("添加了重复的边！");
			}
	
		}
		/// <summary>
		/// 找到边 从this到toNode节点的边，出边表
		/// </summary>
		/// <param name="fromRN"></param>
		internal void RemoveWay(Way re)
		{
			if (re == null )
			{
				throw new ArgumentNullException();
			}
			_dicEdges.Remove(re.GetHashCode());
		}
	
		/// <summary>
		/// 查找方法，新的结构采用出边表
		/// </summary>
		/// <param name="toRoadNode">出节点</param>
		/// <returns></returns>
		public Way FindWay(XNode toRoadNode)
		{
			int iHashkey = Way.GetHashCode(this,toRoadNode);
			if (_dicEdges.ContainsKey(iHashkey))
			{
				return _dicEdges[iHashkey];
			}
			return null;
		}
		
		public bool Contains(int EdgeKey)
		{
			return _dicEdges.ContainsKey(EdgeKey);
		}

		#endregion
		/// <summary>
		/// 控制XNodeID的数量
		/// </summary>
		private static int iXNodeID;
		[System.Obsolete("使用有参数的构造函数")]
		private XNode()
		{
			this._entityID = ++iXNodeID;
            this.EntityType = EntityType.XNode;

			Random rd = new Random();
            

			this.GISGrid = new OxyzPointF(rd.Next(65535), rd.Next(65535));
			// 直接使用上下文的数据结构,bug不应当使用上下文结构
			if (this.GISGrid._X == 0.0f && this.GISGrid._Y == 0.0f)
			{
				ThrowHelper.ThrowArgumentNullException("RoadNode产生了零坐标！");
			}
		}

		public override int GetHashCode()
		{
			int iHash = this.GISGrid.GetHashCode() +this.ID.GetHashCode();
			return iHash.GetHashCode();
		}
		/// <summary>
		/// 更新agent，更新元胞（驾驶），调用服务
		/// </summary>
		public override void UpdateStatus()
		{
			//更新异步agent，如果有的话
			for (int i = 0; i < this.asynAgents.Count; i++)
			{
				AbstractAgent visitor = this.asynAgents[i];
				visitor.VisitUpdate(this);//.VisitUpdate();
			}
		
			var mobileNode = this.Mobiles.First;
			//update mobile on a lane one by one
			while(mobileNode!=null) {
				var mobile = mobileNode.Value;
				//mobile is possibaly be deleted
				mobile.Run(this as StaticOBJ);
				mobileNode = mobileNode.Next;
			}
			this.ServeMobiles();
			base.UpdateStatus();//基类调用了OnStatusChanged 进行绘图服务
		}

		protected override void OnStatusChanged()
		{
			//call its base's method to run services registered on this entity
				this.InvokeServices(this);
		}

		
		/// <summary>
		/// 处理交叉口的等待车辆数量,a mobile that can enter xnode was added to mobilesInn.
		/// see driverstratigy for details
		/// </summary>
		internal override void ServeMobiles()
		{
			while (this.MobilesInn.Count > 0)
			{
				this.Mobiles.Add(this.MobilesInn.Dequeue());
			}

		}

		/// <summary>
		/// 判断指定车道前部第Ahead个位置处是否有元胞占据
		/// </summary>
		public bool IsOccupied(OxyzPointF opPoint)
		{
			//if this point is not added to dictionary ,add it then
			//return this._occupiedPoints.;
			return this.Mobiles.IsOccupied(opPoint);
		}

		internal XNode(OxyzPointF pointCenter)
		{
            this._entityID = ++iXNodeID;
            this.EntityType = EntityType.XNode;
			this.SpatialGrid = pointCenter;


            Random rd = new Random();
            this.GISGrid = new OxyzPointF(rd.Next(65535), rd.Next(65535));
            // 直接使用上下文的数据结构,bug不应当使用上下文结构
            if (this.GISGrid._X == 0.0f && this.GISGrid._Y == 0.0f)
            {
                ThrowHelper.ThrowArgumentNullException("RoadNode产生了零坐标！");
            }

        }


     
        /// <summary>
        ///    /////[System.Obsolete("should be restructed")]
        /// </summary>
        public override int Length
        {
            get
            {
               return base.Length;
             //  return this.Ways.Count;
              //  return this.Shape.Count;
            }
        }
        public override int Width
        {
            get
            {
                return SimSettings.iCarWidth;
            }
        }

    }
}

