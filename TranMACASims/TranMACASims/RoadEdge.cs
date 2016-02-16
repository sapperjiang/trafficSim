using System;
using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.UpdateRule;

namespace SubSys_SimDriving.TrafficModel
{
	public class RoadEdge : RoadEntity
	{
        /// <summary>
        /// ��ǰ����·�ε�ͬ��ʱ��
        /// </summary>
        public static int iTimeStep;

        /// <summary>
        /// ��ʵ������ĸ���ʱ��
        /// </summary>
        public int iCurrTimeStep;

        public TripCostAnalyzer _tripCostAnalyzer;

        private RoadEdge(){}

        [System.Obsolete("�ڲ�������Ҫ�޸�ʹ�ù�����������")]
        public RoadEdge(RoadNode fromRN, RoadNode toRN)
        {
            if (fromRN ==null && toRN == null)
            {
                throw new ArgumentNullException("�޷�ʹ�ÿյĽڵ㹹���");
            }
            this.rnFrom =fromRN;
            this.rnTo = toRN;

            this.synRuleChain = new SynchronicUpdateRuleChain(this);
            this.asynRuleChain = new AsynchronicUpdateRuleChain(this);

            ///��Ҫ���빤��ģʽ
            this.roadLaneChain = new RoadLaneChain();
            //RoadLane rl  = new RoadLane(this,LaneType.Straight);
            //this.roadLaneChain.Add(rl);
            //this.queWaitedCACell = new Queue<CACell>();
        }
        public RoadEdge(RoadNode fromRN, RoadNode toRN,TripCostAnalyzer tripCost):this(fromRN,toRN)
        {
            this._tripCostAnalyzer = tripCost;
        }

        public RoadNode rnFrom;
        public RoadNode rnTo;

        /// <summary>
        /// �ɸ�����ӵ�����з���������ͬ��
        /// </summary>
        /// <param name="rl"></param>
        public void AddRoadLane(RoadLane rl)
        {
            if (rl != null)
            {
                this.roadLaneChain.Add(rl);
                //ͬ�����������ĵ����ݼ�¼
                this.SimDrivingContext._roadLaneHashTable.Add(rl.GetHashCode(), rl);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        /// <summary>
        /// �ɸ���ɾ��������з���������ͬ��
        /// </summary>
        /// <param name="rl"></param>
        public void RemoveRoadLane(RoadLane rl)
        {
            if (rl != null)
            {
                this.roadLaneChain.Remove(rl);
                //ͬ�����������ĵ����ݼ�¼
                this.SimDrivingContext._roadLaneHashTable.Remove(rl.GetHashCode());
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        private int _tripCost;

        /// <summary>
        /// ·�εĽ�ͨ����/�ɱ�
        /// </summary>
        public int TripCost
        {
            get { return _tripCost; }
        }

        /// <summary>
        /// ����·�εĽ�ͨ�ɱ�
        /// </summary>
        public void UpdateTripCost()
        {
            if (this._tripCostAnalyzer != null)
            {
                this._tripCost = _tripCostAnalyzer.GetTripCost(this);
            }
            else
            {
                throw new System.MissingFieldException("û�к��ʵĳ��з��ü����࣡");
            }
        }
        
        /// <summary>
        /// ��������
        /// </summary>
		public SpeedLevel iSpeedLimit;

        /// <summary>
        /// �洢���ڲ��ĳ���roadlane
        /// </summary>
        public RoadLaneChain roadLaneChain;


        /// <summary>
        /// ������ʼ�ڵ�ͽ����ڵ����ߵĹ�ϣֵ
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return string.Concat(rnFrom.GetHashCode().ToString(),rnTo.GetHashCode().ToString()).GetHashCode();
        }
        /// <summary>
        /// ��̬�Ĺ�ϣ��������������ĳ���ߵĹ�ϣֵ
        /// </summary>
        public static int GetHashCode(RoadNode rnFrom,RoadNode rnTo)
        {
            return string.Concat(rnFrom.GetHashCode().ToString(), rnTo.GetHashCode().ToString()).GetHashCode();
        }

        public override void UpdateStatus()
        {
            //�����첽��Ϣ
            for (int i = 0; i < this.asynRuleChain.Count; i++)
            {
                UpdateRule.UpdateRule visitorRule = this.asynRuleChain[i];
                visitorRule.VisitUpdate(this);//.VisitUpdate();
            }
            ////����ͬ����Ϣ
            //foreach (UpdateRule.UpdateRule item in this.synRuleChain)
            //{
            //    if (item != null)
            //    {
            //        item.Update();//visitor.visit һ���������һ�������ߣ��ܶ�ķ�����
            //    }
            //}
        }
	}
	 
}
 
