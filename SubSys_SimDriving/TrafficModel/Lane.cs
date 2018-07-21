using System;
using SubSys_MathUtility;
using SubSys_SimDriving;
using System.Collections;
using System.Collections.Generic;
using SubSys_SimDriving;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// 交通实体里面的车道,原来的lane数据结构
	/// </summary>
	public partial class Lane : StaticOBJ, IComparable<Lane>, IComparer<Lane>
	{
		/// <summary>
		/// 全局的车道计数器，用来初始化车道ID
		/// </summary>
		private static int iCount=0;
        
		public override int Length
		{
			get
			{
				return this.Shape.Count;
			}
		}
		public override int Width
		{
			get
			{
				return SimSettings.iCarWidth;
			}
		}

		/// <summary>
		/// 与用户界面绘图有关的一系列点空间组成。
		/// 车道开始的地点，是交叉口的前方，第一个点，其索引最小，车道结束的地点，是交叉口的后方，一个车道的最末一个点的索引最大
		/// 索引的大小与车道的方向相反。
		/// </summary>
		public override EntityShape Shape
		{
			get
			{
				return  base.Shape;

				//if (eShape.Count == 0)//shape 没有初始化
				//{
				//	CreateShape(ref eShape);
				//}
				//return eShape;
			}
		}

		/// <summary>
		/// 用图形界面坐标系，转化为元胞坐标系,create a shape of a lane
		/// </summary>
		/// <param name="eShape"></param>
		internal  void CreateShape()
		{
			EntityShape prant = this.Container.Shape;
			var pNorm = VectorTool.GetNormal(this.Container.ToVector());
            foreach (var point in prant)
            {
                int iScaler = this.Rank;
                var newPoint = Coordinates.Offset(point, OxyzPointF.Muilt(pNorm,iScaler));
                this.Shape.Add(newPoint);
          }
        }
        //private void CreateShape(ref EntityShape eShape)
        //{
        //    EntityShape esContainer = this.Container.Shape;

        //    var pNorm = VectorTools.GetNormal(this.Container.ToVector());

        //    //the first point
        //    var pFirst = esContainer.Start;
        //    //the end point
        //    var pFEnd = esContainer.End;

        //    double dDistance = Coordinates.Distance(pFirst, pFEnd);
        //    //int loopCount = Convert.ToInt32( dDistance);//距离为100，划分为100等分

        //    //each split  x is 1 int; each split y is 1 int
        //    int iLoop = Convert.ToInt32(dDistance);

        //    double xSplit = (pFEnd._X - pFirst._X) / dDistance;//自身有正负号
        //    double ySplit = (pFEnd._Y - pFirst._Y) / dDistance;//自身有正负号

        //    eShape.Add(pFirst);

        //    var opCurr = new OxyzPointF(0, 0, 0);
        //    //	var opPrev = new OxyzPointF(0,0,0);

        //    for (int i = 1; i < iLoop; i++)//x行 iLoop to make sure each simulate step move equal distance
        //    {
        //        double dX = xSplit * i;
        //        double dY = ySplit * i;
        //        //四舍五入 would miss accuracy
        //        //				int iX = Convert.ToInt32(Math.Round((decimal)dX,0,MidpointRounding.AwayFromZero));
        //        //				int iY = Convert.ToInt32(Math.Round((decimal)dY,0,MidpointRounding.AwayFromZero));
        //        opCurr = new OxyzPointF(pFirst._X + dX, pFirst._Y + dY);
        //    }

        //    eShape.Add(pFEnd);
        //}

        /// <summary>
        /// 没有调用，暂时不重写
        /// </summary>
        /// <param name="iAheadSpace"></param>
        /// <returns></returns>
        internal bool IsLaneBlocked(int iAheadSpace)
		{
			//return this.iLastPos-1 <= iAheadSpace ;
			return false;
		}
		/// <summary>
		/// 已过时，车道的最后一个元胞的位置，应当是Y坐标
		/// </summary>
//		private int _ilastPos;
//		/// <summary>
//		/// 已过时，获取车道的最后一个元胞的位置，如果没有元胞则返回车道长度
//		/// </summary>
//		[System.Obsolete("已过时")]
//		internal int iLastPos
//		{
//			get
//			{
//				Cell ce = this._cells.PeekLast();
//				if (ce== null)
//				{
//					this._ilastPos = this.iLength;
//				}
//				else //if (this._ilastPos > ce.RltPos.Y)
//				{
//					this._ilastPos = ce.Grid.Y;
//				}
//				return _ilastPos;
//
//			}
//		}

		/// <summary>
		///车道的排序，从内侧车道开始的第几个车道,用来对车道进行排序，第一个车道的Rank 是1
		/// </summary>
		public int Rank;

//		[System.Obsolete("已过时，记录车道车队相对位置的数组，新的元胞空间理论建立后停止使用该属性")]
//		public int[] PrevCarPos;

		/// <summary>
		/// 分车道的信号
		/// </summary>
		internal SignalLight SignalLight;


		
		/// <summary>
		/// 信号灯运行函数,modified on date 2016/1/27
		/// </summary>
		/// <param name="iCrtTimeStep">红灯时长</param>
		internal void PlaySignal(int iCrtTimeStep)
		{
			
			if (SignalLight == null)//无信号交叉口
			{
				this.bBlocked =false;
				return;
			}
			if (this.SignalLight.IsGreen(iCrtTimeStep) == false)
			{//红灯或者是黄灯则阻塞
				this.bBlocked =true;
			}
			else//绿灯
			{
				this.bBlocked =false;
			}
		}

		/// <summary>
		/// math coordinates
		/// </summary>
		/// <returns></returns>
        [System.Obsolete("to avoid warning")]
		public override OxyzPointF ToVector()
		{
			return this.Shape.End-this.Shape.Start;
		}
		
		#region 构造函数
		
		//[System.Obsolete("禁止使用的构造形")]
		private Lane()
		{
			this.laneType = LaneType.Straight;
		}

		
		internal Lane(LaneType lt):this(null,lt)
		{
		}
		/// <summary>
		/// 没有进行内部注册，应当由其管理者调用registere进行注册
		/// </summary>
		/// <param name="re"></param>
		/// <param name="lt"></param>
		internal Lane(Way re,LaneType lt)
		{
//			this.PrevCarPos = new int[512];
//			this.PrevCarPos[0] = -1;

			Container = re;
            this.EntityType = EntityType.Lane;

			this.laneType = lt;
			this._entityID = ++Lane.iCount;
			//id property is setted by base.
			//this.ID


		}
		#endregion
		
		/// <summary>
		/// 过时的，这个函数需要修改
		/// </summary>
		/// <param name="ce"></param>
//		[System.Obsolete("过时的，这个函数需要修改")]
//		public void AddCell(Cell ce)
//		{
//			//给容器赋值；
//			ce.Container = this;
//			//修改坐标
//			ce.Grid = new Point(this.Rank, ce.Grid.Y);
//
//			System.Diagnostics.Debug.Assert(ce.Grid.Y < this.iLastPos);
//
//			this._cells.Enqueue(ce);
//		}
		
//		[System.Obsolete("过时的，这个函数需要修改")]
//		public Cell RemoveCell()
//		{
//			return this._cells.Dequeue();
//		}
		internal LaneType laneType;
		
		//	[System.Obsolete("过时的，这个变量不在需要")]
		//	private CellQueue _cells = new CellQueue();

		#region 进入车道的情况，进一步将由cellspace替代
		/// <summary>
		/// 等待进入该车道的等待队列，过时，用mobileInn代替
		/// </summary>
		//	private Queue<Cell> _waitedQueue = new Queue<Cell>();

		/// <summary>
		///过时，
		/// </summary>
		/// <param name="ce"></param>
//		[System.Obsolete("过时的，新版由父类的MobilesInn代替")]
//		public void EnterWaitedQueue(Cell ce)
//		{
//			//给容器赋值；
//			ce.Container = this;
//			this._waitedQueue.Enqueue(ce);
//
//		}
		
		/// <summary>
		/// 将等待队列中的元胞添加到车道元胞中，新版由cellspace类实现该功能
		/// </summary>
//		[System.Obsolete("过时的，新版由父类的MobilesInn代替")]
//		private void DisposeWaitedQueue()
//		{
//			while (this._waitedQueue.Count > 0)
//			{
//				if (this.iLastPos == 0)//如果车道已经满了就不能处理队列了
//				{
//					break;
//				}
//				this.AddCell(this._waitedQueue.Dequeue());
//			}
//		}
		#endregion
		

		/// <summary>
		/// 没有调用visitor模式，调用所有附加的服务、处理等待队列
		/// </summary>
		//[System.Obsolete("调用所有附加的服务和处理 等待队列")]
		public override void UpdateStatus()
		{
			base.UpdateStatus();
		}
		
		protected override void OnStatusChanged()
		{
			//this.DisposeWaitedQueue();//处理等待的元胞

			this.ServeMobiles();
			//调用基类的日志服务
			this.InvokeServices(this);//利用日志服务记录roadLane变量

		}


		#region 两个类相互比较的算法和函数
		public int CompareTo(Lane other)
		{
			if (this.laneType == other.laneType)
			{
				return 0;
			}
			return this.laneType > other.laneType ? 1 : -1;
		}
		public int Compare(Lane x, Lane y)
		{
			return x.CompareTo(y);
		}
		/// <summary>
		/// 静态方法
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static int CompareTo(Lane from, Lane to)
		{
			return from.CompareTo(to);
		}
		#endregion
		
		/// <summary>
		/// this index operator need to be modified
		/// </summary>
//		[System.Obsolete("过时的，新版抛弃Cell")]
//		public MobileEntity this[int index]
//		{
//			get
//			{
//				return null;// this._cells[index];
//			}
//		}
		
//		/// <summary>
//		/// obselete
//		/// </summary>
//		[System.Obsolete("obsolete,replace with mobiles")]
//		public int CellCount
//		{
//			get
//			{
//				return this._cells.Count;
//			}
//		}

//		public IEnumerator<Cell> GetEnumerator()
//		{
//			return this._cells.GetEnumerator();
//		}
	}


	//-----------------2015年1月19日-----------------------------------------
	/// <summary>
	/// 2015年1月19日，对原来的lane进行升级。并保持和原有部分代码的兼容
	/// </summary>
	public partial class Lane
	{
//		/// <summary>
//		/// cellspace 类型的元胞网格空间,，包含该lane的所有元素
//		/// </summary>
//		private CellSpace _Grids;

		private LinkedList<MobileOBJ> _mobiles ;
		/// <summary>
		/// 用来保存存储在交叉口和车道等内部的车辆。
		/// </summary>
		public LinkedList<MobileOBJ> Mobiles {
			get {
				if (this._mobiles==null) {
					this._mobiles = new LinkedList<MobileOBJ>();
				}
				return this._mobiles;
			}
		}
		
		/// <summary>
		/// 将等待队列中的元胞添加到车道元胞中
		/// </summary>
		internal override void ServeMobiles()
		{
			//as long as theres space for mobile to enter ,serve this mobile
			while(this.MobilesInn.Count>0)
			{
				var mobile = this.MobilesInn.Peek();
				//if (this.LaneSpace>mobile.iLength) {
				this.Mobiles.AddLast(this.MobilesInn.Dequeue());
				//}
			}
		}

		
		/// <summary>
		/// 判断从道路起点处到有车占据的地方的元胞网格个数
		/// </summary>
		[System.Obsolete("还不完善")]
		public int LaneSpace
		{
			get
			{
				//如果车道内没有元胞
				if (this.Mobiles.Count==0)
				{//返回车道网格的数量
					return this.Length;//Shape.Count;
				}
				//实体形状的最后一个点,考虑实体有长度
				MobileOBJ me = this.Mobiles.Last.Value;
				//实体形状的最后一个点,考虑实体有
				return this.Shape.GetIndex(me.Shape.End);
			}
		}
		
		
		/// <summary>
		///The Left lane of current lane
		/// </summary>
		public Lane Left
		{
			get {
				
				Way way = this.Container as Way;
				
				//the first lane in way is current lane,it has no left lane
				if (this.Rank==1) {
					return null;
				}
				//不是第一条车道。就是第二条以上的车道。车道编号Rank 要小一号才是左侧车道
				return way.Lanes[this.Rank-2];
			}
		}
		
		/// <summary>
		/// the right lane of current lane
		/// </summary>
		public Lane Right
		{
			get {
				
				Way way = this.Container as Way;
				
				//the last lane in way is current lane,it has no right lane
				if (this.Rank==way.Lanes.Count) {
					return null;
				}
				//不是第一条车道。就是第二条以上的车道。车道编号Rank 要小一号才是左侧车道
				return way.Lanes[this.Rank];
			}
		}
		
		private bool bBlocked=false;

		public  bool IsBlocked
		{
			get{
				return this.bBlocked;
			}
		}
		
		

		
	}

	//_______________2016年1月新增的内容，原有的成员和方法将被部分废弃
//	public class MobilesShelter
//	{
//		internal StaticEntity _Container;
//
//		internal MobilesShelter(StaticEntity container)
//		{
//			this._Container = container;
//		}
//		/// <summary>
//		/// 禁止调用无参数构造函数
//		/// </summary>
//		private MobilesShelter()
//		{
//		}
//
//
//		private LinkedList<MobileEntity> _mobiles = new LinkedList<MobileEntity>();
//
//		/// <summary>
//		/// 第一辆的索引是第一个
//		/// </summary>
//		public LinkedList<MobileEntity> Mobiles
//		{
//			get{return this._mobiles;}
//		}
//
	////		protected void Enter(MobileEntity me)
	////		{
//		////			int iShapeCount =this._mobiles.Count;
//		////
//		////
//		////			//如果队列里没有车辆，允许新增进入。
//		////			//如果车道元胞空间的末尾有空余车位，且空余空间大于车辆长度，则允许车辆进入
//		////			if (this._cellSpace.iSpace>me.Shape.Count) {
//		////
//		////				//下面要做两个事情1、修改元胞空间的状态，该元宝空间被占据了。2、修改车辆形状的坐标。
//		////				this._mobiles.AddLast(me);
//		////
//		////				//修改车辆的元胞坐标系
//		////				for (int i = 0; i < me.Shape.Count; i++) {
//		////		//			me.Shape[i]=this._Container.Shape[i];//container 是lane
//		////		//			CellGrid cg = new CellGrid(me.Shape[i],true);
//		////					//修改元胞空间的坐标
//		////					this._cellSpace.Add(cg);
//		////				}
	////		//	}
	////
	////			//把车辆加入
	////		//	this._mobiles.AddLast(me);
	////
	////		}
	////		/// <summary>
	////		/// 车辆退出车道。换车道这种东西，需要插入和更新。
	////		/// </summary>
	////		/// <param name="me"></param>
	////		protected void Exit(MobileEntity me)
	////		{
	////			this._mobiles.RemoveFirst();
	////
	////			for (int i = 0; i < me.Shape.Count; i++)
	////			{
	////				//this._cellSpace.Remove(me.Shape[i].GetHashCode());
	////			}
	////		}
//
//		protected bool Move(int iStepForward)
//		{
//			return false;
//		}
//
//		protected bool IsEmpty()
//		{
//			return false;
//		}
//
//
//
//
//
//	}
}

