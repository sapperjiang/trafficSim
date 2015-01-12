using SubSys_SimDriving;
using System.Drawing;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.RoutePlan;
using SubSys_MathUtility;
using System;

namespace SubSys_SimDriving
{
	public abstract partial class MobileEntity : TrafficEntity
	{
		/// <summary>
		/// 当前车辆的速度
		/// </summary>
		internal int iSpeed;
		
	}
	
	//--------------------2015年1月11日重新增加的内容-------------------
	/// <summary>
	/// 所有会动的物体的基类形
	/// </summary>
	public abstract partial class MobileEntity : TrafficEntity
	{
		private static int MobileID = 0;
		private bool IsCopyed = false;
		
		public DriveStrategy Strategy;
		
		//  public Shape 形状已经有了，就是继承TrafficEntity的shape
		
		/// <summary>
		/// 对象拷贝和值拷贝
		/// </summary>
		/// <returns></returns>
		public virtual MobileEntity Clone()
		{
			MobileEntity cm = this.MemberwiseClone() as MobileEntity;
			cm.IsCopyed = true;
			//this.EntityType = EntityType.Mobile;
			return cm;
		}
		
		public Color Color;
		
		public EdgeRoute EdgeRoute;
		public NodeRoute NodeRoute;

		internal DriveStrategy Driver = new DefaultDriveAgent();

		~MobileEntity()
		{
			if (this.IsCopyed != true)
			{
				base.UnRegiser();
			}
		}
		//internal SpeedLevel CurrSpeed;
		/// <summary>
		/// 当前车辆的加速度
		/// </summary>
		internal int iAcc = 1;
	
		
		
		#region 属性部分
		
		public override int iLength
		{
			get
			{//车辆的长度就是元胞的长度，车辆的形状，就是几个元胞的形状
				return this.Shape.Count;
			}
		}
		
		#endregion
		
		
		
		protected MobileEntity() { }
		
		[System.Obsolete("过时,因为car类已经过时")]
		public MobileEntity(Car cm)
		{
			//从trafficmodel 继承的保护字段
			this._id = ++MobileEntity.MobileID;
			
			this.EntityType = EntityType.Mobile;
			this.Color = Color.Green;
			this.iSpeed = 0;
			base.Register();
			this.EdgeRoute = new EdgeRoute();
			this.NodeRoute = new NodeRoute();
		}
		/// <summary>
		/// 用作记录态哈希表记录车辆的时间信息，以及用来确定什么时候进入路段
		/// </summary>
		internal int iTimeStep;
		

		/// <summary>
		/// 包围在交叉口内部的节点,内部点使用绝对坐标系
		/// </summary>
		public Track Track = new Track();
		/// <summary>
		/// 使用在交叉口上
		/// </summary>
		/// <param name="iStep"></param>
		internal void TrackMove(int iStep)
		{
			Point p = this.Grid;
			while (iStep-- > 0)
			{
				p = this.Track.NextPoint(p);
			}
			this.Grid = p;
		}

		/// <summary>
		/// 只应当在转换的时候调用一次，寻找轨迹的一个东西，从起始位置出发，前进iAheadSpace个间距时距
		/// </summary>
		/// <param name="iAheadSpace"></param>
		internal virtual void CalcTrack(int iAheadSpace)
		{
			Lane rl = this.Container as Lane;
			if (rl == null)
			{
				ThrowHelper.ThrowArgumentNullException("对不在路段上的元胞调用此次方法是错误的");
			}
			Track mt = this.Track;
			mt.fromLane = rl;
			//中心坐标系的车道的交叉口入口的第一个点
			mt.pFromPos = new Point(rl.Rank - 1, -SimSettings.iMaxLanes + 1);
			
			//获取转向信息
			Way re = rl.Container as Way;
			int iTurn = this.EdgeRoute.GetSwerve(re);

			Way reNext = this.EdgeRoute.FindNext(re);
			if (iTurn == 0)//直接使用中心坐标系
			{
				//车道直向对应
				mt.pToPos = new Point(rl.Rank-1,SimSettings.iMaxLanes);
				if (reNext == null)//没有目标车道，已经到头了
				{
					mt.toLane = null;
				}
				else
				{
					if (reNext.Lanes.Count < rl.Rank)//防止车道不匹配
					{   //目标车道数小于本车道数,进入目标车道的内侧车道
						mt.toLane = reNext.Lanes[0];
					}
					else
					{   //目标车道数大于于本车道数
						mt.toLane = reNext.Lanes[rl.Rank-1];
					}
				}
			}
			if (iTurn == 1)//右转
			{
				//生成1到reNext 车道数量的随机数，即随机选择车道
				int iLaneIndex = - reNext.Lanes.Count + 1;
				mt.pToPos = new Point(  SimSettings.iMaxLanes,iLaneIndex);
				mt.toLane = reNext.Lanes[-iLaneIndex];
			}
			if (iTurn == -1)//左转
			{
				//-4位置的坐标应当为-3 ,后面的也是随机选择车道
				int iLaneIndex = new Random(1).Next(reNext.Lanes.Count)- 1;
				mt.pToPos = new Point(-SimSettings.iMaxLanes + 1,iLaneIndex);
				mt.toLane = reNext.Lanes[iLaneIndex];
			}
			if (iTurn ==2 )
			{
				int iLaneIndex = reNext.Lanes.Count- 1;
				mt.pToPos = new Point(-iLaneIndex,-SimSettings.iMaxLanes+1);
				mt.toLane = reNext.Lanes[iLaneIndex];
			}
			
			//三个点全部转化转换为原生坐标
			mt.pFromPos = Coordinates.GetRealXY(mt.pFromPos, mt.fromLane.Container.ToVector());
			mt.pTempPos = new Point(rl.Rank-1,iAheadSpace-SimSettings.iMaxLanes);//创建点
			mt.pTempPos = Coordinates.GetRealXY(mt.pTempPos, mt.fromLane.ToVector());
			mt.pToPos = Coordinates.GetRealXY(mt.pToPos, mt.fromLane.Container.ToVector());
		}
		
		
		internal void Drive(StaticEntity DriveEnvirnment)
		{
//			this.Driver.dr(DriveEnvirnment);
			//这个方法要重写
//			this.DriveStg.Drive(rN,this);
		}

		[System.Obsolete("坐标系统的问题，postion不赋值 roadhash不复制,iTimeStep有问题")]
		internal CarInfo GetCarInfo()
		{
			CarInfo ci = new CarInfo();//结构，值类型
			ci.iSpeed = this.iSpeed;
			ci.iAcc = this.iAcc;
			ci.iCarHashCode = this.GetHashCode();
			ci.iCarNum = this.ID;
			
			ci.iTimeStep = ISimCtx.iCurrTimeStep;
			
			
			if (this.Container.EntityType == EntityType.Lane)
			{
				ci.iPos = this.Grid.Y+(int)this.Container.Shape[0]._X;
			}
			else if (this.Container.EntityType == EntityType.XNode)
			{
				ci.iPos = this.Container.Grid.X + this.Grid.X-1;
			}
			return ci;
		}

	
		/// <summary>
		/// 这个函数需要重新写，计算当前元胞可以前进的车头时距
		/// </summary>
		/// <param name="iEntityGap"></param>
		/// <param name="iToEntityGap"></param>
		public virtual void GetEntityGap(out int iEntityGap,out int iToEntityGap)
		{
			iEntityGap=0;
			iToEntityGap=0;
		}

		/// <summary>
		/// 计算元胞在交叉口内部可以走多少步
		/// </summary>
		/// <param name="rN"></param>
		/// <param name="ct"></param>
		/// <returns></returns>
		private bool GetTrackGap(XNode rN, Point pcc,out int Gap)
		{
			bool bReachEnd = false;
			int iCount = 0;
			Point p = this.Track.NextPoint(pcc);
			if (p.X == 0 && p.Y == 0)
			{
				bReachEnd = true;
			}
			while (rN.IsBlocked(p) == false)
			{
				p = this.Track.NextPoint(p);
				if (p.X == 0 && p.Y == 0)
				{
					bReachEnd =true;
					break;
				}
				iCount++;
			}
			Gap = iCount;
			return bReachEnd;
		}
		
		
	}
	
}

