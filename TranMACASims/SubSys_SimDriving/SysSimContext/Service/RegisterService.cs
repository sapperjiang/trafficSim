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
		public static new bool IsServiceUp = true;//服务运行的开关变量,系统关键服务不应当停止
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
					//直角坐标情况下，使用端点长度来衡量路段的长度
					
					re.Container = RoadNet.GetInstance();

					//两个端点已经注册的情况下才允许注册
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
						throw new System.Exception("roadedge的两个端点没有注册");
					}
					return;
				}

				XNode rn = tVar as XNode;///道路节点的注册服务比较特殊，

				if (rn != null)
				{
					rn.EntityType = EntityType.XNode;
					rn.iLength = SimSettings.iMaxLanes * 2;//2倍的矩形
					rn.iWidth = rn.iLength;//长宽相等的矩形

					rn.Container = RoadNet.GetInstance();

					///内部邻接表使用了roadnodelist不需要二次注册
					//(SimCtx.NetWork as IRoadNetWork).AddRoadNode(rn);
					return;
				}
				Lane rl = tVar as Lane;
				if (rl != null)
				{
					rl.EntityType = EntityType.Lane;
					rl.Grid = new Point(0, 0);
					//rl.Container =
					//检查RoadEdge是否正常注册了
					Way roadE = ISimCtx.RoadNet.FindWay(rl.Container.GetHashCode());//.From, rl.Container.To);
					if (roadE != null)
					{
						ISimCtx.RoadNet.htLanes.Add(rl.GetHashCode(), rl);
					}
					else
					{
						ThrowHelper.ThrowArgumentException("父类型没有注册");
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

				ThrowHelper.ThrowArgumentException("无法识别的类型，没有注册");

				#endregion
			}
			
		}

		protected override void SubRevoke(ITrafficEntity tVar)
		{
			if (RegisterService.IsServiceUp ==true)
			{
				#region

				ISimContext isc = SimContext.GetInstance();

				Way re = tVar as Way;//反注册
				if (re != null)
				{
					///两个端点已经注册的情况下才允许反注册
					XNode from = isc.RoadNet.FindXNode(re.XNodeFrom);
					XNode to = isc.RoadNet.FindXNode(re.XNodeTo);
					if ( from!=null&& to != null)
					{
						foreach (var lane in re.Lanes)//反注册掉内部所有lanes
						{
							lane.UnRegiser();
						}//然后反注册掉自己
						isc.RoadNet.htWays.Remove(re.GetHashCode());
					}
					return;
				}
				XNode rn = tVar as XNode;//内部使用邻接矩阵反注册
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
				throw new System.Exception("无法识别的类型，没有注册");
				#endregion
			}
		}
		
	}
	
}

