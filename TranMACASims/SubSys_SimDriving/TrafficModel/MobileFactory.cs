using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving;
using SubSys_SimDriving.Agents;

namespace SubSys_DataManage
{
	/// <summary>
	/// 汽车工厂
	/// </summary>
	public class MobileFactory//:IFactory
	{
		public AbstractAgent Build(BuildCommand bc, AgentType et)
		{
			throw new NotImplementedException();
		}
		
//			public TrafficEntity Build(BuildCommand bc, EntityType etype)
//		{
//	throw new NotImplementedException();
//		}

		public MobileEntity Build(BuildCommand bc, EntityType etype)
		{
			switch (etype)
			{
				case EntityType.SmallCar:
					
					//浅表复制一个
					SmallCar  newCar =this.sCar.Clone() as SmallCar;
					newCar.Color = MobileFactory.GetRandomColor();
					return newCar;
					
				case EntityType.Bus:
					Bus  newBus = this.bus.Clone() as Bus;
					newBus.Color = MobileFactory.GetRandomColor();
					return newBus;
					
				case EntityType.Pedastrain:
//					Pedastrain  newBus = this.p.Clone() as Pedastrain;
//					
//					newBus.Color = MobileFactory.GetRandomColor();
					return new Pedastrain();
				case EntityType.LargeTruck:
					return new LargeTruck();
					
				default:
					break;
			}
			throw new  ArgumentException("无法创建参数指定的构造型");
		}
		
		
		SmallCar sCar ;//= new SmallCar()
		Bus bus ;
		LargeTruck largeTruck ;
		
		MobileFactory()
		{
			sCar= new SmallCar();
			bus = new Bus();
			largeTruck = new LargeTruck();
		}
		
		
		static ISimContext isc = SimContext.GetInstance();

		private static Color GetRandomColor()
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
