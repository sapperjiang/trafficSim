using SubSys_SimDriving;
//using SubSys_DataMgr;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.SysSimContext;

namespace SubSys_SimDriving
{
	/**
	 * ����ʱ�ձ��ڷ�����Ҫ���������ʱ��
	 * ����������һ������ʱ����Ϣ��
	 * ������Ϊһ���ӿ��ṩ
	 * 
	 */
	internal abstract class CarTimeSpaceTable
	{
		internal int iTimeStep;
		 
        //internal Point CarPosition;
		 
		private int iRoadSegID;
		 
        //private CarTimeSpaceTableAnalyzer carTimeSpaceTableAnalyzer;
		 
        //private CarTimeSpaceTableAnalyzer carTimeSpaceTableAnalyzer;
		 
		private CarModel carModel;
		 
		private CarModelHashTable carModelHashTable;
		 
		private RunningStatusRoadSegHashTable runningStatusRoadSegHashTable;
		 
	}
	 
}
 
