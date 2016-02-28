using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.RoutePlan;
using System.Drawing;


namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// 过时的，为了保持兼容，建议使用新的类型。
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
		/// 当前车辆的加速度
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
	/// 公共汽车，占用4个元胞网格 。12米的，取决于元胞网格的空间大小
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
	/// 大卡车，占用4个元胞网格
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
	/// 行人，一般占用1个元胞网格
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

