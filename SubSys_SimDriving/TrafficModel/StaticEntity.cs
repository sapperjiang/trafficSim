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
	public abstract partial class StaticOBJ:TrafficOBJ
	{
		
		//_______________2015��1�����������ݣ�ԭ�еĳ�Ա�ͷ����������ַ���________________
		/// <summary>
		/// _2015��1�����������ݣ�ԭ�еĳ�Ա�ͷ����������ַ���
		/// </summary>


		#region ����ȴ���mobileentity�����ݽṹ�ͺ���
		private Queue<MobileOBJ> _mobilesInn = new Queue<MobileOBJ>();

		/// <summary>
		/// �ȴ�����ʵ��ĵ��ĵȴ�mobileEntity��ʵ�����ǵȴ����У�����ںͳ������С�
		/// �����洢����λ�õ���������Ϊ�����뽻��ڲ�ͬ���ڸ����ڲ��ò��õ����ݽṹʵ��
		/// </summary>
		public Queue<MobileOBJ> MobilesInn {
			get {
				return _mobilesInn;
			}
		}

		/// <summary>
		/// ���ڴ�����ʱ�޷����복���ĳ�������ʱ���У�ÿ������ڸ������ڣ�����복����Ԫ���Ļ��棬����һ����·�������ڣ����������복��������ķ�����Ӧ�������ࡣlane��Xnode��д��
		/// </summary>
		/// <param name="me"></param>
		public virtual void EnterInn(MobileOBJ me)
		{
			//��mobile������ֵ��
			me.Container = this;		
			me.Shape.Add(this.Shape.Start);
			
			this._mobilesInn.Enqueue(me);
		}
		
		/// <summary>
		/// ���󷽷�����Ҫlane��xnodeʵ�֣����ȴ������е�mobile��ӵ�update mobile
		/// </summary>
		internal virtual void ServeMobiles(){}
		
		#endregion

		
	}
	
	
}

