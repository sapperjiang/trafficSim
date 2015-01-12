using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
	internal class VMSAgent : AbstractAgent
	{
        internal VMSAgent()
        {
            //this.strAgentName = AgentType.VMSAgent;

            this.priority = AgentPriority.Medium;
            //this.agentType = AgentType.Synchronization;
        }
        internal override void VisitUpdate(XNode rn)
        {
            throw new System.NotImplementedException();
        }
        internal override void VisitUpdate(Way re)
        {
            throw new System.NotImplementedException();
        }

        internal override void VisitUpdate(Lane re)
        {
            throw new System.NotImplementedException();
        }
	}
	 
}
 
