using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
    /// <summary>
    /// ͬ�����¹���һ��ʱ�䲽����ȫ�����µĹ��򣬲�Ӧ��ʹ���޲����Ĺ��캯����������
    /// </summary>
	internal class SynchronicAgents:UpdateAgentChain
    {
        internal SynchronicAgents(){ }
        ///// <summary>
        ///// ���Լ̳е�RoadEdge�Ƿ�Ϊnull����Ӧ��ʹ���޲����Ĺ��캯��
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
 
