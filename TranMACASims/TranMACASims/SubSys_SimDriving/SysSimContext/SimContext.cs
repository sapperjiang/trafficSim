using System.Threading;
using SubSys_SimDriving.TrafficModel;
using System.Collections.Generic;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving;

namespace SubSys_SimDriving.SysSimContext
{

    /// <summary>
    /// 单例模式
    /// </summary>
    public sealed class SimContext
    {
        /// <summary>
        ///单例模式 防止直接调用接口生成该类别
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
            /// 交叉口的最大元胞数，等于Roadedge内部可以容纳的最大车道数量
            /// </summary>
            internal static int iMaxWidth = 6;
            /// <summary>
            /// 元胞车辆的宽度是6
            /// </summary>
            internal static int iCellWidth = 6;

            /// <summary>
            /// 测试用的路段的宽度
            /// </summary>
            internal static int iWidth4Test = 120;

            internal static int iSafeHeadWay = 1;
        }

        internal static int iCurrTimeStep=0;

        /// <summary>
        /// 表示运行时建立的路网的结构，里面包含了对RoadNodeList和RoadEdgeList的引用
        /// </summary>
        internal IRoadNetWork INetWork                              = RoadNetWork.GetInstance() as IRoadNetWork;

        internal SpeedLimitHashTable SpeedLimitList                    = new SpeedLimitHashTable();

        internal VMSHashTable VMSList                                  = new VMSHashTable();

        internal CarModelHashTable CarModelList   =new CarModelHashTable();
        internal SignalLightHashTable SignalLightList                = new SignalLightHashTable();
        /// <summary>
        /// 记录态哈希表
        /// </summary>
        internal RecordStatusRoadSegHashTable RecordStatusRoadSegList  = new RecordStatusRoadSegHashTable();
       
        //运行态哈希表不需要，因为路网模型中中已经有了
        ///// <summary>
        ///// 运行态哈希表
        ///// </summary>
        //internal RunningStatusRoadSegHashTable RunningStatusRoadSegList = new RunningStatusRoadSegHashTable();

        internal UpdateAgentHashTable UpdateAgentList                     = new UpdateAgentHashTable();

        /// <summary>
        /// 获取所有的车道是否有必要，因为该部分已经存在了RoadEdge中了
        /// </summary>
        internal RoadLaneHashTable RoadLaneList                         = new RoadLaneHashTable();
    }
}
 
