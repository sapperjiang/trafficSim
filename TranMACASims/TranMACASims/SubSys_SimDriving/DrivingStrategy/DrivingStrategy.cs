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
            this.cell = cell;
            this.cellCopy = cell.Copy();

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
        internal virtual void Drive(RoadLane rl,Cell cell)
        {
            //不对副本进行操作，利用副本保存原始记录为函数提供参考
            this.cell = cell;
            this.cellCopy = cell.Copy();

            int iHeadWay =0;
            int iAheadSpace = cell.iPos+cell.cmCarModel.iSpeed - rl.iLength;//6-5-1
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
                         cell.iPos= rl.iLength;
                     }
                     else
                     {
                         cell.iPos = cell.nextCell.iPos - SimContext.SimSettings.iSafeHeadWay;//第五个元胞
                     }
                }
            }
            else //没有驾驶出车道，新的位置在车道内部
            {
                iHeadWay = cell.nextCell == null ? rl.iLength : cell.nextCell.iPos;//第六个车和第五个车，车头时距为0
                iHeadWay -= (cell.iPos + 1);//6-5
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
    /// <summary>
    /// 稳健型的
    /// </summary>
    internal class ModerateDrivingStrategy : DrivingStrategy
    {
        internal ModerateDrivingStrategy()
        {
            this.iDesiredSpeed = 5;
        }
        /// <summary>
        /// 转向换道或者交通行为换道
        /// </summary>
        /// <param name="rL"></param>
        /// <param name="iHeadWay"></param>
        [System.Obsolete("什么也没有做")]
        protected override void ShiftLane(RoadLane rL, int iHeadWay)
        {
            //throw new NotImplementedException();
        }
        protected override void NormalRun(RoadLane rL,int iHeadWay)
        {
            int iMoveStep = cellCopy.cmCarModel.iSpeed;
            if (iMoveStep < iHeadWay)
            {
                cell.Move(iMoveStep);//.iPos += iMoveStep;
            }
            else
            {
                cell.Move(iHeadWay);
            }
            //base.NormalRun(cell, iHeadWay);
        }
        protected override void Accelerate(RoadLane rL, int iHeadWay)
        {
            //利用物理公式 S=1/2*a*t^2+v*t;计算一个时间步长内部可以移动的距离
            //iMoveStep = (int)(0.5 * cell.cmCarModel.iAcc + cell.cmCarModel.iSpeed);
            if (cellCopy.cmCarModel.iSpeed < cellCopy.cmCarModel.DriveStg.iDesiredSpeed)//车头时距比计算出的距离大//更新位置
            {
                if (iHeadWay>2*cellCopy.cmCarModel.DriveStg.iDesiredSpeed)//车头时距足够长以提供加速的空间
                {
                    cell.cmCarModel.GearUp();
                }
            }
            //可以加入更复杂的模型比方说空间距离很大，然后调高期望车速
        }
        protected override void Decelerate(RoadLane rL, int iHeadWay)
        {
            //大于期望车速减速
            if (cellCopy.cmCarModel.iSpeed > cellCopy.cmCarModel.DriveStg.iDesiredSpeed)//车头时距比计算出的距离大//更新位置
            {
                cell.cmCarModel.GearDown();
            }
            ///危险减速或者是前进距离过大减速
            //空间限制减速，车头时距小于 新的速度*1时间步长的距离和旧速度*1时间步长之和比较危险，提前减速
             if (iHeadWay<cellCopy.cmCarModel.iSpeed+cell.cmCarModel.iSpeed)
            {
                cell.cmCarModel.GearDown();    
            }
            //base.Decelerate(iHeadWay);
        }

        /// <summary>
        /// while遍历代码在visitor模式中
        /// </summary>
        /// <param name="rn"></param>
        /// <param name="iHeadWay"></param>
        protected override void NormalRun(RoadNode rn, int iHeadWay)
        {
            //IEnumerator<Cell> cellEnum = rn.GetEnumerator();
            //cellEnum.Reset();
            //while (cellEnum.MoveNext())//每个元胞遍历
            //{
            //    Cell ce = cellEnum.Current;

                Index iNew =cell.track.getApproachingPoint(cell.track.iCurrPos);

                //利用iSpeed每一步移动来模拟元胞在一个时间步长内是否可以按照指定速度前进到一个理想位置前进
                int iWhileCount = cell.cmCarModel.iSpeed;//循环时间iSpeed个时间步长
                while (iWhileCount-->0)
                {
                    if (iNew.X == 0 && iNew.Y == 0)//当前位置处是零
                    {//删除在交叉口的，进入路段的
                        if (!cell.track.toLane.IsLaneBlocked(iWhileCount))
                        {////走到终点就移除并且进行转换
                            rn.RemoveCell(cell.track.iToPos, cell.track.toLane.parEntity);
                            cell.track.toLane.waitedQueue.Enqueue(cell);//转换并且入队
                            break;
                        }//否则不进入车道,等待iWhileCount减速到零
                        else 
                        {
                            cell.cmCarModel.iSpeed = 0;//已经没有路可以走，速度为零
                            //或者换车道
                            //this.ShiftLane(null, 0);
                        }
                    }
                    //判断如果没有被占用就更新位置信息
                    if (rn.MoveCell(cell.track.iCurrPos, iNew) == true)
                    {
                        cell.track.iCurrPos = iNew;//如果移动成功就更新为新的元胞坐标
                        //更新为新坐标看是否可以进行移动
                        iNew = cell.track.getApproachingPoint(cell.track.iCurrPos);
                    }
                    else///否则什么一直减速到零
                    {
                        cell.cmCarModel.iSpeed -= cell.cmCarModel.iAcc;
                        iWhileCount = cell.cmCarModel.iSpeed;
                    }      
                //}
            }
        }
        /// <summary>
        /// 简单的不加速
        /// </summary>
        [System.Obsolete("比较简单的初始模型不加速")]        
        protected override void Accelerate(RoadNode rn, int iHeadWay)
        {
        }
        [System.Obsolete("比较简单的初始模型不减速")]        
        protected override void Decelerate(RoadNode rn, int iHeadWay)
        {
        }
    
    }
    /// <summary>
    /// 冒险型的
    /// </summary>
    internal class RushDrivingStrategy : DrivingStrategy
    {
        internal RushDrivingStrategy()
        {
            this.iDesiredSpeed = 7;
        }

        protected override void NormalRun(RoadNode rn, int iHeadWay)
        {
            int iMoveStep = cellCopy.cmCarModel.iSpeed;
            if (iMoveStep < iHeadWay)
            {
                cell.Move(iMoveStep);//.iPos += iMoveStep;
            }
            else
            {
                cell.Move(iHeadWay);
            }

            //base.NormalRun(cell, iHeadWay);
        }
        protected override void Accelerate(RoadNode rn, int iHeadWay)
        {
            //利用物理公式 S=1/2*a*t^2+v*t;计算一个时间步长内部可以移动的距离
            //iMoveStep = (int)(0.5 * cell.cmCarModel.iAcc + cell.cmCarModel.iSpeed);
            if (cellCopy.cmCarModel.iSpeed < cellCopy.cmCarModel.DriveStg.iDesiredSpeed)//车头时距比计算出的距离大//更新位置
            {
                cell.cmCarModel.GearUp();
            }
        }
        protected override void Decelerate(RoadNode rn, int iHeadWay)
        {
            ///危险减速或者是前进距离过大减速
            if (cellCopy.cmCarModel.iSpeed > cellCopy.cmCarModel.DriveStg.iDesiredSpeed)//车头时距比计算出的距离大//更新位置
            {
                cell.cmCarModel.GearDown();
            }
            //空间限制减速，车头时距小于 新的速度*1时间步长的距离和旧速度*1时间步长之和比较危险，提前减速
            if (iHeadWay < cellCopy.cmCarModel.iSpeed + cell.cmCarModel.iSpeed)
            {
                cell.cmCarModel.GearDown();
            }
            //base.Decelerate(iHeadWay);
        }

        protected override void NormalRun(RoadLane rL, int iHeadWay)
        {
            int iMoveStep = cellCopy.cmCarModel.iSpeed;
            if (iMoveStep < iHeadWay)
            {
                cell.Move(iMoveStep);//.iPos += iMoveStep;
            }
            else
            {
                cell.Move(iHeadWay);
            }
            //base.NormalRun(cell, iHeadWay);
        }
        protected override void Accelerate(RoadLane rL, int iHeadWay)
        {
            //利用物理公式 S=1/2*a*t^2+v*t;计算一个时间步长内部可以移动的距离
            //iMoveStep = (int)(0.5 * cell.cmCarModel.iAcc + cell.cmCarModel.iSpeed);
            if (cellCopy.cmCarModel.iSpeed < cellCopy.cmCarModel.DriveStg.iDesiredSpeed)//车头时距比计算出的距离大//更新位置
            {
                cell.cmCarModel.GearUp();
            }
            //可以加入更复杂的模型比方说空间距离很大，然后调高期望车速
        }
        protected override void Decelerate(RoadLane rL, int iHeadWay)
        {
            //大于期望车速减速
            if (cellCopy.cmCarModel.iSpeed > cellCopy.cmCarModel.DriveStg.iDesiredSpeed)//车头时距比计算出的距离大//更新位置
            {
                cell.cmCarModel.GearDown();
            }
            ///危险减速或者是前进距离过大减速
            //空间限制减速，车头时距小于 新的速度*1时间步长的距离和旧速度*1时间步长之和比较危险，提前减速
            if (iHeadWay < cellCopy.cmCarModel.iSpeed + cell.cmCarModel.iSpeed)
            {
                cell.cmCarModel.GearDown();
            }
            //base.Decelerate(iHeadWay);
        }

        protected override void ShiftLane(RoadLane rL, int iHeadWay)
        {
            throw new NotImplementedException();
        }
    }

}
