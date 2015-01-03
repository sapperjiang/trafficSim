using System;
using System.Collections;
using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.MathSupport;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.Agents;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// һ������RoadEdge�ĳ�����RoadLane����һ�������ǻ��ν���ڣ��Լ��Ժ����չ����
    /// </summary>
	internal class RoadEdge : RoadEntity
	{
        /// <summary>
        /// ��ǰ����·�ε�ͬ��ʱ��
        /// </summary>
        internal static int iTimeStep;

        #region ���캯��
        private RoadEdge(){}
        internal RoadEdge(RoadNode from, RoadNode to)
        {
            if (from ==null && to == null)
            {
                throw new ArgumentNullException("�޷�ʹ�ÿյĽڵ㹹���");
            }
            this.from =from;
            this.to = to;

            /////��ʼ�������б�
            //this.synAgents = new SynchronicAgents();
            //this.asynAgents = new AsynchronicAgents();

            ////��ʼ����Ⱥͳ���
            //this.iWidth = 6*this.i
            ///��Ҫ���빤��ģʽ
            this._lanes = new RoadLaneChain();
            //RoadLane rl = new RoadLane(this, LaneType.Straight);
            //this._lanes.Add(rl);//��ӷ���
            //this.queWaitedCACell = new Queue<CACell>();
            //this.Register();//ע���Լ�//������roadnetwork add��������ע��
        }
        internal RoadEdge(RoadNode from, RoadNode to,TripCostAnalyzer tripCost):this(from,to)
        {
            this._tripCostAnalyzer = tripCost;
        }
        #endregion

        internal RoadNode from;
        internal RoadNode to;
         
        #region ·���ڲ��ĳ�����ص����ݽṹ�Ͳ�������
        /// <summary>
        /// �ɸ�����ӵ�����з���������ͬ��
        /// </summary>
        /// <param name="rl"></param>
        internal void AddLane(RoadLane rl)
        {
            if (rl != null)
            {//��ֹ����˽϶�ĳ���
                if (this.Lanes.Count ==SimContext.SimSettings.iMaxWidth)
                {
                    throw new ArgumentOutOfRangeException("�޷���ӳ���" + SimContext.SimSettings.iMaxWidth + "������");
                }
                //��roadlane��ֵ�����ҵ�������roadlane��RoadEdge
                rl.parEntity = this;
                //ͬ�����������ĵ����ݼ�¼
                //this.SimDrivingContext.RoadLaneList.Add(rl.GetHashCode(), rl);
                rl.Register(rl);//

                //����laneRanking ��laneType���򣬲��뵽���ʵ�λ�ò��Ҹ���ǡ����
                //laneRanking���ڽ�����������
                int i = this._lanes.Count ;
                if (i == 0)//��һ��Ҫ��ӵĳ���
                {
                    this._lanes.Add(rl);
                    rl.Rank = 1;
                }

                while (i-->=1)//��������һ���������в������
                {
                    RoadLane rLane = this._lanes[i];//i�Ѿ���С��һ����
                    if (rLane.laneType > rl.laneType)
                    {
                        //���������laneRanking��ֵ��1
                        rLane.Rank += 1;
                        if (i==0)
                        {
                            this.Lanes.Insert(0, rl);//�������ұߵĳ���
                            rl.Rank = 1;
                        }
                    }//rank����һ����ͬ����
                    if (rLane.laneType <= rl.laneType)
                    {   //�����µ�lane����ǰ������i��Ҫ����֮������Ӧ����i+1
                        this._lanes.Insert(i+1, rl);
                        //rl.Rank = i+2;//rank ��������1
                        rl.Rank = i + 2;// this.Lanes.Count;
                        break;
                    }
			    }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        internal void AddLane(LaneType lt)
        {
            RoadLane rl = new RoadLane(this, lt);
            this.AddLane(rl);
        }
        /// <summary>
        /// �ɸ���ɾ��������з���������ͬ��
        /// </summary>
        /// <param name="rl"></param>
        //[System.Obsolete("Ӧ������ʵ�ʵ����ȷ��ɾ��������Ҫ�ĺ�������")]
        internal void RemoveLane(RoadLane rl)
        {
            if (rl != null)
            {
                for (int i = rl.Rank; i < this.Lanes.Count; i++)
                {
                    this.Lanes[i].Rank -= 1;
                }
                this._lanes.Remove(rl);//��rank�������ǵ�rank-1������
                //ͬ�����������ĵ����ݼ�¼
                //this.simContext.RoadLaneList.Remove(rl.GetHashCode());
                rl.UnRegiser(rl);
            }else
            {
                throw new ArgumentNullException();
            }
        }
        /// <summary>
        /// �洢���ڲ��ĳ���roadlane�������simContext ��ͬ
        /// </summary>
        private RoadLaneChain _lanes;
        internal RoadLaneChain Lanes
        {
            get
            {
                return this._lanes;
            }
        }
        #endregion

        #region ���з���

        internal TripCostAnalyzer _tripCostAnalyzer;

        private int _tripCost;
        /// <summary>
        /// ·�εĽ�ͨ����/�ɱ�
        /// </summary>
        internal int TripCost
        {
            get { return _tripCost; }
        }

        /// <summary>
        /// ����·�εĽ�ͨ�ɱ�
        /// </summary>
        internal void UpdateTripCost()
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
        #endregion

        #region ��ϣ����
        /// <summary>
        /// ������ʼ�ڵ�ͽ����ڵ����ߵĹ�ϣֵ
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return string.Concat(from.GetHashCode().ToString(),to.GetHashCode().ToString()).GetHashCode();
        }
        /// <summary>
        /// ��̬�Ĺ�ϣ��������������ĳ���ߵĹ�ϣֵ
        /// </summary>
        internal static int GetHashCode(RoadNode rnFrom,RoadNode rnTo)
        {
            return string.Concat(rnFrom.GetHashCode().ToString(), rnTo.GetHashCode().ToString()).GetHashCode();
        }
        #endregion

        internal override void UpdateStatus()
        {
            //�����첽��Ϣ
            for (int i = 0; i < this.asynAgents.Count; i++)
            {
                Agents.Agent visitorAgent = this.asynAgents[i];
                visitorAgent.VisitUpdate(this);//.VisitUpdate();
            }
            ////����ͬ����Ϣ
            //foreach (UpdateAgent.UpdateAgent item in this.synAgentChain)
            //{
            //    if (item != null)
            //    {
            //        item.Update();//visitor.visit һ���������һ�������ߣ��ܶ�ķ�����
            //    }
            //}
        }

        /// <summary>
        /// ���������ȥ�յ�����
        /// </summary>
        /// <returns></returns>
        [System.Obsolete("������������п��ܲ���������ȫһ����·�ζ˵����꣬�ú�������ͼ�����һ���⣬��ʽ����Ӧ��ʹ��")]
        internal MyPoint ToVector()
        {
            MyPoint p= new MyPoint(to.Postion.X - from.Postion.X, to.Postion.Y - from.Postion.Y);
            if (p.X==0.0f&&p.Y ==0.0f)
            {
                p.X = 12;
                p.Y = 12;
                //throw new Exception("RoadEdge��������������");
            }
            return p;
        }

        /// <summary>
        /// ��ȡ��һ��Road�ڲ�����RoadEdge���Ӧ�ķ���·��
        /// </summary>
        /// <returns></returns>
        internal RoadEdge GetReverse()
        {
            return simContext.INetWork.FindRoadEdge(this.to, this.from);
        }

        /// <summary>
        /// �洢�ӽ����roadNode����·�εĳ�������Ϊʱ�䳬ǰһ��ʱ�䲽����
        /// ��Ҫ��������з�ֹһ��Ԫ���ȸ��µ�·�Σ�Ȼ����·�����ָ���һ�θ�������
        /// </summary>
        Queue<Cell> queWaitedCell = new Queue<Cell>();

        /// <summary>
        /// �޸��źŵ����
        /// </summary>
        /// <param name="sl">�µ��źŵ�</param>
        /// <param name="lt">Ҫ�޸ĵĳ�������</param>
        internal void ModifySignalGroup(SignalLight sl, LaneType lt)
        {
            foreach (RoadLane rl in this.Lanes)
            {
                if (rl.laneType == lt)
                {
                    rl.SignalLight = sl;
                }
            }
        }
        /// <summary>
        /// ·������
        /// </summary>
        internal SpeedLevel iSpeedLimit;
    }
	 
}
 
