using SubSys_SimDriving.RoutePlan;

namespace SubSys_SimDriving
{
	public class CarModel : MobileEntity
	{
		public int Color;
		 
		public int CarType;
		 
		public int Acceleraton;
		 
		private CarTimeSpaceTable carTimeSpaceTable;
		 
		private Route route;

        public CarModel()
        {
            this.EntityAgent = new CarAgent();
        }

        
		 
	}
	 
}
 
