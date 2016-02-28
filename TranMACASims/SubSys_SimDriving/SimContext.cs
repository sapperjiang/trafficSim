using System.Threading;
using SubSys_SimDriving.TrafficModel;
using System.Collections.Generic;
using SubSys_SimDriving;
using System.Windows;


namespace SubSys_SimDriving
{
	public delegate void TimeStepChangHandler(string srMsg);
	public interface ISimContext
	{
		event TimeStepChangHandler OnTimeStepChanged;
		
		int iTimePulse
		{
			get;
			set;
		}

		RoadNet RoadNet
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
		
//		Form Canvas
//		{
//			
//			get;
//			set;
//		}
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
		
		public   event TimeStepChangHandler OnTimeStepChanged;


		private  int _iCurrTimeStep=0;

		/// <summary>
		/// �������е�ʱ�䲽��
		/// </summary>
		public  int iCurrTimeStep {
			get {
				return  this._iCurrTimeStep;
			}
			set {
				this._iCurrTimeStep = value;
				if (this.OnTimeStepChanged!=null) {
					this.OnTimeStepChanged(this.iCurrTimeStep.ToString());
				}
			}
		}
		
		
		public SpeedLimitHTable _SpeedLimits                 = new SpeedLimitHTable();

		public VMSHashTable _VMSEntities                     = new VMSHashTable();

		public CarModelHTable _CarModels                     = new CarModelHTable();
		public SignalLightHTable _SignalLights                = new SignalLightHTable();
		/// <summary>
		/// ��¼̬��ϣ��
		/// </summary>
		public EntityDic _DataRecorder                       = new EntityDic();
		
		public AgentHTable _Agents                           = new AgentHTable();

//		public Form  Canvas
//		{
//			
//				get{return this._Canvas;}
//				set{this._Canvas=value;}
//		}

		/// <summary>
		/// ����ģʽ
		/// </summary>
		int ISimContext.iTimePulse
		{
			get
			{
				return  this.iCurrTimeStep;
			}
			set
			{
				this.iCurrTimeStep = value;
			}
		}

		/// <summary>
		/// ��ʾ����ʱ������·���Ľṹ����������˶�RoadNodeList��RoadEdgeList������
		/// </summary>
		RoadNet ISimContext.RoadNet
		{
			get
			{
				return  RoadNet.GetInstance();;
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

