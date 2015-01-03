using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
	internal class VMSAgent : Agent
	{
        internal VMSAgent()
        {
            //this.strAgentName = AgentType.VMSAgent;

            this.priority = AgentPriority.Medium;
            //this.agentType = AgentType.Synchronization;
        }
        internal override void VisitUpdate(RoadNode rn)
        {
            throw new System.NotImplementedException();
        }
        internal override void VisitUpdate(RoadEdge re)
        {
            throw new System.NotImplementedException();
        }

        internal override void VisitUpdate(RoadLane re)
        {
            throw new System.NotImplementedException();
        }
	}
	 
}
 
