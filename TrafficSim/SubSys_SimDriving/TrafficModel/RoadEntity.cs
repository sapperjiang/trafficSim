using SubSys_SimDriving;
using SubSys_SimDriving.Agents;
using System.Collections.Generic;
using System;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving
{
    /// <summary>
    /// 表示交叉口RoadNode，路段RoadEdge，车道RoadLane,道路Road的基类型
    ///  中介者模式AbstractColleague
    /// </summary>
	internal abstract class RoadEntity : TrafficEntity
	{
        /// <summary>
        /// 抽象中介者，定义了Transfer,accept应当由具体的类进行处理
        /// 中介者模式仅仅隔离两者之间的通信。例如如何找到对方这种活儿
        /// </summary>
        ////protected CellDevolveMediator Mediator= new CellDevolver();
        //internal virtual bool Transfer(Cell ce, RoadEntity re)
        //{
        //    this.Mediator.Transfer(ce, re);
        //    return true;
        //}

        //internal abstract void Accept(Cell ce);


        /// <summary>
        /// 该对象子类的更新时刻
        /// </summary>
        internal int iCurrTimeStep;

        /// <summary>
        /// 路段默认的长度，应当在GUI建立路网的时候使用
        /// </summary>
        internal int iLength=120;
		 
		internal int iwidth;


        /// <summary>
        /// 存储边上定义的异步更新的规则
        /// </summary>
        internal AsynchronicAgents asynAgents=new AsynchronicAgents();

        /// <summary>
        /// 存储边上定义的同步更新的规则
        /// </summary>
        internal SynchronicAgents synAgents= new SynchronicAgents();

        /// <summary>
        /// 调用所有的访问者，进行内部元胞的更新
        /// </summary>
        internal abstract void UpdateStatus();
        //{
        //    //更新异步消息
        //    for (int i = 0; i < this.asynAgentChain.Count; i++)
        //    {
        //        UpdateAgent.UpdateAgent visitorAgent = this.asynAgentChain[i];
        //        visitorAgent.VisitUpdate(this);//.VisitUpdate();
        //    }
        //    ////更新同步消息
        //    //foreach (UpdateAgent.UpdateAgent item in this.synAgentChain)
        //    //{
        //    //    if (item != null)
        //    //    {
        //    //        item.Update();//visitor.visit 一个规则就是一个访问者，很多的访问者
        //    //    }
        //    //}
        //}

        /// <summary>
        ///RoadEdge是item ，Agent是visitor 相当于item.accept(visitor)
        /// </summary>
        /// <param name="ur"></param>
        internal void AcceptSynAgent(Agents.Agent ur)
        {
            if (ur != null)
            {
                //添加到仿真上下文
                this.simContext.UpdateAgentList.Add(ur.GetHashCode(), ur);

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
        internal void AcceptAsynAgent(Agents.Agent ur)
        {
            if (ur != null)
            {
                //添加到仿真上下文
                this.simContext.UpdateAgentList.Add(ur.GetHashCode(), ur);
                this.asynAgents.Add(ur);
            }
            else
            {
                throw new ArgumentNullException("空的更新规则");
            }
        }

        public override int GetHashCode()
        {
            throw new Exception("不应当使用");
            //return base.GetHashCode();
        }
		 
	}
	 
}
 
