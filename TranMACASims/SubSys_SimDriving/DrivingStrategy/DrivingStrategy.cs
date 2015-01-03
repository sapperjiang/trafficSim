using System;
using System.Diagnostics;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.SysSimContext;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
    internal abstract class RoadNodeDriveStrategy
    {
        /// <summary>
        /// 城市路网中，三个元胞.交叉口的期望速度
        /// </summary>
        internal int iDesiredSpeed = 2;
        /// <summary>
        /// 用来临时存储状态，三种状态的修改等于是修改其克隆副本
        /// 最终是要修改和反映到cell中的
        /// </summary>

        [System.Obsolete("交叉口不添加换道，请重载")]
        internal virtual void ShiftLane(CellRunCtx crc)
        { }
        internal virtual void Accelerate(CellRunCtx crc)
        {
            if (crc.iEntityGap >= 2 && crc.Out.iSpeed < this.iDesiredSpeed)
            {
                crc.Out.iSpeed += crc.iAcceleration;
            }
        }
        internal virtual void Decelerate(CellRunCtx crc)
        {
            //交通规则限制减速
            if (crc.Out.iSpeed > this.iDesiredSpeed)
            {
                crc.Out.iSpeed -= crc.iAcceleration;
            }
            //空间限制而减速
            if (crc.iFrontGap < crc.Out.iSpeed)
            {
                crc.Out.iSpeed = crc.iEntityGap;//-crc.iSaftySpace
            }

        }
        internal virtual void NormalRun(CellRunCtx crc)
        {
            //if (crc.iEntityGap > crc.Out.iSpeed)
            //{
                crc.Out.iMoveStepY = crc.Out.iSpeed;
            //}
            //else
            //{
            //    crc.Out.iMoveStepY
            //}
        }
    }
    internal class RoadEdgeStrategy : RoadEdgeDriveStrategy { }
    internal class RoadNodeStrategy : RoadNodeDriveStrategy { }
  
    internal abstract class RoadEdgeDriveStrategy
    {
        /// <summary>
        /// 城市路段的期望速度
        /// </summary>
        internal int iDesiredSpeed=10;

        /// <summary>
        /// 换道的几种原因，1.由于路口转向必须换道，
        /// 2.由于寻求合适的理想行驶状态
        /// </summary>
        [System.Obsolete("换道模型暂时不实现")]
        internal virtual void ShiftLane(CellRunCtx crx)
        {
        }
        internal virtual void NormalRun(CellRunCtx crx)
        {
            //保持安全的车头时距
            //if (crx.Out.iSpeed  <= crx.iFrontGap)//使用输出车速
            //{
            //    crx.Out.iMoveStepY = crx.Out.iSpeed;//*(1- - crx.iSafetyGap;
            //}
            //else
            //{
                if (crx.Out.iSpeed <= crx.iFrontGap)
                {
                    crx.Out.iMoveStepY = crx.Out.iSpeed - 1;
                }
                else//小于速度个车头时距
                {
                    crx.Out.iMoveStepY = crx.Out.iSpeed;
                }

            //}
            //确保前进的距离不是负值
            crx.Out.iMoveStepY = Math.Max(0,crx.Out.iMoveStepY);

        }
        internal virtual void Accelerate(CellRunCtx crx)
        {
            if (crx.iFrontGap > 2 * crx.Out.iSpeed)//保证了加速
            {
                if (crx.Out.iSpeed < this.iDesiredSpeed)
                {
                    //crx.iAcceleration += 1;
                    crx.Out.iSpeed+=crx.iAcceleration; 
                    //crx.iAcceleration = 1;
                }
            }//否则不加速
        }
        internal virtual void Decelerate(CellRunCtx crx)
        {
            //大于期望车速减速
            if (crx.Out.iSpeed > this.iDesiredSpeed)
            {
                crx.Out.iSpeed -= crx.iAcceleration;
            }
            ///危险减速或者是前进距离过大减速
            if (crx.iFrontGap < crx.Out.iSpeed)//可能有bug
            {
                crx.Out.iSpeed = crx.iFrontGap-1;
            }

            if (crx.dRandom<crx.dModerationRatio && crx.Out.iSpeed>1)//随机漫化
            {
                crx.Out.iSpeed -= crx.iAcceleration;
            }
            //确保车速不小于零
            crx.Out.iSpeed = Math.Max(crx.Out.iSpeed,0);

        }
    //一下两个类为酱油
    internal class DefaultRoadNodeDriveStrategy : RoadNodeDriveStrategy
    { }

    internal class DefaultRoadEdgeDriveStrategy : RoadEdgeDriveStrategy
    { }
}      
    /// 驾驶员的驾驶行为策略，每个cell的驾驶员行为不一样
    /// </summary>
    internal abstract class DriveStrategy
    {
        /// <summary>
        /// 城市路网中，三个元胞
        /// </summary>
        //internal int iDesiredSpeed;
        ///// <summary>
        ///// 用来临时存储状态，三种状态的修改等于是修改其克隆副本
        ///// 最终是要修改和反映到cell中的
        ///// </summary>
        //protected Cell varCell;
        //protected Cell varCellCopy;//用于在不同的函数之间保存之前的状态

        // protected RoadLane rl;

        internal RoadNodeDriveStrategy rnd;
        internal RoadEdgeDriveStrategy red;
        
        /// <summary>
        /// 边界为红绿灯的地方是不更新和移动的或者做判断
        /// </summary>
        internal virtual void Drive(RoadEntity rEntity, Cell cell)
        {
            CellRunCtx ctx = new CellRunCtx(rEntity, cell);
            cell.GetEntityGap(out ctx.iEntityGap,out ctx.iToEntityGap);
            ctx.iFrontGap = ctx.iEntityGap+ctx.iToEntityGap;

            ctx.iAcceleration = cell.Car.iAcc;
            ctx.Out.iSpeed = cell.Car.iSpeed;

            switch (rEntity.EntityType)
            {
                case EntityType.RoadEdge:
                  
                    //车道内部的计算方法
                    ctx.iFrontSpeed = cell.nextCell == null ? -1 : cell.nextCell.Car.iSpeed;

                    //控制权转移出去了
                    red.ShiftLane(ctx);//换道
                    red.Accelerate(ctx);//符合条件就加速
              
                    
                    red.Decelerate(ctx);//否则减速
                    red.NormalRun(ctx);//更新位置
                
                //以下处理车的位置
                    RoadEdge re = rEntity as RoadEdge;
                    //处理换道的行为
                    RoadLane rl= cell.Container as RoadLane;

                    if (ctx.Out.iMoveStepY==0)
                    {
                        break;
                    }
                    //还在路段内部
                    if (ctx.Out.iMoveStepY <= ctx.iEntityGap && ctx.iEntityGap>0)
	                {
                        cell.RelativePosition= new Point(cell.RelativePosition.X,cell.RelativePosition.Y+ ctx.Out.iMoveStepY);
	                }else //进入了交叉口
                    { 
                        int iToEntitMoveStep = ctx.Out.iMoveStepY - ctx.iEntityGap;
                        //计算轨迹
                        cell.CalcTrack(1);//初始化到路段的在交叉口的入口处
                        //初始化元胞的位置
                        cell.Track.pCurrPos = cell.Track.pTempPos;//这里已经移动了一步了
                        
                        //交叉口内部移动指定的步长
                        cell.TrackMove(iToEntitMoveStep-1);//修改节点

                        re.roadNodeTo.AddCell(cell);//进入交叉口
                       
                        rl.RemoveCell();//离开原来的路段删除最前面的元胞

                    }
                    break;

                case EntityType.RoadNode:

                    RoadNode rn = rEntity as RoadNode;

                    //控制权转移出去了
                    rnd.ShiftLane(ctx);//换道  
                    rnd.Accelerate(ctx);//符合条件就加速
                    
                    rnd.Decelerate(ctx);//否则减速
                rnd.NormalRun(ctx);//更新位置
                    //修改空间位置
                    if (ctx.Out.iMoveStepY <= ctx.iEntityGap&&ctx.iEntityGap>0)
                    {//修改空间位置
                        Point old = cell.Track.pCurrPos;
                        //修改pCurrPos
                        cell.TrackMove(ctx.Out.iMoveStepY);//计算坐标
                        rn.MoveCell(old, cell.Track.pCurrPos);//移动元胞到指定的位置
                    }
                    else //进入了路段
                    {
                        rn.RemoveCell(cell);//离开交叉口删除cell

                        int iToEntitMoveStep = ctx.Out.iMoveStepY - ctx.iEntityGap;
                        RoadLane to = cell.Track.toLane;
                        if (to!=null)
                        {
                            ///转换坐标
                           cell.Track.pCurrPos = new Point(to.Rank,iToEntitMoveStep);
                            //进入下一个交通实体
                            cell.Track.toLane.EnterWaitedQueue(cell);
	                    }
                    }
                    break;
                default:
                    ThrowHelper.ThrowArgumentException("不正确的参数");
                    break;
            }
            cell.Car.iAcc = Math.Max(ctx.Out.iAcceleration,1);
            cell.Car.iSpeed = ctx.Out.iSpeed;
        }
    }
    internal class DefaultDriveStrategy:DriveStrategy
    {
        public DefaultDriveStrategy()
        {
            this.rnd = new RoadNodeStrategy();
            this.red = new RoadEdgeStrategy();
        }
    }
}