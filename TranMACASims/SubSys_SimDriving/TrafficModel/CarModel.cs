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
		/// ��ǰ�����ļ��ٶ�
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
	/// ����������ռ��4��Ԫ������ ��12�׵ģ�ȡ����Ԫ������Ŀռ��С
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
	/// �󿨳���ռ��4��Ԫ������
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
	/// ���ˣ�һ��ռ��1��Ԫ������
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

