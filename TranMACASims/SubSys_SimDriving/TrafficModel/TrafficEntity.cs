using System.Drawing;
using SubSys_MathUtility;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.SysSimContext.Service;

namespace SubSys_SimDriving
{
    /// <summary>
    /// �۲���ģʽ�е�subject���۲���
    /// </summary>
    public abstract class TrafficEntity : ITrafficEntity
    {
        IService IRegisteService = new RegisterService();
        ServiceMgr SvcMgr = new ServiceMgr();//��������

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
        /// ɾ����ͨʵ��Ҫʹ�õķ���
        /// </summary>
        /// <param name="ils"></param>
        public virtual void RemoveService(IService ils)
        {
            this.SvcMgr.Remove(ils);
        }
        /// <summary>
        /// ��simContext ������Ĵ�����Ϊ
        /// </summary>
        internal virtual void Register()
        {
            IRegisteService.Perform(this);
        }
        internal virtual void UnRegiser()
        {
            IRegisteService.Revoke(this);
        }

        #region ITrafficEntity ��Ա

        protected int _id;
        private int _iWidth;
        private int _iLength;
        private Point _rltPos;//Ԫ������ϵ

        private MyPoint _position;
        //private Point _screanPos;
        //public Point scrnPos
        //{
        //    get { return _screanPos; }
        //    set { _screanPos = value; }
        //}
        private EntityType _entityType;

        /// <summary>
        /// ��Ԫ�����������ʵ��Ŀ�ȣ�ʵ�ʿ�ȵ���iWidth*Ԫ������ľ���
        /// </summary>
        public virtual int iWidth
        {
            get { return _iWidth; }
            set { _iWidth = value; }
        }
        /// <summary>
        /// ��Ԫ�����������ʵ��ĳ��ȡ�ʵ�ʳ��ȵ���iLength*Ԫ������ľ���
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
        /// ����RoadNode������Ϊ��Ԫ������Ϊ��λ��������꣬������Ϊ��Ļ����
        /// ����Ԫ����GUI����
        /// ����RoadLane��RoadEdge�Լ�Cell��������XΪ��������RoadNode��Ԫ������
        /// YΪ��������RoadNode��ƫ�ƣ����ڼ���������
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
        /// ����Ӧ����д�������\����GUI��ͼ������
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
                throw new System.Exception("Ŀ�꺯��û��ע��");
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
 
