using System;
using SubSys_SimDriving;
using System.Collections.Generic;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 交通实体里面的车道
    /// </summary>
    public class RoadLane : RoadEntity, IComparable<RoadLane>, IComparer<RoadLane>
	{
        public RoadEdge parentEntity;

        public RoadLane(RoadEdge re)
        {
            parentEntity = re;
            this.laneType = LaneType.Straight;//默认为直行车道
        }
        public RoadLane(RoadEdge re,LaneType lt)
        {
            parentEntity = re;
            this.laneType = lt;
        }
		public LaneType laneType;
		 
		public CACellChain cellChain = new CACellChain();

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
		 
        public int CompareTo(RoadLane other)
        {
            if (this.laneType == other.laneType)
            {
                return 0;
            }
            return this.laneType > other.laneType ? 1 : -1;
        }
        public int Compare(RoadLane x, RoadLane y)
        {
            return x.CompareTo(y);
        }
        /// <summary>
        /// 静态方法
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static int CompareTo(RoadLane from, RoadLane to)
        {
            return from.CompareTo(to);
        }
    }
	 
}
 
