//using System;
//using System.Diagnostics;
//using System.Collections.Generic;
//using SubSys_SimDriving.TrafficModel;
//using SubSys_SimDriving;
//using SubSys_SimDriving.MathSupport;

//namespace SubSys_SimDriving.TrafficModel
//{
//    /// <summary>
//    /// 冒险型的
//    /// </summary>
//    internal class RushDrivingStrategy : DrivingStrategy
//    {
//        internal RushDrivingStrategy()
//        {
//            this.iDesiredSpeed = 7;
//        }

//        protected override void NormalRun(RoadNode rn, int iHeadWay)
//        {
//            int iMoveStep = innerCellCopy.cmCarModel.iSpeed;
//            if (iMoveStep < iHeadWay)
//            {
//                innerCell.Move(iMoveStep);//.rltPos.Y += iMoveStep;
//            }
//            else
//            {
//                innerCell.Move(iHeadWay);
//            }

//            //base.NormalRun(cell, iHeadWay);
//        }
//        protected override void Accelerate(RoadNode rn, int iHeadWay)
//        {
//            //利用物理公式 S=1/2*a*t^2+v*t;计算一个时间步长内部可以移动的距离
//            //iMoveStep = (int)(0.5 * cell.cmCarModel.iAcc + cell.cmCarModel.iSpeed);
//            if (innerCellCopy.cmCarModel.iSpeed < innerCellCopy.cmCarModel.DriveStg.iDesiredSpeed)//车头时距比计算出的距离大//更新位置
//            {
//                innerCell.cmCarModel.GearUp();
//            }
//        }
//        protected override void Decelerate(RoadNode rn, int iHeadWay)
//        {
//            ///危险减速或者是前进距离过大减速
//            if (innerCellCopy.cmCarModel.iSpeed > innerCellCopy.cmCarModel.DriveStg.iDesiredSpeed)//车头时距比计算出的距离大//更新位置
//            {
//                innerCell.cmCarModel.GearDown();
//            }
//            //空间限制减速，车头时距小于 新的速度*1时间步长的距离和旧速度*1时间步长之和比较危险，提前减速
//            if (iHeadWay < innerCellCopy.cmCarModel.iSpeed + innerCell.cmCarModel.iSpeed)
//            {
//                innerCell.cmCarModel.GearDown();
//            }
//            //base.Decelerate(iHeadWay);
//        }

//        protected override void NormalRun(RoadLane rL, int iHeadWay)
//        {
//            int iMoveStep = innerCellCopy.cmCarModel.iSpeed;
//            if (iMoveStep < iHeadWay)
//            {
//                innerCell.Move(iMoveStep);//.rltPos.Y += iMoveStep;
//            }
//            else
//            {
//                innerCell.Move(iHeadWay);
//            }
//            //base.NormalRun(cell, iHeadWay);
//        }
//        protected override void Accelerate(RoadLane rL, int iHeadWay)
//        {
//            //利用物理公式 S=1/2*a*t^2+v*t;计算一个时间步长内部可以移动的距离
//            //iMoveStep = (int)(0.5 * cell.cmCarModel.iAcc + cell.cmCarModel.iSpeed);
//            if (innerCellCopy.cmCarModel.iSpeed < innerCellCopy.cmCarModel.DriveStg.iDesiredSpeed)//车头时距比计算出的距离大//更新位置
//            {
//                innerCell.cmCarModel.GearUp();
//            }
//            //可以加入更复杂的模型比方说空间距离很大，然后调高期望车速
//        }
//        protected override void Decelerate(RoadLane rL, int iHeadWay)
//        {
//            //大于期望车速减速
//            if (innerCellCopy.cmCarModel.iSpeed > innerCellCopy.cmCarModel.DriveStg.iDesiredSpeed)//车头时距比计算出的距离大//更新位置
//            {
//                innerCell.cmCarModel.GearDown();
//            }
//            ///危险减速或者是前进距离过大减速
//            //空间限制减速，车头时距小于 新的速度*1时间步长的距离和旧速度*1时间步长之和比较危险，提前减速
//            if (iHeadWay < innerCellCopy.cmCarModel.iSpeed + innerCell.cmCarModel.iSpeed)
//            {
//                innerCell.cmCarModel.GearDown();
//            }
//            //base.Decelerate(iHeadWay);
//        }

//        protected override void ShiftLane(RoadLane rL, int iHeadWay)
//        {
//            throw new NotImplementedException();
//        }
//    }

   
//}
