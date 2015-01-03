using SubSys_SimDriving;
using SubSys_SimDriving.Agents;
using System.Collections.Generic;
using System;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving
{
    /// <summary>
    /// 表示交叉口RoadNode，路段RoadEdge，车道RoadLane,道路Road的基类型
    ///  智能体的更新使用了访问者模式
    /// </summary>
	public abstract class RoadEntity : TrafficEntity
	{
         /// <summary>
        /// 该对象子类的更新时刻
        /// </summary>
        internal int iCurrTimeStep;

        /// <summary>
        /// 存储边上定义的异步更新的规则
        /// </summary>
        internal AsynchronicAgents asynAgents = new AsynchronicAgents();

        /// <summary>
        /// 存储边上定义的同步更新的规则
        /// </summary>
        internal SynchronicAgents synAgents = new SynchronicAgents();

        /// <summary>
        /// 调用所有的访问者，进行内部元胞的更新
        /// </summary>
        public virtual void UpdateStatus() 
        {
            this.OnStatusChanged();
        }

        protected abstract void OnStatusChanged();

        /// <summary>
        ///RoadEdge是item ，Agent是visitor 相当于item.accept(visitor)
        /// </summary>
        /// <param name="ur"></param>
        public void AcceptSynAgent(Agents.Agent ur)
        {
            if (ur != null)
            {
                //添加到仿真上下文
                this.ISimCtx.Agents.Add(ur.GetHashCode(), ur);
                this.synAgents.Add(ur);
            }
            else
            {
                throw new ArgumentNullException("空的更新规则");
            }
        }
        /// <summary>
        /// 添加异步更新规则
        /// </summary>
        /// <param name="ur"></param>
        public void AcceptAsynAgent(Agents.Agent ur)
        {
            if (ur != null)
            {
                //添加到仿真上下文
                this.ISimCtx.Agents.Add(ur.GetHashCode(), ur);
                this.asynAgents.Add(ur);
            }
            else
            {
                throw new ArgumentNullException("空的更新规则");
            }
        }

	}
	 
}
 
