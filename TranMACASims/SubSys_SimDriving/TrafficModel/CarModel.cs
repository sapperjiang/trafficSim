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
		public static int SmallCarID = 0;
		
//		private bool IsCopyed = false;
//		public SmallCar Copy()
//		{
//			SmallCar cm = this.MemberwiseClone() as SmallCar;
//			cm.IsCopyed = true;
//			return cm;
//		}

		/// <summary>
		/// base constructor is called first .base constructor is used to do common things
		/// while derived class is used to do characterful charactors
		/// </summary>
		/// <param name="bornContainer"></param>
		public SmallCar()
		{
			this.TypeID = ++SmallCar.SmallCarID;
			this.EntityType = EntityType.SmallCar;
			this.iSpeed = 0;
			this.iAcceleration = 1;
		}

		//internal SpeedLevel CurrSpeed;
		/// <summary>
		/// 当前车辆的加速度
		/// </summary>
		internal int iAcceleration = 1;

	}
	

	public class MediumCar:MobileEntity
	{
		public static int MediumCarID = 0;
		public MediumCar()
		{
			this.TypeID = ++MediumCarID;
			this.EntityType = EntityType.MediumCar;
		}
	}
	

	
	/// <summary>
	/// 公共汽车，占用4个元胞网格 。12米的，取决于元胞网格的空间大小
	/// </summary>
	public class Bus:MobileEntity
	{
		public static int BusID = 0;
		public Bus()
		{
			this.TypeID = ++BusID;
			
			this.EntityType = EntityType.Bus;
			
		}
	}
	
	
	/// <summary>
	/// 大卡车，占用4个元胞网格
	/// </summary>
	public class LargeTruck:MobileEntity
	{
		public static int LargeTruckID = 0;
		
		public LargeTruck()
		{
			this.EntityType = EntityType.LargeTruck;
			this.TypeID = ++LargeTruckID;
		}
	}

	
	/// <summary>
	/// 行人，一般占用1个元胞网格
	/// </summary>
	public class Pedastrain:MobileEntity
	{
		public static int PedastrainID = 0;
		
		public Pedastrain()
		{
			this.EntityType = EntityType.Pedastrain;
			this.TypeID = ++PedastrainID;
		}
	}

}

