using SubSys_SimDriving;

namespace SubSys_SimDriving
{
	public class VMSEntity : TrafficEntity
	{
		/**
		 * VMS��ʾ����Ϣ
		 */
		internal string Message;
		 
		/**
		 * ����ȷ��vms���õķ�Χ��Ԫ������ĳ��ȣ�
		 */
        internal int iArm;


        internal VMSEntity()
        {
    //        this.Register();
        }
        ///// <summary>
        ///// ���캯������
        ///// </summary>
        //internal override void Register()
        //{
        //    this.simContext.VMSList.Add(this.GetHashCode(), this);
        //}
        ///// <summary>
        ///// �Լ�����
        ///// </summary>
        //internal override void UnRegiser()
        //{
        //    this.simContext.VMSList.Remove(this.GetHashCode());
        //}
	}
	 
}
 
