using System;
using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.UpdateRule;

namespace SubSys_SimDriving.TrafficModel
{
	public class RoadEdge : RoadEntity
	{
        /// <summary>
        /// 当前所有路段的同步时刻
        /// </summary>
        public static int iTimeStep;

        /// <summary>
        /// 该实例对象的更新时刻
        /// </summary>
        public int iCurrTimeStep;

        public TripCostAnalyzer _tripCostAnalyzer;

        private RoadEdge(){}

        [System.Obsolete("内部可能需要修改使用工厂方法创建")]
        public RoadEdge(RoadNode fromRN, RoadNode toRN)
        {
            if (fromRN ==null && toRN == null)
            {
                throw new ArgumentNullException("无法使用空的节点构造边");
            }
            this.rnFrom =fromRN;
            this.rnTo = toRN;

            this.synRuleChain = new SynchronicUpdateRuleChain(this);
            this.asynRuleChain = new AsynchronicUpdateRuleChain(this);

            ///需要引入工厂模式
            this.roadLaneChain = new RoadLaneChain();
            //RoadLane rl  = new RoadLane(this,LaneType.Straight);
            //this.roadLaneChain.Add(rl);
            //this.queWaitedCACell = new Queue<CACell>();
        }
        public RoadEdge(RoadNode fromRN, RoadNode toRN,TripCostAnalyzer tripCost):this(fromRN,toRN)
        {
            this._tripCostAnalyzer = tripCost;
        }

        public RoadNode rnFrom;
        public RoadNode rnTo;

        /// <summary>
        /// 由负责添加的类进行仿真上下文同步
        /// </summary>
        /// <param name="rl"></param>
        public void AddRoadLane(RoadLane rl)
        {
            if (rl != null)
            {
                this.roadLaneChain.Add(rl);
                //同步仿真上下文的数据记录
                this.SimDrivingContext._roadLaneHashTable.Add(rl.GetHashCode(), rl);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        /// <summary>
        /// 由负责删除的类进行仿真上下文同步
        /// </summary>
        /// <param name="rl"></param>
        public void RemoveRoadLane(RoadLane rl)
        {
            if (rl != null)
            {
                this.roadLaneChain.Remove(rl);
                //同步仿真上下文的数据记录
                this.SimDrivingContext._roadLaneHashTable.Remove(rl.GetHashCode());
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        private int _tripCost;

        /// <summary>
        /// 路段的交通费用/成本
        /// </summary>
        public int TripCost
        {
            get { return _tripCost; }
        }

        /// <summary>
        /// 更新路段的交通成本
        /// </summary>
        public void UpdateTripCost()
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
        
        /// <summary>
        /// 车道限速
        /// </summary>
		public SpeedLevel iSpeedLimit;

        /// <summary>
        /// 存储边内部的车道roadlane
        /// </summary>
        public RoadLaneChain roadLaneChain;


        /// <summary>
        /// 根据起始节点和结束节点计算边的哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return string.Concat(rnFrom.GetHashCode().ToString(),rnTo.GetHashCode().ToString()).GetHashCode();
        }
        /// <summary>
        /// 静态的哈希函数，用来计算某条边的哈希值
        /// </summary>
        public static int GetHashCode(RoadNode rnFrom,RoadNode rnTo)
        {
            return string.Concat(rnFrom.GetHashCode().ToString(), rnTo.GetHashCode().ToString()).GetHashCode();
        }

        public override void UpdateStatus()
        {
            //更新异步消息
            for (int i = 0; i < this.asynRuleChain.Count; i++)
            {
                UpdateRule.UpdateRule visitorRule = this.asynRuleChain[i];
                visitorRule.VisitUpdate(this);//.VisitUpdate();
            }
            ////更新同步消息
            //foreach (UpdateRule.UpdateRule item in this.synRuleChain)
            //{
            //    if (item != null)
            //    {
            //        item.Update();//visitor.visit 一个规则就是一个访问者，很多的访问者
            //    }
            //}
        }
	}
	 
}
 
