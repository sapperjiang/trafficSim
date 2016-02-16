using SubSys_SimDriving;
using SubSys_SimDriving.SysSimDrivingContext;
namespace SubSys_SimDriving
{
	public interface ITrafficEntity
	{
        /// <summary>
        /// 仿真运行的静态数据，交通实体运行的数据环境，包括各种仿真系统运行需要的数据结构
        /// </summary>
        SysSimDrivingContext.SimDrivingContext SimDrivingContext
        {
            get;
        }

        Agent EntityAgent
        {
            get;
            set;
        }
        /// <summary>
        /// 交通实体的类型，VMS，Car，TrafficLight 等
        /// </summary>
         EntityType EntityType
        {
            get ;
            set;
        }
		 
         int ID
        {
            get;
            set;
        }
		 
        EntityStatus EntityStatus
        {
            get;
            set;
        }
		 
     
	 SubSys_SimDriving.MyPoint Postion
	{
        get;
        set;
	}
		 
	}
	 
}
 
