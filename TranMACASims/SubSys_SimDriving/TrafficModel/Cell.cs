using SubSys_MathUtility;
using System;
using System.Drawing;
using SubSys_SimDriving.SysSimContext;
using System.Collections.Generic;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 程序的GUI可能要求使用对象的坐标来查询路段顶点的位置，采用对象的position用作
    /// </summary>
    /// <typeparam name="T">int</typeparam>
    public class Cell:TrafficEntity
    {
        private Cell() { }
        public Cell(Car cm)
        {
            this.EntityType = EntityType.Cell;
            this.RelativePosition = new Point(0,0);
            this.Car = cm;
        }
        /// <summary>
        /// 用作记录态哈希表记录车辆的时间信息，以及用来确定什么时候进入路段
        /// </summary>
        internal int iTimeStep;
        
        /// <summary>
        /// 使用链式结构支持快速下游访问
        /// </summary>
        internal Cell nextCell;
        /// <summary>
        /// 代表的车辆模型
        /// </summary>
        public Car Car;

        /// <summary>
        /// 相对于道路起点的偏移，记录车辆的空间信息，运行态哈希表和记录态哈希表
        /// </summary>
        //public int iPos;

        /// <summary>
        /// 包围在交叉口内部的节点,内部点使用绝对坐标系
        /// </summary>
        public CarTrack Track = new CarTrack();
        /// <summary>
        /// 使用在交叉口上
        /// </summary>
        /// <param name="iStep"></param>
        internal void TrackMove(int iStep)
        {
            Point p = this.RelativePosition;
            while (iStep-- > 0)
            {
                p = this.Track.NextPoint(p);
            }
            this.RelativePosition = p;

        }

        /// <summary>
        /// Cell的rltPos 与Track.iCurrPos保持一致
        /// </summary>
        public override Point RelativePosition
        {
            get
            {
                return this.Track.pCurrPos;
            }
            set
            {
                this.Track.pCurrPos = value;
            }
        }

        /// <summary>
        /// 只应当在转换的时候调用一次，寻找轨迹的一个东西，从起始位置出发，前进iAheadSpace个间距时距
        /// </summary>
        /// <param name="iAheadSpace"></param>
        internal void CalcTrack(int iAheadSpace)
        {
            RoadLane rl = this.Container as RoadLane;
            if (rl == null)
            {
                throw new ArgumentNullException("对不在路段上的元胞调用此次方法是错误的");
            }
            CarTrack mt = this.Track;
            mt.fromLane = rl;
            ///中心坐标系的车道的交叉口入口的第一个点
            mt.pFromPos = new Point(rl.Rank - 1, -SimSettings.iMaxLanes + 1);
            
            //获取转向信息
            RoadEdge re = rl.Container as RoadEdge;
            int iTurn = this.Car.EdgeRoute.GetSwerve(re);

            RoadEdge reNext = this.Car.EdgeRoute.FindNext(re);
            if (iTurn == 0)//直接使用中心坐标系
            {
                //车道直向对应
                mt.pToPos = new Point(rl.Rank-1,SimSettings.iMaxLanes);
                if (reNext == null)//没有目标车道，已经到头了
                {
                    mt.toLane = null;
                }
                else
                {
                    if (reNext.Lanes.Count < rl.Rank)//防止车道不匹配
                    {   //目标车道数小于本车道数,进入目标车道的内侧车道
                        mt.toLane = reNext.Lanes[0];
                    }
                    else
                    {   //目标车道数大于于本车道数
                        mt.toLane = reNext.Lanes[rl.Rank-1];
                    }
                }
            }
            if (iTurn == 1)//右转
            {
                //生成1到reNext 车道数量的随机数，即随机选择车道
                int iLaneIndex = - reNext.Lanes.Count + 1;
                mt.pToPos = new Point(SimSettings.iMaxLanes,iLaneIndex);
                mt.toLane = reNext.Lanes[-iLaneIndex];
            }
            if (iTurn == -1)//左转
            {
                //-4位置的坐标应当为-3 ,后面的也是随机选择车道
                int iLaneIndex = new Random(1).Next(reNext.Lanes.Count)- 1;
                mt.pToPos = new Point(-SimSettings.iMaxLanes + 1,iLaneIndex);
                mt.toLane = reNext.Lanes[iLaneIndex];
            }
            if (iTurn ==2 )
            {
                int iLaneIndex = reNext.Lanes.Count- 1;
                mt.pToPos = new Point(-iLaneIndex,-SimSettings.iMaxLanes+1);
                mt.toLane = reNext.Lanes[iLaneIndex];
            }
          
            //三个点全部转化转换为原生坐标
            mt.pFromPos = Coordinates.GetRealXY(mt.pFromPos, mt.fromLane.Container.ToVector());
            mt.pTempPos = new Point(rl.Rank-1,iAheadSpace-SimSettings.iMaxLanes);//创建点
            mt.pTempPos = Coordinates.GetRealXY(mt.pTempPos, mt.fromLane.ToVector());
            mt.pToPos = Coordinates.GetRealXY(mt.pToPos, mt.fromLane.Container.ToVector());
        }
      
        
        internal void Drive(RoadEntity rN)
        {
            this.Car.DriveStg.Drive(rN,this);
        }

        /// <summary>
        /// 创建浅表副本
        /// </summary>
        internal Cell Copy()
        {
            Car cm = this.Car.Copy();
            Cell ce = this.MemberwiseClone() as Cell;
            ce.Car = cm;
            return ce;
        }
        [System.Obsolete("坐标系统的问题，postion不赋值 roadhash不复制,iTimeStep有问题")]
        internal CarInfo GetCarInfo()
        {
            CarInfo ci = new CarInfo();//结构，值类型
            ci.iSpeed = this.Car.iSpeed;
            ci.iAcc = this.Car.iAcc;
            ci.iCarHashCode = this.Car.GetHashCode();
            ci.iCarNum = this.Car.ID;
            ci.iTimeStep = SimContext.iCurrTimeStep;
            if (this.Container.EntityType == EntityType.RoadLane)
            {
                ci.iPos = this.RelativePosition.Y+(int)this.Container.Shape[0]._X;
            }
            else if (this.Container.EntityType == EntityType.RoadNode)
            {
                ci.iPos = this.Container.RelativePosition.X + this.RelativePosition.X-1;
            }
            return ci;
        }

        internal void Move(int iHeadWay)
        {
            this.RelativePosition=new Point(this.RelativePosition.X,this.RelativePosition.Y+iHeadWay);
        }

        /// <summary>
        /// 计算当前元胞可以前进的车头时距
        /// </summary>
        /// <param name="iEntityGap"></param>
        /// <param name="iToEntityGap"></param>
        public void GetEntityGap(out int iEntityGap,out int iToEntityGap)
        {
            int EntityGap=0;
            int ToEntityGap=0;
            //车道，然后是路段
            switch (this.Container.EntityType)
            {
                case EntityType.RoadLane:
                    RoadEdge re = this.Container.Container as RoadEdge;
                    //车道内部的计算方法
                    if (this.nextCell == null)//第一个元胞的车头时距是路段+交叉口的
                    {
                        EntityGap = this.Container.iLength - this.RelativePosition.Y;
                        this.CalcTrack(1);//先填充轨迹
                        GetTrackGap(re.roadNodeTo, this.Track.pTempPos,out ToEntityGap);//再计算剩余轨迹
                    }else//后续都是路段上的，没有交叉口上的
                    {
                        EntityGap = this.nextCell.RelativePosition.Y - this.RelativePosition.Y;
                        ToEntityGap = 0;
                    }
                    break;

                case EntityType.RoadNode:

                    RoadNode rN = this.Container as RoadNode;
                    //计算剩余轨迹数量
                    if (GetTrackGap(rN, this.Track.pCurrPos, out EntityGap) ==false)
                    {
                         ToEntityGap = 0;
                    }
                    else
                    {
                        if (this.Track.toLane!=null)
                        {
                             ToEntityGap = this.Track.toLane.iLastPos;
                        }
                    }
                   
                    break;
            }
       
            iEntityGap=EntityGap;
            iToEntityGap = ToEntityGap;
        }

        /// <summary>
        /// 计算元胞在交叉口内部可以走多少步
        /// </summary>
        /// <param name="rN"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private bool GetTrackGap(RoadNode rN, Point pcc,out int Gap)
        {
            bool bReachEnd = false;
            int iCount = 0;
            Point p = this.Track.NextPoint(pcc);
            if (p.X == 0 && p.Y == 0)
            {
                bReachEnd = true;
            }
            while (rN.IsBlocked(p) == false)
            {
                p = this.Track.NextPoint(p);
                if (p.X == 0 && p.Y == 0)
                {
                    bReachEnd =true;
                    break;
                }
                iCount++;
            }
            Gap = iCount;
            return bReachEnd;
        }

    }
   
}