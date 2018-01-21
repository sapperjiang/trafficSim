using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
	public class SignalLightAgent : AbstractAgent
	{
        internal SignalLightAgent()
        {
            //this.strAgentName = AgentType.SignalLightAgent;
            this.priority = AgentPriority.Top;
            //this.agentType = AgentType.Synchronization;
        }

        internal override void VisitUpdate(Way roadEdge)
        {
            if (roadEdge == null)
            {
                throw new System.ArgumentNullException("访问者模式访问对象不能为空，RoadEntity没有赋值！");
            }
            foreach (Lane rl in roadEdge.Lanes)
            {
                rl.PlaySignal(simContext.iCurrTimeStep);
            }
        }

        /// <summary>
        /// 信号灯更新的方法
        /// </summary>
        /// <param name="rN"></param>
      //  [System.Obsolete("to be restructed")]
        internal override void VisitUpdate(XNode rN)
        {
            Way reverse = null;
            foreach (Way re in rN.Ways)
            {   //找到反向的边
                reverse = roadNet.FindWay(re.To, re.From);
                System.Diagnostics.Debug.Assert(reverse != null);
                if (reverse!=null)
                {
                    this.VisitUpdate(reverse);//调用重载更新车道
                }
            }
        }
	}
	 
}
 
