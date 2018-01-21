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
					this._XNodeDriver= new DefaultXNodeDriver();
				}
				return this._XNodeDriver;
			}
		}
		internal WayDriver WayDriver
		{
			get{
				if (this._WayDriver==null) {
					this._WayDriver= new DefaultWayDriver();// XNodeDriver();
					
				}
				
				return this._WayDriver;
			}
		}

		/// <summary>
		/// template method
		/// </summary>
		/// <param name="driveContainer"></param>
		/// <param name="mobile"></param>
		internal virtual void DriveMobile(StaticEntity driveCtx,MobileEntity mobile)
		{

			var dctx=this.Observe(driveCtx,mobile);
			
			switch (driveCtx.EntityType)
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
									GetXNodeGap(currWay.To, temp, out iXNodeGap,mobile);
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
		private bool GetXNodeGap(XNode node, OxyzPointF opCurr, out int iGap,MobileEntity mobile)
		{
			//indicator to tell whether or not  a mobile is blocked
			//bool bReachEnd = false;
			bool bOccupied = false;
			int iCount = 0;

			OxyzPointF p = mobile.Track.NextPoint(opCurr);

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

	public class DefaultDriver:MobileDriver{}

}