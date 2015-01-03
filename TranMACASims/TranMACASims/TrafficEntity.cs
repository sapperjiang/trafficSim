using SubSys_SimDriving;
using SubSys_SimDriving.SysSimDrivingContext;

namespace SubSys_SimDriving
{
	public abstract class TrafficEntity : ITrafficEntity
	{
        protected static SysSimDrivingContext.SimDrivingContext _simDrivingContext= SimDrivingContext.GetSimDrivingContext();

        private Agent _entityAgent;

        public Agent EntityAgent
        {
            get { return _entityAgent; }
            set { _entityAgent = value; }
        }
        private EntityType _enumEntityType;
        private int _id;
        private EntityStatus _entityStatus;
        private MyPoint _position;
        #region ITrafficEntity ≥…‘±

        public SysSimDrivingContext.SimDrivingContext SimDrivingContext
        {
            get { return TrafficEntity._simDrivingContext; }
        }

        public EntityType EntityType
        {
            get
            {
                return this._enumEntityType;
            }
            set
            {
                this._enumEntityType = value;//throw new System.NotImplementedException();
            }
        }

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

        public EntityStatus EntityStatus
        {
            get
            {
                return this._entityStatus; 
            }
            set
            {
this._entityStatus = value;
            }
        }

        public MyPoint Postion
        {
            get
            {
               return this._position ;
            }
            set
            {
                this._position = value;
            }
        }

        #endregion

      
    }
	 
}
 
