using SubSys_MathUtility;
using System;
using System.Drawing;
using SubSys_SimDriving.SysSimContext;

using System.Collections;
using System.Collections.Generic;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// 程序的GUI可能要求使用对象的坐标来查询路段顶点的位置，采用对象的position用作
	/// </summary>
	/// <typeparam name="T">int</typeparam>
	[System.Obsolete("过时的，旧版本使用")]
	public partial class Cell:TrafficEntity
	{
		private Cell() { }
		public Cell(Car cm)
		{
			this.EntityType = EntityType.Cell;
			this.Grid = new Point(0,0);
			this.Car = cm;
		}
		/// <summary>
		/// 用作记录态哈希表记录车辆的时间信息，以及用来确定什么时候进入路段
		/// </summary>
		internal int iTimeStep;
		
		/// <summary>
		/// 使用链式结构支持快速下游访问
		/// </summary>
		internal Cell nextCell;
		/// <summary>
		/// 代表的车辆模型
		/// </summary>
		public Car Car;

		/// <summary>
		/// 相对于道路起点的偏移，记录车辆的空间信息，运行态哈希表和记录态哈希表
		/// </summary>
		//public int iPos;

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
		/// 实体的元胞空间坐标，过时的，建议用spacegrid代替
		/// </summary>
		public override Point Grid
		{
			get
			{
				return this.Track.pCurrPos;
			}
			set
			{
				this.Track.pCurrPos = value;
			}
		}

		/// <summary>
		/// 只应当在转换的时候调用一次，寻找轨迹的一个东西，从起始位置出发，前进iAheadSpace个间距时距
		/// </summary>
		/// <param name="iAheadSpace"></param>
		internal void CalcTrack(int iAheadSpace)
		{
			Lane rl = this.Container as Lane;
			if (rl == null)
			{
				throw new ArgumentNullException("对不在路段上的元胞调用此次方法是错误的");
			}
			Track mt = this.Track;
			mt.fromLane = rl;
			//中心坐标系的车道的交叉口入口的第一个点
			mt.pFromPos = new Point(rl.Rank - 1, -SimSettings.iMaxLanes + 1);
			
			//获取转向信息
			Way re = rl.Container as Way;
			int iTurn = this.Car.EdgeRoute.GetSwerve(re);

			Way reNext = this.Car.EdgeRoute.FindNext(re);
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
				mt.pToPos = new Point(SimSettings.iMaxLanes,iLaneIndex);
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
		
		/// <summary>
		/// 该函数是仿真运行的核心函数
		/// </summary>
		/// <param name="rN"></param>
		internal void Drive(TrafficEntity rN)
		{
			this.Car.DriveStg.Drive(rN,this);
		}

		/// <summary>
		/// 创建浅表副本
		/// </summary>
		internal Cell Copy()
		{
			Car cm = this.Car.Copy();
			Cell ce = this.MemberwiseClone() as Cell;
			ce.Car = cm;
			return ce;
		}
		[System.Obsolete("坐标系统的问题，postion不赋值 roadhash不复制,iTimeStep有问题")]
		internal CarInfo GetCarInfo()
		{
			CarInfo ci = new CarInfo();//结构，值类型
			ci.iSpeed = this.Car.iSpeed;
			ci.iAcc = this.Car.iAcc;
			ci.iCarHashCode = this.Car.GetHashCode();
			ci.iCarNum = this.Car.ID;
		
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

		internal void Move(int iHeadWay)
		{
			this.Grid=new Point(this.Grid.X,this.Grid.Y+iHeadWay);
		}

		/// <summary>
		/// 计算当前元胞可以前进的车头时距
		/// </summary>
		/// <param name="iEntityGap"></param>
		/// <param name="iToEntityGap"></param>
		public void GetEntityGap(out int iEntityGap,out int iToEntityGap)
		{
			int EntityGap=0;
			int ToEntityGap=0;
			//车道，然后是路段
			switch (this.Container.EntityType)
			{
				case EntityType.Lane:
					Way re = this.Container.Container as Way;
					//车道内部的计算方法
					if (this.nextCell == null)//第一个元胞的车头时距是路段+交叉口的
					{
						EntityGap = this.Container.iLength - this.Grid.Y;
						this.CalcTrack(1);//先填充轨迹
						GetTrackGap(re.xNodeTo, this.Track.pTempPos,out ToEntityGap);//再计算剩余轨迹
					}else//后续都是路段上的，没有交叉口上的
					{
						EntityGap = this.nextCell.Grid.Y - this.Grid.Y;
						ToEntityGap = 0;
					}
					break;

				case EntityType.XNode:

					XNode rN = this.Container as XNode;
					//计算剩余轨迹数量
					if (GetTrackGap(rN, this.Track.pCurrPos, out EntityGap) ==false)
					{
						ToEntityGap = 0;
					}
					else
					{
						if (this.Track.toLane!=null)
						{
							ToEntityGap = this.Track.toLane.iLastPos;
						}
					}
					
					break;
			}
			
			iEntityGap=EntityGap;
			iToEntityGap = ToEntityGap;
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
	
	
	/// <summary>
	/// 2015年1月9日，重新修改基础原理，增加空间属性，该类型有太多车辆的行为，将由不同的Mobile代替。实现仿真软件对大车、中型车、小型车的仿真支持
	/// </summary>
	public partial class Cell
	{
		private int _ispaceIndex=0;
		/// <summary>
		/// 相对于车道元胞空间第一个点的位置
		/// </summary>
		public int iSpaceIndex {
			get{return this._ispaceIndex;}
			set{this._ispaceIndex= value;}
		}
		
		/// <summary>
		/// 元胞坐标系的三维坐标
		/// </summary>
		public override OxyzPoint SpatialGrid
		{
			get
			{
				return base.SpatialGrid;
			}
			set
			{
				base.SpatialGrid = value;
			}
		}

	}
	
	
	public struct CellGrid
	{
		public OxyzPoint op ;
		/// <summary>
		/// 相对于车道第一个点的元胞个数.1为第一个点
		/// </summary>
		public int iGridIndex;
		
		public bool bIsEmpty;//=false;
		public CellGrid(OxyzPoint op,bool isEmpty)
		{
			this.op = op;
			this.iGridIndex = 0;
			this.bIsEmpty = isEmpty;
		}
		public int GetHashCode()
		{
			int ihashCode =this.op._X.GetHashCode()+this.op._Y.GetHashCode()+this.op._Z.GetHashCode();
			return ihashCode;
		}
		
	}
	/// <summary>
	/// 装饰者模型，重新定义元胞空间，每个车道（道路）、交叉口（XNode）都有这个东西
	/// 元胞空间要解决cell前后的空位置（车头时距、车后时距）计算问题
	/// </summary>
	public  class CellSpace//:Dictionary<int,CellGird>,LinkedList<Cell>
	{
		internal ITrafficEntity _Container;
		
		internal CellSpace(ITrafficEntity container)
		{
			this._Container = container;
		}
		/// <summary>
		/// 禁止调用无参数构造函数
		/// </summary>
		private CellSpace(){}
		
		/// <summary>
		///保存cellspacegrid的哈希表， key 用cell的xyz欧式坐标重写生成哈希值就是cell 坐标的xy，这样判断cell是否在元胞空间中就很简单,用dictionary.containskey。哈希表是为了快速判断元胞空间的状态，与cellspacequeue配合空间换时间。
		/// </summary>
		private Dictionary<int, CellGrid> _dicCellGrids =  new  Dictionary<int, CellGrid>();
		
		/// <summary>
		/// 利用cellgrid的iGridindex，对字典中的保存的Grid,利用GirdIndex进行排序.GUI画图时候可以只调用一次。可用于GUI画图
		/// </summary>
		private SortedList _slCellGrids = new SortedList();
		
		/// <summary>
		/// 元胞网格cellgrid 的个数，这也是CellSpace可以容纳的最多的Cell数量。
		/// </summary>
		public int Count
		{
			get{
				return this._dicCellGrids.Count;
			}
		}
		
		
		/// <summary>
		/// 添加元胞网格，元胞网格是组成车道的连续元胞点，字典的key是cellgrid的三坐标生成的。
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public   void Add(int key, CellGrid value)
		{
			if (this._dicCellGrids.ContainsKey(key)) {
				ThrowHelper.ThrowArgumentException("已经增加了该CellGrid");
			}
			this._dicCellGrids.Add(key,value);
		}
		public   bool Remove(int key)
		{
			var bIsRemoved = this._dicCellGrids.Remove(key);
			return bIsRemoved;
		}
		
		
		public bool TryGetValue(int key,out CellGrid cg)
		{
			return this._dicCellGrids.TryGetValue(key,out cg);
			
		}	
		
		}

	
	
}