
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
//			//����źŵƹ���
//			xNode.AcceptAsynAgent(ifactory.Build(null, AgentType.SignalLightAgent));
//
//			//�źŵƸ�ֵ
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
                //�½�һ��·��
                EdgeRoute route = new EdgeRoute();
                //����ÿ��·������·
                route.Add(way);
                var car = MobileFactory.BuildSmallCar();
				car.Route = route;
				startLane.EnterInn(car);
			}
		}
		
		//-----------20160131
		/// <summary>
		/// �������е���ѭ��
		/// </summary>
		public static void StartSimulate()
		{
			//���ݼ�¼����
			RegisterServices();

            //add mobile one by one
            SimController.LoadMobiles();

            var network =SimController.ISimCtx.RoadNet;
			while (true) {
				//t�˳�������߷��浽���趨�ķ���ʱ��
				if (bIsExit == true||ISimCtx.iTimePulse>= iSimTimeSteps)
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

				
				if (bIsPause==false) {//���û����ͣ
					while (ISimCtx.iTimePulse++ <= iSimTimeSteps)
					{
						strSimMsg = ISimCtx.iTimePulse.ToString();
						
						if (bIsExit==true||bIsPause  == true)//�˳�������ͣ��ֹͣѭ��
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
						network.iTimePulse = ISimCtx.iTimePulse;
					}
				}
				
			}
		}
	}
	
}

