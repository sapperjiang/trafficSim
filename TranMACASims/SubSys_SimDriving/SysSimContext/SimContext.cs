using System.Threading;
using SubSys_SimDriving.TrafficModel;
using System.Collections.Generic;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving;



namespace SubSys_SimDriving.SysSimContext
{
    public interface ISimContext
    {
        int iCurrTimeStep
        {
          get; //{ return ISimContext._iCurrTimeStep; }
          set; //{ ISimContext._iCurrTimeStep = value; }
        }

        RoadNetWork NetWork
        {
          get; 
        }
         SpeedLimitHTable SpeedLimits
        {
          get; 
          
        }
        VMSHashTable VMSEntities
        {
          get;// { return _VMSEntities; }
       
        }

        CarModelHTable CarModels
        {
          get;// { return _CarModels; }
        
        }
        SignalLightHTable SignalLights
        {
          get;// { return _SignalLights; }
         
        }
                /// <summary>
                /// 记录态哈希表
                /// </summary>
        EntityDic DataRecorder
        {
          get;// { return _DataRecorder; }
      
        }


        AgentHTable Agents
        {
          get; //{ return _UpdateAgents; }
      
        }

    }



    /// <summary>
    /// 单例模式，关于注册，所有注册之前应当检查是否已经注册过了
    /// 应当由roadnetwork承担一部分的工厂责任，外界不应显式的调用注册，
    /// 注册服务对外部组件是透明的，注册使用请求/拒绝模型,注册应当统一使用服务实现
    /// simContext内部所有注册数据应当对外是可读的，不可以修改的
    /// </summary>
    public sealed class SimContext:ISimContext
    {
        private static int SimContextCount = 0;
        /// <summary>
        ///单例模式 防止直接调用接口生成该类别
        /// </summary>
        private SimContext() 
        {
            SimContextCount += 1;
        }

        private static SimContext _simContext;

        public static SimContext GetInstance()
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

        public static int iCurrTimeStep=0;

      
      
        public SpeedLimitHTable _SpeedLimits                 = new SpeedLimitHTable();

        public VMSHashTable _VMSEntities                     = new VMSHashTable();

        public CarModelHTable _CarModels                     = new CarModelHTable();
        public SignalLightHTable _SignalLights                = new SignalLightHTable();
        /// <summary>
        /// 记录态哈希表
        /// </summary>
        public EntityDic _DataRecorder                       = new EntityDic();
       
        public AgentHTable _Agents                           = new AgentHTable();


        int ISimContext.iCurrTimeStep
        {
            get
            {
               return  SimContext.iCurrTimeStep;
            }
            set
            {
                SimContext.iCurrTimeStep = value;
            }
        }

        /// <summary>
        /// 表示运行时建立的路网的结构，里面包含了对RoadNodeList和RoadEdgeList的引用
        /// </summary>
        RoadNetWork ISimContext.NetWork
        {
            get
            {
                return  RoadNetWork.GetInstance();;
            }
        }

        SpeedLimitHTable ISimContext.SpeedLimits
        {
            get
            {
                return this._SpeedLimits; // throw new System.NotImplementedException();
            }
          
        }

        VMSHashTable ISimContext.VMSEntities
        {
            get
            {
                return this._VMSEntities;// throw new System.NotImplementedException();
            }
           
        }

        CarModelHTable ISimContext.CarModels
        {
            get
            {
                return this._CarModels;// throw new System.NotImplementedException();
            }
           
        }

        SignalLightHTable ISimContext.SignalLights
        {
            get
            {
                return this._SignalLights;
            }
          
        }

        EntityDic ISimContext.DataRecorder
        {
            get
            {
                return this._DataRecorder;// throw new System.NotImplementedException();
            }
        
        }

        AgentHTable ISimContext.Agents
        {
            get
            {
                return this._Agents;
            }
      
        }
    }
}
 
