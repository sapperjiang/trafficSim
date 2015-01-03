using System;
using SubSys_SimDriving;
using System.Collections.Generic;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.MathSupport;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 交通实体里面的车道
    /// </summary>
    internal class RoadLane : RoadEntity, IComparable<RoadLane>, IComparer<RoadLane>
	{
        /// <summary>
        /// 每次更新CurrTimeStep的时候就自动处理等待列表
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
        /// 判断从道路起点处到iAheadSpace处是否有元胞
        /// </summary>
        /// <param name="iAheadSpace"></param>
        /// <returns></returns>
        internal bool IsLaneEmpty(int iAheadSpace)
        {
            if (this.cells.PeekLast().iPos<iAheadSpace)
            {
                return false;//最后一个元胞的位置小于车头时距就不为空  
            }
            return true;
        }

        /// <summary>
        ///车道的排序，从内侧车道开始的第几个车道,用来对车道进行排序
        /// </summary>
        internal int Rank;

        /// <summary>
        /// 分车道的信号
        /// </summary>
        internal SignalLight SignalLight;

        internal void PlaySignal(int iCrtTimeStep)
        {
            if (SignalLight == null)//无信号交叉口
            {
                this.parEntity.to.UnblockRoadLane(this);
                return;
                //throw new ArgumentNullException("空的信号灯");
            }
            if (this.SignalLight.IsGreen(iCrtTimeStep) == false)
            {//红灯或者是黄灯则阻塞
                this.parEntity.to.BlockRoadLane(this);
            }
            else//绿灯
            {
                this.parEntity.to.UnblockRoadLane(this);
            }
        }

        internal RoadEdge parEntity;
        
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
        /// <summary>
        /// 没有进行内部注册，应当由其管理者调用registere进行注册
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
            //更新异步消息
            for (int i = 0; i < this.asynAgents.Count; i++)
            {
                Agents.Agent visitorAgent = this.asynAgents[i];
                visitorAgent.VisitUpdate(this);//.VisitUpdate();
            }
            ////更新同步消息
            //foreach (UpdateAgent.UpdateAgent item in this.synAgentChain)
            //{
            //    if (item != null)
            //    {
            //        item.Update();//visitor.visit 一个规则就是一个访问者，很多的访问者
            //    }
            //}
        }


       /// <summary>
       /// 坐标系转换到中央坐标系
       /// </summary>
       /// <param name="i"></param>
       /// <returns></returns>
        internal Index ToCenterXY( int iLaneAhead)
        {
            Index point = new Index(this.Rank, iLaneAhead);
            //坐标系平移变换,将中心x和底部y坐标系平移到中心坐标系
            //使用固定的6个元胞的宽度
            Coordinates.TranslateYAxis(ref point, SysSimContext.SimContext.SimSettings.iMaxWidth);
            return point;
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
    }
	 
}
 
