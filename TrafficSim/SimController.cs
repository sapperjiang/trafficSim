
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
		
		//���·�ɱ�����-fis
		internal static Way ReA1;
		internal static Way ReA2;
		internal static Way ReA3;
		internal static Way ReA4;
		internal static Way ReB1;
		internal static Way ReB2;
		
		
		private static IFactory AddSignalGroup(IFactory ifactory, XNode xNode)
		{
			ifactory = (new AgentFactory()) as IFactory;
			//����źŵƹ���
			xNode.AcceptAsynAgent(ifactory.Build(null, AgentType.SignalLightAgent));

			//�źŵƸ�ֵ
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
		/// �����ÿ��ʱ�䲽�裬���»��Ƶ�·����
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
		/// ������ݼ�¼����
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
			//������δ��벻֪�������
			if (--SimController.iCarCount > 0)
			{
				int iLane = 0;// iCarCount % 3;
				//�½�һ��·��
				EdgeRoute route = new EdgeRoute();
				route.Add(SimController.ReA1);
				//��ӽڵ㡪fis
				route.Add(SimController.ReA2);
//							erA.Add(SimController.ReA3);
//							erA.Add(SimController.ReA4);
				//����ÿ��·������·
				Lane startLane = SimController.ReA1.Lanes[iLane];
				
				startLane.EnterInn(MobileSimulator.MakeMobile(route,startLane));
				
				//Lane r2 = SimController.ReB.Lanes[iLane];
				//Ϊÿ��ԭ��ѡ�����·��
				//	rA.EnterWaitedQueue(SubSys_SimDriving.TrafficModel.MobileSimulator.MakeCell(erA));
				//r2.EnterWaitedQueue(SubSys_SimDriving.TrafficModel.CarSimulator.MakeCell(er));
			}
		}
		
		//-----------20160131
		/// <summary>
		/// �������е���ѭ��
		/// </summary>
		public static void StartSimulate()
		{
			//���ݼ�¼����
			RegisterService();
			
			var network =SimController.ISimCtx.RoadNet;
			
			while (true) {
				//t�˳�������߷��浽���趨�ķ���ʱ��
				if (bIsExit == true||ISimCtx.iCurrTimeStep>= iSimTimeSteps)
				{
					if (OnSimulateStoped!=null) {
						OnSimulateStoped(null,null);
					}
					break;
				}

				//�߳����߼�����Դ����
				Thread.Sleep(SimController.iSimInterval);
				//����Ӧ�ó�������¼���������ꡢ����˵���
				Application.DoEvents();
				
				SimController.LoadMobiles();
				
				if (bIsPause==false) {//���û����ͣ
					while (ISimCtx.iCurrTimeStep++ <= iSimTimeSteps)
					{
						strSimMsg = ISimCtx.iCurrTimeStep.ToString();
						
						if (bIsExit==true||bIsPause  == true)//�˳�������ͣ��ֹͣѭ��
						{
							break;
						}
						
						Thread.Sleep(SimController.iSimInterval);
						Application.DoEvents();

						//����RoadNodes
						foreach (XNode item in network.XNodes)
						{
							if (bIsExit == true || bIsPause == true)
							{
								break;
							}
							item.UpdateStatus();
						}
						//����roadEdge
						foreach (Way item in network.Ways)
						{
							if (bIsExit == true || bIsPause == true)
							{
								break;
							}

							//���ȸ����Լ���
							item.UpdateStatus();
						}
						//·������ʱ�����������
						network.iCurrTimeStep = ISimCtx.iCurrTimeStep;
					}
				}
				
			}
		}
	}
	
}

