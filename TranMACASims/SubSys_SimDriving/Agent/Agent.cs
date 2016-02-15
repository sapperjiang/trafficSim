using System;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
    /// <summary>
    /// 这个是访问者模式的抽象visitor，各种各样的visit（update）
    /// </summary>
	public abstract class AbstractAgent
	{
        /// <summary>
        /// 被访问对象
        /// </summary>
        protected SimContext simContext = SimContext.GetInstance();
        internal IRoadNet roadNet = RoadNet.GetInstance();
        internal AgentPriority priority;
        internal AgentType agentType;
        internal string strAgentName;


        /// <summary>
        /// visit实际进行访问的地方
        /// </summary>
        internal abstract void VisitUpdate(Way re);


        internal abstract void VisitUpdate(XNode re);

        internal virtual void VisitUpdate(Lane re) 
        {
            throw new NotImplementedException("不应当调用base的visit");
        }
	}
	 
}
 
