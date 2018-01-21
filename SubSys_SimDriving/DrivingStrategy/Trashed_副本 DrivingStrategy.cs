using System;
using System.Diagnostics;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.MathSupport;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 驾驶员的驾驶行为策略，每个cell的驾驶员行为不一样
    /// </summary>
    internal abstract class DrivingStrategy
    {
        internal int iDesiredSpeed;
        /// <summary>
        /// 用来临时存储状态，三种状态的修改等于是修改其克隆副本
        /// 最终是要修改和反映到cell中的
        /// </summary>
        protected Cell cell;
        protected Cell cellCopy;//用于在不同的函数之间保存之前的状态
        /// <summary>
        /// 边界红绿灯的地方是不更新和移动的或者做判断
        /// </summary>
        /// <param name="rN"></param>
        /// <param name="cell"></param>
        internal virtual void Drive(RoadNode rN, Cell cell)
        {
            this.innerCell = cell;
            this.innerCellCopy = cell.Copy();

            this.NormalRun(rN, 0);//更新位置
            this.Accelerate(rN, 0);//符合条件就加速
            this.Decelerate(rN, 0);//否则减速
        }
        protected abstract void NormalRun(RoadNode rn,int iHeadWay);
        protected abstract void Accelerate(RoadNode rn,int iHeadWay);
        protected abstract void Decelerate(RoadNode rn,int iHeadWay);

        /// <summary>
        /// 要行进的车辆元胞，
        /// </summary>
        /// <param name="cell">要行进的车辆元胞</param>
        internal virtual void Drive(RoadEdge re,Cell cell)
        {
            //不对副本进行操作，利用副本保存原始记录为函数提供参考
            this.innerCell = cell;
            this.innerCellCopy = cell.Copy();

            int iHeadWay =0;
            int iAheadSpace = cell.rltPos.Y+cell.cmCarModel.iSpeed - rl.iLength;//6-5-1
            if (iAheadSpace > 0)///车辆应当驾驶出了车道
            {
                //超出了车道范围，进入了交叉口
                RoadNode toRoadNode = rl.parEntity.to;
                //如果车道的出口被堵塞
                if (toRoadNode.IsLaneBlocked(rl) == false)
                {  //填充车辆的转向之类的东西，为交接做准备
                    cell.FillTrack(rl);
                    //前进的距离比车头时距大。进入交叉口，并且交叉口没有元胞
                    toRoadNode.AddCell(cell);
                    rl.cells.Dequeue();
                }
                else //交叉口被堵塞，可能是信号灯。可能是车辆
                {//如果是第一辆车并且被堵塞，就移动到车道的头上进行等待，
                    //不是第一辆车，前面有车堵在了车道上，就安排进入前一辆车的位置(更新后)减去安全位置
                     if(cell.nextCell == null)
                     {
                         cell.rltPos.Y= rl.iLength;
                     }
                     else
                     {
                         cell.rltPos.Y = cell.nextCell.rltPos.Y - SimContext.SimSettings.iSafeHeadWay;//第五个元胞
                     }
                }
            }
            else //没有驾驶出车道，新的位置在车道内部
            {
                iHeadWay = cell.nextCell == null ? rl.iLength : cell.nextCell.rltPos.Y;//第六个车和第五个车，车头时距为0
                iHeadWay -= (cell.rltPos.Y + 1);//6-5
                this.ShiftLane(rl,iHeadWay);//换道
                this.NormalRun(rl, iHeadWay);//更新位置
                this.Accelerate(rl, iHeadWay);//符合条件就加速
                this.Decelerate(rl, iHeadWay);//否则减速
            }
        }
        /// <summary>
        /// 换道的几种原因，1.由于路口转向必须换道，
        /// 2.由于寻求合适的理想行驶状态
        /// </summary>
        [System.Obsolete("换道模型暂时不实现")]
        protected abstract void ShiftLane(RoadLane rL, int iHeadWay);
        protected abstract void NormalRun(RoadLane rL, int iHeadWay);
        protected abstract void Accelerate(RoadLane rL, int iHeadWay);
        protected abstract void Decelerate(RoadLane rL, int iHeadWay);

    }
}
