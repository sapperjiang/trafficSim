using System;
using System.Collections;
using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.MathSupport;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.Agents;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 一般来讲RoadEdge的长度与RoadLane长度一样，但是环形交叉口，以及以后的拓展除外
    /// </summary>
	internal class RoadEdge : RoadEntity
	{
        /// <summary>
        /// 当前所有路段的同步时刻
        /// </summary>
        internal static int iTimeStep;

        #region 构造函数
        private RoadEdge(){}
        internal RoadEdge(RoadNode from, RoadNode to)
        {
            if (from ==null && to == null)
            {
                throw new ArgumentNullException("无法使用空的节点构造边");
            }
            this.from =from;
            this.to = to;

            /////初始化规则列表
            //this.synAgents = new SynchronicAgents();
            //this.asynAgents = new AsynchronicAgents();

            ////初始化宽度和长度
            //this.iWidth = 6*this.i
            ///需要引入工厂模式
            this._lanes = new RoadLaneChain();
            //RoadLane rl = new RoadLane(this, LaneType.Straight);
            //this._lanes.Add(rl);//添加方法
            //this.queWaitedCACell = new Queue<CACell>();
            //this.Register();//注册自己//更改由roadnetwork add方法进行注册
        }
        internal RoadEdge(RoadNode from, RoadNode to,TripCostAnalyzer tripCost):this(from,to)
        {
            this._tripCostAnalyzer = tripCost;
        }
        #endregion

        internal RoadNode from;
        internal RoadNode to;
         
        #region 路段内部的车道相关的数据结构和操作函数
        /// <summary>
        /// 由负责添加的类进行仿真上下文同步
        /// </summary>
        /// <param name="rl"></param>
        internal void AddLane(RoadLane rl)
        {
            if (rl != null)
            {//防止添加了较多的车道
                if (this.Lanes.Count ==SimContext.SimSettings.iMaxWidth)
                {
                    throw new ArgumentOutOfRangeException("无法添加超过" + SimContext.SimSettings.iMaxWidth + "个车道");
                }
                //给roadlane赋值便于找到包含该roadlane的RoadEdge
                rl.parEntity = this;
                //同步仿真上下文的数据记录
                //this.SimDrivingContext.RoadLaneList.Add(rl.GetHashCode(), rl);
                rl.Register(rl);//

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
                //this.simContext.RoadLaneList.Remove(rl.GetHashCode());
                rl.UnRegiser(rl);
            }else
            {
                throw new ArgumentNullException();
            }
        }
        /// <summary>
        /// 存储边内部的车道roadlane，这个与simContext 不同
        /// </summary>
        private RoadLaneChain _lanes;
        internal RoadLaneChain Lanes
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
        internal void UpdateTripCost()
        {
            if (this._tripCostAnalyzer != null)
            {
                this._tripCost = _tripCostAnalyzer.GetTripCost(this);
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
            return string.Concat(from.GetHashCode().ToString(),to.GetHashCode().ToString()).GetHashCode();
        }
        /// <summary>
        /// 静态的哈希函数，用来计算某条边的哈希值
        /// </summary>
        internal static int GetHashCode(RoadNode rnFrom,RoadNode rnTo)
        {
            return string.Concat(rnFrom.GetHashCode().ToString(), rnTo.GetHashCode().ToString()).GetHashCode();
        }
        #endregion

        internal override void UpdateStatus()
        {
            //更新异步消息
            for (int i = 0; i < this.asynAgents.Count; i++)
            {
                Agents.Agent visitorAgent = this.asynAgents[i];
                visitorAgent.VisitUpdate(this);//.VisitUpdate();
            }
            ////更新同步消息
            //foreach (UpdateAgent.UpdateAgent item in this.synAgentChain)
            //{
            //    if (item != null)
            //    {
            //        item.Update();//visitor.visit 一个规则就是一个访问者，很多的访问者
            //    }
            //}
        }

        /// <summary>
        /// 起点向量减去终点向量
        /// </summary>
        /// <returns></returns>
        [System.Obsolete("随机数发生器有可能产生两个完全一样的路段端点坐标，该函数的试图解决这一问题，正式程序不应当使用")]
        internal MyPoint ToVector()
        {
            MyPoint p= new MyPoint(to.Postion.X - from.Postion.X, to.Postion.Y - from.Postion.Y);
            if (p.X==0.0f&&p.Y ==0.0f)
            {
                p.X = 12;
                p.Y = 12;
                //throw new Exception("RoadEdge产生了零向量！");
            }
            return p;
        }

        /// <summary>
        /// 获取在一个Road内部的与RoadEdge相对应的反向路段
        /// </summary>
        /// <returns></returns>
        internal RoadEdge GetReverse()
        {
            return simContext.INetWork.FindRoadEdge(this.to, this.from);
        }

        /// <summary>
        /// 存储从交叉口roadNode进入路段的车辆，因为时间超前一个时间步长，
        /// 需要放入队列中防止一个元胞先更新到路段，然后在路段内又更新一次更新两次
        /// </summary>
        Queue<Cell> queWaitedCell = new Queue<Cell>();

        /// <summary>
        /// 修改信号灯组合
        /// </summary>
        /// <param name="sl">新的信号灯</param>
        /// <param name="lt">要修改的车道类型</param>
        internal void ModifySignalGroup(SignalLight sl, LaneType lt)
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
 
