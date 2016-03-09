using System;
using System.Linq;
using System.Drawing;

using SubSys_SimDriving.TrafficModel;
using SubSys_MathUtility;

namespace SubSys_SimDriving
{
	
	/// <summary>
	/// 汽车工厂
	/// </summary>
	public class MobileFactory:IMobileFactory
	{

		public MobileEntity Build(EntityType etype)
		{
			switch (etype)
			{
				case EntityType.SmallCar:
					
					SmallCar  newCar = new SmallCar();
					newCar.Color = MobileFactory.RandomColor();
					
					newCar.SpatialGrid =OxyzPointF.Default;
					
				
					return newCar;
					
//				case EntityType.Bus:
//					Bus  newBus = this.bus.Clone() as Bus;
//					newBus.Color = MobileFactory.GetRandomColor();
//					return newBus;
//
//				case EntityType.Pedastrain:
					///Pedastrain  newBus = this.p.Clone() as Pedastrain;
					////newBus.Color = MobileFactory.GetRandomColor();
//					return new Pedastrain();
//				case EntityType.LargeTruck:
//					return new LargeTruck();
					
				default:
					break;
			}
			throw new  ArgumentException("无法创建参数指定的构造型");
		}
		
		public MobileFactory()
		{
			//sCar= new SmallCar();
			//	bus = new Bus();
			//		largeTruck = new LargeTruck();
		}

		private static Color RandomColor()
		{
			Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
			//  对于C#的随机数，没什么好说的
			System.Threading.Thread.Sleep(RandomNum_First.Next(50));
			Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
			//  为了在白色背景上显示，尽量生成深色
			int int_Red = RandomNum_First.Next(256);
			int int_Green = RandomNum_Sencond.Next(256);
			int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
			int_Blue = (int_Blue > 255) ? 255 : int_Blue;
			return Color.FromArgb(int_Red, int_Green, int_Blue);
		}

	}
}
