using System;
using System.Diagnostics;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving;
using System.Drawing;
using SubSys_MathUtility;


namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// 	//should be abstract
	/// </summary>
	public abstract class XNodeDriver
	{
		internal virtual void DriveMobile(MobileOBJ mobile,DriveCtx dctx)
		{

			var currNode = mobile.Container as XNode;
			
			//控制权转移出去了
			this.LaneChanging(dctx);//换道
			this.Accelerate(dctx);//符合条件就加速

			this.Decelerate(dctx);//否则减速
			this.NormalRun(dctx);//更新位置
			
			if (mobile.ID ==2) {
				;
			}
			//still runing within a xnode
			if (dctx.Params.iMoveY <= dctx.iXNodeGap&&dctx.iXNodeGap>0)
			{
				//modify a mobiles's position
				mobile.Move(dctx.Params.iMoveY);
			}
			else //enter a lane from its container xnode
			{
				if (dctx.Params.iMoveY >dctx.iFrontHeadWay) {
					ThrowHelper.ThrowArgumentException("前进距离大于车头时距会导致撞车");
				}
				
				currNode.Mobiles.Remove(mobile);
				
				int iXNodeStep = dctx.Params.iMoveY - dctx.iXNodeGap;
				
				var toLane = mobile.Track.ToLane;
				if (toLane!=null)
				{
					toLane.MobilesInn.Enqueue(mobile);
					
					//tempraryly modify mobile to get prepareed for moving
					//mobile.Shape.Start = toLane.Shape.Start;//bug here to modified in the future
					mobile.Container=toLane;//a moible cross a lane and a xnode since it has a ilength
					
					mobile.Move(iXNodeStep);
				}
			}

			mobile.iAcceleration = Math.Max(dctx.Params.iAcceleration,1);
			mobile.iSpeed = dctx.Params.iSpeed;
			
		}
		
		
		/// <summary>
		/// 城市路网中，交叉口的期望速度
		/// </summary>
		internal int iDesiredSpeed = 2;


		
		/// <summary>
		/// To make a mobile on a crossing chang its drive track
		/// </summary>
		/// <param name="dctx"></param>
		internal virtual void LaneChanging(DriveCtx dctx)
		{ }
		internal virtual void Accelerate(DriveCtx dctx)
		{
			if (dctx.iFrontHeadWay >= 2 && dctx.Params.iSpeed < this.iDesiredSpeed)
			{
				dctx.Params.iSpeed += dctx.iAcceleration;
			}
		}
		internal virtual void Decelerate(DriveCtx dctx)
		{
			//交通规则限制减速
			if (dctx.Params.iSpeed > this.iDesiredSpeed)
			{
				dctx.Params.iSpeed -= dctx.iAcceleration;
			}
			//空间限制而减速
			if (dctx.iFrontHeadWay < dctx.Params.iSpeed)
			{
				dctx.Params.iSpeed = dctx.iFrontHeadWay;//-crc.iSaftySpace
			}

		}
		/// <summary>
		/// Prudent .neither aggressive nor slow
		/// </summary>
		/// <param name="crc"></param>
		internal virtual void NormalRun(DriveCtx crc)
		{
			//if (crc.iEntityGap > crc.Out.iSpeed)
			//{
			crc.Params.iMoveY = crc.Params.iSpeed;
			//}
			//else
			//{
			//    crc.Out.iMoveStepY
			//}
		}
		

	}
	/// <summary>
	/// 	//should be abstract
	/// </summary>
	public abstract class WayDriver
	{
		
		internal virtual void DriveMobile(MobileOBJ mobile,DriveCtx dctx)
		{
	
			var currLane = mobile.Container as Lane;
			var currWay = currLane.Container as Way;
			
			if (dctx.IsReachEnd == true) {
				currLane.Mobiles.RemoveFirst();
				return;
			}
			
			//Before updating mobiles status,update that on roadNode
			//the following four funtions are in charge of making decisions,not executing
			this.LaneChanging(dctx);//换道
			this.Accelerate(dctx);//符合条件就加速
			this.Decelerate(dctx);//否则减速
			this.NormalRun(dctx);//更新位置
			
			//To execute params setted hereinbefore
			
			// if a mobile stops ,it must be blocked by traffic light or a mobile ahead
			if (dctx.Params.iMoveY==0)
			{
				return;
			}
			//还在路段内部
			if (dctx.Params.iMoveY <= dctx.iLaneGap)
			{
				mobile.Move(dctx.Params.iMoveY);
				mobile.iSpeed = dctx.Params.iSpeed;
				mobile.iAcceleration = dctx.Params.iAcceleration;
				
			}else //进入了交叉口
			{
				//原有的车道删除该车辆
				currLane.Mobiles.RemoveFirst();

				//进入交叉口
				mobile.Container = currWay.To;
				//calculate steps  to move in a xnode
				int iXNodeMoveStep = dctx.Params.iMoveY - dctx.iLaneGap;
				
				//recalculate moblie shape position
				mobile.Move(iXNodeMoveStep);
				//进入交叉口的等待队列
				currWay.To.MobilesInn.Enqueue(mobile);
			}
			
			mobile.iAcceleration = Math.Max(dctx.Params.iAcceleration,1);
			mobile.iSpeed = dctx.Params.iSpeed;
			
		}
		
		
		/// <summary>
		/// 城市路段的期望速度
		/// </summary>
		internal int iDesiredSpeed=10;

		/// <summary>
		/// 换道的几种原因，1.由于路口转向必须换道，
		/// 2.由于寻求合适的理想行驶状态
		/// </summary>
		[System.Obsolete("换道模型暂时不实现")]
		internal virtual void LaneChanging(DriveCtx crx)
		{
		}
		/// <summary>
		/// 匀速行驶
		/// </summary>
		/// <param name="crx"></param>
		internal virtual void NormalRun(DriveCtx crx)
		{

			if (crx.Params.iSpeed +crx.iSafeHeadWay<= crx.iFrontHeadWay)
			{
				crx.Params.iMoveY = crx.Params.iSpeed;
			}
			else//小于速度个车头时距
			{
				crx.Params.iMoveY = crx.Params.iSpeed-crx.iSafeHeadWay;
			}

			//确保前进的距离不是负值
			crx.Params.iMoveY = Math.Max(0,crx.Params.iMoveY);

		}
		/// <summary>
		/// 加速行驶
		/// </summary>
		/// <param name="crx"></param>
		internal virtual void Accelerate(DriveCtx crx)
		{
			if (crx.iFrontHeadWay > 2 * crx.Params.iSpeed)//保证了加速
			{
				if (crx.Params.iSpeed < this.iDesiredSpeed)
				{
					//crx.iAcceleration += 1;
					crx.Params.iSpeed+=crx.iAcceleration;
					//crx.iAcceleration = 1;
				}
			}//否则不加速
		}
		/// <summary>
		/// 减速行驶
		/// </summary>
		/// <param name="crx"></param>
		internal virtual void Decelerate(DriveCtx crx)
		{
			//大于期望车速减速
			if (crx.Params.iSpeed > this.iDesiredSpeed)
			{
				crx.Params.iSpeed -= crx.iAcceleration;
			}
			///危险减速或者是前进距离过大减速
			if ( crx.Params.iSpeed > crx.iFrontHeadWay+crx.iSafeHeadWay)//可能有bug
			{
				crx.Params.iSpeed = crx.iFrontHeadWay-crx.iSafeHeadWay;
			}

			if (crx.dRandom<crx.dModerationRatio && crx.Params.iSpeed>1)//随机漫化
			{
				crx.Params.iSpeed -= crx.iAcceleration;
			}
			//确保车速不小于零
			crx.Params.iSpeed = Math.Max(crx.Params.iSpeed,0);

		}
		
		
		
		
	}


	public class DefaultXNodeDriver:XNodeDriver{}
	public class DefaultWayDriver:WayDriver{}
	
	//internal class WayAgent : WayDriver { }
	//internal class XNodeAgent : XNodeDriver { }

}