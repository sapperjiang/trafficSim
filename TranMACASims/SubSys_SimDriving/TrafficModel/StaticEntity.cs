using SubSys_SimDriving;

using System.Collections.Generic;
using System;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving
{
    /// <summary>
    /// ��ʾ�����XNode��·��way������Lane,��·Road�Ļ�����
    ///  ������ĸ���ʹ���˷�����ģʽ
    /// </summary>
    public abstract partial class StaticEntity:TrafficEntity
	{
         /// <summary>
        /// �ö�������ĸ���ʱ��
        /// </summary>
        internal int iCurrTimeStep;

	}
    
    //_______________2015��1�����������ݣ�ԭ�еĳ�Ա�ͷ����������ַ���________________
    /// <summary>
    /// _2015��1�����������ݣ�ԭ�еĳ�Ա�ͷ����������ַ���
    /// </summary>
    public abstract partial class StaticEntity:TrafficEntity
    {
        	LinkedList<MobileEntity> _mobiles = new LinkedList<MobileEntity>();
		/// <summary>
		/// ��������Ԫ���ռ��ϵ�Ԫ��������Ԫ��Ϊ�����������Ԫ������Ҫ�þ���
		/// </summary>
		public LinkedList<MobileEntity> Mobiles {
			get {
				return _mobiles;
			}
			set {
				_mobiles = value;
			}
		}
		
		#region ����ȴ���mobileentity�����ݽṹ�ͺ���
	
		
		/// <summary>
		/// �ȴ�����ó����ĵȴ�mobileEntity
		/// </summary>
		private Queue<MobileEntity> _mobilesInn = new Queue<MobileEntity>();

		public Queue<MobileEntity> MobilesInn {
			get {
				return _mobilesInn;
			}
		}

		/// <summary>
		/// ���ڴ�����ʱ�޷����복���ĳ�������ʱ���У�ÿ������ڸ������ڣ�����복����Ԫ���Ļ��棬����һ����·�������ڣ����������복��������ķ�����Ӧ�������ࡣlane��Xnode��д��
		/// </summary>
		/// <param name="me"></param>
		public virtual void EnterInn(MobileEntity me)
		{
			//��������ֵ��
//			me.Container = this._Container;
			this._mobilesInn.Enqueue(me);
		}
		
		/// <summary>
		/// ���󷽷�����Ҫlane��xnodeʵ�֣����ȴ������е�Ԫ����ӵ�����Ԫ���У��°���cellspace��ʵ�ָù���
		/// </summary>
		internal abstract void ServeMobiles();		
	
		#endregion
		
		/// <summary>
		/// cellspace ���͵�Ԫ������ռ�,��������lane������Ԫ��
		/// </summary>
		private CellSpace _Grids;
		
    }
	 
}
 
