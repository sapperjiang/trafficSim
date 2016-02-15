using System;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
    /// <summary>
    /// ����Ƿ�����ģʽ�ĳ���visitor�����ָ�����visit��update��
    /// </summary>
	public abstract class AbstractAgent
	{
        /// <summary>
        /// �����ʶ���
        /// </summary>
        protected SimContext simContext = SimContext.GetInstance();
        internal IRoadNet roadNet = RoadNet.GetInstance();
        internal AgentPriority priority;
        internal AgentType agentType;
        internal string strAgentName;


        /// <summary>
        /// visitʵ�ʽ��з��ʵĵط�
        /// </summary>
        internal abstract void VisitUpdate(Way re);


        internal abstract void VisitUpdate(XNode re);

        internal virtual void VisitUpdate(Lane re) 
        {
            throw new NotImplementedException("��Ӧ������base��visit");
        }
	}
	 
}
 
