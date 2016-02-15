using SubSys_SimDriving;
using System.Drawing;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.RoutePlan;
using SubSys_MathUtility;
using System;
using System.Collections.Generic;

namespace SubSys_SimDriving
{
	public abstract partial class MobileEntity : TrafficEntity
	{
		/// <summary>
		/// ��ǰ�������ٶ�
		/// </summary>
		internal int iSpeed;
		
	}
	
	//--------------------2015��1��11���������ӵ�����-------------------
	/// <summary>
	/// ���лᶯ�Ľ�ͨʵ��Ļ���
	/// </summary>
	public abstract partial class MobileEntity : TrafficEntity
	{
		private static int MobileID = 0;
		private bool IsCopyed = false;
		
		public DriveStrategy Strategy;
		
		/// <summary>
		/// ���󿽱���ֵ����
		/// </summary>
		/// <returns></returns>
		public virtual MobileEntity Clone()
		{
			MobileEntity cm = this.MemberwiseClone() as MobileEntity;
			cm.IsCopyed = true;
			//this.EntityType = EntityType.Mobile;
			return cm;
		}
		
		public Color _Color;
		
		public EdgeRoute Route;//=new EdgeRoute();
		//public NodeRoute _nodeRoute;//=new NodeRoute();

		internal DriveStrategy Driver = new DefaultDriveAgent();

		~MobileEntity()
		{
			if (this.IsCopyed != true) {
				base.UnRegiser();
			}
		}
		//internal SpeedLevel CurrSpeed;
		/// <summary>
		/// ��ǰ�����ļ��ٶ�
		/// </summary>
		internal int iAcceleration = 1;
		
		#region ���Բ���
		
		public override int iLength {
			get {//�����ĳ��Ⱦ���Ԫ���ĳ��ȣ���������״�����Ǽ���Ԫ������״
				return this.Shape.Count;
			}
		}
		
		#endregion
		
		
		//innisialization without container parameter is forbidden.
//		public MobileEntity()
//		{
//		}
//
//		[System.Obsolete("��ʱ,��Ϊcar���Ѿ���ʱ")]
//		public MobileEntity(SmallCar cm)
//		{
//			//��trafficmodel �̳еı����ֶ�
//			this._EntityID = ++MobileEntity.MobileID;
//
//			this.EntityType = EntityType.Mobile;
//			this.Color = Color.Green;
//			this.iSpeed = 0;
//			base.Register();
//			this.EdgeRoute = new EdgeRoute();
//			this.NodeRoute = new NodeRoute();
//		}
		
		
		public MobileEntity(StaticEntity bornContainer)
		{
			this._EntityID = ++TrafficEntity.EntityCounter;
			
			//this.EntityType = EntityType.SmallCar;
//			this.Color = Color.Green;
//			this.iSpeed = 0;
			base.Register();
			
			this.Route = new EdgeRoute();
			// this._nodeRoute = new NodeRoute();
			
			this._container = bornContainer;
			
			this.Shape.Add(this.Container.Shape.Start);

		}
		
		/// <summary>
		/// ������¼̬��ϣ���¼������ʱ����Ϣ���Լ�����ȷ��ʲôʱ�����·��
		/// </summary>
		internal int iTimeStep;
		

		/// <summary>
		/// ��Χ�ڽ�����ڲ��Ľڵ�,�ڲ���ʹ�þ�������ϵ
		/// </summary>
		private Track track ;
		
		
		public Track Track
		{
			get{
				if (this.track==null) {
					this.track = new Track(this);
				}
				
				return this.track;
			}
			
		}

		
		internal void Drive(StaticEntity DriveEnvirnment)
		{
//			this.Driver.dr(DriveEnvirnment);
			//�������Ҫ��д
//			this.DriveStg.Drive(rN,this);
		}
		
		[System.Obsolete("����ϵͳ�����⣬postion����ֵ roadhash������,iTimeStep������")]
		internal CarInfo GetCarInfo()
		{
			CarInfo ci = new CarInfo();//�ṹ��ֵ����
			ci.iSpeed = this.iSpeed;
			ci.iAcc = this.iAcceleration;
			ci.iCarHashCode = this.GetHashCode();
			ci.iCarNum = this.ID;
			
			ci.iTimeStep = ISimCtx.iCurrTimeStep;
			
			
			if (this.Container.EntityType == EntityType.Lane) {
				ci.iPos = this.Grid.Y + (int)this.Container.Shape[0]._X;
			} else if (this.Container.EntityType == EntityType.XNode) {
				ci.iPos = this.Container.Grid.X + this.Grid.X - 1;
			}
			return ci;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rN">��������Ľ���ڻ��ߣ���ǰ����ڣ��򼴽��뿪�Ľ����</param>
		/// <param name="ct"></param>
		/// <returns>weher or not reaching its destination </returns>
		/// 
		
		/// <summary>
		/// ����mobile�ڽ�����ڲ������߶��ٲ�
		/// </summary>
		/// <param name="rN"></param>
		/// <param name="pCurrent">current position</param>
		/// <param name="Gap">return value</param>
		/// <returns></returns>
		private bool GetXNodeTrackGap(XNode rN, OxyzPoint pCurrent, out int Gap)
		{
			bool bReachEnd = false;
			int iCount = 0;

			OxyzPoint p = Track.NextPoint(pCurrent);
			while (rN.IsOccupied(p) == false) {
				if (p._X == 0 && p._Y == 0) {
					bReachEnd = true;
					break;
				}
				p = Track.NextPoint(p);

				iCount++;
			}
			Gap = iCount;
			return bReachEnd;
		}
		
		//------------------------------20160115------------------------------
		/// <summary>
		/// ��ȡ�������Ƚ����ǰһ������ǰһ���������ڷ�null,the frontmobile is that entered a lane early than the current one
		/// </summary>
		public MobileEntity FrontMobile {
			get {
				Lane ln = this.Container as Lane;
				var lme = ln.Mobiles.Find(this).Previous;
				if (lme != null) {
					return lme.Value;
				}
				return null;
			}
		}
		/// <summary>
		///�ڳ����ϻ�ȡ�����ĺ�һ����
		/// </summary>
		public MobileEntity RearMobile {
			get {
				Lane ln = this.Container as Lane;
				var lme = ln.Mobiles.Find(this).Next;
				if (lme != null) {
					return lme.Value;
				}
				return null;
			}
		}
		
		
		/// <summary>
		/// All mobiles can observe and return its runnning context
		/// observe function must be called with the first mobile in front
		/// </summary>
		/// <returns></returns>
		internal DriveContext Observe()
		{
			DriveContext dctx = new DriveContext(this.Container as StaticEntity);
			
			dctx.iAcceleration = this.iAcceleration;
			dctx.iSpeed = this.iSpeed;
			
			switch (this.Container.EntityType) {
					
					// calculate headway on the left/right/current lane of current mobile
				case EntityType.Lane:
					
					var currLane = this.Container as Lane;
					
					var currWay = currLane.Container as Way;
					//current mobile
					int iCurrentStart = currLane.Shape.GetIndex(this.Shape.Start);
					//current mobile
					int iCurrentEnd	  = currLane.Shape.GetIndex(this.Shape.End);
//					int iLaneGap =  currLane.iLength-iCurrentStart;
					
					if (this.FrontMobile!=null){
						dctx.iFrontHeadWay = currLane.Shape.GetIndex(this.FrontMobile.Shape.End)-iCurrentStart;
						dctx.iFrontSpeed = this.FrontMobile.iSpeed;
						dctx.iLaneGap = dctx.iFrontHeadWay;
						
					}
					//front mobile is null, current mobile is the first one on this lane
					//the first mobile needs to deal with a traffic light or/and a crossing(XNode)
					else {//this.FrontMobile==null)
						
						//front mobile,there's a signal light playing on the lane
						
						//deal with that signal light
						if (currLane.IsBlocked==true) {
							dctx.iFrontHeadWay=currLane.iLength-iCurrentStart;
						}
						//deal with that crossing
						else{//the current mobile is the first one to deal with a crossing
							//�ȼ��㳵���Ĺ켣��where a mobile is heading for. right .left or straight forward
							
							
							int iXNodeGap  = -1;
							int iLaneGap =  currLane.iLength-1-iCurrentStart;
							if (iLaneGap<3*this.iSpeed)//space si more than triple car speed.
							{
								this.Track.Update();
								
								if (this.Track.ToLane!=null) {
									
									//class acts as parameters will pass its address to functions,so clone is used here
									var temp  = currLane.Shape.End.Clone();

									//�ټ���ʣ��켣
									GetXNodeTrackGap(currWay.XNodeTo, temp, out iXNodeGap);
								}
							}

							dctx.iXNodeGap = iXNodeGap;
							dctx.iLaneGap = iLaneGap;
							dctx.iFrontHeadWay = iXNodeGap+iLaneGap;
							dctx.iFrontSpeed = -1;
						}
					}
					
					//rear mobile
					dctx.iRearHeadWay = iCurrentStart;
					if (this.RearMobile!=null) {
						dctx.iRearHeadWay = iCurrentEnd-currLane.Shape.GetIndex(this.RearMobile.Shape.Start);
						dctx.iRearSpeed = this.RearMobile.iSpeed;
						
					}else{//rear mobile is empty
						dctx.iRearHeadWay = iCurrentStart;//rear mobiel
						dctx.iRightRearSpeed = -1;
					}
					
					//get dirving context on the left lane
					this.GetSidesDrivingContext(currLane.LeftLane,LaneType.Left,iCurrentStart,iCurrentEnd,ref dctx);
					//get dirving context on the right lane
					this.GetSidesDrivingContext(currLane.RightLane,LaneType.Right,iCurrentStart,iCurrentEnd,ref dctx);
					
					break;
					
				case EntityType.XNode:
					
					XNode rN = this.Container as XNode;
					
					int iLaneEntityGap = 0;
					int iXNodeEntityGap = 0;
					//����ʣ��켣����//���pcurrPosû��ͷ��iXnodeGap������
					
					this.GetXNodeTrackGap(rN, this.Track.opCurrPos, out iXNodeEntityGap);
					
					var toLane = this.Track.ToLane;
					//���㳵���ϵĳ���
					if (toLane != null) {
						if (toLane.MobilesInn.Count>0) {//theres already mobiles waiting to enter tolane.
							iLaneEntityGap = 0;
						}else {
							var mobile = toLane.Mobiles.Last;
							if (mobile!=null) {
								iLaneEntityGap = toLane.Shape.GetIndex(mobile.Value.Shape.End);
							}else//no mobiles running at tolane
							{
								iLaneEntityGap = toLane.iLength;
							}
						}
					}
					
					dctx.iLaneGap=iLaneEntityGap;
					dctx.iXNodeGap = iXNodeEntityGap;
					
					dctx.iFrontHeadWay = iXNodeEntityGap+iLaneEntityGap;
					break;
					
				case EntityType.Way:
					throw new NotImplementedException("��Ӧ�ô������������Ӧ���ڳ����ϣ������ǽ������");
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
		private void  GetSidesDrivingContext(Lane lane,LaneType lanetype,int iCurrentStart,int iCurrentEnd ,ref DriveContext dc)
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
				int iLeastGap = lane.iLength;
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
					
					var rearMobile = mobile.RearMobile;
					if (rearMobile!=null) {
						iRearHeadWay =iCurrentEnd - lane.Shape.GetIndex(rearMobile.Shape.Start);
						iRearSpeed = rearMobile.iSpeed;
						
					}
				}//nearest mobile on the left is at the behind
				else{
					//make it postive
					iRearHeadWay = Math.Abs(iLeastGap);
					iRearSpeed=mobile.iSpeed;
					
					var frontMobile = mobile.FrontMobile;
					if (frontMobile!=null) {
						iFrontHeadWay =lane.Shape.GetIndex(frontMobile.Shape.End)-iCurrentStart;
						iFrontSpeed=FrontMobile.iSpeed;
					}
				}
			}
			else {
				iFrontHeadWay = lane.iLength-iCurrentStart;
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

		//------------------------20160130--------------------------------

		/// <summary>
		/// a mobiles'current lane/fromlane/tolane is recalculated each time on it enters a XNode
		/// </summary>
		public override ITrafficEntity Container {
			get {
				return base.Container;
			}
			set {
				//each time container is modified , fromlane/Tolane is recalculated
				if (this.Container.EntityType==EntityType.Lane&&
				    value.EntityType== EntityType.XNode){
					this.Track.Update();
				}
				
				base.Container = value;
			}
		}

		internal void Move(int iStep)
		{
			
			switch (this.Container.EntityType) {
				case EntityType.Lane:
					var currLane = this.Container as Lane;
					
					if (currLane.Shape.End._X == 28&&currLane.Shape.End._Y==20&&currLane.Shape[0]._X==8) {
						;
					}
					
					int iCurrIndex = currLane.Shape.GetIndex(this.Shape.Start);
					if (iCurrIndex==-1) {//this mobile is not in lane
						iCurrIndex = 0;
					}


					for (int i = 0; i < this.Shape.Count; i++) {
						if (iStep+iCurrIndex>=this.Shape.Count) {
							
							this.PrevShape=this.Shape;
							this.Shape[i]=currLane.Shape[iCurrIndex+iStep-i];

						}
						
					}
					break;
					
				case EntityType.XNode:
					
					OxyzPoint  p = this.Track.Current;
					
					var stack = new Stack<OxyzPoint>(iStep);
					//stack dump as the following
					//a  	mobile start
					//b      mobile mid
					//c		mobile end
					
					//calculate track point
					while (iStep-- > 0) {
						p = Track.NextPoint(p);
						
						stack.Push(p);
					}
					//modify mobile
					this.PrevShape=this.Shape;
					for (int i = 0; i < this.Shape.Count; i++) {
						
						this.Shape[i]=stack.Pop();
					}
					break;
					
					default:break;
			}

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



		public override	int GetHashCode()
		{
			return this.ID.GetHashCode();
		}

		
		public override EntityShape Shape
		{
			get
			{
				//var way = this;
				
				EntityShape eShape = base.Shape;
				
				if (eShape.Count>0) {
					if (eShape.Start._X == 28&&eShape.Start._Y==20) {
						;
					}
				}
				return eShape	;
			}
		}
		
		
	}
	
	/// <summary>
	/// �������������Ϊ3ά�ȿռ�����
	/// </summary>
	public  partial class Track
	{
		
		[System.Obsolete("repalce with opTempPos")]
		internal Point pTempPos;
		[System.Obsolete("repalce with opCurrPos")]
		public Point pCurrPos;
		[System.Obsolete("repalce with opFromPos")]
		public Point pFromPos;
		[System.Obsolete("repalce with opToPos")]
		public Point pToPos;
		
		internal Point NextPoint(Point iCurrPoint)
		{
			
			Point iNew = iCurrPoint;
			//�㷨��֤ÿһ��ʱ�䲽���ڶ���Ŀ���յ�ӽ�������Ϊ�����䵽�յ�ľ����С
			int iX = iCurrPoint.X - this.pToPos.X;//��ǰλ�ü�ȥĿ��λ��
			int iY = iCurrPoint.Y - this.pToPos.Y;
			if (iX != 0)//����0�����ʲôҲ����
			{
				iNew.X = iX > 0 ? --iNew.X : ++iNew.X;
			}
			if (iY != 0)//����0�����ʲôҲ����
			{
				iNew.Y = iY > 0 ? --iNew.Y : ++iNew.Y;
			}
			if (iX==0&&iY==0)///�Ѿ�������Ŀ��ص㣬������������ֵΪ0
			{
				iNew = new Point(0, 0);
			}
			return iNew;
		}
		
		//----------------------------------20160131
		public OxyzPoint opCurrPos;
		public OxyzPoint opNextPos;
		
		
//		private OxyzPoint opTempPos;//=new OxyzPoint();
//		public OxyzPoint Temp
//		{
//			get{if (this.opTempPos ==null) {
//					this.opTempPos = new OxyzPoint(0,0,0);
//				}
//				return this.opTempPos;
//			}
//		}

		private OxyzPoint opFrom;
		
		public OxyzPoint From
		{
			get{if (this.opTo ==null) {
					this.opTo = new OxyzPoint(0,0,0);
				}
				return this.opTo;
			}
			
		}
		private OxyzPoint opTo;
		
		public OxyzPoint To
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
			return this.toLane;
		}
		set{
			if (this.FromLane == this.ToLane) {
				ThrowHelper.ThrowArgumentException("ǰ������Ҫȥ�ĳ�����ͬһ������");
			}
			this.toLane = value;
			if (this.toLane!=null) {
				this.opTo = this.toLane.Shape.Start;
			}
		}
	}
	
	//internal OxyzPoint opCurrent;
	
	public OxyzPoint Current
	{
		get{
			return this.mobile.Shape.Start;
		}
	}
	public OxyzPoint Next
	{
		get {
			return this.NextPoint(this.Current);
		}
	}
	
	private MobileEntity mobile;
	internal Track(MobileEntity me)
	{
		this.opCurrPos = me.Shape.Start;
		this.mobile = me;
	}
	/// <summary>
	/// should be declared as private,for
	/// </summary>
	public Track(){}
	//private Track(){}
	
	public  OxyzPoint NextPoint(OxyzPoint iCurrPoint)
	{

		OxyzPoint iNew = iCurrPoint;
		//�㷨��֤ÿһ��ʱ�䲽���ڶ���Ŀ���յ�ӽ�������Ϊ�����䵽�յ�ľ����С
		int iX = iCurrPoint._X - this.To._X;//��ǰλ�ü�ȥĿ��λ��
		int iY = iCurrPoint._Y - this.To._Y;
		///////////////////////////////
//			int iX = iCurrPoint._X - op._X;//��ǰλ�ü�ȥĿ��λ��
//			int iY = iCurrPoint._Y - op._Y;
		if (iX != 0)//����0�����ʲôҲ����
		{
			iNew._X = iX > 0 ? --iNew._X : ++iNew._X;
		}
		if (iY != 0)//����0�����ʲôҲ����
		{
			iNew._Y = iY > 0 ? --iNew._Y : ++iNew._Y;
		}
		if (iX==0&&iY==0)///�Ѿ�������Ŀ��ص㣬������������ֵΪ0
		{
			iNew = new OxyzPoint(0, 0);
		}
		return iNew;
	}
	
	
	/// <summary>
	/// ��ת���ʱ����ã����ݳ���·����Ѱ�ҳ���Ҫ�������һ������������ת��ֱ�У�������ʼλ�ó�����ǰ��iAheadSpace�����ʱ��
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
			this.opTo = OxyzPoint.Default;
			return;
		}
		
		int iTurn = mobile.Route.GetDirection(currWay);

		//����ֱ��
		switch (iTurn) {
				
			case 0:
				//go straight foword
				if (nextWay.Lanes.Count < currLane.Rank) {//next way has less lanes than a mobiles'current one
					
					int iIdx =new Random(1).Next(nextWay.Lanes.Count) - 1;
					iIdx = iIdx<0?0:iIdx;
					this.ToLane = nextWay.Lanes[iIdx];
					
				} else {   //otherwise Ŀ�공���������ڱ�������
					this.ToLane = nextWay.Lanes[currLane.Rank - 1];
				}
				
				break;
				
			case 1://turn right
				//the outside lane is rightful for a mobile obeying traffic  regulations
				this.ToLane = nextWay.Lanes[nextWay.Lanes.Count -1];
				break;
				
			case -1://turn left
				int iIndex = new Random(1).Next(nextWay.Lanes.Count) - 1;
				this.ToLane = nextWay.Lanes[iIndex];
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