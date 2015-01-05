
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SubSys_DataManage;
using SubSys_Graphics;
using SubSys_SimDriving.Agents;
using SubSys_SimDriving.ModelFactory;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.SysSimContext.Service;
using SubSys_SimDriving.TrafficModel;

using GISTranSim.DataOutput;

namespace SubSys_SimDriving
{
	internal static class SimController
	{
		internal static event EventHandler OnSimulateOver ;
		
		internal static ISimContext ISCtx = SimContext.GetInstance();
		internal static IService roadEdgePaintService; //= PaintServiceMgr.GetService(PaintServiceType.RoadEdge, frMain);
		internal static IService roadNodePaintService; //= PaintServiceMgr.GetService(PaintServiceType.RoadEdge, frMain);
		internal static IRoadNetWork iroadNetwork;//= simContext.NetWork;

		internal static bool bIsExit = false;
		internal static int iRoadWidth = 500;//1000m

		internal static string strSimMsg = null;

		internal static int iSimInterval = 1000;
		internal static int iSimTimeSteps = 420;
		internal static bool bIsPause = false;
		internal static int iCarCount = 2;

		internal static RoadEdge ReA;
		internal static RoadEdge ReB;
		
		/// <summary>
		/// 废弃的函数。主要作用是1、创建路网并初始化；2、将窗体传递给动画重绘服务，用于每次仿真更新
		/// </summary>
		/// <param name="frMain">用作GDI+每次绘制仿真动画的画布</param>
		internal static void ConfigSimEnvironment(Control frMain)
		{
			
			IAbstractFactory iabstractFacotry = new TrafficEntityFactory();
			
			int iBase = 2;
			RoadNode rnA= iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase,20)),EntityType.RoadNode) as RoadNode;
			RoadNode rnB = iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase + iRoadWidth, 20)), EntityType.RoadNode) as RoadNode;
			RoadNode rnC = iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase + 2 * iRoadWidth, 20)), EntityType.RoadNode) as RoadNode;
			RoadNode rnD = iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase, 70)), EntityType.RoadNode) as RoadNode;
			RoadNode rnE = iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase + iRoadWidth, 70)), EntityType.RoadNode) as RoadNode;
			RoadNode rnF = iabstractFacotry.BuildEntity(new  RoadNodeBuildCommand(new Point(iBase + 2 * iRoadWidth, 70)), EntityType.RoadNode) as RoadNode;
			RoadNode rnG = iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase, 120)), EntityType.RoadNode) as RoadNode;
			RoadNode rnH = iabstractFacotry.BuildEntity(new  RoadNodeBuildCommand(new Point(iBase + iRoadWidth, 120)), EntityType.RoadNode) as RoadNode;
			RoadNode rnI = iabstractFacotry.BuildEntity(new  RoadNodeBuildCommand(new Point(iBase + 2 * iRoadWidth, 120)), EntityType.RoadNode) as RoadNode;

			if (iroadNetwork ==null)
			{
				iroadNetwork = ISCtx.NetWork;
			}
			iroadNetwork.AddRoadNode(rnA);
			iroadNetwork.AddRoadNode(rnB);
			iroadNetwork.AddRoadNode(rnC);
			iroadNetwork.AddRoadNode(rnD);
			iroadNetwork.AddRoadNode(rnE);
			iroadNetwork.AddRoadNode(rnF);
			iroadNetwork.AddRoadNode(rnG);
			iroadNetwork.AddRoadNode(rnH);
			iroadNetwork.AddRoadNode(rnI);

			SimController.ReA= iroadNetwork.AddRoadEdge(rnA,rnB);
			SimController.ReB=iroadNetwork.AddRoadEdge(rnB,rnC);
			iroadNetwork.AddRoadEdge(rnB, rnA);
			//
			iroadNetwork.AddRoadEdge(rnC,rnB);

			iroadNetwork.AddRoadEdge(rnD,rnE);
			iroadNetwork.AddRoadEdge(rnE,rnD);
			
			iroadNetwork.AddRoadEdge(rnE,rnF);
			iroadNetwork.AddRoadEdge(rnF,rnE);
			
			iroadNetwork.AddRoadEdge(rnG,rnH);
			iroadNetwork.AddRoadEdge(rnH,rnG);
			iroadNetwork.AddRoadEdge(rnH,rnI);
			iroadNetwork.AddRoadEdge(rnI,rnH);
			
			iroadNetwork.AddRoadEdge(rnA,rnD);
			iroadNetwork.AddRoadEdge(rnD,rnA);
			
			iroadNetwork.AddRoadEdge(rnB,rnE);
			iroadNetwork.AddRoadEdge(rnE,rnB);
			
			iroadNetwork.AddRoadEdge(rnC,rnF);
			iroadNetwork.AddRoadEdge(rnF,rnC);
			
			iroadNetwork.AddRoadEdge(rnD,rnG);
			iroadNetwork.AddRoadEdge(rnG,rnD);
			
			iroadNetwork.AddRoadEdge(rnE,rnH);
			iroadNetwork.AddRoadEdge(rnH,rnE);
			
			iroadNetwork.AddRoadEdge(rnF,rnI);
			iroadNetwork.AddRoadEdge(rnI,rnF);

			foreach (var item in iroadNetwork.RoadEdges)
			{
				RoadEdgeFacory.BuildTwoWay(item, 1, 1, 1);
			}

			//iroadNetwork.UpdateCompleted +=  new UpdateHandler(RepaintNetWork);

			iroadNetwork.UpdateCompleted+=RepaintNetWork;
			roadEdgePaintService = PainterManager.GetService(PaintServiceType.RoadEdge, frMain);
			roadNodePaintService = PainterManager.GetService(PaintServiceType.RoadNode, frMain);

		}

		


		internal  static void InitializePaintService(Control frMain)
		{
			
			//            iroadNetwork.UpdateCompleted +=  new UpdateHandler(RepaintNetWork);
			//C#的新语法？
			if (iroadNetwork ==null) {
				iroadNetwork = SimController.ISCtx.NetWork;
			}
			iroadNetwork.UpdateCompleted +=RepaintNetWork;

			roadEdgePaintService = PainterManager.GetService(PaintServiceType.RoadEdge, frMain);
			roadNodePaintService = PainterManager.GetService(PaintServiceType.RoadNode, frMain);

		}
		
		private static IAbstractFactory AddSignalGroup(IAbstractFactory iaf, RoadNode rnE)
		{
			iaf = (new AgentFactory()) as IAbstractFactory;
			//添加信号灯规则
			rnE.AcceptAsynAgent(iaf.BuildAgent(null, AgentType.SignalLightAgent));

			//信号灯赋值
			iaf = new TrafficEntityFactory();
			SignalLight sl = iaf.BuildEntity(null, EntityType.SignalLight) as SignalLight;

			foreach (RoadNode item in iroadNetwork.RoadNodes)
			{
				foreach (RoadEdge roadEdge in item.RoadEdges)
				{
					roadEdge.GetReverse().ModifySignalGroup(sl, LaneType.Straight);
				}
			}
			return iaf;
		}

		/// <summary>
		/// 仿真的每个时间步骤，重新绘制道路网。
		/// </summary>
		static void RepaintNetWork()
		{
			foreach (var item in iroadNetwork.RoadEdges)
			{
				if (bIsExit == true || bIsPause == true)
				{
					break;
				}
				roadEdgePaintService.Perform(item);
			}

			foreach (var item in iroadNetwork.RoadNodes)
			{
				if (bIsExit == true || bIsPause == true)
				{
					break;
				}
				roadNodePaintService.Perform(item);
			}
		}

		/// <summary>
		/// 添加数据记录服务
		/// </summary>
		/// <param name="frMain"></param>
		/// <param name="irn"></param>
		private static void AttachRecordService()
		{
			ISimContext isc = SimContext.GetInstance();

			IService ils = new DataRecordService(isc);
			ils.IsRunning = true;

			foreach (var item in iroadNetwork.RoadEdges)
			{
				foreach (var lane in item.Lanes)
				{
					lane.AddService(ils);
				}
			}
			foreach (var item in iroadNetwork.RoadNodes)
			{
				item.AddService(ils);
			}

		}

		
		public static void Start()
		{
			AttachRecordService();
			
			while (true) {
				//t退出命令或者仿真到了设定的仿真时长
				if (bIsExit == true||SimContext.iCurrTimeStep>= iSimTimeSteps)
				{
					OnSimulateOver(null,null);
					break;
				}

				//线程休眠减少资源消耗
				Thread.Sleep(iSimInterval);
				//处理应用程序界面事件。如点击鼠标、点击菜单等
				Application.DoEvents();
				
				if (bIsPause==false) {//如果没有暂停
					while (SimContext.iCurrTimeStep++ <= iSimTimeSteps)
					{						
						strSimMsg = SimContext.iCurrTimeStep.ToString();
						
						if (bIsExit==true||bIsPause  == true)//退出或者暂停都停止循环
						{
							break;
						}
						
						Thread.Sleep(iSimInterval);
						Application.DoEvents();

						if (--iCarCount > 0)
						{
							int iLane = 1;// iCarCount % 3;

							EdgeRoute er = new EdgeRoute();
							er.Add(SimController.ReA);
							//er.Add(SimController.ReB);

							RoadLane rl = SimController.ReA.Lanes[iLane];
							rl.EnterWaitedQueue(CarSimulator.MakeCell(er));
						}
						
						IRoadNetWork irn = RoadNetWork.GetInstance();
						//先更新item然后更新RoadNodes
						foreach (RoadNode item in irn.RoadNodes)
						{
							if (bIsExit == true || bIsPause == true)
							{
								break;
							}
							item.UpdateStatus();
						}
						//更新roadEdge
						foreach (RoadEdge item in irn.RoadEdges)
						{
							if (bIsExit == true || bIsPause == true)
							{
								break;
							}
							item.UpdateStatus();//首先更新自己。利用访问者模型是外在驱动Cell的一种思想
						}
						//路网仿真时间计数器更新
						irn.iCurrTimeStep = SimContext.iCurrTimeStep;
					}
				}
				
			}
		}
	}
	
}

