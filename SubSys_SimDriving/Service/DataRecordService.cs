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

        protected override void SubPerform(IEntity entity)
        {
            switch (entity.EntityType)
            {
                case EntityType.Lane://附加到车道上，如车道收集器

                    foreach (var mobile in (entity as Lane).Mobiles)
                    {
                      sc.DataRecorder.Record(mobile.GetHashCode(), mobile.CurrState);
                    }
                    break;

                case EntityType.XNode:

                    foreach (var mobile in (entity as XNode).Mobiles)
                    {
                        sc.DataRecorder.Record(mobile.GetHashCode(), mobile.CurrState);
                    }
                    break;
                default:
                    ThrowHelper.ThrowArgumentException("不支持的记录类型，应当使用在车道和交叉口上");
                    break;
            }
        }

       // [System.Obsolete("Datarecorder 的ulog方法没有实现 ")]
        protected override void SubRevoke(IEntity tVar)
        {
            throw new System.NotImplementedException();
        }
    }
      
}
 
