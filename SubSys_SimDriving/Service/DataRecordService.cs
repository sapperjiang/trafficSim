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
                case EntityType.Lane://���ӵ������ϣ��糵���ռ���

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
                    ThrowHelper.ThrowArgumentException("��֧�ֵļ�¼���ͣ�Ӧ��ʹ���ڳ����ͽ������");
                    break;
            }
        }

       // [System.Obsolete("Datarecorder ��ulog����û��ʵ�� ")]
        protected override void SubRevoke(IEntity tVar)
        {
            throw new System.NotImplementedException();
        }
    }
      
}
 
