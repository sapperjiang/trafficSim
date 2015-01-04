using System;
using System.Collections;
using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_MathUtility;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.Agents;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// 一般来讲RoadEdge的长度与RoadLane长度一样，但是环形交叉口，以及以后的拓展除外
	/// </summary>
	public class RoadEdge : RoadEntity
	{
		/// <summary>
		/// 当前所有路段的同步时刻
		/// </summary>
		internal static int iTimeStep;

		internal static int iRoadEdgeCount = 0;
		#region 构造函数 部分成员的初始化由RegiserService进行
		//public RoadEdge():this(new RoadNode(),new RoadNode()){}
		/// <summary>
		/// 强制先构造节点
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		internal RoadEdge(RoadNode from, RoadNode to)
		{
			if (from ==null && to == null)
			{
				throw new ArgumentNullException("无法使用空的节点构造边");
			}
			this.roadNodeFrom =from;
			this.roadNodeTo = to;
			this._lanes = new RoadLaneChain();

			this._id = RoadEdge.iRoadEdgeCount++;

		}
		
		internal RoadEdge(Point from, Point to)
		{
			
	
			this.roadNodeFrom =new RoadNode(from);;
			this.roadNodeTo =  new RoadNode(to);
			this._lanes = new RoadLaneChain();

			this._id = RoadEdge.iRoadEdgeCount++;

		}
		
		internal RoadEdge(RoadNode from, RoadNode to,TripCostAnalyzer tripCost):this(from,to)
		{
			this._tripCostAnalyzer = tripCost;
		}
		#endregion

		public override int iLength
		{
			get {
				int preNodeDistance = Coordinates.Distance(this.roadNodeFrom.RelativePosition, this.roadNodeTo.RelativePosition);

				int iRealLength = preNodeDistance- 2* SimSettings.iMaxLanes;
				if (iRealLength<10)
				{
					ThrowHelper.ThrowArgumentException("两个节点之间距离太短");
				}
				return iRealLength;
			}
		}
		public override int iWidth
		{
			get { return this.Lanes.Count*SimSettings.iCarWidth; }
		}

		public RoadNode roadNodeFrom;
		public RoadNode roadNodeTo;
		
		#region 路段内部的车道相关的数据结构和操作函数
		/// <summary>
		/// 由负责添加的类进行仿真上下文同步,内部进行了RoadLane注册
		/// </summary>
		/// <param name="rl"></param>
		internal void AddLane(RoadLane rl)
		{
			if (rl != null)
			{
				//防止添加了较多的车道
				if (this.Lanes.Count ==SimSettings.iMaxLanes)
				{
					throw new ArgumentOutOfRangeException("无法添加超过" + SimSettings.iMaxLanes + "个车道");
				}
				rl.Container = this;//两行代码一定不要高反了

				rl.Register();// //同步仿真上下文的数据记录

				
				//按照laneRanking 和laneType排序，插入到合适的位置并且给予恰当的
				//laneRanking便于进行坐标索引
				int i = this._lanes.Count ;
				if (i == 0)//第一个要添加的车道
				{
					this._lanes.Add(rl);
					rl.Rank = 1;
				}
				while (i-->=1)//个数超过一个车道进行插入操作
				{
					RoadLane rLane = this._lanes[i];//i已经变小了一个数
					if (rLane.laneType > rl.laneType)
					{
						//将后续大的laneRanking的值增1
						rLane.Rank += 1;
						if (i==0)
						{
							this.Lanes.Insert(0, rl);//插入最右边的车道
							rl.Rank = 1;
						}
					}//rank最大的一个相同车道
					if (rLane.laneType <= rl.laneType)
					{   //插入新的lane，当前索引是i，要插入之后，索引应当是i+1
						this._lanes.Insert(i+1, rl);
						//rl.Rank = i+2;//rank 比索引大1
						rl.Rank = i + 2;// this.Lanes.Count;
						break;
					}
				}
				//this.ilength =端点的长度//端点坐标之间的距离
			}
			else
			{
				throw new ArgumentNullException();
			}
		}
		internal void AddLane(LaneType lt)
		{
			RoadLane rl = new RoadLane(this, lt);
			this.AddLane(rl);
		}

		internal RoadLane GetLane(RoadLane rlCurr, string strLorR)
		{
			RoadLane rl = null;
			switch (strLorR)
			{
				case "L":
					if (rlCurr.Rank>1)//大于1才有左边的车道
					{
						rl = this._lanes[rl.Rank - 2];//左侧的索引为rank-2；
					}
					break;
				case "R":
					if (rlCurr.Rank < this.Lanes.Count)//大于1才有左边的车道
					{
						rl = this._lanes[rl.Rank];//右侧的索引为rank
					}
					break;
				default:
					ThrowHelper.ThrowArgumentException("错误的额参数2");
					break;
			}
			return rl;

		}
		/// <summary>
		/// 由负责删除的类进行仿真上下文同步
		/// </summary>
		/// <param name="rl"></param>
		//[System.Obsolete("应当根据实际的情况确定删除车道需要的函数类型")]
		internal void RemoveLane(RoadLane rl)
		{
			if (rl != null)
			{
				for (int i = rl.Rank; i < this.Lanes.Count; i++)
				{
					this.Lanes[i].Rank -= 1;
				}
				this._lanes.Remove(rl);//第rank个车道是第rank-1个类型
				//同步仿真上下文的数据记录
				rl.UnRegiser();//进行反注册
			}else
			{
				throw new ArgumentNullException();
			}
		}
		/// <summary>
		/// 存储边内部的车道roadlane，这个与simContext 不同
		/// </summary>
		private RoadLaneChain _lanes;
		public RoadLaneChain Lanes
		{
			get
			{
				return this._lanes;
			}
		}
		#endregion

		#region 出行费用

		internal TripCostAnalyzer _tripCostAnalyzer;

		private int _tripCost;
		/// <summary>
		/// 路段的交通费用/成本
		/// </summary>
		internal int TripCost
		{
			get { return _tripCost; }
		}

		/// <summary>
		/// 更新路段的交通成本
		/// </summary>
		[System.Obsolete("不建议使用更新成功")]
		internal void UpdateTripCost()
		{
			if (this._tripCostAnalyzer != null)
			{
				//this._tripCost = _tripCostAnalyzer.GetTripCost(this);
			}
			else
			{
				throw new System.MissingFieldException("没有合适的出行费用计算类！");
			}
		}
		#endregion

		#region 哈希函数
		/// <summary>
		/// 根据起始节点和结束节点计算边的哈希值
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			//return RoadEdge.iRoadEdgeCount;
			return string.Concat(roadNodeFrom.GetHashCode().ToString(), roadNodeTo.GetHashCode().ToString()).GetHashCode();
		}
		/// <summary>
		/// 静态的哈希函数，用来计算某条边的哈希值
		/// </summary>
		internal static int GetHashCode(RoadNode rnFrom,RoadNode rnTo)
		{
			return string.Concat(rnFrom.GetHashCode().ToString(), rnTo.GetHashCode().ToString()).GetHashCode();
		}
		#endregion

		/// <summary>
		/// 调用visitor 模式如vmsagent等。然后驱动元胞模型，然后调用所有服务
		/// </summary>
		public override void UpdateStatus()
		{
			////更新异步消息
			for (int i = 0; i < this.asynAgents.Count; i++)
			{
				Agents.Agent visitorAgent = this.asynAgents[i];
				visitorAgent.VisitUpdate(this);//.VisitUpdate();
			}
			//用roadedge调用元胞的drive 目的在于让车辆可以换道
			foreach (var lane in this.Lanes)
			{
				for (int i = 0; i < lane.CellCount; i++)
				{
					lane[i].Drive(this);//这是一个元胞的方法
				}
				lane.UpdateStatus();//调用注册在车道上的服务。
			}
			base.UpdateStatus();//调用注册在路段上的服务，如RoadEdgePaintService
		}
		/// <summary>
		/// 路段的OnStatusChanged委托给RoadLane处理
		/// </summary>
		[System.Obsolete("调用了服务")]
		protected override void OnStatusChanged()
		{
			this.InvokeServices(this);
		}

		/// <summary>
		/// 起点向量减去终点向量
		/// </summary>
		/// <returns></returns>
		[System.Obsolete("随机数发生器有可能产生两个完全一样的路段端点坐标，该函数的试图解决这一问题，正式程序不应当使用")]
		public override MyPoint ToVector()
		{
			MyPoint p = new MyPoint(roadNodeTo.RelativePosition.X - roadNodeFrom.RelativePosition.X, roadNodeTo.RelativePosition.Y - roadNodeFrom.RelativePosition.Y);
			if (p._X == 0.0f && p._Y == 0.0f)
			{
				p._X = 12;
				p._Y = 12;
				//throw new Exception("RoadEdge产生了零向量！");
			}
			return p;
		}

		public override EntityShape Shape
		{
			get
			{
				EntityShape eShape = base.Shape;

				if (eShape.Count == 0)//shape 没有初始化
				{
					int pX =this.roadNodeTo.RelativePosition.X - this.roadNodeFrom.RelativePosition.X;
					int pY =  this.roadNodeTo.RelativePosition.Y - this.roadNodeFrom.RelativePosition.Y;
					//向量等分
					float dLq = this.iLength + 2 * SimSettings.iMaxLanes;//分母
					float xSplit = pX / dLq;//自身有正负号
					float ySplit = pY / dLq;//自身有正负号
					//计算起点
					int iOffset = SimSettings.iMaxLanes;
					eShape.Add(new MyPoint(this.roadNodeFrom.RelativePosition.X + iOffset * xSplit, this.roadNodeFrom.RelativePosition.Y + iOffset * ySplit));
					//计算终点
					eShape.Add(new MyPoint(this.roadNodeTo.RelativePosition.X - iOffset * xSplit, this.roadNodeTo.RelativePosition.Y - iOffset * ySplit));
				}
				return eShape;
			}
		}

		/// <summary>
		/// 获取在一个Road内部的与RoadEdge相对应的反向路段
		/// </summary>
		/// <returns></returns>
		public RoadEdge GetReverse()
		{
			return (ISimCtx.NetWork as IRoadNetWork).FindRoadEdge(this.roadNodeTo, this.roadNodeFrom);
		}

		/// <summary>
		/// 存储从交叉口roadNode进入路段的车辆，因为时间超前一个时间步长，
		/// 需要放入队列中防止一个元胞先更新到路段，然后在路段内又更新一次更新两次
		/// </summary>
		private Queue<Cell> queWaitedCell = new Queue<Cell>();

		/// <summary>
		/// 修改信号灯组合
		/// </summary>
		/// <param name="sl">新的信号灯</param>
		/// <param name="lt">要修改的车道类型</param>
		public void ModifySignalGroup(SignalLight sl, LaneType lt)
		{
			foreach (RoadLane rl in this.Lanes)
			{
				if (rl.laneType == lt)
				{
					rl.SignalLight = sl;
				}
			}
		}
		/// <summary>
		/// 路段限速
		/// </summary>
		internal SpeedLevel iSpeedLimit;

		
		
	}
	
}

