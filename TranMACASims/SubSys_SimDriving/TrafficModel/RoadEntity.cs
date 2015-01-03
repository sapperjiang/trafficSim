using SubSys_SimDriving;
using SubSys_SimDriving.Agents;
using System.Collections.Generic;
using System;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving
{
    /// <summary>
    /// ��ʾ�����RoadNode��·��RoadEdge������RoadLane,��·Road�Ļ�����
    ///  ������ĸ���ʹ���˷�����ģʽ
    /// </summary>
	public abstract class RoadEntity : TrafficEntity
	{
         /// <summary>
        /// �ö�������ĸ���ʱ��
        /// </summary>
        internal int iCurrTimeStep;

        /// <summary>
        /// �洢���϶�����첽���µĹ���
        /// </summary>
        internal AsynchronicAgents asynAgents = new AsynchronicAgents();

        /// <summary>
        /// �洢���϶����ͬ�����µĹ���
        /// </summary>
        internal SynchronicAgents synAgents = new SynchronicAgents();

        /// <summary>
        /// �������еķ����ߣ������ڲ�Ԫ���ĸ���
        /// </summary>
        public virtual void UpdateStatus() 
        {
            this.OnStatusChanged();
        }

        protected abstract void OnStatusChanged();

        /// <summary>
        ///RoadEdge��item ��Agent��visitor �൱��item.accept(visitor)
        /// </summary>
        /// <param name="ur"></param>
        public void AcceptSynAgent(Agents.Agent ur)
        {
            if (ur != null)
            {
                //��ӵ�����������
                this.ISimCtx.Agents.Add(ur.GetHashCode(), ur);
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
        public void AcceptAsynAgent(Agents.Agent ur)
        {
            if (ur != null)
            {
                //��ӵ�����������
                this.ISimCtx.Agents.Add(ur.GetHashCode(), ur);
                this.asynAgents.Add(ur);
            }
            else
            {
                throw new ArgumentNullException("�յĸ��¹���");
            }
        }

	}
	 
}
 
