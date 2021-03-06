using SubSys_SimDriving;

namespace SubSys_SimDriving
{
	public class SignalLight : TrafficOBJ
	{
		/// <summary>
		/// 周期时长
		/// </summary>
		public int SigCycle=60;
		/// <summary>
		/// 绿灯时间
		/// </summary>
		public int GreenLength=30;

		/// <summary>
		/// 红灯时间30
		/// </summary>
		public int RedLength=30;
		
		/// <summary>
		/// 黄灯时间，暂时不使用
		/// </summary>
		public int YellowLength=0;

		/// <summary>
		/// 相位差，从仿真基准时间开始的时刻
		/// </summary>
		public int iOffSet=0;

		internal bool bIsWorking = false;

		internal bool IsGreen(int iCurrTimeStep)
		{
			//奇数红灯偶数绿灯
			int iTime = iCurrTimeStep - this.iOffSet;//减去相位差
			iTime %= GreenLength + RedLength+YellowLength;//取周期的余数进行验算
			if(iTime<=GreenLength)//在绿灯范围内
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// 内部调用信号灯注册一下
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
		///// 构造函数调用
		///// </summary>
		//internal override void Register()
		//{
		//    this.simContext.SignalLightList.Add(this.GetHashCode(),this);
		//}
		///// <summary>
		///// 自己调用
		///// </summary>
		//internal override void UnRegiser()
		//{
		//    this.simContext.SignalLightList.Remove(this.GetHashCode());
		//}
		
	}
	
}

