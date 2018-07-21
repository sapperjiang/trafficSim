using SubSys_SimDriving;

namespace SubSys_SimDriving
{
	public class SignalLight : TrafficOBJ
	{
		/// <summary>
		/// ����ʱ��
		/// </summary>
		public int SigCycle=60;
		/// <summary>
		/// �̵�ʱ��
		/// </summary>
		public int GreenLength=30;

		/// <summary>
		/// ���ʱ��30
		/// </summary>
		public int RedLength=30;
		
		/// <summary>
		/// �Ƶ�ʱ�䣬��ʱ��ʹ��
		/// </summary>
		public int YellowLength=0;

		/// <summary>
		/// ��λ��ӷ����׼ʱ�俪ʼ��ʱ��
		/// </summary>
		public int iOffSet=0;

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
			//base.Register();
		}

		~SignalLight()
		{
	//		base.UnRegiser();
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

