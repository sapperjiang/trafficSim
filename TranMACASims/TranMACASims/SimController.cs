
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

namespace SubSys_SimDriving
{
    internal static class SimController
    {
        internal static ISimContext ISCtx = SimContext.GetInstance();
        internal static IService roadEdgePaintService; //= PaintServiceMgr.GetService(PaintServiceType.RoadEdge, frMain);
        internal static IService roadNodePaintService; //= PaintServiceMgr.GetService(PaintServiceType.RoadEdge, frMain);
        internal static IRoadNetWork iroadNetwork;//= simContext.NetWork;

        internal static bool bIsExit = false;
        internal static int iRoadWidth = 500;//1000m

        internal static string strSimMsg = null;

        internal static int iSimInterval = 1000;
        internal static int iSimTimeSteps = 420;
        internal static bool IsPause = false;
        internal static int iCarCount = 2;

        internal static RoadEdge ReA;
        internal static RoadEdge ReB;
        
       /// <summary>
       /// �����ĺ�������Ҫ������1������·������ʼ����2�������崫�ݸ������ػ��������ÿ�η������
       /// </summary>
       /// <param name="frMain">����GDI+ÿ�λ��Ʒ��涯���Ļ���</param>
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

            iroadNetwork.UpdateCompleted +=  new UpdateHandler(RepaintNetWork);

            roadEdgePaintService = PaintServiceMgr.GetService(PaintServiceType.RoadEdge, frMain);
            roadNodePaintService = PaintServiceMgr.GetService(PaintServiceType.RoadNode, frMain);

        }

//               internal static void SimulationInitialize()
//        {
//  
//        	IAbstractFactory iabstractFacotry = new TrafficEntityFactory();
//            
//            int iBase = 2;
//            RoadNode rnA= iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase,20)),EntityType.RoadNode) as RoadNode;
//            RoadNode rnB = iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase + iRoadWidth, 20)), EntityType.RoadNode) as RoadNode;
//            RoadNode rnC = iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase + 2 * iRoadWidth, 20)), EntityType.RoadNode) as RoadNode;
//            RoadNode rnD = iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase, 70)), EntityType.RoadNode) as RoadNode;
//            RoadNode rnE = iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase + iRoadWidth, 70)), EntityType.RoadNode) as RoadNode;
//            RoadNode rnF = iabstractFacotry.BuildEntity(new  RoadNodeBuildCommand(new Point(iBase + 2 * iRoadWidth, 70)), EntityType.RoadNode) as RoadNode;
//            RoadNode rnG = iabstractFacotry.BuildEntity(new RoadNodeBuildCommand(new Point(iBase, 120)), EntityType.RoadNode) as RoadNode;
//            RoadNode rnH = iabstractFacotry.BuildEntity(new  RoadNodeBuildCommand(new Point(iBase + iRoadWidth, 120)), EntityType.RoadNode) as RoadNode;
//            RoadNode rnI = iabstractFacotry.BuildEntity(new  RoadNodeBuildCommand(new Point(iBase + 2 * iRoadWidth, 120)), EntityType.RoadNode) as RoadNode;
//
//            if (roadNetwork ==null)
//            {
//                roadNetwork = ISCtx.NetWork;
//            }
//            roadNetwork.AddRoadNode(rnA);
//            roadNetwork.AddRoadNode(rnB);
//            roadNetwork.AddRoadNode(rnC);
//            roadNetwork.AddRoadNode(rnD);
//            roadNetwork.AddRoadNode(rnE);
//            roadNetwork.AddRoadNode(rnF);
//            roadNetwork.AddRoadNode(rnG);
//            roadNetwork.AddRoadNode(rnH);
//            roadNetwork.AddRoadNode(rnI);
//
//            SimController.ReA= roadNetwork.AddRoadEdge(rnA,rnB);
//            SimController.ReB=roadNetwork.AddRoadEdge(rnB,rnC);
//            roadNetwork.AddRoadEdge(rnB, rnA);
//            //
//            roadNetwork.AddRoadEdge(rnC,rnB);
//
//            roadNetwork.AddRoadEdge(rnD,rnE);
//            roadNetwork.AddRoadEdge(rnE,rnD);
//            
//            roadNetwork.AddRoadEdge(rnE,rnF);
//            roadNetwork.AddRoadEdge(rnF,rnE);
//		    
//            roadNetwork.AddRoadEdge(rnG,rnH);
//            roadNetwork.AddRoadEdge(rnH,rnG);
//            roadNetwork.AddRoadEdge(rnH,rnI);
//            roadNetwork.AddRoadEdge(rnI,rnH);
//		    
//            roadNetwork.AddRoadEdge(rnA,rnD);
//            roadNetwork.AddRoadEdge(rnD,rnA);
//		    
//            roadNetwork.AddRoadEdge(rnB,rnE);
//            roadNetwork.AddRoadEdge(rnE,rnB);
//		    
//            roadNetwork.AddRoadEdge(rnC,rnF);
//            roadNetwork.AddRoadEdge(rnF,rnC);
//		    
//            roadNetwork.AddRoadEdge(rnD,rnG);
//            roadNetwork.AddRoadEdge(rnG,rnD);
//		    
//            roadNetwork.AddRoadEdge(rnE,rnH);
//            roadNetwork.AddRoadEdge(rnH,rnE);
//		    
//            roadNetwork.AddRoadEdge(rnF,rnI);
//            roadNetwork.AddRoadEdge(rnI,rnF);
//
//            foreach (var item in roadNetwork.RoadEdges)
//            {
//                RoadEdgeFacory.BuildTwoWay(item, 1, 1, 1);
//            }
//
//            roadNetwork.UpdateCompleted +=  new UpdateHandler(RepaintNetWork);
//
//            ipsRoadEdge = PaintServiceMgr.GetService(PaintServiceType.RoadEdge, frMain);
//            ipsRoadNode = PaintServiceMgr.GetService(PaintServiceType.RoadNode, frMain);
//
//        }

           internal static void PaintServiceInitialize(Control frMain)
        {
  
//            iroadNetwork.UpdateCompleted +=  new UpdateHandler(RepaintNetWork);
//C#�����﷨��
            iroadNetwork.UpdateCompleted +=RepaintNetWork;

            roadEdgePaintService = PaintServiceMgr.GetService(PaintServiceType.RoadEdge, frMain);
            roadNodePaintService = PaintServiceMgr.GetService(PaintServiceType.RoadNode, frMain);

        }
        
        private static IAbstractFactory AddSignalGroup(IAbstractFactory iaf, RoadNode rnE)
        {
            iaf = (new AgentFactory()) as IAbstractFactory;
            //����źŵƹ���
            rnE.AcceptAsynAgent(iaf.BuildAgent(null, AgentType.SignalLightAgent));

            //�źŵƸ�ֵ
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
        /// �����ÿ��ʱ�䲽�裬���»��Ƶ�·����
        /// </summary>
        static void RepaintNetWork()
        {
            foreach (var item in iroadNetwork.RoadEdges)
            {
                if (bIsExit == true || IsPause == true)
                {
                    break;
                }
                roadEdgePaintService.Perform(item);
            }

            foreach (var item in iroadNetwork.RoadNodes)
            {
                if (bIsExit == true || IsPause == true)
                {
                    break;
                }
                roadNodePaintService.Perform(item);
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
            while (SimContext.iCurrTimeStep++ <= iSimTimeSteps)
            {
                Thread.Sleep(iSimInterval);

                strSimMsg = SimContext.iCurrTimeStep.ToString();

                if (bIsExit == true || IsPause==true)
                {
                    break;
                }
                Application.DoEvents();

                iCarCount--;

                if (iCarCount > 0)
                {
                    int iLane = 1;// iCarCount % 3;

                    EdgeRoute er = new EdgeRoute();
                    er.Add(SimController.ReA);
                    //er.Add(SimController.ReB);

                    RoadLane rl = SimController.ReA.Lanes[iLane];
                    rl.EnterWaitedQueue(CarSimulator.MakeCell(er));

                }
 
                IRoadNetWork irn = RoadNetWork.GetInstance();
                //�ȸ���itemȻ�����RoadNodes
                foreach (RoadNode item in irn.RoadNodes)
                {
                    if (bIsExit == true || IsPause == true)
                    {
                        break;
                    }
                    item.UpdateStatus();
                }
                //����roadEdge 
                foreach (RoadEdge item in irn.RoadEdges)
                {
                    if (bIsExit == true || IsPause == true)
                    {
                        break;
                    }
                    item.UpdateStatus();//���ȸ����Լ������÷�����ģ������������Cell��һ��˼��
                }
              
                irn.iCurrTimeStep = SimContext.iCurrTimeStep;
            }
        }
    }
	 
}
 
