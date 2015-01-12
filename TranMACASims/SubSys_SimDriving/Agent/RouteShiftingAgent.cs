using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
	internal class RouteShiftingAgent : AbstractAgent
	{
        internal RouteShiftingAgent()
        {
            //this.strAgentName = AgentType.VMSAgent;
            this.priority = AgentPriority.Medium;
            //this.agentType = AgentType.Synchronization;
        }
        internal override void VisitUpdate(Way roadEdge)
        {
            if (roadEdge == null)
            {
                throw new System.ArgumentException("������ģʽ���ʶ�����Ϊ�գ�RoadEntityû�и�ֵ��");
            }
            //����ÿ������
            foreach (Lane rl in roadEdge.Lanes)
            {
                this.VisitUpdate(rl);
            }
        }

        internal override void VisitUpdate(Lane roadLane)
        {
            throw new System.NotImplementedException();
        }
        internal override void VisitUpdate(XNode roadLane)
        {
            throw new System.NotImplementedException();
        }
	}
	 
}
 
