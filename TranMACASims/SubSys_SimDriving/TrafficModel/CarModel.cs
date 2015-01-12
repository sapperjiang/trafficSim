using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.RoutePlan;
using System.Drawing;


namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// ��ʱ�ģ�Ϊ�˱��ּ��ݣ�����ʹ���µ����͡�
	/// </summary>
	[System.Obsolete("Ϊ�����¼���ʹ�ã�����ʹ���µ�����")]
	public class Car: MobileEntity
	{
        private static int CarID = 0;
        private bool IsCopyed = false;
        public Car Copy()
        {
            Car cm = this.MemberwiseClone() as Car;
            cm.IsCopyed = true;
            return cm;
        }
		public System.Drawing.Color Color;
		 
        public EdgeRoute EdgeRoute;
        public NodeRoute NodeRoute;

        internal DriveStrategy DriveStg = new DefaultDriveAgent(); 

        public Car()
        {
            this._id = ++CarID;
            this.EntityType = EntityType.Mobile;
            this.Color = Color.Green;
            this.iSpeed = 0;
            base.Register();
            this.EdgeRoute = new EdgeRoute();
            this.NodeRoute = new NodeRoute();

        }
         ~Car()
        {
            if (this.IsCopyed != true)
            {
                base.UnRegiser();
            }
        }
        //internal SpeedLevel CurrSpeed;
        /// <summary>
        /// ��ǰ�����ļ��ٶ�
        /// </summary>
        internal int iAcc = 1;
        
        private void test()
        {
        }

    }
	
	
	
	public class MediumCar:MobileEntity
	{
		public MediumCar()
		{
			this.EntityType = EntityType.MediumCar;
			this.Strategy = StrategyFactory.Create(StrategyType.Default);
//			this.co
		}
	}
	
	
	public class SmallCar:MobileEntity
	{
		public SmallCar()
		{
			this.EntityType = EntityType.SmallCar;
			this.Strategy = StrategyFactory.Create(StrategyType.Default);
			this.Color = Color.White;
		}
		public override MobileEntity Clone()
		{
			return this.MemberwiseClone() as SmallCar;
		}
	}
	
	/// <summary>
	/// ����������ռ��4��Ԫ������ ��12�׵ģ�ȡ����Ԫ������Ŀռ��С
	/// </summary>
	public class Bus:MobileEntity
	{
		public Bus()
		{
			this.EntityType = EntityType.Bus;
			this.Strategy = StrategyFactory.Create(StrategyType.Default);
		}
	}
	
	
	/// <summary>
	/// �󿨳���ռ��4��Ԫ������
	/// </summary>
	public class LargeTruck:MobileEntity
	{
		public LargeTruck()
		{
			this.EntityType = EntityType.LargeTruck;
			this.Strategy = StrategyFactory.Create(StrategyType.Default);
		}
	}
	
	
	/// <summary>
	/// ���ˣ�һ��ռ��1��Ԫ������
	/// </summary>
	public class Pedastrain:MobileEntity
	{
		public Pedastrain()
		{
			this.EntityType = EntityType.Pedastrain;
			this.Strategy = StrategyFactory.Create(StrategyType.Default);
		}
	}
	 
}
 
