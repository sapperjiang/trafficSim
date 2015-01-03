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
                /// ��¼̬��ϣ��
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
    /// ����ģʽ������ע�ᣬ����ע��֮ǰӦ������Ƿ��Ѿ�ע�����
    /// Ӧ����roadnetwork�е�һ���ֵĹ������Σ���粻Ӧ��ʽ�ĵ���ע�ᣬ
    /// ע�������ⲿ�����͸���ģ�ע��ʹ������/�ܾ�ģ��,ע��Ӧ��ͳһʹ�÷���ʵ��
    /// simContext�ڲ�����ע������Ӧ�������ǿɶ��ģ��������޸ĵ�
    /// </summary>
    public sealed class SimContext:ISimContext
    {
        private static int SimContextCount = 0;
        /// <summary>
        ///����ģʽ ��ֱֹ�ӵ��ýӿ����ɸ����
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
        /// ��¼̬��ϣ��
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
        /// ��ʾ����ʱ������·���Ľṹ����������˶�RoadNodeList��RoadEdgeList������
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
 
