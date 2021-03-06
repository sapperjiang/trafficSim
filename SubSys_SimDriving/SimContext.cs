using System.Threading;
using SubSys_SimDriving.TrafficModel;
using System.Collections.Generic;
using SubSys_SimDriving;
//using System.Windows;


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
		/// 记录态哈希表
		/// </summary>
		EntityDics DataRecorder
		{
			get;// { return _DataRecorder; }
			
		}


		AgentDics Agents
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
		
		public   event TimeStepChangHandler OnTimeStepChanged;


		private  int _iCurrTimeStep=0;

		/// <summary>
		/// 仿真运行的时间步骤
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
		/// 记录态哈希表
		/// </summary>
		public EntityDics _DataRecorder                       = new EntityDics();
		
		public AgentDics _Agents                           = new AgentDics();

//		public Form  Canvas
//		{
//			
//				get{return this._Canvas;}
//				set{this._Canvas=value;}
//		}

		/// <summary>
		/// 单例模式
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
		/// 表示运行时建立的路网的结构，里面包含了对RoadNodeList和WayList的引用
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

		EntityDics ISimContext.DataRecorder
		{
			get
			{
				return this._DataRecorder;// throw new System.NotImplementedException();
			}
			
		}

		AgentDics ISimContext.Agents
		{
			get
			{
				return this._Agents;
			}
			
		}
		

	}
}

