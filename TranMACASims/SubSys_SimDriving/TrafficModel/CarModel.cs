using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.RoutePlan;
using System.Drawing;


namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// ��ʱ�ģ�Ϊ�˱��ּ��ݣ�����ʹ���µ����͡�
	/// </summary>
	public class SmallCar: MobileEntity
	{
		private static int smallCarID = 0;
		
		private bool IsCopyed = false;
		public SmallCar Copy()
		{
			SmallCar cm = this.MemberwiseClone() as SmallCar;
			cm.IsCopyed = true;
			return cm;
		}

		internal MobileDriver DriveStg;// = new DefaultDriveAgent();

		/// <summary>
		/// base constructor is called first .base constructor is used to do common things
		/// while derived class is used to do characterful charactors
		/// </summary>
		/// <param name="bornContainer"></param>
		public SmallCar(StaticEntity bornContainer):base(bornContainer)
		{
			this.TypeID = ++SmallCar.smallCarID;
			this.EntityType = EntityType.SmallCar;
		//	this.Color = Color.Green;
			this.iSpeed = 0;
			this.iAcceleration = 1;
			//base.Register();
			this.Route = new EdgeRoute();
		}
		
		
		~SmallCar()
		{
			if (this.IsCopyed != true)
			{
			//	base.UnRegiser(); 
			}
		}
		//internal SpeedLevel CurrSpeed;
		/// <summary>
		/// ��ǰ�����ļ��ٶ�
		/// </summary>
		internal int iAcceleration = 1;

		public override int GetHashCode()
		{
		//	return this.ID.GetHashCode();
			return base.GetHashCode();
		}
		
	}
	

//	public class MediumCar:MobileEntity
//	{
	////		public MediumCar()
	////		{
	////			this.EntityType = EntityType.MediumCar;
	////			this.Strategy = StrategyFactory.Create(StrategyType.Default);
	//////			this.co
	////		}
//	}
	

	
	/// <summary>
	/// ����������ռ��4��Ԫ������ ��12�׵ģ�ȡ����Ԫ������Ŀռ��С
	/// </summary>
//	public class Bus:MobileEntity
//	{
//		public Bus()
//		{
//			this.EntityType = EntityType.Bus;
//			this.Strategy = StrategyFactory.Create(StrategyType.Default);
//		}
//	}
	
	
	/// <summary>
	/// �󿨳���ռ��4��Ԫ������
	/// </summary>
//	public class LargeTruck:MobileEntity
//	{
//		public LargeTruck()
//		{
//			this.EntityType = EntityType.LargeTruck;
//			this.Strategy = StrategyFactory.Create(StrategyType.Default);
//		}
//	}
//
	
	/// <summary>
	/// ���ˣ�һ��ռ��1��Ԫ������
	/// </summary>
//	public class Pedastrain:MobileEntity
//	{
//		public Pedastrain()
//		{
//			this.EntityType = EntityType.Pedastrain;
//			this.Strategy = StrategyFactory.Create(StrategyType.Default);
//		}
//	}
//
}

