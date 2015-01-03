using System;
using SubSys_SimDriving;
using System.Collections.Generic;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.MathSupport;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// ��ͨʵ������ĳ���
    /// </summary>
    internal class RoadLane : RoadEntity, IComparable<RoadLane>, IComparer<RoadLane>
	{
        /// <summary>
        /// ÿ�θ���CurrTimeStep��ʱ����Զ�����ȴ��б�
        /// </summary>
        internal int CurrTimeStep
        {
            get 
            {
                return this.iCurrTimeStep;
            }
            set
            {
                this.iCurrTimeStep = value;
                this.DisposeWaitedQueue();
            }
        }

        internal bool IsLaneBlocked(int iAheadSpace)
        {
            return this.iLastPos-1 >= iAheadSpace ;
        }

        private void OnChangeStatus()
        { 

        }
        private void DisposeWaitedQueue()
        {
            while (this.waitedQueue.Count>0)
            {
                this.cells.Enqueue(this.waitedQueue.Dequeue());
            }
        }

        private int ilastPos;
        internal int iLastPos
        {
            get
            {
                if (this.ilastPos > this.cells.PeekLast().iPos)
                {
                    this.ilastPos = this.cells.PeekLast().iPos ;
                }
                return ilastPos;                    
            }
            set
            {
                if (this.ilastPos>value)
                {
                    this.ilastPos = value;
                }
            }
        }
        /// <summary>
        /// �жϴӵ�·��㴦��iAheadSpace���Ƿ���Ԫ��
        /// </summary>
        /// <param name="iAheadSpace"></param>
        /// <returns></returns>
        internal bool IsLaneEmpty(int iAheadSpace)
        {
            if (this.cells.PeekLast().iPos<iAheadSpace)
            {
                return false;//���һ��Ԫ����λ��С�ڳ�ͷʱ��Ͳ�Ϊ��  
            }
            return true;
        }

        /// <summary>
        ///���������򣬴��ڲ೵����ʼ�ĵڼ�������,�����Գ�����������
        /// </summary>
        internal int Rank;

        /// <summary>
        /// �ֳ������ź�
        /// </summary>
        internal SignalLight SignalLight;

        internal void PlaySignal(int iCrtTimeStep)
        {
            if (SignalLight == null)//���źŽ����
            {
                this.parEntity.to.UnblockRoadLane(this);
                return;
                //throw new ArgumentNullException("�յ��źŵ�");
            }
            if (this.SignalLight.IsGreen(iCrtTimeStep) == false)
            {//��ƻ����ǻƵ�������
                this.parEntity.to.BlockRoadLane(this);
            }
            else//�̵�
            {
                this.parEntity.to.UnblockRoadLane(this);
            }
        }

        internal RoadEdge parEntity;
        
        [System.Obsolete("��ֹʹ�õĹ�����")]
        private RoadLane()
        {
            this.laneType = LaneType.Straight; 
        }

        /// <summary>
        /// ������������������
        /// </summary>
        /// <param name="re"></param>
        internal RoadLane(RoadEdge re):this(re,LaneType.Straight){
        }
        /// <summary>
        /// û�н����ڲ�ע�ᣬӦ����������ߵ���registere����ע��
        /// </summary>
        /// <param name="re"></param>
        /// <param name="lt"></param>
        internal RoadLane(RoadEdge re,LaneType lt)
        {
            parEntity = re;
            this.laneType = lt;
        }
        
		internal LaneType laneType;
		internal CellChain cells = new CellChain();
        internal Queue<Cell> waitedQueue = new Queue<Cell>();
        
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
       /// ����ϵת������������ϵ
       /// </summary>
       /// <param name="i"></param>
       /// <returns></returns>
        internal Index ToCenterXY( int iLaneAhead)
        {
            Index point = new Index(this.Rank, iLaneAhead);
            //����ϵƽ�Ʊ任,������x�͵ײ�y����ϵƽ�Ƶ���������ϵ
            //ʹ�ù̶���6��Ԫ���Ŀ��
            Coordinates.TranslateYAxis(ref point, SysSimContext.SimContext.SimSettings.iMaxWidth);
            return point;
        }
 
         #region �������໥�Ƚϵ��㷨�ͺ���
        public int CompareTo(RoadLane other)
        {
            if (this.laneType == other.laneType)
            {
                return 0;
            }
            return this.laneType > other.laneType ? 1 : -1;
        }
        public int Compare(RoadLane x, RoadLane y)
        {
            return x.CompareTo(y);
        }
        /// <summary>
        /// ��̬����
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static int CompareTo(RoadLane from, RoadLane to)
        {
            return from.CompareTo(to);
        }
        #endregion
    }
	 
}
 
