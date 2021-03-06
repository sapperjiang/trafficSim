
using System.Drawing;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.RoutePlan;
using SubSys_MathUtility;
using System;
using System.Collections.Generic;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 所有会动的交通实体的基类
    /// </summary>
    public abstract partial class MobileOBJ : TrafficOBJ
	{
		/// <summary>
		/// 当前车辆的速度
		/// </summary>
		internal int iSpeed;

		private static int MobileID = 0;
		private bool IsCopyed = false;

		
		/// <summary>
		/// 对象拷贝和值拷贝
		/// </summary>
		/// <returns></returns>
		public virtual MobileOBJ Clone()
		{
			MobileOBJ cm = this.MemberwiseClone() as MobileOBJ;
			cm.IsCopyed = true;
			//this.EntityType = EntityType.Mobile;
			return cm;
		}
		
		//public Color _Color;
		
		public EdgeRoute Route;//=new EdgeRoute();
		//public NodeRoute _nodeRoute;//=new NodeRoute();

		private MobileDriver _driver ;//
		
		internal MobileDriver Driver
		{
			get{
				if (this._driver == null) {
					this._driver = new DefaultDriver();
				}
				return this._driver;
			}
			set{
				this._driver=value;
			}
		}

		~MobileOBJ()
		{
			if (this.IsCopyed != true) {
				//	base.UnRegiser();
			}
		}
		//internal SpeedLevel CurrSpeed;
		/// <summary>
		/// 当前车辆的加速度
		/// </summary>
		internal int iAcceleration = 1;
		
		#region 属性部分
		
		public override int Length {
			get {//车辆的长度就是元胞的长度，车辆的形状，就是几个元胞的形状
				return this.Shape.Count;
			}
		}
		
		#endregion

		protected MobileOBJ(StaticOBJ bornContainer)
		{
			this._entityID = ++TrafficOBJ.EntityCounter;
			this.Route = new EdgeRoute();
			this._container = bornContainer;
		}
		
		protected MobileOBJ()
		{
			this._entityID = ++TrafficOBJ.EntityCounter;
			this.Route = new EdgeRoute();
			this.iSpeed = 0;
			this.iAcceleration = 1;
			
		}
		/// <summary>
		/// 用作记录态哈希表记录车辆的时间信息，以及用来确定什么时候进入路段
		/// </summary>
		internal int iTimeStep;
		

		/// <summary>
		/// 包围在交叉口内部的节点,内部点使用绝对坐标系
		/// </summary>
		private Track _track ;
		
		
		public Track Track
		{
			get{
				if (this._track==null) {
					this._track = new Track(this);
				}
				
				return this._track;
			}
		}

        internal void Run(StaticOBJ DriveEnvirnment)
		{
			this.Driver.DriveMobile(DriveEnvirnment,this);
		}

        [System.Obsolete("如果要知道车辆当前所在车道，则需要重新换到")]
        internal CarInfo CurrState
		{
            get
            {
                CarInfo ci = new CarInfo();//结构，值类型
                ci.iSpeed = this.iSpeed;
                ci.iAcc = this.iAcceleration;
                ci.iCarHashCode = this.GetHashCode();
                ci.iCarNum = this.ID;
                ci.iContainerHash = this.Container.GetHashCode();
                ci.iContainerHash = this.Container.GetHashCode();
                ci.currPos = this.Head;
                ci.iTimeStep = ISimCtx.iTimePulse;
                ci.iDrivedMileage = this.DrivedMileage;

                return ci;
            }
        }
		
		//------------------------------20160115------------------------------
		/// <summary>
		/// 获取车道上先进入的前一辆车，前一辆车不存在返null,
		/// the frontmobile is that entered a lane early than the current one
		/// if called on a xnode. return null
		/// </summary>
		public MobileOBJ Front {
			get {
				if (this.Container.EntityType == EntityType.XNode) {
					return null;
				}
				var ln = this.Container as Lane;
				var lme = ln.Mobiles.Find(this).Previous;
				if (lme != null)//&&lme.Previous!=null)
				{
					return lme.Value;
				}
				return null;
			}
		}
		/// <summary>
		///在车道上获取车辆的后一辆车
		/// </summary>
		public MobileOBJ Rear {
			get {
				Lane ln = this.Container as Lane;
				var lme = ln.Mobiles.Find(this).Next;
				if (lme != null) {
					return lme.Value;
				}
				return null;
			}
		}
		
		
//		/// <summary>
//		/// All mobiles can observe and return its runnning context
//		/// observe function must be called with the first mobile in front
//		/// </summary>
//		/// <returns></returns>
//		internal DriveCtx Observe()
//		{
//			DriveCtx dctx = new DriveCtx(this.Container as StaticEntity);
//
//			dctx.iAcceleration = this.iAcceleration;
//			dctx.iSpeed = this.iSpeed;
//
//			switch (this.Container.EntityType) {
//
//					// calculate headway on the left/right/current lane of current mobile
//				case EntityType.Lane:
//
//					var currLane = this.Container as Lane;
//
//					var currWay = currLane.Container as Way;
//					//current mobile
//					int iCurrStart = currLane.Shape.GetIndex(this.Shape.Start);
//					//current mobile
//					int iCurrEnd	  = currLane.Shape.GetIndex(this.Shape.End);
		////					int iLaneGap =  currLane.iLength-iCurrentStart;
//
//					if (this.Front!=null){
//					//behiand 14 .front 15. iLaneGap need to reduce 1
//						dctx.iLaneGap = currLane.Shape.GetIndex(this.Front.Shape.End)-iCurrStart-1;
//						dctx.iFrontSpeed = this.Front.iSpeed;
//						dctx.iXNodeGap = 0;
//						dctx.iFrontHeadWay = dctx.iLaneGap+dctx.iXNodeGap;
//					}
//					//front mobile is null, current mobile is the first one on this lane
//					//the first mobile needs to deal with a traffic light or/and a crossing(XNode)
//					else {//this.FrontMobile==null)
//
//						//front mobile,there's a signal light playing on the lane
//
//						//deal with that signal light
//						if (currLane.IsBlocked==true) {
//							dctx.iFrontHeadWay=currLane.Length-iCurrStart;
//						}
//						//deal with that crossing
//						else{//the current mobile is the first one to deal with a crossing
//							//先计算车辆的轨迹，where a mobile is heading for. right .left or straight forward
//							int iXNodeGap  = 0;
//							int iLaneGap =  currLane.Length-1-iCurrStart;
//							if (iLaneGap<=3*this.iSpeed)//space si more than triple car speed.
//							{
//								this.Track.Update();
//
//								if (this.Track.ToLane!=null) {
//
//									//class acts as parameters will pass its address to functions,so clone is used here
//									var temp  = currLane.Shape.End;
		////									if (this.ID == 2) {
		////										;
		////									}
//									//再计算剩余轨迹
//									GetXNodeGap(currWay.XNodeTo, temp, out iXNodeGap);
//								}else//toLane == null. reach destination
//								{
//									dctx.IsReachEnd =true;
//									iXNodeGap = 10;//to let the first car go away
//									iLaneGap = 0;
//								}
//							}
//
//							dctx.iXNodeGap = iXNodeGap;
//							dctx.iLaneGap = iLaneGap;
//							dctx.iFrontHeadWay = iXNodeGap+iLaneGap;
//							dctx.iFrontSpeed = -1;
//						}
//					}
//
//					//rear mobile
//					dctx.iRearHeadWay = iCurrStart;
//					if (this.Rear!=null) {
//						dctx.iRearHeadWay = iCurrEnd-currLane.Shape.GetIndex(this.Rear.Shape.Start);
//						dctx.iRearSpeed = this.Rear.iSpeed;
//
//					}else{//rear mobile is empty
//						dctx.iRearHeadWay = iCurrStart;//rear mobiel
//						dctx.iRightRearSpeed = -1;
//					}
//
//					//get dirving context on the left lane
//					this.GetSidesContext(currLane.Left,LaneType.Left,iCurrStart,iCurrEnd,ref dctx);
//					//get dirving context on the right lane
//					this.GetSidesContext(currLane.Right,LaneType.Right,iCurrStart,iCurrEnd,ref dctx);
//
//					break;
//
//				case EntityType.XNode:
//
//					var xnode = this.Container as XNode;
//
//					int iLaneEnGap = 0;
//					int iXNodeEnGap = 0;
//					//计算剩余轨迹数量//如果pcurrPos没到头，iXnodeGap等于零
//
//					//when a mobile is on a xnode .its headway is xnodeGap for a secend mobile
//					//the frist mobile bIsBlocked is never true;while its following may be true
//					var bIsBlocked =this.GetXNodeGap(xnode, this.Track.Current, out iXNodeEnGap);
//
		////					if (this.ID == 2) {//for debug
		////						;
		////					}
		////
//					if (bIsBlocked == false) {//the first mobile
//						var toLane = this.Track.ToLane;
//						//计算车道上的长度
//						if (toLane != null) {//no destination lane means a mobile has reach its destnation.
//							if (toLane.MobilesInn.Count>0) {//theres already mobiles waiting to enter tolane.
//								iLaneEnGap = 0;
//							}else {
//								var mobile = toLane.Mobiles.Last;
//								if (mobile!=null) {
//									iLaneEnGap = toLane.Shape.GetIndex(mobile.Value.Shape.End);
//								}else//no mobiles running at tolane
//								{
//									iLaneEnGap = toLane.Length;
//								}
//							}
//						}
//					}else//the a mobile blocked by its previous mobile on a xnode
//					{
//						iLaneEnGap = 0;
//					}
//
//
//					dctx.iLaneGap=iLaneEnGap;
//					dctx.iXNodeGap = iXNodeEnGap;
//
//					dctx.iFrontHeadWay = iXNodeEnGap+iLaneEnGap;
//
//
//					break;
//
//				case EntityType.Way:
//					throw new NotImplementedException("不应该传入这个参数，应在在车道上，或者是交叉口上");
//					break;
//
//					default:break;
//
//			}
//
//			return dctx;
//
//		}


//		/// <summary>
//		/// get left and right driving context
//		/// </summary>
//		/// <param name="lane">lane</param>
//		/// <param name="lanetype">lanetype of current lane</param>
//		/// <param name="iCurrentStart">headway of current mobile index</param>
//		/// <param name="iCurrentEnd">rear of current mobile index</param>
//		/// <param name="dc">out parameters</param>
//		private void  GetSidesContext(Lane lane,LaneType lanetype,int iCurrentStart,int iCurrentEnd ,ref DriveCtx dc)
//		{
//			//to make sure current lane got a lefe lane
//			if (lane==null)return ;
//
//			//headway on the lane
//			int iFrontHeadWay	=-1;
//			int iRearHeadWay	=-1;
//			int iFrontSpeed =-1;
//			int iRearSpeed	=-1;
//
//			//there's no mobile on lane
//			if (lane.Mobiles.Count>0)
//			{
//
//				//there's mobiles on lane
//				int iLeastGap = lane.Length;
//				int iTempGap = iLeastGap;
//				MobileEntity mobile=null;
//
//				//loop to find two adjacent mobiles on the lane.one rear,one ahead of the current mobile
//				foreach (var element in lane.Mobiles) {
//					iTempGap = lane.Shape.GetIndex(element.Shape.End)-iCurrentStart;
//					//make it positive,to find the nearest mobile on the left lane
//					if (Math.Abs(iTempGap)<Math.Abs(iLeastGap)) {
//						iLeastGap = iTempGap;
//						mobile=element;
//					}
//				}
//
//				//nearest mobile on the left is at the front
//				if (iLeastGap>=0) {
//					iFrontHeadWay=iLeastGap;
//					iFrontSpeed = mobile.iSpeed;
//
//					var rearMobile = mobile.Rear;
//					if (rearMobile!=null) {
//						iRearHeadWay =iCurrentEnd - lane.Shape.GetIndex(rearMobile.Shape.Start);
//						iRearSpeed = rearMobile.iSpeed;
//
//					}
//				}//nearest mobile on the left is at the behind
//				else{
//					//make it postive
//					iRearHeadWay = Math.Abs(iLeastGap);
//					iRearSpeed=mobile.iSpeed;
//
//					var frontMobile = mobile.Front;
//					if (frontMobile!=null) {
//						iFrontHeadWay =lane.Shape.GetIndex(frontMobile.Shape.End)-iCurrentStart;
//						iFrontSpeed=Front.iSpeed;
//					}
//				}
//			}
//			else {
//				iFrontHeadWay = lane.Length-iCurrentStart;
//				iRearHeadWay = iCurrentEnd;
//			}
//			//make driving observation true
//			switch (lanetype) {
//				case LaneType.Right:
//					dc.iRightFrontHeadWay 	= iFrontHeadWay;
//					dc.iRightFrontSpeed = iFrontSpeed;
//					dc.iRightRearHeadWay 	= iRearHeadWay;
//					dc.iRightRearSpeed  =iRearSpeed;
//					break;
//				case LaneType.Left:
//					dc.iLeftFrontHeadWay 	= iFrontHeadWay;
//					dc.iLeftFrontSpeed  = iFrontSpeed;
//					dc.iLeftRearHeadWay 	= iRearHeadWay;
//					dc.iLeftRearSpeed   =iRearSpeed;
//					break;
//				default:
//					throw new ArgumentException("parameter lanetype error!");
//					break;
//			}
//		}

		//------------------------20160130--------------------------------

		/// <summary>
		/// a mobiles'current lane/fromlane/tolane is recalculated each time on it enters a XNode
		/// </summary>
		public override IEntity Container {
			get {
				return base.Container;
			}
			set {

                if (base.Container!=null)
                {
                    //each time container is modified , fromlane/Tolane is recalculated
                    if (base.Container.EntityType==EntityType.Lane&&
				        value.EntityType== EntityType.XNode){
					    this.Track.Update();
				    }
                    if (base.Container.EntityType != value.EntityType)
                    {
                        this.iDrivedMileage += this.CurrMileage;
                    }
                }

				base.Container = value;
			}
		}

        //private int iCurrMileage=0;k
        private int CurrMileage
        {
            get {
                switch (this.Container.EntityType)
                {
                    case EntityType.Lane:
                        var currLane = this.Container as Lane;
                        int iCurrIndex = currLane.Shape.GetIndex(this.Head);
                        return iCurrIndex;
                        break;

                    case EntityType.XNode:
                       return (int)Coordinates.Distance(this.Head, this.Track.From);//.
                        break;

                    default:
                        ThrowHelper.ThrowArgumentException("小汽车所在的道路不是车道也不是交叉口无法获取当前里程");
                        break;
                }
                return 0;
            }
        }
        /// <summary>
        /// 元胞里程，要乘以元胞长度才是真实里程
        /// </summary>
        private int iDrivedMileage = 0;
        public int DrivedMileage
        {
            get
            {
                return iDrivedMileage+CurrMileage;
            }
        }

        /// <summary>
        /// move a car ahead for isteps
        /// </summary>
        /// <param name="iStep"></param>
        internal void Move(int iStep)
		{
			switch (this.Container.EntityType) {
				case EntityType.Lane:
					var currLane = this.Container as Lane;
					
					int iCurrIndex = currLane.Shape.GetIndex(this.Shape.Start);
					if (iCurrIndex==-1) {//this mobile is not in lane
						iCurrIndex = 0;
					}


					for (int i = 0; i < this.Shape.Count; i++) {
						if (iStep+iCurrIndex>=this.Shape.Count) {
							//deepclone is neceserry
							this.PrevShape=this.Shape.DeepClone();
							this.Shape[i]=currLane.Shape[iCurrIndex+iStep-i];
						}
						
					}
					break;
					
				case EntityType.XNode:
					
					//modify mobile
					this.PrevShape=this.Shape.DeepClone();
					
					var  temp = this.Track.Current;
					
					var stack = new Stack<OxyzPointF>(iStep);
					//stack dump as the following
					//a  	mobile start
					//b      mobile mid
					//c		mobile end
					int iCount = this.Shape.Count;
					
					
					//var queue = new Queue<OxyzPoint>(iCount);
					
					//save old value
					for (int i =iCount-1; i >= 0; i--) {
						stack.Push(this.Shape[i].Clone());
					}

					//calculate track point
					while (iStep-- > 0) {
						temp = Track.NextPoint(temp);
						stack.Push(temp);
					}
					
					//update new shape
					for (int i = 0; i < iCount; i++) {
						this.Shape[i]=stack.Pop();
					}
					
					break;
					
					default:break;
			}

			//to tell it changed
			//this.IsMoved = true;
		}
		
		private EntityShape _prevShape;
		public EntityShape PrevShape
		{
			get{
				if (this._prevShape ==null) {
					
					this._prevShape = new EntityShape();
				}
				if (this._prevShape.Count==0&&this.Shape.Count>0) {
					this._prevShape =this.Shape;
				}
				return this._prevShape;
			}
			set{
				
				this._prevShape = value;
			}
			
		}


		/// <summary>
		/// crossing to judge if a mobile is within crossing
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.ID.GetHashCode();
		}


        public OxyzPointF Head
        {
            get {
                return this.Shape.Start;
            }
        }

    }
	
	/// <summary>
	/// 交叉口坐标升级为3维度空间坐标
	/// </summary>
	public  partial class Track
	{

		private OxyzPointF opFrom;
		
		public OxyzPointF From
		{
			get{
//				if (this.opTo ==null) {
//					this.opTo = new OxyzPoint(0,0,0);
//				}
				return this.opFrom;
			}
			
		}
		private OxyzPointF opTo;
		
		public OxyzPointF To
		{
			get{
				return this.opTo;
			}

		}
		
		
		private Lane fromLane;
		
		public Lane FromLane
		{
			get{
				
				return this.fromLane;
			}
			set{
				this.fromLane = value;
				if (this.fromLane!=null) {
					this.opFrom = this.fromLane.Shape.End;
				}
				
			}
		}
		
		private Lane toLane;
		
		public Lane ToLane
		{
			get{
				//the follwing if should be removed in 1.0 version
				if (this.fromLane == this.toLane) {
					ThrowHelper.ThrowArgumentException("前车道和要去的车道是同一条车道");
				}
				return this.toLane;
			}
			set{

				this.toLane = value;
				if (this.toLane!=null) {
					this.opTo = this.toLane.Shape.Start;
				}
			}
		}
		/// <summary>
		/// return a copy of mobile shape start
		/// </summary>
		public OxyzPointF Current
		{
			get{
				return this.mobile.Head;
			}
		}
		/// <summary>
		/// return a copyfo mobile's next track point
		/// </summary>
		public OxyzPointF Next
		{
			get {
				return this.NextPoint(this.Current);
			}
		}
		
		private MobileOBJ mobile;
		internal Track(MobileOBJ me)
		{
			//this.opCurrPos = me.Shape.Start.Clone();
			this.mobile = me;
		}
		/// <summary>
		/// should be declared as private,for
		/// </summary>
		public Track(){}
		//private Track(){}
		
		public OxyzPointF NextPoint(OxyzPointF iCurrPoint)
		{

			OxyzPointF iNew = iCurrPoint;
			//算法保证每一个时间步长内都向目标终点接近，就是为了让其到终点的距离变小
			int iX =(int) (iCurrPoint._X - this.To._X);//当前位置减去目的位置
			int iY =(int) (iCurrPoint._Y - this.To._Y);
			///////////////////////////////
//			int iX = iCurrPoint._X - op._X;//当前位置减去目的位置
//			int iY = iCurrPoint._Y - op._Y;
			if (iX != 0)//等于0的情况什么也不做
			{
				iNew._X = iX > 0 ? --iNew._X : ++iNew._X;
			}
			if (iY != 0)//等于0的情况什么也不做
			{
				iNew._Y = iY > 0 ? --iNew._Y : ++iNew._Y;
			}
			if (iX==0&&iY==0)///已经到达了目标地点，两个点的坐标差值为0
			{
				iNew = new OxyzPointF(0, 0);
			}
			return iNew;
		}
		
		
		/// <summary>
		/// 在转弯的时候调用，根据车辆路径，寻找车辆要进入的下一条车道（左右转、直行）。从起始位置出发，前进iAheadSpace个间距时距
		/// </summary>
		internal virtual void Update()
		{
			if (this.mobile == null) {
				
				ThrowHelper.ThrowArgumentException("a track has no mobile before updating ,assigned one to it through constructor  internal Track(MobileEntity me) ");
			}
			if (mobile.Container.EntityType == EntityType.XNode) {
				return;
			}
			
			Lane currLane = mobile.Container as Lane;
			//get its tolane
			var currWay = currLane.Container as Way;
			var nextWay = this.mobile.Route.FindNext(currWay);
			
			//shape end means a point on a narrow like "---->"
			//a signal light is playing at a lane's shape end
			this.FromLane = currLane;
			this.opFrom = currLane.Shape.End;
			
			
			if (nextWay == null) {//a moblie is reaching its destination
				this.ToLane = null;
				//	this.opTo = OxyzPointF.Default;
				this.opTo =currLane.Shape.End;
				return;
			}
			
			int iTurn = mobile.Route.GetDirection(currWay);

			//车辆直行
			switch (iTurn) {
					
				case 0:
					//go straight foword
					if (nextWay.Lanes.Count < currLane.Rank) {//next way has less lanes than a mobiles'current one
						
						int iIdx =new Random(1).Next(nextWay.Lanes.Count) - 1;
						iIdx = iIdx<0?0:iIdx;
						this.ToLane = nextWay.Lanes[iIdx];
						
					} else {   //otherwise 目标车道数大于于本车道数
						this.ToLane = nextWay.Lanes[currLane.Rank - 1];
					}
					
					break;
					
				case 1://turn right
					//the outside lane is rightful for a mobile obeying traffic  regulations
					this.ToLane = nextWay.Lanes[nextWay.Lanes.Count -1];
					break;
					
				case -1://turn left
					//	int iIndex = new Random(1).Next(nextWay.Lanes.Count) - 1;
					this.ToLane = nextWay.Lanes[0];
					break;
					
				case 2://turn back
					//while truning backward a inside lane is rightful
					this.ToLane = nextWay.Lanes[0];
					break;
					
			}

			//shape start means a point at the end of a narrow like "---->"
			this.opTo = this.ToLane.Shape.Start;
		}
		
		
		
		
		
	}
}