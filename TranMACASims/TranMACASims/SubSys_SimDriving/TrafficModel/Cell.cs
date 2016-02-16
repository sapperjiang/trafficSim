using SubSys_SimDriving.MathSupport;
using System;
using SubSys_SimDriving.SysSimContext;
using System.Collections.Generic;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 程序的GUI可能要求使用对象的坐标来查询路段顶点的位置，采用对象的position用作
    /// </summary>
    /// <typeparam name="T">int</typeparam>
    internal class Cell:TrafficEntity
    {
        private Cell() { }
        internal Cell(CarModel cm)
        {
            this.cmCarModel = cm;
        }
        
        /// <summary>
        /// 使用链式结构支持快速下游访问
        /// </summary>
        internal Cell nextCell;
        /// <summary>
        /// 代表的车辆模型
        /// </summary>
        internal CarModel cmCarModel;

        /// <summary>
        /// 相对于道路起点的偏移，记录车辆的空间信息，运行态哈希表和记录态哈希表
        /// </summary>
        internal int iPos;

        /// <summary>
        /// 包围在交叉口内部的节点
        /// </summary>
        internal MovingTrack track;
        internal void FillTrack(RoadLane rl)
        {
            if (rl == null)
            {
                throw new ArgumentNullException("track的fromlane参数没有赋值，请先对其赋值");
            }
            MovingTrack mt = this.track;
            mt.fromLane = rl;
            mt.iFromPos = new Index(rl.Rank, 1);
            Coordinates.ToCenterXY(ref mt.iFromPos);//转换为交叉口坐标
            //获取转向信息
            int iTurn = this.cmCarModel.EdgeRoute.getTurnDirection(rl.parEntity);
            RoadEdge reNext = this.cmCarModel.EdgeRoute.FindNext(rl.parEntity);
            if (iTurn == 0)//直接使用中心坐标系
            {
                //车道直向对应
                mt.iToPos = new Index(rl.Rank,SimContext.SimSettings.iMaxWidth);
                if (reNext.Lanes.Count >= rl.Rank)//防止车道不匹配
                {
                    mt.toLane = reNext.Lanes[0];
                }else 
                {
                    mt.toLane = reNext.Lanes[rl.Rank];
                }
            }
            if (iTurn == 1)//
            {
                //生成1到reNext 车道数量的随机数，即随机选择车道
                int iLaneIndex = - new Random(1).Next(reNext.Lanes.Count)+ 1;
                mt.iToPos = new Index(SimContext.SimSettings.iMaxWidth,iLaneIndex);
                mt.toLane = reNext.Lanes[-iLaneIndex];
            }
            if (iTurn == -1)
            {
                //-4位置的坐标应当为-3 ,后面的也是随机选择车道
                int iLaneIndex = new Random(1).Next(reNext.Lanes.Count)- 1;
                mt.iToPos = new Index(-SimContext.SimSettings.iMaxWidth + 1,iLaneIndex);
                mt.toLane = reNext.Lanes[iLaneIndex];
            }
        }
        internal void Drive(RoadLane rL)
        {
            this.cmCarModel.DriveStg.Drive(rL,this);
        }
        internal void Drive(RoadNode rN)
        {
            this.cmCarModel.DriveStg.Drive(rN,this);
        }
        /// <summary>
        /// 用作记录态哈希表记录车辆的时间信息，以及用来确定什么时候进入路段
        /// </summary>
        internal int iTimeStep;
        /// <summary>
        /// 创建浅表副本
        /// </summary>
        internal Cell Copy()
        {
            return this.MemberwiseClone() as Cell;
        }
        internal void Move(int iHeadWay)
        {
            this.iPos += iHeadWay;
        }
    }
   
}