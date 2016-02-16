using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Service
{
    public class DataRecordService : Service
    {
        //public static bool IsServiceUp = true;
        ISimContext sc;
        private DataRecordService()
        { }
        public DataRecordService(ISimContext isc)
        {
            sc = isc;
        }

        protected override void SubPerform(ITrafficEntity tVar)
        {
            switch (tVar.EntityType)
            {
                case EntityType.Lane://附加到车道上，如车道收集器

//                    foreach (var item in tVar as Lane)
//                    {
//                         //Cell ce = (Cell)tVar;元胞，然后是其容器
//                 //       sc.DataRecorder.Record(item.Container.GetHashCode(), item.GetCarInfo());
//                 
//                 
//                 
//                    }
                    break;

                case EntityType.XNode:

//                    foreach (var item in tVar as XNode)
//                    {
//                      //  sc.DataRecorder.Record(item.Container.GetHashCode(), item.GetCarInfo());
//                    }
                    break;
                default:
                    ThrowHelper.ThrowArgumentException("不支持的记录类型，应当使用在车道和交叉口上");
                    break;
            }
        }

        [System.Obsolete("Datarecorder 的ulog方法没有实现 ")]
        protected override void SubRevoke(ITrafficEntity tVar)
        {
            throw new System.NotImplementedException();
        }
    }
      
}
 
