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
                case EntityType.Lane://���ӵ������ϣ��糵���ռ���

//                    foreach (var item in tVar as Lane)
//                    {
//                         //Cell ce = (Cell)tVar;Ԫ����Ȼ����������
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
                    ThrowHelper.ThrowArgumentException("��֧�ֵļ�¼���ͣ�Ӧ��ʹ���ڳ����ͽ������");
                    break;
            }
        }

        [System.Obsolete("Datarecorder ��ulog����û��ʵ�� ")]
        protected override void SubRevoke(ITrafficEntity tVar)
        {
            throw new System.NotImplementedException();
        }
    }
      
}
 
