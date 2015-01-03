using SubSys_SimDriving;

namespace SubSys_SimDriving
{
	internal class VMSEntity : TrafficEntity
	{
		/**
		 * VMS显示的信息
		 */
		internal string Message;
		 
		/**
		 * 用来确定vms作用的范围（元胞网格的长度）
		 */
        internal int iArm;


        internal VMSEntity()
        {
            this.Register(this);
        }
        ///// <summary>
        ///// 构造函数调用
        ///// </summary>
        //internal override void Register()
        //{
        //    this.simContext.VMSList.Add(this.GetHashCode(), this);
        //}
        ///// <summary>
        ///// 自己调用
        ///// </summary>
        //internal override void UnRegiser()
        //{
        //    this.simContext.VMSList.Remove(this.GetHashCode());
        //}
	}
	 
}
 
