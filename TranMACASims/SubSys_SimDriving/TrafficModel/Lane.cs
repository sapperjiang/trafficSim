using System;
using SubSys_MathUtility;
using SubSys_SimDriving;
using System.Collections.Generic;
using SubSys_SimDriving.SysSimContext;
using System.Drawing;

namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// 交通实体里面的车道,原来的lane数据结构
	/// </summary>
	public partial class Lane : StaticEntity, IComparable<Lane>, IComparer<Lane>
	{
		/// <summary>
		/// 全局的车道计数器，用来初始化车道ID
		/// </summary>
		private static int iLaneCount=0;
		///// <summary>
		///// 每次更新CurrTimeStep的时候就自动处理等待列表
		///// </summary>
		public override int iLength
		{
			get
			{
				return this.Container.iLength;
			}
		}
		public override int iWidth
		{
			get
			{
				return SimSettings.iCarWidth;
			}
		}

		public override EntityShape Shape
		{
			get
			{
				EntityShape eShape = base.Shape;

				if (eShape.Count == 0)//shape 没有初始化
				{
					CreateShape(eShape);
				}
				return eShape;
			}
		}

		/// <summary>
		/// 可以用新的元胞空间概念重写该函数
		/// </summary>
		/// <param name="eShape"></param>
		private  void CreateShape(EntityShape eShape)
		{

			EntityShape es = this.Container.Shape;

			OxyzPointF pNorm = VectorTools.GetNormalVector(this.Container.ToVector());
			OxyzPointF mpOffset = new OxyzPointF(pNorm._X*(this.Rank - 0.5f),pNorm._Y * (this.Rank - 0.5f));
			//平移坐标
			OxyzPointF pFirst = Coordinates.Offset(es[0], mpOffset);
			//计算终点
			OxyzPointF pFEnd = Coordinates.Offset(es[es.Count - 1], mpOffset);
			
			OxyzPointF mp = new OxyzPointF(pFEnd._X-pFirst._X,pFEnd._Y-pFirst._Y);
			int iLoopCount = this.iLength;//元胞长度，初始化参见registerservice

			float xSplit = mp._X / iLoopCount;//自身有正负号
			float ySplit = mp._Y / iLoopCount;//自身有正负号
			
			eShape.Add(pFirst);
			for (int i = 1; i < iLoopCount; i++)//x行
			{   //中点
				eShape.Add(new OxyzPointF(pFirst._X + (i-0.5f) * xSplit, pFirst._Y + (i-0.5f) * ySplit));
			}
			eShape.Add(pFEnd);
		}
		
		/// <summary>
		/// 没有调用，暂时不重写
		/// </summary>
		/// <param name="iAheadSpace"></param>
		/// <returns></returns>
		internal bool IsLaneBlocked(int iAheadSpace)
		{
			return this.iLastPos-1 <= iAheadSpace ;
		}
		/// <summary>
		/// 已过时，车道的最后一个元胞的位置，应当是Y坐标
		/// </summary>
		private int _ilastPos;
		/// <summary>
		/// 已过时，获取车道的最后一个元胞的位置，如果没有元胞则返回车道长度
		/// </summary>
		[System.Obsolete("已过时")]
		internal int iLastPos
		{
			get
			{
				Cell ce = this._cells.PeekLast();
				if (ce== null)
				{
					this._ilastPos = this.iLength;
				}
				else //if (this._ilastPos > ce.RltPos.Y)
				{
					this._ilastPos = ce.Grid.Y;
				}
				return _ilastPos;
				
			}
		}

		
	
	
		/// <summary>
		///车道的排序，从内侧车道开始的第几个车道,用来对车道进行排序
		/// </summary>
		public int Rank;

		[System.Obsolete("已过时，记录车道车队相对位置的数组，新的元胞空间理论建立后停止使用该属性")]
		public int[] PrevCarPos;

		/// <summary>
		/// 分车道的信号
		/// </summary>
		internal SignalLight SignalLight;


		
		/// <summary>
		/// 信号灯运行函数
		/// </summary>
		/// <param name="iCrtTimeStep"></param>
		internal void PlaySignal(int iCrtTimeStep)
		{
			XNode rN = ( this.Container as Way).xNodeTo;
			if (SignalLight == null)//无信号交叉口
			{
				rN.UnblockLane(this);
				return;
			}
			if (this.SignalLight.IsGreen(iCrtTimeStep) == false)
			{//红灯或者是黄灯则阻塞
				rN.BlockLane(this);
			}
			else//绿灯
			{
				rN.UnblockLane(this);
			}
		}

		public override OxyzPointF ToVector()
		{
			OxyzPoint pA = this.Shape[0];
			OxyzPoint pB = this.Shape[this.Shape.Count - 1];
			return new OxyzPointF(pB._X-pA._X,pB._Y-pA._Y);
		}
		
		#region 构造函数
		
		[System.Obsolete("禁止使用的构造形")]
		private Lane()
		{
			this.laneType = LaneType.Straight;
		}

		/// <summary>
		/// 调用了两参数构造形
		/// </summary>
		/// <param name="re"></param>
		internal Lane(Way re):this(re,LaneType.Straight){
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
			this.PrevCarPos = new int[512];
			this.PrevCarPos[0] = -1;

			Container = re;
			this.laneType = lt;
			this._id = Lane.iLaneCount++;
			
			//初始化cellspace
			this._Grids = new CellSpace(this);

		}
		#endregion
		
		/// <summary>
		/// 过时的，这个函数需要修改
		/// </summary>
		/// <param name="ce"></param>
		public void AddCell(Cell ce)
		{
			//给容器赋值；
			ce.Container = this;
			//修改坐标
			ce.Grid = new Point(this.Rank, ce.Grid.Y);

			System.Diagnostics.Debug.Assert(ce.Grid.Y < this.iLastPos);

			this._cells.Enqueue(ce);
		}
		public Cell RemoveCell()
		{
			return this._cells.Dequeue();
		}
		internal LaneType laneType;
		
		private CellQueue _cells = new CellQueue();

		#region 进入车道的情况，进一步将由cellspace替代
		/// <summary>
		/// 等待进入该车道的等待队列
		/// </summary>
		private Queue<Cell> _waitedQueue = new Queue<Cell>();

		/// <summary>
		/// 注册容器，新版将由cellspace替代
		/// </summary>
		/// <param name="ce"></param>
		public void EnterWaitedQueue(Cell ce)
		{
			//给容器赋值；
			ce.Container = this;
			this._waitedQueue.Enqueue(ce);
		}
		
		/// <summary>
		/// 将等待队列中的元胞添加到车道元胞中，新版由cellspace类实现该功能
		/// </summary>
		private void DisposeWaitedQueue()
		{
			while (this._waitedQueue.Count > 0)
			{
				if (this.iLastPos == 0)//如果车道已经满了就不能处理队列了
				{
					break;
				}
				this.AddCell(this._waitedQueue.Dequeue());
			}
		}
		#endregion
		

		/// <summary>
		/// 没有调用visitor模式，调用所有附加的服务、处理等待队列
		/// </summary>
		[System.Obsolete("调用所有附加的服务和处理 等待队列")]
		public override void UpdateStatus()
		{
			base.UpdateStatus();
		}
		protected override void OnStatusChanged()
		{
			this.DisposeWaitedQueue();//处理等待的元胞

			//调用基类的日志服务
			this.InvokeService(this);//利用日志记录roadLane变量
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
		public Cell this[int index]
		{
			get
			{
				return this._cells[index];
			}
		}
		public int CellCount
		{
			get
			{
				return this._cells.Count;
			}
		}

		public IEnumerator<Cell> GetEnumerator()
		{
			return this._cells.GetEnumerator();
		}
	}
	
	
//-----------------2015年1月19日-----------------------------------------	
	/// <summary>
	/// 2015年1月19日，对原来的lane进行升级。并保持和原有部分代码的兼容
	/// </summary>
	public partial class Lane
	{
		/// <summary>
		/// cellspace 类型的元胞网格空间,，包含该lane的所有元素
		/// </summary>
		private CellSpace _Grids;
		
		/// <summary>
		/// 将等待队列中的元胞添加到车道元胞中，新版由cellspace类实现该功能
		/// </summary>
		internal override void ServeMobiles()
		{
//			while (this._mobilesInn.Count > 0)
//			{
//				if (this.SpaceCount == 0)//如果车道已经满了就不能处理队列了
//				{
//					break;
//				}else{
//					this.AddCell(this._mobilesInn.Dequeue());
//				}
//			}
		}

		
		/// <summary>
		/// 判断从道路起点处到有车的地方的元胞网格个数
		/// </summary>
		[System.Obsolete("还不完善")]
		public int SpaceCount
		{
			get
			{
				//如果车道内没有元胞
				if (this.Mobiles.Count==0)
				{//返回车道网格的数量
					return this._Grids.Count;
				}
				//实体形状的最后一个点,考虑实体有长度
				MobileEntity me = this.Mobiles.Last.Value;
				//实体形状的最后一个点,考虑实体有长度
				OxyzPoint op =me.Shape.End;

				CellGrid cg = new CellGrid();
			
				if ( this._Grids.TryGetValue(op.GetHashCode(),out cg)==true) {
					return	cg.iGridIndex;
				}
				return -1;
			}
		}
		
	}
}

