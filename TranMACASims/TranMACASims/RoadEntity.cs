using SubSys_SimDriving;
using SubSys_SimDriving.UpdateRule;
using System.Collections.Generic;
using System;

namespace SubSys_SimDriving
{
	public abstract class RoadEntity : TrafficEntity
	{
        /// <summary>
        /// Ԫ�������Ŀ����6
        /// </summary>
        protected static int iCellWidth = 6;

        public int iWidth4Test = 120;

        public int iWidth;
		 
		public int iLength;


        /// <summary>
        /// �洢���϶�����첽���µĹ���
        /// </summary>
        public AsynchronicUpdateRuleChain asynRuleChain;

        /// <summary>
        /// �洢���϶����ͬ�����µĹ���
        /// </summary>
        public SynchronicUpdateRuleChain synRuleChain;

        /// <summary>
        /// �������еķ����ߣ������ڲ�Ԫ���ĸ���
        /// </summary>
        public abstract void UpdateStatus();
        //{
        //    //�����첽��Ϣ
        //    for (int i = 0; i < this.asynRuleChain.Count; i++)
        //    {
        //        UpdateRule.UpdateRule visitorRule = this.asynRuleChain[i];
        //        visitorRule.VisitUpdate(this);//.VisitUpdate();
        //    }
        //    ////����ͬ����Ϣ
        //    //foreach (UpdateRule.UpdateRule item in this.synRuleChain)
        //    //{
        //    //    if (item != null)
        //    //    {
        //    //        item.Update();//visitor.visit һ���������һ�������ߣ��ܶ�ķ�����
        //    //    }
        //    //}
        //}

        /// <summary>
        ///RoadEdge��item ��rule��visitor �൱��item.accept(visitor)
        /// </summary>
        /// <param name="ur"></param>
        public void AcceptSynchronicRule(UpdateRule.UpdateRule ur)
        {
            if (ur != null)
            {
                //��ӵ�����������
                this.SimDrivingContext._updateRuleHashTable.Add(ur.GetHashCode(), ur);

                this.synRuleChain.Add(ur);
            }
            else
            {
                throw new ArgumentNullException("�յĸ��¹���");
            }
        }

        public void AcceptAsynchronicRule(UpdateRule.UpdateRule ur)
        {
            if (ur != null)
            {
                //��ӵ�����������
                this.SimDrivingContext._updateRuleHashTable.Add(ur.GetHashCode(), ur);
                this.asynRuleChain.Add(ur);
            }
            else
            {
                throw new ArgumentNullException("�յĸ��¹���");
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
		 
	}
	 
}
 
