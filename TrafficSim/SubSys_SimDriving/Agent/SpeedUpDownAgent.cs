using System.Diagnostics;
using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
    internal class SpeedUpDownAgent : Agent
    {
        internal SpeedUpDownAgent()
        {   
            this.strAgentName = AgentName.SpeedUpDownAgent;
            this.priority = AgentPriority.Medium;
            this.agentType = AgentType.Synchronization;
        }
        /// <summary>
        /// 接受roadEdge参数
        /// </summary>
        /// <param name="roadEdge"></param>
        internal override void VisitUpdate(RoadEdge roadEdge)
        {
            if (roadEdge == null)
            {
                throw new System.ArgumentException("访问者模式访问对象不能为空，RoadEntity没有赋值！");
            }
            //遍历每条车道
            foreach (RoadLane rl in roadEdge.Lanes)
            {
                this.VisitUpdate(rl);
            }
        }
        internal sealed override void VisitUpdate(RoadNode roadNode)
        {
            if (roadNode == null)
            {
                throw new System.ArgumentException("访问者模式访问对象不能为空，roadNode没有赋值！");
            }
            IEnumerator<Cell> ceEnum = roadNode.GetEnumerator();
            ceEnum.Reset();
            while (ceEnum.MoveNext())
            {
                Cell ce = ceEnum.Current;
                ce.Drive(roadNode);
            }

        }

        /// <summary>
        /// 遍历一个车道里面的所有元胞对象，对于机动车来说，其行为有一下顺序
        /// 前行加速到期望速度，期望速度行驶。或者换道加速到期望速度，这个驾驶员行为交通模型
        /// 还是很麻烦的
        /// </summary>
        /// <param name="rL"></param>
        //internal override void VisitUpdate(RoadLane rL)
        //{
        //    int iHeadWay = 0;
        //    Cell ca = rL.cells.PeekLast();//队列末尾一个元素
        //    int iOutCount = 0;//记录要删除多少元素 
        //    while (ca != null)
        //    {
        //        //没有下一个元胞就使用路段长度，否则使用下一个元胞的位置
        //        iHeadWay = ca.nextCell == null ? rL.iLength : ca.iPos;
        //        iHeadWay -= ca.iPos;//计算车头时距
        //        //利用物理公式 S=1/2*a*t^2+v*t;计算一个时间步长内部可以移动的距离
        //        int iMoveStep = (int)(0.5 * ca.cmCarModel.iAcc + ca.cmCarModel.iSpeed);
        //        if (iMoveStep <= iHeadWay)//车头时距比计算出的距离大
        //        {
        //            ca.cmCarModel.iSpeed += ca.cmCarModel.iAcc;//更新速度
        //            ca.iPos += iMoveStep;//位置更新
        //        }else//iMoveStep 比较大，如果车辆是在路口。那么iHeadWay是0
        //            //iMoveStep 大说明下一个时间步长内就要驶离车道进入交叉口了
        //        { 
        //            int iAheadSpace = iMoveStep-iHeadWay;

        //            Debug.Assert(rL.parEntity!=null);
        //            Debug.Assert(rL.parEntity.to != null);

        //            ///如果为false元胞将要移动出车道，应当将其删除
        //            RoadNode toRoadNode = rL.parEntity.to;
        //            if (toRoadNode.IsAheadBlocked(rL,iAheadSpace)==false)
        //            {
        //                //前进的距离比车头时距大。进入交叉口，并且交叉口没有元胞
        //                                       //添加到交叉口
        //                toRoadNode.AddCell(rL, iAheadSpace, ca);
        //                iOutCount++;//记录要出去的车辆元胞数
        //            }
        //        }
        //        ca = ca.nextCell;//前进到下一个ca元胞，进行更新
        //    }//while 结束

        //    Debug.Assert(iOutCount<=rL.cells.Count);
        //    //删除已经驶出路段的车辆元胞让已经驶出的路段元胞
        //    while (iOutCount-- > 0)
        //    {
        //        rL.cells.Dequeue();
        //    }

        //}
        /// <summary>
        /// 旧版本是由队列末尾向队列头遍历，该版本是由队列头向队列尾遍历，
        /// 这个函数可能要引入策略模式，因为每个驾驶员行为都不一样根据策略形式
        ///元胞状态的更新可能要引入观察者模式
        /// </summary>
        /// <param name="rL"></param>
        internal override void VisitUpdate(RoadLane rL)
        {
            IEnumerator<Cell> enumCell = rL.cells.GetEnumerator();
            enumCell.Reset();

            while (enumCell.MoveNext())
            {
                Cell ca = enumCell.Current;
                ca.Drive(rL);
            }//while 结束
        }

    }
}
