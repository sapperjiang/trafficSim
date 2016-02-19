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
	internal  class XNodeDriver
	{
		internal virtual void DriveMobile(MobileEntity mobile,DriveCtx dctx)
		{

			var currNode = mobile.Container as XNode;
			
			//控制权转移出去了
			this.LaneChanging(dctx);//换道
			this.Accelerate(dctx);//符合条件就加速

			this.Decelerate(dctx);//否则减速
			this.NormalRun(dctx);//更新位置
			
//			if (mobile.ID ==2) {
//				;
//			}
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
		/// 城市路网中，三个元胞.交叉口的期望速度
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
	internal  class WayDriver
	{
		
		internal virtual void DriveMobile(MobileEntity mobile,DriveCtx dctx)
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
				mobile.Container = currWay.XNodeTo;
				//calculate steps  to move in a xnode
				int iXNodeMoveStep = dctx.Params.iMoveY - dctx.iLaneGap;
				
				//recalculate moblie shape position
				mobile.Move(iXNodeMoveStep);
				//进入交叉口的等待队列
				currWay.XNodeTo.MobilesInn.Enqueue(mobile);
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
	
	/// <summary>
	/// 驾驶员的驾驶行为策略，每个mobileebtity驾驶员行为不一样
	/// an abstract driver interfaces
	/// </summary>
	public abstract partial class MobileDriver
	{
		internal XNodeDriver _XNodeDriver;
		internal WayDriver _WayDriver;
		
		internal XNodeDriver XNodeDriver
		{
			get{
				if (this._XNodeDriver==null) {
					this._XNodeDriver= new XNodeDriver();
				}
				return this._XNodeDriver;
			}
		}
		internal WayDriver WayDriver
		{
			get{
				if (this._WayDriver==null) {
					this._WayDriver= new WayDriver();// XNodeDriver();
					
				}
				
				return this._WayDriver;
			}
		}

		/// <summary>
		/// template method
		/// </summary>
		/// <param name="driveContainer"></param>
		/// <param name="mobile"></param>
		internal virtual void DriveMobile(StaticEntity driveContainer,MobileEntity mobile)
		{

			var dctx=this.Observe(driveContainer,mobile);
			
			switch (driveContainer.EntityType)
			{
					//use way  as container for lane changing
				case EntityType.Lane:
					
					this.WayDriver.DriveMobile(mobile,dctx);
					
					break;

				case EntityType.XNode:
				
					this.XNodeDriver.DriveMobile(mobile,dctx);
					
					break;
				default:
					ThrowHelper.ThrowArgumentException("不正确的参数");
					break;
			}
			
			mobile.iAcceleration = Math.Max(dctx.Params.iAcceleration,1);
			mobile.iSpeed = dctx.Params.iSpeed;
			
		}
		
		
		///////////////////////////////////////////////////
		internal DriveCtx Observe(StaticEntity driveEn,MobileEntity mobile)
		{
			DriveCtx dctx = new DriveCtx(driveEn);
				

			dctx.iAcceleration = mobile.iAcceleration;
			dctx.iSpeed = mobile.iSpeed;
			
			switch (driveEn.EntityType) {
					
					// calculate headway on the left/right/current lane of current mobile
				case EntityType.Lane:
					
					var currLane = mobile.Container as Lane;
					
					var currWay = currLane.Container as Way;
					//current mobile
					int iCurrStart = currLane.Shape.GetIndex(mobile.Shape.Start);
					//current mobile
					int iCurrEnd	  = currLane.Shape.GetIndex(mobile.Shape.End);
//					int iLaneGap =  currLane.iLength-iCurrentStart;
					
					if (mobile.Front!=null){
						//behiand 14 .front 15. iLaneGap need to reduce 1
						dctx.iLaneGap = currLane.Shape.GetIndex(mobile.Front.Shape.End)-iCurrStart-1;
						dctx.iFrontSpeed = mobile.Front.iSpeed;
						dctx.iXNodeGap = 0;
						dctx.iFrontHeadWay = dctx.iLaneGap+dctx.iXNodeGap;
					}
					//front mobile is null, current mobile is the first one on this lane
					//the first mobile needs to deal with a traffic light or/and a crossing(XNode)
					else {//this.FrontMobile==null)
						
						//front mobile,there's a signal light playing on the lane
						
						//deal with that signal light
						if (currLane.IsBlocked==true) {
							dctx.iFrontHeadWay=currLane.Length-iCurrStart;
						}
						//deal with that crossing
						else{//the current mobile is the first one to deal with a crossing
							//先计算车辆的轨迹，where a mobile is heading for. right .left or straight forward
							int iXNodeGap  = 0;
							int iLaneGap =  currLane.Length-1-iCurrStart;
							if (iLaneGap<=3*mobile.iSpeed)//space si more than triple car speed.
							{
								mobile.Track.Update();
								
								if (mobile.Track.ToLane!=null) {
									
									//class acts as parameters will pass its address to functions,so clone is used here
									var temp  = currLane.Shape.End;
//									if (this.ID == 2) {
//										;
//									}
									//再计算剩余轨迹
									GetXNodeGap(currWay.XNodeTo, temp, out iXNodeGap,mobile);
								}else//toLane == null. reach destination
								{
									dctx.IsReachEnd =true;
									iXNodeGap = 10;//to let the first car go away
									iLaneGap = 0;
								}
							}

							dctx.iXNodeGap = iXNodeGap;
							dctx.iLaneGap = iLaneGap;
							dctx.iFrontHeadWay = iXNodeGap+iLaneGap;
							dctx.iFrontSpeed = -1;
						}
					}
					
					//rear mobile
					dctx.iRearHeadWay = iCurrStart;
					if (mobile.Rear!=null) {
						dctx.iRearHeadWay = iCurrEnd-currLane.Shape.GetIndex(mobile.Rear.Shape.Start);
						dctx.iRearSpeed = mobile.Rear.iSpeed;
						
					}else{//rear mobile is empty
						dctx.iRearHeadWay = iCurrStart;//rear mobiel
						dctx.iRightRearSpeed = -1;
					}
					
					//get dirving context on the left lane
					this.GetSidesContext(currLane.Left,LaneType.Left,iCurrStart,iCurrEnd,ref dctx);
					//get dirving context on the right lane
					this.GetSidesContext(currLane.Right,LaneType.Right,iCurrStart,iCurrEnd,ref dctx);
					
					break;
					
				case EntityType.XNode:
					
					var xnode = driveEn as XNode;
					
					int iLaneEnGap = 0;
					int iXNodeEnGap = 0;
					//计算剩余轨迹数量//如果pcurrPos没到头，iXnodeGap等于零
					
					//when a mobile is on a xnode .its headway is xnodeGap for a secend mobile
					//the frist mobile bIsBlocked is never true;while its following may be true
					var bIsBlocked =this.GetXNodeGap(xnode, mobile.Track.Current, out iXNodeEnGap,mobile);

					if (bIsBlocked == false) {//the first mobile
						var toLane = mobile.Track.ToLane;
						//计算车道上的长度
						if (toLane != null) {//no destination lane means a mobile has reach its destnation.
							if (toLane.MobilesInn.Count>0) {//theres already mobiles waiting to enter tolane.
								iLaneEnGap = 0;
							}else {
								var lastMobile = toLane.Mobiles.Last;
								if (lastMobile!=null) {
									iLaneEnGap = toLane.Shape.GetIndex(lastMobile.Value.Shape.End);
								}else//no mobiles running at tolane
								{
									iLaneEnGap = toLane.Length;
								}
							}
						}
					}else//the a mobile blocked by its previous mobile on a xnode
					{
						iLaneEnGap = 0;
					}

					
					dctx.iLaneGap=iLaneEnGap;
					dctx.iXNodeGap = iXNodeEnGap;
					
					dctx.iFrontHeadWay = iXNodeEnGap+iLaneEnGap;
					
					
					break;
					
				case EntityType.Way:
					throw new NotImplementedException("不应该传入这个参数，应在在车道上，或者是交叉口上");
					break;
					
					default:break;
					
			}
			
			return dctx;
			
		}


		/// <summary>
		/// get left and right driving context
		/// </summary>
		/// <param name="lane">lane</param>
		/// <param name="lanetype">lanetype of current lane</param>
		/// <param name="iCurrentStart">headway of current mobile index</param>
		/// <param name="iCurrentEnd">rear of current mobile index</param>
		/// <param name="dc">out parameters</param>
		private void  GetSidesContext(Lane lane,LaneType lanetype,int iCurrentStart,int iCurrentEnd ,ref DriveCtx dc)
		{
			//to make sure current lane got a lefe lane
			if (lane==null)return ;
			
			//headway on the lane
			int iFrontHeadWay	=-1;
			int iRearHeadWay	=-1;
			int iFrontSpeed =-1;
			int iRearSpeed	=-1;
			
			//there's no mobile on lane
			if (lane.Mobiles.Count>0)
			{
				
				//there's mobiles on lane
				int iLeastGap = lane.Length;
				int iTempGap = iLeastGap;
				MobileEntity mobile=null;
				
				//loop to find two adjacent mobiles on the lane.one rear,one ahead of the current mobile
				foreach (var element in lane.Mobiles) {
					iTempGap = lane.Shape.GetIndex(element.Shape.End)-iCurrentStart;
					//make it positive,to find the nearest mobile on the left lane
					if (Math.Abs(iTempGap)<Math.Abs(iLeastGap)) {
						iLeastGap = iTempGap;
						mobile=element;
					}
				}
				
				//nearest mobile on the left is at the front
				if (iLeastGap>=0) {
					iFrontHeadWay=iLeastGap;
					iFrontSpeed = mobile.iSpeed;
					
					var rearMobile = mobile.Rear;
					if (rearMobile!=null) {
						iRearHeadWay =iCurrentEnd - lane.Shape.GetIndex(rearMobile.Shape.Start);
						iRearSpeed = rearMobile.iSpeed;
						
					}
				}//nearest mobile on the left is at the behind
				else{
					//make it postive
					iRearHeadWay = Math.Abs(iLeastGap);
					iRearSpeed=mobile.iSpeed;
					
					var frontMobile = mobile.Front;
					if (frontMobile!=null) {
						iFrontHeadWay =lane.Shape.GetIndex(frontMobile.Shape.End)-iCurrentStart;
						iFrontSpeed=mobile.Front.iSpeed;
					}
				}
			}
			else {
				iFrontHeadWay = lane.Length-iCurrentStart;
				iRearHeadWay = iCurrentEnd;
			}
			//make driving observation true
			switch (lanetype) {
				case LaneType.Right:
					dc.iRightFrontHeadWay 	= iFrontHeadWay;
					dc.iRightFrontSpeed = iFrontSpeed;
					dc.iRightRearHeadWay 	= iRearHeadWay;
					dc.iRightRearSpeed  =iRearSpeed;
					break;
				case LaneType.Left:
					dc.iLeftFrontHeadWay 	= iFrontHeadWay;
					dc.iLeftFrontSpeed  = iFrontSpeed;
					dc.iLeftRearHeadWay 	= iRearHeadWay;
					dc.iLeftRearSpeed   =iRearSpeed;
					break;
				default:
					throw new ArgumentException("parameter lanetype error!");
					break;
			}
		}

		
		/// <summary>
		/// 计算mobile在交叉口内部可以走多少步
		/// </summary>
		/// <param name="rN"></param>
		/// <param name="pCurrent">current position</param>
		/// <param name="Gap">return value</param>
		/// <returns></returns>
		private bool GetXNodeGap(XNode node, OxyzPoint opCurr, out int iGap,MobileEntity mobile)
		{
			//indicator to tell whether or not  a mobile is blocked
			//bool bReachEnd = false;
			bool bOccupied = false;
			int iCount = 0;

			OxyzPoint p = mobile.Track.NextPoint(opCurr);

			while ((bOccupied=node.IsOccupied(p)) == false) {
				if (p._X == 0 && p._Y == 0) {
//					bReachEnd = true;
					break;
				}
				p = mobile.Track.NextPoint(p);

				iCount++;
			}
			iGap = iCount;
			//return bReachEnd
			return bOccupied;
		}
		
		////////////////////////////////////////////////////////////////
		
	}
	
	public class DefaultDriver:MobileDriver
	{}
	
	//internal class WayAgent : WayDriver { }
	//internal class XNodeAgent : XNodeDriver { }

}