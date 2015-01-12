
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SubSys_DataManage;
using SubSys_Graphics;
using SubSys_SimDriving.Agents;

using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.SysSimContext.Service;
using SubSys_SimDriving.TrafficModel;

using GISTranSim.DataOutput;

namespace SubSys_SimDriving
{
	internal static class SimController
	{
		internal static event EventHandler OnSimulateStoped ;
		
		internal static ISimContext ISimCtx = SimContext.GetInstance();
		
		internal static IService WayPainter; //= PaintServiceMgr.GetService(PaintServiceType.RoadEdge, frMain);
		internal static IService xNodePainter; //= PaintServiceMgr.GetService(PaintServiceType.RoadEdge, frMain);
		
		internal static IRoadNet iroadNet;//= simContext.NetWork;

		internal static bool bIsExit = false;
		internal static int iRoadWidth = 500;//1000m

		internal static string strSimMsg = null;

		internal static int iSimInterval = 1000;
		internal static int iSimTimeSteps = 420;
		internal static bool bIsPause = false;
		internal static int iCarCount = 2;

		internal static Way ReA;
		internal static Way ReB;
		

		internal  static void InitializePainters(Control frMain)
		{
			if (iroadNet ==null) {
				iroadNet = SimController.ISimCtx.RoadNet;
			}
			iroadNet.Updated +=RepaintNetWork;

			WayPainter = PainterManager.GetService(PaintServiceType.Way, frMain);
			xNodePainter = PainterManager.GetService(PaintServiceType.XNode, frMain);
		}
		
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
		static void RepaintNetWork()
		{
			foreach (var item in iroadNet.Ways)
			{
				if (bIsExit == true || bIsPause == true)
				{
					break;
				}
				WayPainter.Perform(item);
			}

			foreach (var item in iroadNet.XNodes)
			{
				if (bIsExit == true || bIsPause == true)
				{
					break;
				}
				xNodePainter.Perform(item);
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

			foreach (var item in iroadNet.Ways)
			{
				foreach (var lane in item.Lanes)
				{
					lane.AddService(ils);
				}
			}
			foreach (var item in iroadNet.XNodes)
			{
				item.AddService(ils);
			}

		}

		
		/// <summary>
		/// 仿真运行的主循环
		/// </summary>
		public static void Run()
		{
			//数据记录服务
			AttachRecordService();
			
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
				Thread.Sleep(iSimInterval);
				//处理应用程序界面事件。如点击鼠标、点击菜单等
				Application.DoEvents();
				
				if (bIsPause==false) {//如果没有暂停
					while (ISimCtx.iCurrTimeStep++ <= iSimTimeSteps)
					{
						strSimMsg = ISimCtx.iCurrTimeStep.ToString();
						
						if (bIsExit==true||bIsPause  == true)//退出或者暂停都停止循环
						{
							break;
						}
						
						Thread.Sleep(iSimInterval);
						Application.DoEvents();

						//下面这段代码不知道干嘛的
						if (--iCarCount > 0)
						{
							int iLane = 1;// iCarCount % 3;

							EdgeRoute er = new EdgeRoute();
							er.Add(SimController.ReA);
							//er.Add(SimController.ReB);

							Lane rl = SimController.ReA.Lanes[iLane];
							rl.EnterWaitedQueue(SubSys_SimDriving.TrafficModel.CarSimulator.MakeCell(er));
						}
						
						IRoadNet irn =SimController.ISimCtx.RoadNet;
						//先更新item然后更新RoadNodes
						foreach (XNode item in irn.XNodes)
						{
							if (bIsExit == true || bIsPause == true)
							{
								break;
							}
							item.UpdateStatus();
						}
						//更新roadEdge
						foreach (Way item in irn.Ways)
						{
							if (bIsExit == true || bIsPause == true)
							{
								break;
							}
							//首先更新自己。利用访问者模型是外在驱动Cell的一种思想
							item.UpdateStatus();
						}
						//路网仿真时间计数器更新
						irn.iCurrTimeStep = ISimCtx.iCurrTimeStep;
					}
				}
				
			}
		}
	}
	
}

