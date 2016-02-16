using System;
using System.Diagnostics;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving;
using System.Drawing;
using SubSys_MathUtility;


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
		internal virtual void LaneChanging(DriveContext dctx)
		{ }
		internal virtual void Accelerate(DriveContext dctx)
		{
			if (dctx.iLaneGap >= 2 && dctx.DriveParam.iSpeed < this.iDesiredSpeed)
			{
				dctx.DriveParam.iSpeed += dctx.iAcceleration;
			}
		}
		internal virtual void Decelerate(DriveContext crc)
		{
			//交通规则限制减速
			if (crc.DriveParam.iSpeed > this.iDesiredSpeed)
			{
				crc.DriveParam.iSpeed -= crc.iAcceleration;
			}
			//空间限制而减速
			if (crc.iFrontHeadWay < crc.DriveParam.iSpeed)
			{
				crc.DriveParam.iSpeed = crc.iLaneGap;//-crc.iSaftySpace
			}

		}
		/// <summary>
		/// Prudent .neither aggressive nor slow
		/// </summary>
		/// <param name="crc"></param>
		internal virtual void NormalRun(DriveContext crc)
		{
			//if (crc.iEntityGap > crc.Out.iSpeed)
			//{
			crc.DriveParam.iMoveY = crc.DriveParam.iSpeed;
			//}
			//else
			//{
			//    crc.Out.iMoveStepY
			//}
		}
	}

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
		internal virtual void LaneChanging(DriveContext crx)
		{
		}
		/// <summary>
		/// 匀速行驶
		/// </summary>
		/// <param name="crx"></param>
		internal virtual void NormalRun(DriveContext crx)
		{

			if (crx.DriveParam.iSpeed <= crx.iFrontHeadWay)
			{
				crx.DriveParam.iMoveY = crx.DriveParam.iSpeed;
			}
			else//小于速度个车头时距
			{
				crx.DriveParam.iMoveY = crx.DriveParam.iSpeed;
			}

			//确保前进的距离不是负值
			crx.DriveParam.iMoveY = Math.Max(0,crx.DriveParam.iMoveY);

		}
		/// <summary>
		/// 加速行驶
		/// </summary>
		/// <param name="crx"></param>
		internal virtual void Accelerate(DriveContext crx)
		{
			if (crx.iFrontHeadWay > 2 * crx.DriveParam.iSpeed)//保证了加速
			{
				if (crx.DriveParam.iSpeed < this.iDesiredSpeed)
				{
					//crx.iAcceleration += 1;
					crx.DriveParam.iSpeed+=crx.iAcceleration;
					//crx.iAcceleration = 1;
				}
			}//否则不加速
		}
		/// <summary>
		/// 减速行驶
		/// </summary>
		/// <param name="crx"></param>
		internal virtual void Decelerate(DriveContext crx)
		{
			//大于期望车速减速
			if (crx.DriveParam.iSpeed > this.iDesiredSpeed)
			{
				crx.DriveParam.iSpeed -= crx.iAcceleration;
			}
			///危险减速或者是前进距离过大减速
			if (crx.iFrontHeadWay < crx.DriveParam.iSpeed)//可能有bug
			{
				crx.DriveParam.iSpeed = crx.iFrontHeadWay-1;
			}

			if (crx.dRandom<crx.dModerationRatio && crx.DriveParam.iSpeed>1)//随机漫化
			{
				crx.DriveParam.iSpeed -= crx.iAcceleration;
			}
			//确保车速不小于零
			crx.DriveParam.iSpeed = Math.Max(crx.DriveParam.iSpeed,0);

		}
		
	}
	
	/// <summary>
	/// 驾驶员的驾驶行为策略，每个mobileebtity驾驶员行为不一样
	/// </summary>
	public abstract partial class DriveStrategy
	{
		internal XNodeDriveStrategy _WayStrategy;
		internal WayDriveStrategy _XNodeStrategy;
		
//		/// <summary>
//		/// 边界为红绿灯的地方是不更新和移动的或者做判断
//		/// </summary>
//		[System.Obsolete("replace with DriveMobile ")]
//		internal virtual void Drive(TrafficEntity staticEntity, Cell cell)
//		{
//			DriveContext ctx = new DriveContext(staticEntity, cell);
//			//cell.GetEntityGap(out ctx.iLaneGap,out ctx.iXNodeGap);
//			ctx.iFrontHeadWay = ctx.iLaneGap+ctx.iXNodeGap;
//
//			ctx.iAcceleration = cell.Car.iAcceleration;
//			ctx.DriveParam.iSpeed = cell.Car.iSpeed;
//
//			switch (staticEntity.EntityType)
//			{
//				case EntityType.Way:
//					
//					//车道内部的计算方法
//					ctx.iFrontSpeed = cell.nextCell == null ? -1 : cell.nextCell.Car.iSpeed;
//
//					//控制权转移出去了，这几个函数只是决策，告诉drivingcontext 如何走，下一步是执行
//					_XNodeStrategy.LaneChanging(ctx);//换道
//					_XNodeStrategy.Accelerate(ctx);//符合条件就加速
//					
//					
//					_XNodeStrategy.Decelerate(ctx);//否则减速
//					_XNodeStrategy.NormalRun(ctx);//更新位置
//					
//					//以下处理车的位置
//					Way re = staticEntity as Way;
//					//处理车辆的前进和换道行为
//					Lane rl= cell.Container as Lane;
//
//					if (ctx.DriveParam.iMoveY==0)
//					{
//						break;
//					}
//					//还在路段内部
//					if (ctx.DriveParam.iMoveY <= ctx.iLaneGap && ctx.iLaneGap>0)
//					{
//						cell.Grid= new Point(cell.Grid.X,cell.Grid.Y+ ctx.DriveParam.iMoveY);
//					}else //进入了交叉口
//					{
//						int iToEntitMoveStep = ctx.DriveParam.iMoveY - ctx.iLaneGap;
//						//计算轨迹
//						cell.CalcTrack(1);//初始化到路段的在交叉口的入口处
//						//初始化元胞的位置
//						cell.Track.pCurrPos = cell.Track.pTempPos;//这里已经移动了一步了
//						
//						//交叉口内部移动指定的步长
//						cell.TrackMove(iToEntitMoveStep-1);//修改节点
//
//						re.XNodeTo.AddCell(cell);//进入交叉口
//						
//						rl.RemoveCell();//离开原来的路段删除最前面的元胞
//
//					}
//					break;
//
//				case EntityType.XNode:
//
//					XNode rn = staticEntity as XNode;
//
//					//控制权转移出去了
//					_WayStrategy.LaneChanging(ctx);//换道
//					_WayStrategy.Accelerate(ctx);//符合条件就加速
//					
//					_WayStrategy.Decelerate(ctx);//否则减速
//					_WayStrategy.NormalRun(ctx);//更新位置
//					//修改空间位置
//					if (ctx.DriveParam.iMoveY <= ctx.iLaneGap&&ctx.iLaneGap>0)
//					{//修改空间位置
//						Point old = cell.Track.pCurrPos;
//						//修改pCurrPos
//						cell.TrackMove(ctx.DriveParam.iMoveY);//计算坐标
//						rn.MoveCell(old, cell.Track.pCurrPos);//移动元胞到指定的位置
//					}
//					else //进入了路段
//					{
//						rn.RemoveCell(cell);//离开交叉口删除cell
//
//						int iToEntitMoveStep = ctx.DriveParam.iMoveY - ctx.iLaneGap;
//						Lane to = cell.Track.ToLane;
//						if (to!=null)
//						{
//							///转换坐标
//							cell.Track.pCurrPos = new Point(to.Rank,iToEntitMoveStep);
//							//进入下一个交通实体
//							cell.Track.ToLane.EnterWaitedQueue(cell);
//						}
//					}
//					break;
//				default:
//					ThrowHelper.ThrowArgumentException("不正确的参数");
//					break;
//			}
//			cell.Car.iAcceleration = Math.Max(ctx.DriveParam.iAcceleration,1);
//			cell.Car.iSpeed = ctx.DriveParam.iSpeed;
//		}
//		
//		
	}
	
	/// <summary>
	/// 2016/1/27
	/// </summary>
	public abstract partial class DriveStrategy
	{
		internal virtual void DriveMobile(StaticEntity driveContainer,MobileEntity mobile)
		{
			
			var dctx=mobile.Observe();
			
			switch (driveContainer.EntityType)
			{
				case EntityType.Way:
	
					//Before updating mobiles status,update that on roadNode
					//the following four funtions are in charge of making decisions,not executing
					_XNodeStrategy.LaneChanging(dctx);//换道
					_XNodeStrategy.Accelerate(dctx);//符合条件就加速
					_XNodeStrategy.Decelerate(dctx);//否则减速
					_XNodeStrategy.NormalRun(dctx);//更新位置
					
					//To execute params setted hereinbefore

					Way currWay = driveContainer as Way;
					// if a mobile stops ,it must be blocked by traffic light or a mobile ahead
					if (dctx.DriveParam.iMoveY==0)
					{
						break;
					}
					//还在路段内部
					if (dctx.DriveParam.iMoveY <= dctx.iLaneGap)
					{
						mobile.Move(dctx.DriveParam.iMoveY);
						mobile.iSpeed = dctx.DriveParam.iSpeed;
						mobile.iAcceleration = dctx.DriveParam.iAcceleration;
						
					}else //进入了交叉口
					{
						var currLane = mobile.Container as Lane;
						//原有的车道删除该车辆
						currLane.Mobiles.RemoveFirst();

						//进入交叉口
						mobile.Container = currWay.XNodeTo;
						//calculate steps  to move in a xnode
						int iXNodeMoveStep = dctx.DriveParam.iMoveY - dctx.iLaneGap;
						
						//recalculate moblie shape position
						mobile.Move(iXNodeMoveStep);
						//进入交叉口的等待队列
						currWay.XNodeTo.MobilesInn.Enqueue(mobile);
					}
					break;

				case EntityType.XNode:

					var currNode = driveContainer as XNode;
					
					//控制权转移出去了
					_WayStrategy.LaneChanging(dctx);//换道
					_WayStrategy.Accelerate(dctx);//符合条件就加速

					_WayStrategy.Decelerate(dctx);//否则减速
					_WayStrategy.NormalRun(dctx);//更新位置
					
					//still runing within a xnode
					if (dctx.DriveParam.iMoveY <= dctx.iXNodeGap&&dctx.iXNodeGap>0)
					{
						//modify a mobiles's position
						mobile.Move(dctx.DriveParam.iMoveY);
					}
					else //进入了路段
					{
						if (dctx.DriveParam.iMoveY >dctx.iFrontHeadWay) {
							ThrowHelper.ThrowArgumentException("前进距离大于车头时距会导致撞车");
						}
						
						currNode.Mobiles.Remove(mobile);
						//var mobiledebug =
						
						int iXNodeMoveStep = dctx.DriveParam.iMoveY - dctx.iXNodeGap;
						
						var toLane = mobile.Track.ToLane;
						if (toLane!=null)
						{
							toLane.MobilesInn.Enqueue(mobile);
							
							//tempraryly modify mobile to get prepareed for moving 
							//mobile.Shape.Start = toLane.Shape.Start;//bug here to modified in the future
							mobile.Container=toLane;//a moible cross a lane and a xnode since it has a ilength
							
							mobile.Move(iXNodeMoveStep);		
						}

					}
					break;
				default:
					ThrowHelper.ThrowArgumentException("不正确的参数");
					break;
			}
			
			mobile.iAcceleration = Math.Max(dctx.DriveParam.iAcceleration,1);
			mobile.iSpeed = dctx.DriveParam.iSpeed;
			
		}
		
	}
	
	
	internal class WayStrategy : WayDriveStrategy { }
	internal class XNodeStrategy : XNodeDriveStrategy { }
	

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