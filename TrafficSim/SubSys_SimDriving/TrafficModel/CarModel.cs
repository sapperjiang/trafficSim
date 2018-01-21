using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.RoutePlan;


namespace SubSys_SimDriving
{
	internal class CarModel : MobileEntity
	{
		internal int Color;
		 
		internal int CarType;

        internal EdgeRoute EdgeRoute;
        internal NodeRoute NodeRoute;

        internal DrivingStrategy DriveStg = new ModerateDrivingStrategy(); 
        internal CarModel()
        {
            //this.EntityAgent = new Agent();
            //this.Register(this);
            this.iSpeed = 5;
            base.Register(this);
        }
        //internal SpeedLevel CurrSpeed;
        /// <summary>
        /// 当前车辆的加速度
        /// </summary>
        internal int iAcc = 2;

        /// <summary>
        /// 加速
        /// </summary>
        internal void GearUp()
        {
            if (this.iSpeed < SpeedLevel.iCellLevelTwelve)
            {
                this.iSpeed += this.iAcc;
            }else
            {
                throw new System.Exception("Speed out of Range！");
            }
        }
        /// <summary>
        /// 减速
        /// </summary>
        internal void GearDown()
        {
            if (this.iSpeed>this.iAcc)
            {
                this.iSpeed -= this.iAcc;
            }else
            {
                throw new System.Exception("Speed out of Range！");
            }
        }

    }
	 
}
 
