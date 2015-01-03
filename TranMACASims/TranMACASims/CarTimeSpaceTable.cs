using SubSys_SimDriving;
//using SubSys_DataMgr;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.SysSimDrivingContext;

namespace SubSys_SimDriving
{
	/**
	 * ����ʱ�ձ��ڷ�����Ҫ���������ʱ��
	 * ����������һ������ʱ����Ϣ��
	 * ������Ϊһ���ӿ��ṩ
	 * 
	 */
	public abstract class CarTimeSpaceTable
	{
		public int iTimeStep;
		 
        //public Point CarPosition;
		 
		private int iRoadSegID;
		 
        //private CarTimeSpaceTableAnalyzer carTimeSpaceTableAnalyzer;
		 
        //private CarTimeSpaceTableAnalyzer carTimeSpaceTableAnalyzer;
		 
		private Route route;
		 
		private CarModel carModel;
		 
		private CarModelHashTable carModelHashTable;
		 
		private RunningStatusRoadSegHashTable runningStatusRoadSegHashTable;
		 
	}
	 
}
 
