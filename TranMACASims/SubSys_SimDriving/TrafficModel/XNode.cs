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
	/// 使用矩阵类型的结构意味着不支持五路交叉.环路的支持有待讨论，三路交叉是支持的
	/// 表示道路交叉口的类
	/// </summary>
	public class XNode : StaticEntity
	{
		/// <summary>
		/// 路段转化为中心坐标点,iahead  不应当小于零
		/// </summary>
		private Point MakeCenterXY(Lane rl, int iAhead)
		{
			return new Point(rl.Rank, iAhead - SimSettings.iMaxLanes);
		}


		#region 车道操作函数


		/// <summary>
		/// 判断指定车道前部第Ahead个位置处是否有元胞占据
		/// </summary>
		internal bool IsBlocked(Lane rl, int iAhead)
		{
			Point irltXY = this.MakeCenterXY(rl,iAhead);
			Point iRealXY = Coordinates.GetRealXY(irltXY,rl.ToVector());
			return cells.IsBlocked(iRealXY.X, iRealXY.Y);
		}
		/// <summary>
		/// 判断指定车道前部第Ahead个位置处是否有元胞占据
		/// </summary>
		internal bool IsBlocked(Point iRealXY)
		{
			return cells.IsBlocked(iRealXY.X, iRealXY.Y);
		}
		
		//20160215
		internal bool IsBlocked(OxyzPoint pPoint)
		{
			
			ThrowHelper.ThrowArgumentException("not yet implemented");
			return false;
			//return cells.IsBlocked(pPoint._X, pPoint._Y);
		}

		
		
		/// <summary>
		/// 将车道堵塞
		/// </summary>
		/// <param name="rl"></param>
		internal void BlockLane(Lane rl)
		{
			if (rl == null)
			{
				throw new ArgumentNullException();
			}
			if (IsBlocked(rl,1)==false)//空的则添加
			{
				this.AddCell(rl, 1);
			}
		}
		/// <summary>
		/// 将车道疏通
		/// </summary>
		/// <param name="rl"></param>
		internal void UnblockLane(Lane rl)
		{
			if (rl == null)
			{
				throw new ArgumentNullException();
			}
			if (IsBlocked(rl, 1)==true)//非空则删除
			{//用null 将位置 rl.rank 和第1-6个位置的元胞占据
				this.RemoveCell(rl, 1);//(id, rl.parEntity, null);
			}
		} /// <summary>
		/// 判断第x个车道前面是否有iAheadSpace个车辆
		/// </summary>
		/// <returns></returns>
		internal bool IsLaneBlocked(Lane rl, int iAheadSpace)
		{
			bool isBlocked = false;
			for (int i = 1; i <= iAheadSpace; i++)
			{
				isBlocked = this.IsBlocked(rl, i);
				if (isBlocked == true)
					break;
			}
			return isBlocked;
		}

		internal bool IsLaneBlocked(Lane rl)
		{
			return this.IsBlocked(rl, 1);
		}
		#endregion

		#region 元胞操作函数
		/// <summary>
		/// 为红绿灯添加准备的方法，不是正常的元胞
		/// </summary>
		private void AddCell(Lane rl, int iAheadSpace)
		{
			Point ipt = this.MakeCenterXY(rl, 1);
			ipt = Coordinates.GetRealXY(ipt, rl.ToVector());
			cells.Add(ipt.X, ipt.Y, null);//堵塞作用的元胞可以为null
		}

		/// <summary>
		/// 在指定的点添加一个元胞，
		/// </summary>
		internal void AddCell(Cell ca)
		{
			ca.Container = this;
			cells.Add(ca.Track.pCurrPos.X,ca.Track.pCurrPos.Y, ca);
		}
		/// <summary>
		/// 要求两个参数是绝对坐标
		/// </summary>
		/// <param name="iOldPoint"></param>
		/// <param name="iNewPoint"></param>
		/// <returns></returns>
		[System.Obsolete("cell is not used in new version of this software ,replace with mobile")]
		internal bool MoveCell(Point iOldPoint, Point iNewPoint)
		{
			return cells.Move(iOldPoint, iNewPoint);
		}

		internal bool RemoveCell(Cell ce)
		{
			return this.cells.Remove(ce.Track.pCurrPos.X, ce.Track.pCurrPos.Y);
		}

		/// <summary>
		/// 按照指定的路段，路段前部的距离进行删除元胞
		/// </summary>
		/// <param name="rl">旋转坐标系所要用到的计算旋转角度的向量</param>
		/// <param name="iAheadSpace">前行距离数</param>
		internal bool RemoveCell(Lane rl, int iAheadSpace)
		{
			Point ipt = this.MakeCenterXY(rl, 1);
			Point iRealIndex = Coordinates.GetRealXY(ipt, rl.ToVector());
			return cells.Remove(iRealIndex.X, iRealIndex.Y);
		}
		
		/// <summary>
		/// 新的roadnode的哈希散列值由其中心Position的哈希值和其ID构成
		/// </summary>
		/// <returns></returns>
		private HashMatrix<Cell> cells = new HashMatrix<Cell>();
		
//		internal Cell this[int index]
//		{
//			get
//			{
//				return this.cells[index];
//			}
//		}
//		internal ICollection<int> Keys
//		{
//			get
//			{
//				return this.cells.Keys;
//			}
//		}
		
		#endregion
		
		

		/// <summary>
		/// 存贮本节点所有出边的哈希表，键值是代表边的RoadEdge哈希，值是代表RoadEdge
		/// </summary>
		private Dictionary<int, Way> dicEdge = new Dictionary<int,Way>();

		/// <summary>
		/// 提供对哈希矩阵内部元素的遍历
		/// </summary>
		/// <returns></returns>
		public IEnumerator<Cell> GetEnumerator()
		{
			return this.cells.GetEnumerator();
		}

		public ICollection RoadEdges
		{
			get
			{
				return this.dicEdge.Values;
			}
		}
		#region 用来保存邻接矩阵中节点出边的边成员,不应当使用RoadNetwork之外的类访问这些成员
		/// <summary>
		/// 注意在出边表中，保持roadedge的from字段是this节点，否则函数抛出异常
		/// </summary>
		/// <param name="roadEdge"></param>
		internal void AddWay(Way way)
		{
			if (way != null)
			{
				if (!Contains(way.GetHashCode()))
				{
					//加入判断是否是当前点的出边的信息防止出错
					if (way.XNodeFrom !=this)
					{
						throw new Exception("添加了不属于该顶点的边");
					}
					dicEdge.Add(way.GetHashCode(), way);
				}
				else
				{
					throw new ArgumentException("添加了重复的边！");
				}
			}
			else
			{
				throw new ArgumentNullException();
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
			dicEdge.Remove(re.GetHashCode());
		}
		internal void RemoveWay(XNode toRN)
		{
			if (toRN == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.obj);
			}
			dicEdge.Remove(Way.GetHashCode(this,toRN));
		}
		
		/// <summary>
		/// 查找方法，新的结构采用出边表
		/// </summary>
		/// <param name="toRoadNode">出节点</param>
		/// <returns></returns>
		public Way FindWay(XNode toRoadNode)
		{
			int iHashkey = Way.GetHashCode(this,toRoadNode);
			if (dicEdge.ContainsKey(iHashkey))
			{
				return dicEdge[iHashkey];
			}
			return null;
		}
		
		public bool Contains(int EdgeKey)
		{
			return dicEdge.ContainsKey(EdgeKey);
		}

		#endregion
		/// <summary>
		/// 控制RoadNodeID的数量
		/// </summary>
		private static int iRoadNodeID;
		[System.Obsolete("使用有参数的构造函数")]
		internal XNode()
		{
			this._EntityID = ++iRoadNodeID;
			Random rd = new Random();

			this.GISPosition = new OxyzPointF(rd.Next(65535), rd.Next(65535));
			// 直接使用上下文的数据结构,bug不应当使用上下文结构
			if (this.GISPosition._X == 0.0f && this.GISPosition._Y == 0.0f)
			{
				ThrowHelper.ThrowArgumentNullException("RoadNode产生了零坐标！");
			}
		}
		internal XNode(Point rltPostion)
		{
			this._EntityID = ++iRoadNodeID;
			Random rd = new Random();
			this.Grid = rltPostion;
			this.GISPosition = new OxyzPointF(rd.Next(65535), rd.Next(65535));
		}
		
		public override int GetHashCode()
		{
			int iHash = this.GISPosition.GetHashCode() +this.ID.GetHashCode();
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
			
			//通过操作另外一个集合进行
//			foreach (var item in new List<int>(this.Keys))
//			{
//				this[item].Drive(this);
//			}
			
			base.UpdateStatus();//基类调用了OnStatusChanged 进行绘图服务
		}

		protected override void OnStatusChanged()
		{
			this.InvokeService(this);
		}
		
		
		
		
		/// <summary>
		/// 处理交叉口的等待车辆数量
		/// </summary>
		internal override void ServeMobiles()
		{
//			while (this.MobilesInn.Count > 0)
//			{
//				if (this.SpaceCount == 0)//如果车道已经满了就不能处理队列了
//				{
//					break;
//				}else{
//					this.AddCell(this._mobilesInn.Dequeue());
//				}
//			}
//
		}
		
		
		/// <summary>
		/// a container to tell if  a point within a xnode is occupied!
		/// </summary>
		Dictionary<int,bool>  _occupiedPoints = new Dictionary<int,bool>();
		
		
		/// <summary>
		/// 判断指定车道前部第Ahead个位置处是否有元胞占据
		/// </summary>
		public bool IsOccupied(OxyzPoint opPoint)
		{
			//if this point is not added to dictionary ,add it then
			return this._occupiedPoints.ContainsKey(opPoint.GetHashCode());
		}

		internal XNode(OxyzPoint pointCenter)
		{
			this._EntityID = ++iRoadNodeID;
			//Random rd = new Random();
			this.SpatialGrid = pointCenter;
			
			//this.Grid = pointCenter;
		//	this.GISPosition = new OxyzPointF(rd.Next(65535), rd.Next(65535));
		}
		
		
	}
}

