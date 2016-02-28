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
	/// �۲���ģʽ�е�subject���۲���
	/// </summary>
	public abstract partial class TrafficEntity : ITrafficEntity
	{
		public static int EntityCounter = 0;
		
		public int TypeID = 0;
		
		//IService _IregService = new RegisterService();
		//���������
		ServiceMgr _serviceMgr = new ServiceMgr();

		public virtual void AddService(IService ils)
		{
			this._serviceMgr.Add(ils);
		}
		/// <summary>
		/// ���÷���Ĺ���
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
		/// ɾ����ͨʵ��Ҫʹ�õķ���
		/// </summary>
		/// <param name="ils"></param>
		public virtual void RemoveService(IService ils)
		{
			this._serviceMgr.Remove(ils);
		}
		/// <summary>
		/// ��simContext ������Ĵ�����Ϊ,ע�᲻Ӧ������Ϊ���񣬷���ÿ��ѭ�����ڶ�����
		/// ע������Ӧ���ڹ�����������ɾ����
		/// </summary>
//		internal virtual void Register()
//		{
//			_IregService.Perform(this);
//		}
//		internal virtual void UnRegiser()
//		{
//			_IregService.Revoke(this);
//		}

		#region ITrafficEntity ��Ա

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
		
		
		//Ԫ������ϵ
		private Point _pntGrid;
		
		private EntityType _entityType;

//		/// <summary>
//		/// Ԫ������ϵ,��ʱ������ʹ��spaceGrid
//		/// </summary>
//		[System.Obsolete("outdated use spacilgrid ")]
//		public virtual Point Grid
//		{
//			get { return _pntGrid; }
//			set { _pntGrid = value; }
//		}
//		
		
		/// <summary>
		/// ��Ԫ�����������ʵ��Ŀ�ȣ�ʵ�ʿ�ȵ���iWidth*Ԫ������ľ���
		/// </summary>
		public virtual int Width
		{
			get { return _iWidth; }
			set { _iWidth = value; }
		}
		/// <summary>
		/// ��Ԫ�����������ʵ��ĳ��ȡ�ʵ�ʳ��ȵ���iLength*Ԫ������ľ���
		/// </summary>
		public virtual int Length
		{
			get { return _iLength; }
			set { _iLength = value; }
		}
		/// <summary>
		/// TrafficEntity�����������,�ǽ�ͨ��,����.���ǽ���
		/// </summary>
		public virtual EntityType EntityType
		{
			get { return _entityType; }
			set { _entityType = value; }
		}
		
		/// <summary>
		/// ����RoadNode������Ϊ��Ԫ������Ϊ��λ��������꣬������Ϊ��Ļ����
		/// ����Ԫ����GUI����
		/// ����RoadLane��RoadEdge�Լ�Cell��������XΪ��������RoadNode��Ԫ������
		/// YΪ��������RoadNode��ƫ�ƣ����ڼ���������
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

		#region δ������GIS
		private OxyzPointF _position;
		/// <summary>
		/// δ������GISϵͳԤ����GIS����ϵ
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
		/// ����Ӧ����д�������\����GUI��ͼ������
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
				throw new System.Exception("Ŀ�꺯��û��ע��");
			}
			set
			{
				this._container = value;
			}
		}
		
		private string _strName;
		/// <summary>
		/// ��ͨʵ������ơ���·�������������
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


		#region ״̬���º���
		
		/// <summary>
		/// �洢���϶�����첽���µĹ���
		/// </summary>
		internal AsynchronicAgents asynAgents = new AsynchronicAgents();

		/// <summary>
		/// �洢���϶����ͬ�����µĹ���
		/// </summary>
		internal SynchronicAgents synAgents = new SynchronicAgents();

		/// <summary>
		/// ��ʱ�ģ�ԭ�еĵ��ú������������еķ����ߣ������ڲ�Ԫ���ĸ���
		/// </summary>
		[System.Obsolete("��ʱ�ģ�ԭ�еĵ��ú������������еķ����ߣ������ڲ�Ԫ���ĸ���")]
		public virtual void UpdateStatus()
		{
			this.OnStatusChanged();
		}
		protected virtual void OnStatusChanged()
		{
			throw new NotImplementedException("�����˻����״̬�ı亯���ǲ��Ե�");
		}
		
		

		/// <summary>
		///RoadEdge��item ��Agent��visitor �൱��item.accept(visitor)
		/// </summary>
		/// <param name="ur"></param>
		public void AcceptSynAgent(Agents.AbstractAgent ur)
		{
			if (ur != null)
			{
				//��ӵ�����������
				this.ISimCtx.Agents.Add(ur.GetHashCode(), ur);
				this.synAgents.Add(ur);
			}
			else
			{
				throw new ArgumentNullException("�յĸ��¹���");
			}
		}
		/// <summary>
		/// ����첽���¹���
		/// </summary>
		/// <param name="ur"></param>
		public void AcceptAsynAgent(Agents.AbstractAgent ur)
		{
			if (ur != null)
			{
				//��ӵ�����������
				this.ISimCtx.Agents.Add(ur.GetHashCode(), ur);
				this.asynAgents.Add(ur);
			}
			else
			{
				throw new ArgumentNullException("�յĸ��¹���");
			}
		}
		#endregion
		
	}
	
	
	/// <summary>
	/// 2015��1��19�ո��£������ӵ����ݡ�
	/// </summary>
	public abstract partial class TrafficEntity:ITrafficEntity
	{
		//3dԪ���ռ�
		private OxyzPointF _oxyzGrid;
		/// <summary>
		/// 3dԪ���ռ䣬Ϊ����չGIS��׼��
		/// </summary>
		public virtual  OxyzPointF SpatialGrid
		{
			get { return this._oxyzGrid; }
			set { _oxyzGrid = value; }
		}

	}
}

