using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.SysSimContext.Service
{
    public class DataRecordService : Service
    {
        public static bool IsServiceUp = true;//�������еĿ��ر���
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
                case EntityType.RoadLane://���ӵ������ϣ��糵���ռ���

                    foreach (var item in tVar as RoadLane)
                    {
                         //Cell ce = (Cell)tVar;Ԫ����Ȼ����������
                        sc.DataRecorder.Record(item.Container.GetHashCode(), item.GetCarInfo());
                    }
                    break;

                case EntityType.RoadNode:

                    foreach (var item in tVar as RoadNode)
                    {
                        //Cell ce = (Cell)tVar;
                        sc.DataRecorder.Record(item.Container.GetHashCode(), item.GetCarInfo());
                    }
                    break;
                default:
                    ThrowHelper.ThrowArgumentException("��֧�ֵļ�¼���ͣ�Ӧ��ʹ���ڳ����ͽ������");
                    break;
            }
            //if (tVar.EntityType == EntityType.Cell)
            //{
            //    Cell ce = (Cell)tVar;
            //    SimContext.GetInstance().DataRecorder.Record(ce.Container, ce.GetCarInfo());
            //}
        }

        [System.Obsolete("Datarecorder ��ulog����û��ʵ�� ")]
        protected override void SubRevoke(ITrafficEntity tVar)
        {
            throw new System.NotImplementedException();
        }
    }
      
}
 
