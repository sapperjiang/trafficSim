using System;
using System.Diagnostics;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.SysSimContext;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
	internal abstract class XNodeDriveStrategy
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
		internal virtual void ShiftLane(RunCtx crc)
		{ }
		internal virtual void Accelerate(RunCtx crc)
		{
			if (crc.iEntityGap >= 2 && crc.Out.iSpeed < this.iDesiredSpeed)
			{
				crc.Out.iSpeed += crc.iAcceleration;
			}
		}
		internal virtual void Decelerate(RunCtx crc)
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
		internal virtual void NormalRun(RunCtx crc)
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
	internal class WayStrategy : WayDriveStrategy { }
	internal class XNodeStrategy : XNodeDriveStrategy { }
	
	internal abstract class WayDriveStrategy
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
		internal virtual void ShiftLane(RunCtx crx)
		{
		}
		internal virtual void NormalRun(RunCtx crx)
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
		internal virtual void Accelerate(RunCtx crx)
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
		internal virtual void Decelerate(RunCtx crx)
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
		public class DefaultRoadNodeDriveStrategy : XNodeDriveStrategy
		{ }

		public class DefaultRoadEdgeDriveStrategy : WayDriveStrategy
		{ }
	}
	/// <summary>
	/// 驾驶员的驾驶行为策略，每个mobileebtity驾驶员行为不一样,享元模式
	/// </summary>
	public abstract partial class DriveStrategy
	{
		internal XNodeDriveStrategy _WayStrategy;
		internal WayDriveStrategy _XNodeStrategy;
		
		/// <summary>
		/// 边界为红绿灯的地方是不更新和移动的或者做判断
		/// </summary>
		internal virtual void Drive(TrafficEntity rEntity, Cell cell)
		{
			RunCtx ctx = new RunCtx(rEntity, cell);
			cell.GetEntityGap(out ctx.iEntityGap,out ctx.iToEntityGap);
			ctx.iFrontGap = ctx.iEntityGap+ctx.iToEntityGap;

			ctx.iAcceleration = cell.Car.iAcc;
			ctx.Out.iSpeed = cell.Car.iSpeed;

			switch (rEntity.EntityType)
			{
				case EntityType.Way:
					
					//车道内部的计算方法
					ctx.iFrontSpeed = cell.nextCell == null ? -1 : cell.nextCell.Car.iSpeed;

					//控制权转移出去了
					_XNodeStrategy.ShiftLane(ctx);//换道
					_XNodeStrategy.Accelerate(ctx);//符合条件就加速
					
					
					_XNodeStrategy.Decelerate(ctx);//否则减速
					_XNodeStrategy.NormalRun(ctx);//更新位置
					
					//以下处理车的位置
					Way re = rEntity as Way;
					//处理换道的行为
					Lane rl= cell.Container as Lane;

					if (ctx.Out.iMoveStepY==0)
					{
						break;
					}
					//还在路段内部
					if (ctx.Out.iMoveStepY <= ctx.iEntityGap && ctx.iEntityGap>0)
					{
						cell.Grid= new Point(cell.Grid.X,cell.Grid.Y+ ctx.Out.iMoveStepY);
					}else //进入了交叉口
					{
						int iToEntitMoveStep = ctx.Out.iMoveStepY - ctx.iEntityGap;
						//计算轨迹
						cell.CalcTrack(1);//初始化到路段的在交叉口的入口处
						//初始化元胞的位置
						cell.Track.pCurrPos = cell.Track.pTempPos;//这里已经移动了一步了
						
						//交叉口内部移动指定的步长
						cell.TrackMove(iToEntitMoveStep-1);//修改节点

						re.xNodeTo.AddCell(cell);//进入交叉口
						
						rl.RemoveCell();//离开原来的路段删除最前面的元胞

					}
					break;

				case EntityType.XNode:

					XNode rn = rEntity as XNode;

					//控制权转移出去了
					_WayStrategy.ShiftLane(ctx);//换道
					_WayStrategy.Accelerate(ctx);//符合条件就加速
					
					_WayStrategy.Decelerate(ctx);//否则减速
					_WayStrategy.NormalRun(ctx);//更新位置
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
						Lane to = cell.Track.toLane;
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
	public abstract partial class DriveStrategy
	{
		internal virtual void DriveCar(StaticEntity driveCtx,MobileEntity me)
		{
			switch (me.EntityType)
			{
				case EntityType.Way:
					
//					
//					RunCtx ctx = new RunCtx(me, cell);
//					
//					cell.GetEntityGap(out ctx.iEntityGap,out ctx.iToEntityGap);
//					
//					ctx.iFrontGap = ctx.iEntityGap+ctx.iToEntityGap;
//
//					ctx.iAcceleration = cell.Car.iAcc;
//					ctx.Out.iSpeed = cell.Car.iSpeed;
//					
//					
//					
//					
//					//车道内部的计算方法
//					ctx.iFrontSpeed = cell.nextCell == null ? -1 : cell.nextCell.Car.iSpeed;

//					//控制权转移出去了
//					_XNodeStrategy.ShiftLane(ctx);//换道
//					_XNodeStrategy.Accelerate(ctx);//符合条件就加速
//					
//					
//					_XNodeStrategy.Decelerate(ctx);//否则减速
//					_XNodeStrategy.NormalRun(ctx);//更新位置
					
//					//以下处理车的位置
//					Way re = me as Way;
//					//处理换道的行为
//					Lane rl= cell.Container as Lane;
//
//					if (ctx.Out.iMoveStepY==0)
//					{
//						break;
//					}
//					//还在路段内部
//					if (ctx.Out.iMoveStepY <= ctx.iEntityGap && ctx.iEntityGap>0)
//					{
//						cell.Grid= new Point(cell.Grid.X,cell.Grid.Y+ ctx.Out.iMoveStepY);
//					}else //进入了交叉口
//					{
//						int iToEntitMoveStep = ctx.Out.iMoveStepY - ctx.iEntityGap;
//						//计算轨迹
//						cell.CalcTrack(1);//初始化到路段的在交叉口的入口处
//						//初始化元胞的位置
//						cell.Track.pCurrPos = cell.Track.pTempPos;//这里已经移动了一步了
//						
//						//交叉口内部移动指定的步长
//						cell.TrackMove(iToEntitMoveStep-1);//修改节点
//
//						re.xNodeTo.AddCell(cell);//进入交叉口
//						
//						rl.RemoveCell();//离开原来的路段删除最前面的元胞

	//				}
					break;

				case EntityType.XNode:

//					XNode rn = me as XNode;
//
//					//控制权转移出去了
//					_WayStrategy.ShiftLane(ctx);//换道
//					_WayStrategy.Accelerate(ctx);//符合条件就加速
//					
//					_WayStrategy.Decelerate(ctx);//否则减速
//					_WayStrategy.NormalRun(ctx);//更新位置
//					//修改空间位置
//					if (ctx.Out.iMoveStepY <= ctx.iEntityGap&&ctx.iEntityGap>0)
//					{//修改空间位置
//						Point old = cell.Track.pCurrPos;
//						//修改pCurrPos
//						cell.TrackMove(ctx.Out.iMoveStepY);//计算坐标
//						rn.MoveCell(old, cell.Track.pCurrPos);//移动元胞到指定的位置
//					}
//					else //进入了路段
//					{
//						rn.RemoveCell(cell);//离开交叉口删除cell
//
//						int iToEntitMoveStep = ctx.Out.iMoveStepY - ctx.iEntityGap;
//						Lane to = cell.Track.toLane;
//						if (to!=null)
//						{
//							///转换坐标
//							cell.Track.pCurrPos = new Point(to.Rank,iToEntitMoveStep);
//							//进入下一个交通实体
//							cell.Track.toLane.EnterWaitedQueue(cell);
//						}
//					}
					break;
				default:
					ThrowHelper.ThrowArgumentException("不正确的参数");
					break;
			}
			
//			cell.Car.iAcc = Math.Max(ctx.Out.iAcceleration,1);
//			cell.Car.iSpeed = ctx.Out.iSpeed;
			
		}
		
	}
	
	public class DefaultDriveAgent:DriveStrategy
	{
		public DefaultDriveAgent()
		{
			this._WayStrategy = new XNodeStrategy();
			this._XNodeStrategy = new WayStrategy();
		}
	}
	
	public enum StrategyType
	{
		Default= 0,
		other = 1
	}
	public class StrategyFactory
	{
		
		public static DriveStrategy Create(StrategyType st)
		{
			switch (st) {
				case StrategyType.Default:
					return new DefaultDriveAgent() ;
					//    				break;
					default:break;
			}
			throw new ArgumentNullException("没有该类型");
			
		}
	}
	
	
}