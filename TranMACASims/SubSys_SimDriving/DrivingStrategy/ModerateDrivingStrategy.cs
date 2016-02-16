//using System;
//using System.Drawing;
//using System.Diagnostics;
//using System.Collections.Generic;
//using SubSys_SimDriving.TrafficModel;
//using SubSys_SimDriving.SysSimContext;
//using SubSys_MathTools;

//namespace SubSys_SimDriving.TrafficModel
//{

/// <summary>
/// 驾驶员的驾驶行为策略，每个cell的驾驶员行为不一样
/// </summary>
//internal abstract class DrivingStrategy
//{
//    /// <summary>
//    /// 城市路网中，三个元胞
//    /// </summary>
//    internal int iDesiredSpeed;
//    /// <summary>
//    /// 用来临时存储状态，三种状态的修改等于是修改其克隆副本
//    /// 最终是要修改和反映到cell中的
//    /// </summary>
//    protected Cell varCell;
//    protected Cell varCellCopy;//用于在不同的函数之间保存之前的状态

//    protected int iHeadWay;//车头时距
//    //private void GetCarHeadWay()
//    //{
//    //    RoadLane rl = this.varCell.container;
//    //}
//    protected RoadLane rl;

//    /// <summary>
//    /// 边界红绿灯的地方是不更新和移动的或者做判断
//    /// </summary>
//    internal virtual void Drive(RoadNode rN, Cell cell)
//    {
//        this.varCell = cell;
//        this.varCellCopy = cell.Copy();

//        this.iHeadWay = 0;

//        this.ShiftLane(rN);
//        this.NormalRun(rN);//更新位置
//        this.Accelerate(rN);//符合条件就加速
//        this.Decelerate(rN);//否则减速
//    }
//    protected abstract void ShiftLane(RoadNode rn);
//    protected abstract void NormalRun(RoadNode rn);
//    protected abstract void Accelerate(RoadNode rn);
//    protected abstract void Decelerate(RoadNode rn);

//    /// <summary>
//    /// 要行进的车辆元胞，
//    /// </summary>
//    /// <param name="cell">要行进的车辆元胞</param>
//    internal virtual void Drive(RoadEdge re, Cell ce)
//    {
//        //不对副本进行操作，利用副本保存原始记录为函数提供参考
//        this.varCell = ce;
//        this.varCellCopy = ce.Copy();

//        this.rl = re.Lanes[ce.rltPos.X - 1];

//        //System.Diagnostics.Debug.Assert(rl.Rank == ce.rltPos.X);

//        //if (ce.nextCell == null)//第一个元胞
//        //{
//        //    this.iHeadWay = rl.iLength;
//        //}
//        //else//不是第一个元胞
//        //{
//        //    this.iHeadWay = ce.nextCell.rltPos.Y - ce.rltPos.Y;
//        //}

//        this.ShiftLane(re);//换道
//        this.NormalRun(re);//更新位置
//        this.Accelerate(re);//符合条件就加速
//        this.Decelerate(re);//否则减速

//    }
//    /// <summary>
//    /// 换道的几种原因，1.由于路口转向必须换道，
//    /// 2.由于寻求合适的理想行驶状态
//    /// </summary>
//    [System.Obsolete("换道模型暂时不实现")]
//    protected abstract void ShiftLane(RoadEdge re);
//    protected abstract void NormalRun(RoadEdge re);
//    protected abstract void Accelerate(RoadEdge re);
//    protected abstract void Decelerate(RoadEdge re);

//}
///// <summary>

//    /// <summary>
//    /// 稳健型的
//    /// </summary>
//    internal class ModerateDriveStrategy : DriveStrategy
//    {
//        internal ModerateDriveStrategy()
//        {
//            this.iDesiredSpeed = 4;
//        }
//        /// <summary>
//        /// 转向换道或者交通行为换道
//        /// </summary>
//        /// <param name="rL"></param>
//        /// <param name="iHeadWay"></param>
//        [System.Obsolete("什么也没有做")]
//        protected override void ShiftLane(RoadEdge rL)
//        {
//            //throw new NotImplementedException();
//        }
//        protected override void NormalRun(RoadEdge re)
//        {
//            Point p = varCell.rltPos;

//            int iAheadSpace =p.Y+ varCell.Car.iSpeed - rl.iLength;//6-5-1
//            //车头时距比车辆可以行驶的距离小
//            if (iAheadSpace>=0)///车辆应当驾驶出了车道
//            {
//                //超出了车道范围，进入了交叉口
//                //如果车道的出口被堵塞
//                if (re.To.IsLaneBlocked(rl) == false)
//                {  //填充车辆的转向之类的东西，为交接做准备
//                    varCell.CalcTrack(rl,iAheadSpace+1);
//                    //前进的距离比车头时距大。进入交叉口，并且交叉口没有元胞
//                    re.To.AddCell(varCell);
//                    rl.RemoveCell();
//                }
//                else //交叉口被堵塞，可能是信号灯。可能是车辆
//                {//如果是第一辆车并且被堵塞，就移动到车道的头上进行等待，
//                    //不是第一辆车，前面有车堵在了车道上，就安排进入前一辆车的位置(更新后)减去安全位置
//                    Point rltP = varCell.rltPos;
//                    if (varCell.nextCell == null)
//                    {
//                        rltP.Y = rl.iLength;
//                    }
//                    else
//                    {
//                        rltP.Y = varCell.nextCell.rltPos.Y - SimSettings.iSafeHeadWay;//第五个元胞
//                    }
//                }
//            }
//            else //没有驾驶出车道，新的位置在车道内部
//            {
//                this.iHeadWay = varCell.nextCell == null ? rl.iLength : varCell.nextCell.rltPos.Y;//第六个车和第五个车，车头时距为0
//                this.iHeadWay -= (varCell.rltPos.Y + 1);//6-5
//            }
//            int iMoveStep = varCellCopy.Car.iSpeed;

//            if (iMoveStep < iHeadWay)
//            {
//                varCell.Move(iMoveStep);//.rltPos.Y += iMoveStep;
//            }
//            else
//            {
//                varCell.Move(iHeadWay);
//            }
            
//        }
//        protected override void Accelerate(RoadEdge rL)
//        {
//            //利用物理公式 S=1/2*a*t^2+v*t;计算一个时间步长内部可以移动的距离
//            //iMoveStep = (int)(0.5 * cell.cmCarModel.iAcc + cell.cmCarModel.iSpeed);
//            if (varCellCopy.Car.iSpeed < varCellCopy.Car.DriveStg.iDesiredSpeed)//车头时距比计算出的距离大//更新位置
//            {
//                if (iHeadWay>2*varCellCopy.Car.iSpeed)//.DriveStg.iDesiredSpeed)//车头时距足够长以提供加速的空间
//                {
//                    varCell.Car.GearUp();
//                }
//            }
//            //可以加入更复杂的模型比方说空间距离很大，然后调高期望车速
//        }

//        [System.Obsolete("是否应当一直减速？")]
//        protected override void Decelerate(RoadEdge re)
//        {
//            //大于期望车速减速
//            if (varCellCopy.Car.iSpeed > varCellCopy.Car.DriveStg.iDesiredSpeed)//车头时距比计算出的距离大//更新位置
//            {
//                varCell.Car.GearDown();
//            }
//            ///危险减速或者是前进距离过大减速
//            //空间限制减速，车头时距小于 新的速度*1时间步长的距离和旧速度*1时间步长之和比较危险，提前减速
//            //if (iHeadWay < varCellCopy.cmCarModel.iSpeed + varCell.cmCarModel.iSpeed)
//                if (iHeadWay < varCell.Car.iSpeed)
//            {
//                varCell.Car.GearDown();    
//            }
//        }

//        //[System.Obsolete("什么也没有做")]
//        //protected override void ShiftLane(RoadNode rn)
//        //{
//        //    //throw new NotImplementedException();
//        //}
//        //protected override void NormalRun(RoadNode rn)
//        //{
//        //    ////rn.InvokeServices(rn);
//        //    Point iNew =varCell.Track.getNextPoint(varCell.Track.iCurrPos);

//        //    //利用iSpeed每一步移动来模拟元胞在一个时间步长内是否可以按照指定速度前进到一个理想位置前进
//        //    int iWhileCount = 2;// varCell.cmCarModel.iSpeed;//循环时间iSpeed个时间步长
//        //    while (iWhileCount-->0)
//        //    {
//        //        if (iNew.X == 0 && iNew.Y == 0)//当前位置处是零
//        //        {//删除在交叉口的，进入路段的

//        //            //如果没有下一个目的路段
//        //            if (varCell.Track.toLane == null)
//        //            {
//        //                rn.RemoveCell(varCell);//将其删除
//        //                return;
//        //            }
//        //            if (varCell.Track.toLane.IsLaneBlocked(iWhileCount) == false)
//        //            {////走到终点就移除并且进行转换
//        //                rn.RemoveCell(varCell);
//        //                varCell.Track.toLane.EnterWaitedQueue(varCell);//转换并且入队
//        //                break;
//        //            }//否则不进入车道,等待iWhileCount减速到零
//        //            else
//        //            {
//        //                //在交叉口处等待
//        //                varCell.cmCarModel.iSpeed = iWhileCount;//已经没有路可以走，速度为零
//        //                //或者换车道
//        //                //this.ShiftLane(null, 0);
//        //                break;
//        //            }
//        //        }
//        //        else
//        //        {
//        //            //判断如果没有被占用就更新位置信息
//        //            if (rn.MoveCell(varCell.Track.iCurrPos, iNew) == true)
//        //            {
//        //                varCell.Track.iCurrPos = iNew;//如果移动成功就更新为新的元胞坐标
//        //                //更新为新坐标看是否可以进行移动
//        //                iNew = varCell.Track.getNextPoint(varCell.Track.iCurrPos);
//        //            }
//        //            else///否则一直减速到零
//        //            {
//        //                varCell.cmCarModel.iSpeed -= varCell.cmCarModel.iAcc;
//        //                iWhileCount = varCell.cmCarModel.iSpeed;
//        //            }
//        //        } 
//        //    }
//        //}
//        ///// <summary>
//        ///// 简单的不加速
//        ///// </summary>
//        //[System.Obsolete("比较简单的初始模型不加速")]        
//        //protected override void Accelerate(RoadNode rn)
//        //{
//        //}
//        //[System.Obsolete("比较简单的初始模型不减速")]        
//        //protected override void Decelerate(RoadNode rn)
//        //{
//        //}
    
//    }
  
//}
