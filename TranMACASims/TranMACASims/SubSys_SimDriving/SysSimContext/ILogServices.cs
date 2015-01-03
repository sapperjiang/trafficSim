using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.SysSimContext
{
    /// <summary>
    /// 观察者接口
    /// </summary>
    internal interface ILogService  
	{
        void Log(TrafficEntity tVar);
        void UnLog(TrafficEntity tVar);
    }

    internal abstract class Logger : ILogService
    {
        //private LogObjectDispacher LogOBJDispacher=null;

        //protected void AppendDispather(LogObjectDispacher dispatcher)
        //{
            
        //}
        //{
        //    //LogOBJDispacher.Dispatch(tVar);
        //}
        //void Log(TrafficEntity tVar)
        //{

        //}

        //void ILogService.UnLog(TrafficEntity tVar)
        //{

        //}
        public abstract void  Log(TrafficEntity tVar);

        public abstract void  UnLog(TrafficEntity tVar);
    }

    internal class RegisterLogger : Logger
    {
        SimContext simContext = SimContext.GetInstance();

        public override void Log(TrafficEntity tVar)
        {
            SignalLight sg = tVar as SignalLight;
            if (sg != null)
            {
                simContext.SignalLightList.Add(sg.GetHashCode(), sg);
            }
            VMSEntity ve = tVar as VMSEntity;
            if (ve != null)
            {
                simContext.VMSList.Add(sg.GetHashCode(),ve);
            }
            RoadEdge re = tVar as RoadEdge;
            if (re != null)
            {
                simContext.INetWork.AddRoadEdge(re);
            }
            RoadNode rn = tVar as RoadNode;
            if (rn != null)
            {
                simContext.INetWork.AddRoadNode(rn);
            }
            RoadLane rl = tVar as RoadLane;
            if (rl != null)
            {
                simContext.RoadLaneList.Add(rl.GetHashCode(),rl);
            }
            CarModel cm = tVar as CarModel;
            if (cm != null)
            {
                simContext.CarModelList.Add(cm.GetHashCode(), cm);
            }
            //Cell ce = tVar as Cell;
            //if (ce != null)
            //{
            //    simContext.cell
            //}
        }

        public override void UnLog(TrafficEntity tVar)
        {
            SignalLight sg = tVar as SignalLight;
            if (sg != null)
            {
                simContext.SignalLightList.Remove(sg.GetHashCode());
            }
            VMSEntity ve = tVar as VMSEntity;
            if (ve != null)
            {
                simContext.VMSList.Remove(sg.GetHashCode());
            }
            RoadEdge re = tVar as RoadEdge;
            if (re != null)
            {
                simContext.INetWork.RemoveRoadEdge(re.from,re.to);
            }
            RoadNode rn = tVar as RoadNode;
            if (rn != null)
            {
                simContext.INetWork.RemoveRoadNode(rn);
            }
            RoadLane rl = tVar as RoadLane;
            if (rl != null)
            {
                simContext.RoadLaneList.Remove(rl.GetHashCode());
            }
            CarModel cm = tVar as CarModel;
            if (cm != null)
            {
                simContext.CarModelList.Remove(cm.GetHashCode());
            }
        }
    }
    internal class DataRecordLogger : Logger
    {
        SimContext simContext = SimContext.GetInstance();

        public override void Log(TrafficEntity tVar)
        {

            Cell ce = tVar as Cell;
            if (ce != null)
            {
                CarModel cm = simContext.CarModelList.Get(ce.cmCarModel.GetHashCode());
                //simContext.CarModelList.Add(cm.GetHashCode(), cm);
            }
        }

        public override void UnLog(TrafficEntity tVar)
        {
            Cell ce = tVar as Cell;
            if (ce != null)
            {
                CarModel cm = simContext.CarModelList.Get(ce.cmCarModel.GetHashCode());
                //simContext.CarModelList.Add(cm.GetHashCode(), cm);
            }
        }
    }
    //internal abstract class LogObjectDispacher
    //{
    //    public abstract void Dispatch(TrafficEntity tVar);
    //}
	 
}
 
