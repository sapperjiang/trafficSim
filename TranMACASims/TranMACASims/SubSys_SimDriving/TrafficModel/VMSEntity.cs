using SubSys_SimDriving;

namespace SubSys_SimDriving
{
	internal class VMSEntity : TrafficEntity
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
            this.Register(this);
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
 
