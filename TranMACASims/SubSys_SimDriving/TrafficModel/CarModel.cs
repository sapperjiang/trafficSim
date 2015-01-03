using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.RoutePlan;
using System.Drawing;


namespace SubSys_SimDriving.TrafficModel
{
	public class Car : MobileEntity
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

        internal DriveStrategy DriveStg = new DefaultDriveStrategy(); 

        public Car()
        {
            this._id = ++CarID;
            this.EntityType = EntityType.CarModel;
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

        ///// <summary>
        ///// 加速
        ///// </summary>
        //internal void GearUp()
        //{
        //    if (this.iSpeed < SpeedLevel.iCellLevelTwelve)
        //    {
        //        this.iSpeed += this.iAcc;
        //    }else
        //    {
        //        throw new System.Exception("Speed out of Range！");
        //    }
        //}
        ///// <summary>
        ///// 减速
        ///// </summary>
        //internal void GearDown()
        //{
        //    if (this.iSpeed>this.iAcc)
        //    {
        //        this.iSpeed -= this.iAcc;
        //    }else
        //    {
        //        this.iSpeed = 0;
        //       // throw new System.Exception("Speed out of Range！");
        //    }
        //}

    }
	 
}
 
