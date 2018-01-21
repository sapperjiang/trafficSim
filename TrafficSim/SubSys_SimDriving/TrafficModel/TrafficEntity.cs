using SubSys_SimDriving;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.Agents;

namespace SubSys_SimDriving
{
    /// <summary>
    /// 观察者模式中的subject被观察者
    /// </summary>
    internal abstract class TrafficEntity : ITrafficEntity
    {
        ILogService IlogService = new RegisterLogger();
        //ILogService IlogService = new RegisterLogger();//其他类型的log服务
        LogServicesMgr LogMgr = new LogServicesMgr();

        /// <summary>
        /// 向simContext 报道类的创建行为
        /// </summary>
        internal virtual void Register(TrafficEntity teVar)
        {
            IlogService.Log(teVar);
        }
        internal virtual void UnRegiser(TrafficEntity teVar)
        {
            IlogService.UnLog(teVar);
        }

        //  protected SysSimContext.SimContext simContext= SimContext.GetInstance();

        private Agent _entityAgent;

        internal Agent EntityAgent
        {
            get { return _entityAgent; }
            set { _entityAgent = value; }
        }
        private EntityType _enumEntityType;
        private int _id;
        private EntityStatus _entityStatus;
        private MyPoint _position;
        #region ITrafficEntity 成员

        public SysSimContext.SimContext simContext
        {
            get { return SimContext.GetInstance(); }
        }

        //public EntityType EntityType
        //{
        //    get
        //    {
        //        return this._enumEntityType;
        //    }
        //    set
        //    {
        //        this._enumEntityType = value;//throw new System.NotImplementedException();
        //    }
        //}

        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        //        public EntityStatus EntityStatus
        //        {
        //            get
        //            {
        //                return this._entityStatus; 
        //            }
        //            set
        //            {
        //this._entityStatus = value;
        //            }
        //        }

        public MyPoint Postion
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
    }
}
 
