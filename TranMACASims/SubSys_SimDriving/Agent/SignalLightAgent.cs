using System.Collections.Generic;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
	public class SignalLightAgent : Agent
	{
        internal SignalLightAgent()
        {
            //this.strAgentName = AgentType.SignalLightAgent;
            this.priority = AgentPriority.Top;
            //this.agentType = AgentType.Synchronization;
        }

        internal override void VisitUpdate(RoadEdge roadEdge)
        {
            if (roadEdge == null)
            {
                throw new System.ArgumentNullException("访问者模式访问对象不能为空，RoadEntity没有赋值！");
            }
            foreach (RoadLane rl in roadEdge.Lanes)
            {
                rl.PlaySignal(SimContext.iCurrTimeStep);
            }
        }

        /// <summary>
        /// 信号灯更新的方法
        /// </summary>
        /// <param name="rN"></param>
        internal override void VisitUpdate(RoadNode rN)
        {
            RoadEdge reverse = null;
            foreach (RoadEdge re in rN.RoadEdges)
            {   //找到反向的边
                reverse = RoadNet.FindRoadEdge(re.roadNodeTo, re.roadNodeFrom);
                System.Diagnostics.Debug.Assert(reverse != null);
                if (reverse!=null)
                {
                    this.VisitUpdate(reverse);//调用重载更新车道
                }
            }
        }
	}
	 
}
 
