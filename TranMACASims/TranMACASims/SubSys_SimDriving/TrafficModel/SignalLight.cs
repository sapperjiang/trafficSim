using SubSys_SimDriving;

namespace SubSys_SimDriving
{
	internal class SignalLight : TrafficEntity
	{
        /// <summary>
        /// ����ʱ��
        /// </summary>
		internal int SigCycle=60;
        /// <summary>
        /// �̵�ʱ��
        /// </summary>
		internal int GreenLength=30;

		/// <summary>
		/// ���ʱ��30
		/// </summary>
		internal int RedLength=30;
		 
        /// <summary>
        /// �Ƶ�ʱ�䣬��ʱ��ʹ��
        /// </summary>
		internal int YellowLength=0;

        /// <summary>
        /// ��λ��ӷ����׼ʱ�俪ʼ��ʱ��
        /// </summary>
        internal int iOffSet=0;

        internal bool bIsWorking = false;

        internal bool IsGreen(int iCurrTimeStep) 
        {
            //�������ż���̵�
            int iTime = iCurrTimeStep - this.iOffSet;//��ȥ��λ��
            iTime %= GreenLength + RedLength+YellowLength;//ȡ���ڵ�������������
            if(iTime<=GreenLength)//���̵Ʒ�Χ��
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// �ڲ������źŵ�ע��һ��
        /// </summary>
        internal SignalLight()
        {
            base.Register(this);
            //this.Register();
        }
        ///// <summary>
        ///// ���캯������
        ///// </summary>
        //internal override void Register()
        //{
        //    this.simContext.SignalLightList.Add(this.GetHashCode(),this);
        //}
        ///// <summary>
        ///// �Լ�����
        ///// </summary>
        //internal override void UnRegiser()
        //{
        //    this.simContext.SignalLightList.Remove(this.GetHashCode());
        //}
		 
	}
	 
}
 
