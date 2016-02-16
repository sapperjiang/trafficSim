using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
    /// <summary>
    /// 同步更新规则，一个时间步长内全部更新的规则，不应当使用无参数的构造函数创建该类
    /// </summary>
	internal class SynchronicAgents:UpdateAgentChain
    {
        internal SynchronicAgents(){ }
        ///// <summary>
        ///// 测试继承的RoadEdge是否为null，不应当使用无参数的构造函数
        ///// </summary>
        ///// <param name="re"></param>
        //internal SynchronicUpdateAgentChain(RoadEdge re)
        //{
        //}

        internal override void AddUpdateAgent(Agent ur)
        {
            base.AddUpdateAgent(ur);
        }

	}
	 
}
 
