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

				Way re = tVar as Way;
				if (re != null)
				{
					re.EntityType = EntityType.Way;
					re.Grid = new Point(0,0);
					//ֱ����������£�ʹ�ö˵㳤��������·�εĳ���
					
					re.Container = RoadNet.GetInstance();

					//�����˵��Ѿ�ע�������²�����ע��
					if (ISimCtx.RoadNet.FindXNode(re.XNodeTo) != null
					    && ISimCtx.RoadNet.FindXNode(re.XNodeFrom) != null)
					{
						if (ISimCtx.RoadNet.htWays.ContainsKey(re.GetHashCode())==false)
						{
							ISimCtx.RoadNet.htWays.Add(re.GetHashCode(), re);
						}
					}
					else
					{
						throw new System.Exception("roadedge�������˵�û��ע��");
					}
					return;
				}

				XNode rn = tVar as XNode;///��·�ڵ��ע�����Ƚ����⣬

				if (rn != null)
				{
					rn.EntityType = EntityType.XNode;
					rn.iLength = SimSettings.iMaxLanes * 2;//2���ľ���
					rn.iWidth = rn.iLength;//������ȵľ���

					rn.Container = RoadNet.GetInstance();

					///�ڲ��ڽӱ�ʹ����roadnodelist����Ҫ����ע��
					//(SimCtx.NetWork as IRoadNetWork).AddRoadNode(rn);
					return;
				}
				Lane rl = tVar as Lane;
				if (rl != null)
				{
					rl.EntityType = EntityType.Lane;
					rl.Grid = new Point(0, 0);
					//rl.Container =
					//���RoadEdge�Ƿ�����ע����
					Way roadE = ISimCtx.RoadNet.FindWay(rl.Container.GetHashCode());//.From, rl.Container.To);
					if (roadE != null)
					{
						ISimCtx.RoadNet.htLanes.Add(rl.GetHashCode(), rl);
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
					sg.Grid = new Point(0, 0);
					ISimCtx.SignalLights.Add(sg.GetHashCode(), sg);
					return;
				}
				VMSEntity ve = tVar as VMSEntity;
				if (ve != null)
				{
					ve.EntityType = EntityType.VMSEntity;
					ve.Grid = new Point(0, 0);
					ISimCtx.VMSEntities.Add(sg.GetHashCode(), ve);
					return;
				}

				
				SmallCar cm = tVar as SmallCar;
				if (cm != null)
				{
					cm.EntityType = EntityType.SmallCar;
					cm.Grid = new Point(0,0);

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

				Way re = tVar as Way;//��ע��
				if (re != null)
				{
					///�����˵��Ѿ�ע�������²�����ע��
					XNode from = isc.RoadNet.FindXNode(re.XNodeFrom);
					XNode to = isc.RoadNet.FindXNode(re.XNodeTo);
					if ( from!=null&& to != null)
					{
						foreach (var lane in re.Lanes)//��ע����ڲ�����lanes
						{
							lane.UnRegiser();
						}//Ȼ��ע����Լ�
						isc.RoadNet.htWays.Remove(re.GetHashCode());
					}
					return;
				}
				XNode rn = tVar as XNode;//�ڲ�ʹ���ڽӾ���ע��
				if (rn != null)
				{
					isc.RoadNet.htXNodes.Remove(rn.GetHashCode()); return;
				}
				
				Lane rl = tVar as Lane;
				if (rl != null)
				{
					isc.RoadNet.htLanes.Remove(rl.GetHashCode());
					return;
				}

				SmallCar cm = tVar as SmallCar;
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

