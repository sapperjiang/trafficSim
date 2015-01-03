using System.Threading;
using SubSys_SimDriving.TrafficModel;
using System.Collections.Generic;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving;

namespace SubSys_SimDriving.SysSimContext
{

    /// <summary>
    /// ����ģʽ
    /// </summary>
    public sealed class SimContext
    {
        /// <summary>
        ///����ģʽ ��ֱֹ�ӵ��ýӿ����ɸ����
        /// </summary>
        private SimContext() { }

        private static SimContext _simContext;

        internal static SimContext GetInstance()
        {
            if (_simContext == null)
            {
                Mutex mutext = new Mutex();
                mutext.WaitOne();
                _simContext = new SimContext();
                mutext.Close();
                mutext = null;
            }
            return _simContext;
        }

        internal class SimSettings
        {
            /// <summary>
            /// ����ڵ����Ԫ����������Roadedge�ڲ��������ɵ���󳵵�����
            /// </summary>
            internal static int iMaxWidth = 6;
            /// <summary>
            /// Ԫ�������Ŀ����6
            /// </summary>
            internal static int iCellWidth = 6;

            /// <summary>
            /// �����õ�·�εĿ��
            /// </summary>
            internal static int iWidth4Test = 120;

            internal static int iSafeHeadWay = 1;
        }

        internal static int iCurrTimeStep=0;

        /// <summary>
        /// ��ʾ����ʱ������·���Ľṹ����������˶�RoadNodeList��RoadEdgeList������
        /// </summary>
        internal IRoadNetWork INetWork                              = RoadNetWork.GetInstance() as IRoadNetWork;

        internal SpeedLimitHashTable SpeedLimitList                    = new SpeedLimitHashTable();

        internal VMSHashTable VMSList                                  = new VMSHashTable();

        internal CarModelHashTable CarModelList   =new CarModelHashTable();
        internal SignalLightHashTable SignalLightList                = new SignalLightHashTable();
        /// <summary>
        /// ��¼̬��ϣ��
        /// </summary>
        internal RecordStatusRoadSegHashTable RecordStatusRoadSegList  = new RecordStatusRoadSegHashTable();
       
        //����̬��ϣ����Ҫ����Ϊ·��ģ�������Ѿ�����
        ///// <summary>
        ///// ����̬��ϣ��
        ///// </summary>
        //internal RunningStatusRoadSegHashTable RunningStatusRoadSegList = new RunningStatusRoadSegHashTable();

        internal UpdateAgentHashTable UpdateAgentList                     = new UpdateAgentHashTable();

        /// <summary>
        /// ��ȡ���еĳ����Ƿ��б�Ҫ����Ϊ�ò����Ѿ�������RoadEdge����
        /// </summary>
        internal RoadLaneHashTable RoadLaneList                         = new RoadLaneHashTable();
    }
}
 
