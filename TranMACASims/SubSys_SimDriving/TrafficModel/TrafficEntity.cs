using System.Drawing;
using SubSys_MathUtility;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.SysSimContext.Service;

namespace SubSys_SimDriving
{
    /// <summary>
    /// 观察者模式中的subject被观察者
    /// </summary>
    public abstract class TrafficEntity : ITrafficEntity
    {
        IService IRegisteService = new RegisterService();
        ServiceMgr SvcMgr = new ServiceMgr();//服务管理池

        public virtual void AddService(IService ils)
        {
            this.SvcMgr.Add(ils);
        }
        public virtual void InvokeServices(ITrafficEntity te)
        {
            foreach (IService item in this.SvcMgr)
            {
                item.Perform(te);
            }
        }

        /// <summary>
        /// 删除交通实体要使用的服务
        /// </summary>
        /// <param name="ils"></param>
        public virtual void RemoveService(IService ils)
        {
            this.SvcMgr.Remove(ils);
        }
        /// <summary>
        /// 向simContext 报道类的创建行为
        /// </summary>
        internal virtual void Register()
        {
            IRegisteService.Perform(this);
        }
        internal virtual void UnRegiser()
        {
            IRegisteService.Revoke(this);
        }

        #region ITrafficEntity 成员

        protected int _id;
        private int _iWidth;
        private int _iLength;
        private Point _rltPos;//元胞坐标系

        private MyPoint _position;
        //private Point _screanPos;
        //public Point scrnPos
        //{
        //    get { return _screanPos; }
        //    set { _screanPos = value; }
        //}
        private EntityType _entityType;

        /// <summary>
        /// 用元胞个数计算的实体的宽度，实际宽度等于iWidth*元胞代表的距离
        /// </summary>
        public virtual int iWidth
        {
            get { return _iWidth; }
            set { _iWidth = value; }
        }
        /// <summary>
        /// 用元胞个数计算的实体的长度。实际长度等于iLength*元胞代表的距离
        /// </summary>
        public virtual int iLength
        {
            get { return _iLength; }
            set { _iLength = value; }
        }
        public EntityType EntityType
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
        public virtual Point RelativePosition
        {
            get { return _rltPos; }
            set { _rltPos = value; }
        }
        public ISimContext ISimCtx
        {
            get { return SimContext.GetInstance(); }
        }

        public int ID
        {
            get
            {
                return this._id;
            }

        }

        public MyPoint gisPos
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

        private EntityShape _entityShape = new EntityShape();
        private ITrafficEntity _container;

        /// <summary>
        /// 子类应当重写这个属性\用于GUI画图的属性
        /// </summary>
        public virtual EntityShape Shape
        {
            get 
            {
                if (this._entityShape!=null)
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
                //ThrowHelper.ThrowArgumentNullException(ExceptionArgument.obj);
            }
            set
            {
                this._container = value;
            }
        }
        private string _strName;
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

        public virtual MyPoint ToVector()
        {
            throw new System.NotImplementedException();
        }

        #endregion



    }
}
 
