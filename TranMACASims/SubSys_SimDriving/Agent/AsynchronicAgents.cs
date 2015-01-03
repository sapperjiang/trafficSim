using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
	/// <summary>
    /// 异步更新规则一个时间步长只更新一个的规则,不应当使用无参数的构造函数创建该类
	/// </summary>
    internal class AsynchronicAgents : UpdateAgentChain
	{
        internal AsynchronicAgents()
        { }
         /// <summary>
        /// 测试继承的RoadEdge是否为null，不应当使用无参数的构造函数
        /// </summary>
        /// <param name="re"></param>
        //internal AsynchronicUpdateAgentChain(RoadEdge re)
        //{
        //}

	}
	 
}
 
