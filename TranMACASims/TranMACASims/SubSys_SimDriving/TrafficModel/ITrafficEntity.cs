using SubSys_SimDriving;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.Agents;
namespace SubSys_SimDriving
{
	public interface ITrafficEntity
	{
        /// <summary>
        /// �������еľ�̬���ݣ���ͨʵ�����е����ݻ������������ַ���ϵͳ������Ҫ�����ݽṹ
        /// </summary>
        SysSimContext.SimContext simContext
        {
            get;
        }

        //Agent EntityAgent
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// ��ͨʵ������ͣ�VMS��Car��TrafficLight ��
        /// </summary>
        // EntityType EntityType
        //{
        //    get ;
        //    set;
        //}
		 
        int ID
        {
            get;
            set;
        }
		 
        //EntityStatus EntityStatus
        //{
        //    get;
        //    set;
        //}
		 
     
	    SubSys_SimDriving.MyPoint Postion
	{
        get;
        set;
	}
		 
	}
	 
}
 
