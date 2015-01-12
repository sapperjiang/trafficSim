
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
		/// ������ݼ�¼����
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
		/// �������е���ѭ��
		/// </summary>
		public static void Run()
		{
			//���ݼ�¼����
			AttachRecordService();
			
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
				Thread.Sleep(iSimInterval);
				//����Ӧ�ó�������¼���������ꡢ����˵���
				Application.DoEvents();
				
				if (bIsPause==false) {//���û����ͣ
					while (ISimCtx.iCurrTimeStep++ <= iSimTimeSteps)
					{
						strSimMsg = ISimCtx.iCurrTimeStep.ToString();
						
						if (bIsExit==true||bIsPause  == true)//�˳�������ͣ��ֹͣѭ��
						{
							break;
						}
						
						Thread.Sleep(iSimInterval);
						Application.DoEvents();

						//������δ��벻֪�������
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
						//�ȸ���itemȻ�����RoadNodes
						foreach (XNode item in irn.XNodes)
						{
							if (bIsExit == true || bIsPause == true)
							{
								break;
							}
							item.UpdateStatus();
						}
						//����roadEdge
						foreach (Way item in irn.Ways)
						{
							if (bIsExit == true || bIsPause == true)
							{
								break;
							}
							//���ȸ����Լ������÷�����ģ������������Cell��һ��˼��
							item.UpdateStatus();
						}
						//·������ʱ�����������
						irn.iCurrTimeStep = ISimCtx.iCurrTimeStep;
					}
				}
				
			}
		}
	}
	
}

