using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
	internal class VMSAgent : Agent
	{
        internal VMSAgent()
        {
            this.strAgentName = AgentName.VMSAgent;

            this.priority = AgentPriority.Medium;
            this.agentType = AgentType.Synchronization;
        }
        internal override void VisitUpdate(RoadNode rn)
        {
            System.Windows.Forms.MessageBox.Show("VMSAgent Updated");
        }
        internal override void VisitUpdate(RoadEdge re)
        {
            System.Windows.Forms.MessageBox.Show("VMSAgent Updated");
        }

        internal override void VisitUpdate(RoadLane re)
        {
            System.Windows.Forms.MessageBox.Show("VMSAgent Updated");
        }
	}
	 
}
 
