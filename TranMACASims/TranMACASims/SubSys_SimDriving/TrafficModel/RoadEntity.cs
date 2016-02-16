using SubSys_SimDriving;
using SubSys_SimDriving.Agents;
using System.Collections.Generic;
using System;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving
{
    /// <summary>
    /// ��ʾ�����RoadNode��·��RoadEdge������RoadLane,��·Road�Ļ�����
    ///  �н���ģʽAbstractColleague
    /// </summary>
	internal abstract class RoadEntity : TrafficEntity
	{
        /// <summary>
        /// �����н��ߣ�������Transfer,acceptӦ���ɾ��������д���
        /// �н���ģʽ������������֮���ͨ�š���������ҵ��Է����ֻ��
        /// </summary>
        ////protected CellDevolveMediator Mediator= new CellDevolver();
        //internal virtual bool Transfer(Cell ce, RoadEntity re)
        //{
        //    this.Mediator.Transfer(ce, re);
        //    return true;
        //}

        //internal abstract void Accept(Cell ce);


        /// <summary>
        /// �ö�������ĸ���ʱ��
        /// </summary>
        internal int iCurrTimeStep;

        /// <summary>
        /// ·��Ĭ�ϵĳ��ȣ�Ӧ����GUI����·����ʱ��ʹ��
        /// </summary>
        internal int iLength=120;
		 
		internal int iwidth;


        /// <summary>
        /// �洢���϶�����첽���µĹ���
        /// </summary>
        internal AsynchronicAgents asynAgents=new AsynchronicAgents();

        /// <summary>
        /// �洢���϶����ͬ�����µĹ���
        /// </summary>
        internal SynchronicAgents synAgents= new SynchronicAgents();

        /// <summary>
        /// �������еķ����ߣ������ڲ�Ԫ���ĸ���
        /// </summary>
        internal abstract void UpdateStatus();
        //{
        //    //�����첽��Ϣ
        //    for (int i = 0; i < this.asynAgentChain.Count; i++)
        //    {
        //        UpdateAgent.UpdateAgent visitorAgent = this.asynAgentChain[i];
        //        visitorAgent.VisitUpdate(this);//.VisitUpdate();
        //    }
        //    ////����ͬ����Ϣ
        //    //foreach (UpdateAgent.UpdateAgent item in this.synAgentChain)
        //    //{
        //    //    if (item != null)
        //    //    {
        //    //        item.Update();//visitor.visit һ���������һ�������ߣ��ܶ�ķ�����
        //    //    }
        //    //}
        //}

        /// <summary>
        ///RoadEdge��item ��Agent��visitor �൱��item.accept(visitor)
        /// </summary>
        /// <param name="ur"></param>
        internal void AcceptSynAgent(Agents.Agent ur)
        {
            if (ur != null)
            {
                //��ӵ�����������
                this.simContext.UpdateAgentList.Add(ur.GetHashCode(), ur);

                this.synAgents.Add(ur);
            }
            else
            {
                throw new ArgumentNullException("�յĸ��¹���");
            }
        }

        /// <summary>
        /// ����첽���¹���
        /// </summary>
        /// <param name="ur"></param>
        internal void AcceptAsynAgent(Agents.Agent ur)
        {
            if (ur != null)
            {
                //��ӵ�����������
                this.simContext.UpdateAgentList.Add(ur.GetHashCode(), ur);
                this.asynAgents.Add(ur);
            }
            else
            {
                throw new ArgumentNullException("�յĸ��¹���");
            }
        }

        public override int GetHashCode()
        {
            throw new Exception("��Ӧ��ʹ��");
            //return base.GetHashCode();
        }
		 
	}
	 
}
 
