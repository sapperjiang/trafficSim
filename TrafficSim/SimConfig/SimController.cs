
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SubSys_Graphics;

using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving;
using SubSys_SimDriving.Service;
using SubSys_SimDriving.TrafficModel;


namespace SubSys_SimDriving
{
	internal static class SimController
	{
		internal static event EventHandler OnSimulateStoped ;

		internal static event EventHandler OnSimulateImpulse;

        internal static ISimContext ISimCtx = SimContext.GetInstance();

		internal static Control Canvas;
		
		internal static IRoadNet iroadNet;

		internal static bool bIsExit = false;
		//internal static int iRoadWidth = 50;//1000m

		internal static string strSimMsg = null;

		internal static int iSimInterval = 100;
		internal static int iSimTimeSteps = 4200;
		internal static bool bIsPause = false;
		internal static int iMobileCount =4;
		
	
//		private static IFactory AddSignalGroup(IFactory ifactory, XNode xNode)
//		{
//			ifactory = (new AgentFactory()) as IFactory;
//			//添加信号灯规则
//			xNode.AcceptAsynAgent(ifactory.Build(null, AgentType.SignalLightAgent));
//
//			//信号灯赋值
//			ifactory = new StaticFactory();
//			SignalLight sl = ifactory.Build(null, EntityType.SignalLight) as SignalLight;
//
//			foreach (XNode item in iroadNet.XNodes)
//			{
//				foreach (Way roadEdge in item.Ways)
//				{
//					roadEdge.GetReverse().ModifySignalGroup(sl, LaneType.Straight);
//				}
//			}
//			return ifactory;
//		}

		/// <summary>
		/// 仿真的每个时间步骤，重新绘制道路网。
		/// </summary>
		public static void RepaintNetWork(object sender, MouseEventArgs e)
		{
			IService isPainter = PainterManager.GetService(PaintServiceType.Way, SimController.Canvas);
			isPainter.IsRunning = true;
			foreach (var item in SimController.ISimCtx.RoadNet.Ways)
			{
				if (bIsExit == true || bIsPause == true)
				{
					break;
				}
				isPainter.Perform(item);
			}
			isPainter = PainterManager.GetService(PaintServiceType.XNode, SimController.Canvas);

			isPainter.IsRunning = true;
			foreach (var item in SimController.ISimCtx.RoadNet.XNodes)
			{
				if (bIsExit == true || bIsPause == true)
				{
					break;
				}
				isPainter.Perform(item);
			}
		}

		/// <summary>
		/// 添加数据记录服务
		/// </summary>
		/// <param name="frMain"></param>
		/// <param name="irn"></param>
		private static void RegisterServices()
		{
			ISimContext isc = SimContext.GetInstance();

			IService isDataRecorder = new DataRecordService(isc);
			isDataRecorder.IsRunning = true;
			
			IService IPainter = PainterManager.GetService(PaintServiceType.Way, SimController.Canvas);
			IPainter.IsRunning = true;
			foreach (var way in SimController.ISimCtx.RoadNet.Ways)
			{
				foreach (var lane in way.Lanes)
				{
					lane.AddService(isDataRecorder);
				}
				
				way.AddService(IPainter);
				
			}
			
			IPainter = PainterManager.GetService(PaintServiceType.XNode, SimController.Canvas);
			IPainter.IsRunning = true;
			foreach (var xnode in SimController.ISimCtx.RoadNet.XNodes)
			{
				xnode.AddService(isDataRecorder);
				
				xnode.AddService(IPainter);
			}
			
		}
		
		private static void LoadMobiles()
		{
            IRoadNet inet = RoadNet.GetInstance();
            Way way = null;//= new Way(;
            foreach (var item in inet.Ways)
            {
                way = item;
            }
            Lane startLane = way.Lanes[0];
            int i = SimController.iMobileCount;
            while (i-- > 0)
			{
                //新建一条路由
                EdgeRoute route = new EdgeRoute();
                //设置每段路走哪条路
                route.Add(way);
                var car = MobileFactory.BuildSmallCar();
				car.Route = route;
				startLane.EnterInn(car);
			}
		}
		
		//-----------20160131
		/// <summary>
		/// 仿真运行的主循环
		/// </summary>
		public static void StartSimulate()
		{
			//数据记录服务
			RegisterServices();

            //add mobile one by one
            SimController.LoadMobiles();

            var network =SimController.ISimCtx.RoadNet;
			while (true) {
				//t退出命令或者仿真到了设定的仿真时长
				if (bIsExit == true||ISimCtx.iTimePulse>= iSimTimeSteps)
				{
					if (OnSimulateStoped!=null) {
						OnSimulateStoped(null,null);
					}
					break;
				}


                

                //线程休眠减少资源消耗
                Thread.Sleep(SimController.iSimInterval);
				//处理应用程序界面事件。如点击鼠标、点击菜单等
				Application.DoEvents();

				
				if (bIsPause==false) {//如果没有暂停
					while (ISimCtx.iTimePulse++ <= iSimTimeSteps)
					{
						strSimMsg = ISimCtx.iTimePulse.ToString();
						
						if (bIsExit==true||bIsPause  == true)//退出或者暂停都停止循环
						{
							break;
						}
                        if (OnSimulateImpulse != null)
                        {
                            OnSimulateImpulse(null, null);
                        }


                        SimController.iSimInterval =500;
						Thread.Sleep(SimController.iSimInterval);
						Application.DoEvents();
						
                    

						//更新RoadNodes
						foreach (XNode item in network.XNodes)
						{
							if (bIsExit == true || bIsPause == true)
							{
								break;
							}
							item.UpdateStatus();
						}
						//更新roadEdge
						foreach (Way item in network.Ways)
						{
							if (bIsExit == true || bIsPause == true)
							{
								break;
							}

							//首先更新自己。
							item.UpdateStatus();
						}
						//路网仿真时间计数器更新
						network.iTimePulse = ISimCtx.iTimePulse;
					}
				}
				
			}
		}
	}
	
}

