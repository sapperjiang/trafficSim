using System.Drawing;
using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_MathUtility;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.SysSimContext.Service
{
    internal class RegisterService : Service
    {
        internal RegisterService()
        {
            this.IsRunning = true;
        }
        public static new bool IsServiceUp = true;//�������еĿ��ر���,ϵͳ�ؼ�����Ӧ��ֹͣ
        protected override void SubPerform(ITrafficEntity tVar)
        {
            if (RegisterService.IsServiceUp ==true)
            {
                ISimContext ISimCtx = SimContext.GetInstance();
                   #region

                RoadEdge re = tVar as RoadEdge;
                if (re != null)
                {
                    re.EntityType = EntityType.RoadEdge;
                    re.RelativePosition = new Point(0,0);
                    ///ֱ����������£�ʹ�ö˵㳤��������·�εĳ���
                   
                    re.Container = RoadNetWork.GetInstance();

                    ///�����˵��Ѿ�ע�������²�����ע��
                    if (ISimCtx.NetWork.FindRoadNode(re.roadNodeTo) != null
                        && ISimCtx.NetWork.FindRoadNode(re.roadNodeFrom) != null)
                    {
                        ISimCtx.NetWork.RoadEdgeList.Add(re.GetHashCode(), re);
                    }
                    else
                    {
                        throw new System.Exception("roadedge�������˵�û��ע��");
                    }
                    return;
                }

                RoadNode rn = tVar as RoadNode;///��·�ڵ��ע�����Ƚ����⣬

                if (rn != null)
                {
                    rn.EntityType = EntityType.RoadNode;
                    rn.iLength = SimSettings.iMaxLanes * 2;//2���ľ���
                    rn.iWidth = rn.iLength;//������ȵľ���

                    rn.Container = RoadNetWork.GetInstance();

                    ///�ڲ��ڽӱ�ʹ����roadnodelist����Ҫ����ע��
                    //(SimCtx.NetWork as IRoadNetWork).AddRoadNode(rn);
                    return;
                }
                RoadLane rl = tVar as RoadLane;
                if (rl != null)
                {
                    rl.EntityType = EntityType.RoadLane;
                    rl.RelativePosition = new Point(0, 0);
                    //rl.Container = 
                    //���RoadEdge�Ƿ�����ע����
                    RoadEdge roadE = ISimCtx.NetWork.FindRoadEdge(rl.Container.GetHashCode());//.From, rl.Container.To);
                    if (roadE != null)
                    {
                        ISimCtx.NetWork.RoadLanes.Add(rl.GetHashCode(), rl);
                    }
                    else
                    {
                        ThrowHelper.ThrowArgumentException("������û��ע��");
                    }

                    return;
                }
            SignalLight sg = tVar as SignalLight;
            if (sg != null)
            {
                sg.EntityType = EntityType.SignalLight;
                sg.RelativePosition = new Point(0, 0);
                ISimCtx.SignalLights.Add(sg.GetHashCode(), sg);
                return;
            }
            VMSEntity ve = tVar as VMSEntity;
            if (ve != null)
            {
                ve.EntityType = EntityType.VMSEntity;
                ve.RelativePosition = new Point(0, 0);
                ISimCtx.VMSEntities.Add(sg.GetHashCode(), ve);
                return;
            }

           
            Car cm = tVar as Car;
            if (cm != null)
            {
                cm.EntityType = EntityType.CarModel;
                cm.RelativePosition = new Point(0,0);

                ISimCtx.CarModels.Add(cm.GetHashCode(), cm);
                return;
            }

            ThrowHelper.ThrowArgumentException("�޷�ʶ������ͣ�û��ע��");

            #endregion
            }
         
        }

        protected override void SubRevoke(ITrafficEntity tVar)
        {
           if (RegisterService.IsServiceUp ==true)
            {
                #region


                ISimContext isc = SimContext.GetInstance();

            RoadEdge re = tVar as RoadEdge;//��ע��
            if (re != null)
            {
                ///�����˵��Ѿ�ע�������²�����ע��
                RoadNode from = isc.NetWork.FindRoadNode(re.roadNodeFrom);
                RoadNode to = isc.NetWork.FindRoadNode(re.roadNodeTo);
                if ( from!=null&& to != null)
                {
                    foreach (var lane in re.Lanes)//��ע����ڲ�����lanes
                    {
                        lane.UnRegiser();
                    }//Ȼ��ע����Լ�
                    isc.NetWork.RoadEdgeList.Remove(re.GetHashCode());
                }
                return;
            }
            RoadNode rn = tVar as RoadNode;//�ڲ�ʹ���ڽӾ���ע��
            if (rn != null)
            {
                isc.NetWork.RoadNodeList.Remove(rn.GetHashCode()); return;
            }
               
            RoadLane rl = tVar as RoadLane;
            if (rl != null)
            {
                isc.NetWork.RoadLanes.Remove(rl.GetHashCode());
                return;
            }

            Car cm = tVar as Car;
            if (cm != null)
            {
                isc.CarModels.Remove(cm.GetHashCode());
                return;
            }
            SignalLight sg = tVar as SignalLight;
            if (sg != null)
            {
                isc.SignalLights.Remove(sg.GetHashCode());
                return;
            }
            VMSEntity ve = tVar as VMSEntity;
            if (ve != null)
            {
                isc.VMSEntities.Remove(sg.GetHashCode());
                return;
            }
            throw new System.Exception("�޷�ʶ������ͣ�û��ע��");
#endregion
            }
        }
    
    }
  
}
 
