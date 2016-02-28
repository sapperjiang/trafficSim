using System;
using System.Drawing;
using SubSys_MathUtility;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.Agents;
using SubSys_SimDriving.Service;

namespace SubSys_SimDriving
{
	/// <summary>
	/// 观察者模式中的subject被观察者
	/// </summary>
	public abstract partial class TrafficEntity : ITrafficEntity
	{
		public static int EntityCounter = 0;
		
		public int TypeID = 0;
		
		//IService _IregService = new RegisterService();
		//服务管理器
		ServiceMgr _serviceMgr = new ServiceMgr();

		public virtual void AddService(IService ils)
		{
			this._serviceMgr.Add(ils);
		}
		/// <summary>
		/// 调用服务的功能
		/// </summary>
		/// <param name="te"></param>
		public virtual void InvokeServices(ITrafficEntity entity)
		{
			foreach (IService service in this._serviceMgr)
			{
				service.Perform(entity);
			}
		}

		/// <summary>
		/// 删除交通实体要使用的服务
		/// </summary>
		/// <param name="ils"></param>
		public virtual void RemoveService(IService ils)
		{
			this._serviceMgr.Remove(ils);
		}
		/// <summary>
		/// 向simContext 报道类的创建行为,注册不应当声明为服务，服务每个循环周期都调用
		/// 注册这类应当在工厂方法里面删除。
		/// </summary>
//		internal virtual void Register()
//		{
//			_IregService.Perform(this);
//		}
//		internal virtual void UnRegiser()
//		{
//			_IregService.Revoke(this);
//		}

		#region ITrafficEntity 成员

		protected int _entityID;
		private int _iWidth;
		private int _iLength;
		private Color _color;
		
		
		public Color Color
		{
			get{
				return this._color;
			}
			set{
				this._color = value;
			}
		}
		
		
		//元胞坐标系
		private Point _pntGrid;
		
		private EntityType _entityType;

//		/// <summary>
//		/// 元胞坐标系,过时，建议使用spaceGrid
//		/// </summary>
//		[System.Obsolete("outdated use spacilgrid ")]
//		public virtual Point Grid
//		{
//			get { return _pntGrid; }
//			set { _pntGrid = value; }
//		}
//		
		
		/// <summary>
		/// 用元胞个数计算的实体的宽度，实际宽度等于iWidth*元胞代表的距离
		/// </summary>
		public virtual int Width
		{
			get { return _iWidth; }
			set { _iWidth = value; }
		}
		/// <summary>
		/// 用元胞个数计算的实体的长度。实际长度等于iLength*元胞代表的距离
		/// </summary>
		public virtual int Length
		{
			get { return _iLength; }
			set { _iLength = value; }
		}
		/// <summary>
		/// TrafficEntity所代表的类型,是交通灯,车辆.还是建筑
		/// </summary>
		public virtual EntityType EntityType
		{
			get { return _entityType; }
			set { _entityType = value; }
		}
		
		/// <summary>
		/// 对于RoadNode该坐标为以元胞长度为单位的相对坐标，该坐标为屏幕坐标
		/// 除以元胞的GUI长度
		/// 对于RoadLane和RoadEdge以及Cell，该坐标X为相对于起点RoadNode的元胞个数
		/// Y为相对于起点RoadNode的偏移（即第几个车道）
		/// </summary>
		
		public ISimContext ISimCtx
		{
			get { return SimContext.GetInstance(); }
		}

		public int ID
		{
			get
			{
				return this._entityID;
			}
		}

		#region 未来兼容GIS
		private OxyzPointF _position;
		/// <summary>
		/// 未来兼容GIS系统预留的GIS坐标系
		/// </summary>
		public OxyzPointF GISGrid
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
			}
		}

		#endregion
		
		private EntityShape _entityShape;// = new EntityShape();
		
		protected ITrafficEntity _container;

		/// <summary>
		/// 子类应当重写这个属性\用于GUI画图的属性
		/// </summary>
		public virtual EntityShape Shape
		{
			get
			{
				if (this._entityShape==null)
				{
					this._entityShape = new EntityShape();
				}
				return this._entityShape;
			}
		}
		
		public virtual ITrafficEntity Container
		{
			get
			{
				if (this._container != null)
				{
					return this._container;
				}
				throw new System.Exception("目标函数没有注册");
			}
			set
			{
				this._container = value;
			}
		}
		
		private string _strName;
		/// <summary>
		/// 交通实体的名称、道路名、交叉口名等
		/// </summary>
		public string Name
		{
			get
			{
				return this._strName;
			}
			set
			{
				this._strName = value;
			}
		}

		public virtual OxyzPointF ToVector()
		{
			throw new System.NotImplementedException();
		}

		#endregion


		#region 状态更新函数
		
		/// <summary>
		/// 存储边上定义的异步更新的规则
		/// </summary>
		internal AsynchronicAgents asynAgents = new AsynchronicAgents();

		/// <summary>
		/// 存储边上定义的同步更新的规则
		/// </summary>
		internal SynchronicAgents synAgents = new SynchronicAgents();

		/// <summary>
		/// 过时的，原有的调用函数、调用所有的访问者，进行内部元胞的更新
		/// </summary>
		[System.Obsolete("过时的，原有的调用函数、调用所有的访问者，进行内部元胞的更新")]
		public virtual void UpdateStatus()
		{
			this.OnStatusChanged();
		}
		protected virtual void OnStatusChanged()
		{
			throw new NotImplementedException("调用了基类的状态改变函数是不对的");
		}
		
		

		/// <summary>
		///RoadEdge是item ，Agent是visitor 相当于item.accept(visitor)
		/// </summary>
		/// <param name="ur"></param>
		public void AcceptSynAgent(Agents.AbstractAgent ur)
		{
			if (ur != null)
			{
				//添加到仿真上下文
				this.ISimCtx.Agents.Add(ur.GetHashCode(), ur);
				this.synAgents.Add(ur);
			}
			else
			{
				throw new ArgumentNullException("空的更新规则");
			}
		}
		/// <summary>
		/// 添加异步更新规则
		/// </summary>
		/// <param name="ur"></param>
		public void AcceptAsynAgent(Agents.AbstractAgent ur)
		{
			if (ur != null)
			{
				//添加到仿真上下文
				this.ISimCtx.Agents.Add(ur.GetHashCode(), ur);
				this.asynAgents.Add(ur);
			}
			else
			{
				throw new ArgumentNullException("空的更新规则");
			}
		}
		#endregion
		
	}
	
	
	/// <summary>
	/// 2015年1月19日更新，新增加的内容。
	/// </summary>
	public abstract partial class TrafficEntity:ITrafficEntity
	{
		//3d元胞空间
		private OxyzPointF _oxyzGrid;
		/// <summary>
		/// 3d元胞空间，为了扩展GIS做准备
		/// </summary>
		public virtual  OxyzPointF SpatialGrid
		{
			get { return this._oxyzGrid; }
			set { _oxyzGrid = value; }
		}

	}
}

