using System;
using SubSys_MathUtility;
using SubSys_SimDriving;
using System.Collections.Generic;
using SubSys_SimDriving.SysSimContext;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// ��ͨʵ������ĳ���
    /// </summary>
    public class RoadLane : RoadEntity, IComparable<RoadLane>, IComparer<RoadLane>
	{
        private static int iRoadLaneCount=0;
        ///// <summary>
        ///// ÿ�θ���CurrTimeStep��ʱ����Զ�����ȴ��б�
        ///// </summary>
        public override int iLength
        {
            get
            {
                return this.Container.iLength;
            }
        }
        public override int iWidth
        {
            get
            {
                return SimSettings.iCarWidth;
            }
        }

        public override EntityShape Shape
        {
            get
            {
                EntityShape eShape = base.Shape;

                if (eShape.Count == 0)//shape û�г�ʼ��
                {
                    CreateShape(eShape);

                    //EntityShape es = this.Container.EntityShape;

                    //MyPoint pUnitNormVect = VectorTools.GetNormalVector(this.Container.ToVector());
                    //MyPoint mpOffset = new MyPoint(pUnitNormVect._X * (this.Rank - 1), pUnitNormVect._Y * (this.Rank - 1));
                    ////ƽ������
                    //MyPoint pFirst = Coordinates.Offset(es[0], mpOffset);
                    ////�����յ�
                    //MyPoint pFEnd = Coordinates.Offset( es[es.Count-1], mpOffset);

                    ////��ӵ�shape
                    //eShape.Add(pFirst);
                    //eShape.Add(pFEnd);

                }
                return eShape;
            }
        }

        private  void CreateShape(EntityShape eShape)
        {

            EntityShape es = this.Container.Shape;

            MyPoint pNorm = VectorTools.GetNormalVector(this.Container.ToVector());
            MyPoint mpOffset = new MyPoint(pNorm._X*(this.Rank - 0.5f),pNorm._Y * (this.Rank - 0.5f));
            //ƽ������
            MyPoint pFirst = Coordinates.Offset(es[0], mpOffset);
            //�����յ�
            MyPoint pFEnd = Coordinates.Offset(es[es.Count - 1], mpOffset);

         
            MyPoint mp = new MyPoint(pFEnd._X-pFirst._X,pFEnd._Y-pFirst._Y);
            int iLoopCount = this.iLength;//Ԫ�����ȣ���ʼ���μ�registerservice

            float xSplit = mp._X / iLoopCount;//������������
            float ySplit = mp._Y / iLoopCount;//������������

            //MyPoint tep=new MyPoint(pFirst._X + iLoopCount * xSplit, pFirst._Y + iLoopCount * ySplit);
            //System.Diagnostics.Debug.Assert(pFEnd==tep);
           
            eShape.Add(pFirst);
            for (int i = 1; i < iLoopCount; i++)//x��
            {   //�е�
                eShape.Add(new MyPoint(pFirst._X + (i-0.5f) * xSplit, pFirst._Y + (i-0.5f) * ySplit));   
            }
            eShape.Add(pFEnd);
        }
     
        internal bool IsLaneBlocked(int iAheadSpace)
        {
            return this.iLastPos-1 <= iAheadSpace ;
        }
        /// <summary>
        /// �۲���ģʽ�и���֪ͨ�ڲ�Ԫ���޸�/����״̬�Ĵ���
        /// </summary>
     
   
        /// <summary>
        /// ���������һ��Ԫ����λ�ã�Ӧ����Y����
        /// </summary>
        private int _ilastPos;

        /// <summary>
        /// ��ȡ���������һ��Ԫ����λ�ã����û��Ԫ���򷵻س�������
        /// </summary>
        internal int iLastPos
        {
            get
            {
                Cell ce = this.cells.PeekLast();
                if (ce== null)
                {
                    this._ilastPos = this.iLength;
                }
                else //if (this._ilastPos > ce.RltPos.Y)
                {
                    this._ilastPos = ce.RelativePosition.Y;
                }
                return _ilastPos;
            
            }
        }

        /// <summary>
        /// �жϴӵ�·��㴦��iAheadSpace���Ƿ���Ԫ��
        /// </summary>
        /// <param name="iAheadSpace"></param>
        /// <returns></returns>
        internal bool IsLaneEmpty(int iAheadSpace)
        {
            if (this.cells.PeekLast().RelativePosition.Y<iAheadSpace)
            {
                return false;//���һ��Ԫ����λ��С�ڳ�ͷʱ��Ͳ�Ϊ��  
            }
            return true;
        }

        /// <summary>
        ///���������򣬴��ڲ೵����ʼ�ĵڼ�������,�����Գ�����������
        /// </summary>
        public int Rank;

        public int[] PrevCarPos;

        /// <summary>
        /// �ֳ������ź�
        /// </summary>
        internal SignalLight SignalLight;


        private RoadEdge GetContainer()
        {
            return this.Container as RoadEdge;
        }
        internal void PlaySignal(int iCrtTimeStep)
        {
            RoadNode rN = this.GetContainer().roadNodeTo;
            if (SignalLight == null)//���źŽ����
            {
                rN.UnblockLane(this);
                return;
            }
            if (this.SignalLight.IsGreen(iCrtTimeStep) == false)
            {//��ƻ����ǻƵ�������
                rN.BlockLane(this);
            }
            else//�̵�
            {
                rN.UnblockLane(this);
            }
        }

        public override MyPoint ToVector()
        {
            MyPoint pA = this.Shape[0];
            MyPoint pB = this.Shape[this.Shape.Count - 1];
            return new MyPoint(pB._X-pA._X,pB._Y-pA._Y);
        }
        
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
        internal RoadLane(LaneType lt):this(null,lt)
        {
        }
        /// <summary>
        /// û�н����ڲ�ע�ᣬӦ����������ߵ���registere����ע��
        /// </summary>
        /// <param name="re"></param>
        /// <param name="lt"></param>
        internal RoadLane(RoadEdge re,LaneType lt)
        {
            this.PrevCarPos = new int[512];
            this.PrevCarPos[0] = -1;

            Container = re;
            this.laneType = lt;
            this._id = RoadLane.iRoadLaneCount++;

        }
        public void AddCell(Cell ce)
        {
            //��������ֵ��
            ce.Container = this;
            //�޸�����
            ce.RelativePosition = new Point(this.Rank, ce.RelativePosition.Y);

            System.Diagnostics.Debug.Assert(ce.RelativePosition.Y < this.iLastPos);

            this.cells.Enqueue(ce);
        }
        public Cell RemoveCell()
        {
            return this.cells.Dequeue();
        }
		internal LaneType laneType;
		private CellLinkedQueue cells = new CellLinkedQueue();

        private Queue<Cell> waitedQueue = new Queue<Cell>();

        /// <summary>
        /// ע������
        /// </summary>
        /// <param name="ce"></param>
        public void EnterWaitedQueue(Cell ce)
        {
            //��������ֵ��
            ce.Container = this;
            this.waitedQueue.Enqueue(ce);
        }
        private void DisposeWaitedQueue()
        {
            while (this.waitedQueue.Count > 0)
            {
                if (this.iLastPos == 0)//��������Ѿ����˾Ͳ��ܴ��������
                {
                    break;
                }
                this.AddCell(this.waitedQueue.Dequeue());
            }
        }

        /// <summary>
        /// û�е���visitorģʽ���������и��ӵķ��񡢴���ȴ�����
        /// </summary>
        [System.Obsolete("�������и��ӵķ���ʹ��� �ȴ�����")]
        public override void UpdateStatus()
        {
            base.UpdateStatus();
        }
        protected override void OnStatusChanged()
        {
            this.DisposeWaitedQueue();//����ȴ���Ԫ��

            //���û������־����
            this.InvokeServices(this);//������־��¼roadLane����
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
        public Cell this[int index]
        {
            get 
            {
                return this.cells[index];
            }
        }
        public int CellCount 
        {
            get
            {
                return this.cells.Count;
            }
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            return this.cells.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
            //return base.GetHashCode();
        }
    }
	 
}
 
