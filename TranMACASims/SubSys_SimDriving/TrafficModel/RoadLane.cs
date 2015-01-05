using System;
using SubSys_MathUtility;
using SubSys_SimDriving;
using System.Collections.Generic;
using SubSys_SimDriving.SysSimContext;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 交通实体里面的车道
    /// </summary>
    public class RoadLane : RoadEntity, IComparable<RoadLane>, IComparer<RoadLane>
	{
        private static int iRoadLaneCount=0;
        ///// <summary>
        ///// 每次更新CurrTimeStep的时候就自动处理等待列表
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

                if (eShape.Count == 0)//shape 没有初始化
                {
                    CreateShape(eShape);

                    //EntityShape es = this.Container.EntityShape;

                    //MyPoint pUnitNormVect = VectorTools.GetNormalVector(this.Container.ToVector());
                    //MyPoint mpOffset = new MyPoint(pUnitNormVect._X * (this.Rank - 1), pUnitNormVect._Y * (this.Rank - 1));
                    ////平移坐标
                    //MyPoint pFirst = Coordinates.Offset(es[0], mpOffset);
                    ////计算终点
                    //MyPoint pFEnd = Coordinates.Offset( es[es.Count-1], mpOffset);

                    ////添加到shape
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
            //平移坐标
            MyPoint pFirst = Coordinates.Offset(es[0], mpOffset);
            //计算终点
            MyPoint pFEnd = Coordinates.Offset(es[es.Count - 1], mpOffset);

         
            MyPoint mp = new MyPoint(pFEnd._X-pFirst._X,pFEnd._Y-pFirst._Y);
            int iLoopCount = this.iLength;//元胞长度，初始化参见registerservice

            float xSplit = mp._X / iLoopCount;//自身有正负号
            float ySplit = mp._Y / iLoopCount;//自身有正负号

            //MyPoint tep=new MyPoint(pFirst._X + iLoopCount * xSplit, pFirst._Y + iLoopCount * ySplit);
            //System.Diagnostics.Debug.Assert(pFEnd==tep);
           
            eShape.Add(pFirst);
            for (int i = 1; i < iLoopCount; i++)//x行
            {   //中点
                eShape.Add(new MyPoint(pFirst._X + (i-0.5f) * xSplit, pFirst._Y + (i-0.5f) * ySplit));   
            }
            eShape.Add(pFEnd);
        }
     
        internal bool IsLaneBlocked(int iAheadSpace)
        {
            return this.iLastPos-1 <= iAheadSpace ;
        }
        /// <summary>
        /// 观察者模式中负责通知内部元胞修改/保存状态的代码
        /// </summary>
     
   
        /// <summary>
        /// 车道的最后一个元胞的位置，应当是Y坐标
        /// </summary>
        private int _ilastPos;

        /// <summary>
        /// 获取车道的最后一个元宝的位置，如果没有元胞则返回车道长度
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
        /// 判断从道路起点处到iAheadSpace处是否有元胞
        /// </summary>
        /// <param name="iAheadSpace"></param>
        /// <returns></returns>
        internal bool IsLaneEmpty(int iAheadSpace)
        {
            if (this.cells.PeekLast().RelativePosition.Y<iAheadSpace)
            {
                return false;//最后一个元胞的位置小于车头时距就不为空  
            }
            return true;
        }

        /// <summary>
        ///车道的排序，从内侧车道开始的第几个车道,用来对车道进行排序
        /// </summary>
        public int Rank;

        public int[] PrevCarPos;

        /// <summary>
        /// 分车道的信号
        /// </summary>
        internal SignalLight SignalLight;


        private RoadEdge GetContainer()
        {
            return this.Container as RoadEdge;
        }
        internal void PlaySignal(int iCrtTimeStep)
        {
            RoadNode rN = this.GetContainer().roadNodeTo;
            if (SignalLight == null)//无信号交叉口
            {
                rN.UnblockLane(this);
                return;
            }
            if (this.SignalLight.IsGreen(iCrtTimeStep) == false)
            {//红灯或者是黄灯则阻塞
                rN.BlockLane(this);
            }
            else//绿灯
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
        
        [System.Obsolete("禁止使用的构造形")]
        private RoadLane()
        {
            this.laneType = LaneType.Straight; 
        }

        /// <summary>
        /// 调用了两参数构造形
        /// </summary>
        /// <param name="re"></param>
        internal RoadLane(RoadEdge re):this(re,LaneType.Straight){
        }
        internal RoadLane(LaneType lt):this(null,lt)
        {
        }
        /// <summary>
        /// 没有进行内部注册，应当由其管理者调用registere进行注册
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
            //给容器赋值；
            ce.Container = this;
            //修改坐标
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
        /// 注册容器
        /// </summary>
        /// <param name="ce"></param>
        public void EnterWaitedQueue(Cell ce)
        {
            //给容器赋值；
            ce.Container = this;
            this.waitedQueue.Enqueue(ce);
        }
        private void DisposeWaitedQueue()
        {
            while (this.waitedQueue.Count > 0)
            {
                if (this.iLastPos == 0)//如果车道已经满了就不能处理队列了
                {
                    break;
                }
                this.AddCell(this.waitedQueue.Dequeue());
            }
        }

        /// <summary>
        /// 没有调用visitor模式，调用所有附加的服务、处理等待队列
        /// </summary>
        [System.Obsolete("调用所有附加的服务和处理 等待队列")]
        public override void UpdateStatus()
        {
            base.UpdateStatus();
        }
        protected override void OnStatusChanged()
        {
            this.DisposeWaitedQueue();//处理等待的元胞

            //调用基类的日志服务
            this.InvokeServices(this);//利用日志记录roadLane变量
        }


         #region 两个类相互比较的算法和函数
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
        /// 静态方法
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
 
