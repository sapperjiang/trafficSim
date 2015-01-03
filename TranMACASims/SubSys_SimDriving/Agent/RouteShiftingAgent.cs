using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
	internal class RouteShiftingAgent : Agent
	{
        internal RouteShiftingAgent()
        {
            //this.strAgentName = AgentType.VMSAgent;
            this.priority = AgentPriority.Medium;
            //this.agentType = AgentType.Synchronization;
        }
        internal override void VisitUpdate(RoadEdge roadEdge)
        {
            if (roadEdge == null)
            {
                throw new System.ArgumentException("������ģʽ���ʶ�����Ϊ�գ�RoadEntityû�и�ֵ��");
            }
            //����ÿ������
            foreach (RoadLane rl in roadEdge.Lanes)
            {
                this.VisitUpdate(rl);
            }
        }

        internal override void VisitUpdate(RoadLane roadLane)
        {
            throw new System.NotImplementedException();
        }
        internal override void VisitUpdate(RoadNode roadLane)
        {
            throw new System.NotImplementedException();
        }
	}
	 
}
 
