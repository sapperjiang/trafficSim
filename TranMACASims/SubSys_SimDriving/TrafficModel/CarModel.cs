using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.RoutePlan;
using System.Drawing;


namespace SubSys_SimDriving.TrafficModel
{
	/// <summary>
	/// 过时的，为了保持兼容，建议使用新的类型。
	/// </summary>
	[System.Obsolete("为了向下兼容使用，建议使用新的类型")]
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
        /// 当前车辆的加速度
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
	/// 公共汽车，占用4个元胞网格 。12米的，取决于元胞网格的空间大小
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
	/// 大卡车，占用4个元胞网格
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
	/// 行人，一般占用1个元胞网格
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
 
