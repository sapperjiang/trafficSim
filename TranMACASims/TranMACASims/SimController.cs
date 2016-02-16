
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SubSys_DataManage;
using SubSys_Graphics;
using SubSys_SimDriving.Agents;

using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving;
using SubSys_SimDriving.Service;
using SubSys_SimDriving.TrafficModel;

//using GISTranSim.DataOutput;


namespace SubSys_SimDriving
{
	internal static class SimController
	{
		internal static event EventHandler OnSimulateStoped ;
		
		internal static ISimContext ISimCtx = SimContext.GetInstance();
		internal static  IService isEntityPainter;
		
		internal static Form Canvas;
		
//		internal static IService WayPainter; //= PaintServiceMgr.GetService(PaintServiceType.RoadEdge, frMain);
//		internal static IService XNodePainter; //= PaintServiceMgr.GetService(PaintServiceType.RoadEdge, frMain);
		
		internal static IRoadNet iroadNet;//= simContext.NetWork;

		internal static bool bIsExit = false;
		internal static int iRoadWidth = 50;//1000m

		internal static string strSimMsg = null;

		internal static int iSimInterval = 100;
		internal static int iSimTimeSteps = 4200;
		internal static bool bIsPause = false;
		internal static int iCarCount = 2;
		
		//添加路由表内容-fis
		internal static Way ReA1;
		internal static Way ReA2;
		internal static Way ReA3;
		internal static Way ReA4;
		internal static Way ReB1;
		internal static Way ReB2;
		
		
		private static IFactory AddSignalGroup(IFactory ifactory, XNode xNode)
		{
			ifactory = (new AgentFactory()) as IFactory;
			//添加信号灯规则
			xNode.AcceptAsynAgent(ifactory.Build(null, AgentType.SignalLightAgent));

			//信号灯赋值
			ifactory = new StaticFactory();
			SignalLight sl = ifactory.Build(null, EntityType.SignalLight) as SignalLight;

			foreach (XNode item in iroadNet.XNodes)
			{
				foreach (Way roadEdge in item.RoadEdges)
				{
					roadEdge.GetReverse().ModifySignalGroup(sl, LaneType.Straight);
				}
			}
			return ifactory;
		}

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
		private static void RegisterService()
		{
			ISimContext isc = SimContext.GetInstance();

			IService isDataRecorder = new DataRecordService(isc);
			isDataRecorder.IsRunning = true;
			
			IService isPainter = PainterManager.GetService(PaintServiceType.Way, SimController.Canvas);
			isPainter.IsRunning = true;
			foreach (var way in SimController.ISimCtx.RoadNet.Ways)
			{
				foreach (var lane in way.Lanes)
				{
					lane.AddService(isDataRecorder);
				}
				
				way.AddService(isPainter);
				
			}
			
			isPainter = PainterManager.GetService(PaintServiceType.XNode, SimController.Canvas);

			isPainter.IsRunning = true;
			
			
			foreach (var xnode in SimController.ISimCtx.RoadNet.XNodes)
			{
				xnode.AddService(isDataRecorder);
				
				xnode.AddService(isPainter);
			}
			
		}
		
		
		
		private static void LoadMobiles()
		{
			//下面这段代码不知道干嘛的
			if (--SimController.iCarCount > 0)
			{
				int iLane = 0;// iCarCount % 3;
				//新建一条路由
				EdgeRoute route = new EdgeRoute();
				route.Add(SimController.ReA1);
				//添加节点—fis
				route.Add(SimController.ReA2);
//							erA.Add(SimController.ReA3);
//							erA.Add(SimController.ReA4);
				//设置每段路走哪条路
				Lane startLane = SimController.ReA1.Lanes[iLane];
				
				startLane.EnterInn(MobileSimulator.MakeMobile(route,startLane));
				
				//Lane r2 = SimController.ReB.Lanes[iLane];
				//为每个原包选择出行路由
				//	rA.EnterWaitedQueue(SubSys_SimDriving.TrafficModel.MobileSimulator.MakeCell(erA));
				//r2.EnterWaitedQueue(SubSys_SimDriving.TrafficModel.CarSimulator.MakeCell(er));
			}
		}
		
		//-----------20160131
		/// <summary>
		/// 仿真运行的主循环
		/// </summary>
		public static void StartSimulate()
		{
			//数据记录服务
			RegisterService();
			
			var network =SimController.ISimCtx.RoadNet;
			
			while (true) {
				//t退出命令或者仿真到了设定的仿真时长
				if (bIsExit == true||ISimCtx.iCurrTimeStep>= iSimTimeSteps)
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
				
				SimController.LoadMobiles();
				
				if (bIsPause==false) {//如果没有暂停
					while (ISimCtx.iCurrTimeStep++ <= iSimTimeSteps)
					{
						strSimMsg = ISimCtx.iCurrTimeStep.ToString();
						
						if (bIsExit==true||bIsPause  == true)//退出或者暂停都停止循环
						{
							break;
						}
						
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
						network.iCurrTimeStep = ISimCtx.iCurrTimeStep;
					}
				}
				
			}
		}
	}
	
}

