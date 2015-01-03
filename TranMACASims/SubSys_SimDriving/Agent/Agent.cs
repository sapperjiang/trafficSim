using System;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
    /// <summary>
    /// ����Ƿ�����ģʽ�ĳ���visitor�����ָ�����visit��update��
    /// </summary>
	public abstract class Agent
	{
        /// <summary>
        /// �����ʶ���
        /// </summary>
        //protected RoadEntity RoadEntity;
        protected SysSimContext.SimContext simContext = SysSimContext.SimContext.GetInstance();
        internal IRoadNetWork RoadNet = RoadNetWork.GetInstance();
        internal AgentPriority priority;
        internal AgentType agentType;
        internal string strAgentName;

        ///// <summary>
        ///// ׼�����ʱ�������
        ///// </summary>
        ///// <param name="roadentity"></param>
        //internal virtual void PrepareVisit(RoadEntity roadentity)
        //{
        //    RoadEntity = roadentity;
        //}
        /// <summary>
        /// visitʵ�ʽ��з��ʵĵط�
        /// </summary>
        internal abstract void VisitUpdate(RoadEdge re);
        //{
        //    System.Windows.Forms.MessageBox.Show("�����˻����͵�VisitUpdate RoadEdge����");
        //    //throw new Exception("call  base processes");
        //}

        internal abstract void VisitUpdate(RoadNode re);
        //{
        //    System.Windows.Forms.MessageBox.Show("�����˻����͵�VisitUpdate RoadNode����");
        //    //throw new Exception("call  base processes");
        //}
        internal virtual void VisitUpdate(RoadLane re) 
        {
            throw new NotImplementedException("��Ӧ������base��visit");
        }
        //{
        //    System.Windows.Forms.MessageBox.Show("�����˻����͵�VisitUpdate RoadLane����");
        //    //throw new Exception("call  base processes");
        //}
	}
	 
}
 
