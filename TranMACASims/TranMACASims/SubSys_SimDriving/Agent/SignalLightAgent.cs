using System.Collections.Generic;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
	internal class SignalLightAgent : Agent
	{
        internal SignalLightAgent()
        {
            this.strAgentName = AgentName.TrafficLightAgent;
            this.priority = AgentPriority.Top;
            this.agentType = AgentType.Synchronization;
        }

        internal override void VisitUpdate(RoadEdge roadEdge)
        {
            if (roadEdge == null)
            {
                throw new System.ArgumentNullException("������ģʽ���ʶ�����Ϊ�գ�RoadEntityû�и�ֵ��");
            }
            foreach (RoadLane rl in roadEdge.Lanes)
            {
                rl.PlaySignal(SimContext.iCurrTimeStep);
            }
        }

        internal override void VisitUpdate(RoadLane roadLane)
        {
            throw new System.NotImplementedException("û��ʵ��");
            //System.Windows.Forms.MessageBox.Show("DecelerateAgent Updated");
        }
        /// <summary>
        /// �źŵƸ��µķ���
        /// </summary>
        /// <param name="rN"></param>
        internal override void VisitUpdate(RoadNode rN)
        {
            RoadEdge reverse = null;
            foreach (RoadEdge re in rN.RoadEdges)
            {   //�ҵ�����ı�
                reverse = RoadNet.FindRoadEdge(re.to, re.from);
                System.Diagnostics.Debug.Assert(reverse != null);
                if (reverse!=null)
                {
                    this.VisitUpdate(reverse);//�������ظ��³���
                }
            }
        }
	}
	 
}
 
