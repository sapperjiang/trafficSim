using System;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
    /// <summary>
    /// 这个是访问者模式的抽象visitor，各种各样的visit（update）
    /// </summary>
	public abstract class Agent
	{
        /// <summary>
        /// 被访问对象
        /// </summary>
        //protected RoadEntity RoadEntity;
        protected SysSimContext.SimContext simContext = SysSimContext.SimContext.GetInstance();
        internal IRoadNetWork RoadNet = RoadNetWork.GetInstance();
        internal AgentPriority priority;
        internal AgentType agentType;
        internal string strAgentName;

        ///// <summary>
        ///// 准备访问被访问者
        ///// </summary>
        ///// <param name="roadentity"></param>
        //internal virtual void PrepareVisit(RoadEntity roadentity)
        //{
        //    RoadEntity = roadentity;
        //}
        /// <summary>
        /// visit实际进行访问的地方
        /// </summary>
        internal abstract void VisitUpdate(RoadEdge re);
        //{
        //    System.Windows.Forms.MessageBox.Show("调用了基类型的VisitUpdate RoadEdge方法");
        //    //throw new Exception("call  base processes");
        //}

        internal abstract void VisitUpdate(RoadNode re);
        //{
        //    System.Windows.Forms.MessageBox.Show("调用了基类型的VisitUpdate RoadNode方法");
        //    //throw new Exception("call  base processes");
        //}
        internal virtual void VisitUpdate(RoadLane re) 
        {
            throw new NotImplementedException("不应当调用base的visit");
        }
        //{
        //    System.Windows.Forms.MessageBox.Show("调用了基类型的VisitUpdate RoadLane方法");
        //    //throw new Exception("call  base processes");
        //}
	}
	 
}
 
