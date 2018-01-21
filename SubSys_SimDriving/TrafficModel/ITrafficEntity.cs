using System.Drawing;
using System.Collections.Generic;

using SubSys_SimDriving;
using SubSys_MathUtility;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving
{
	public interface ITrafficEntity
	{
		/// <summary>
		/// �������еľ�̬���ݣ���ͨʵ�����е����ݻ������������ַ���ϵͳ������Ҫ�����ݽṹ
		/// </summary>
		ISimContext ISimCtx
		{
			get;
		}
		int ID
		{
			get;
		}
		string Name
		{
			get;
			set;
		}
		/// <summary>
		/// ������ͨʵ�壬���·��·�ε�������·���ǳ���������
		/// </summary>
		ITrafficEntity Container
		{
			get;
			set;
		}

		
		/// <summary>
		/// ��ͨʵ������ͣ�VMS��Car��TrafficLight ��
		/// </summary>
		EntityType EntityType
		{
			get;
			set;
		}


		OxyzPointF ToVector();
		
		int Width
		{
			get;
			set;
		}
		int Length
		{
			get;
			set;
		}

		/// <summary>
		/// �����GIS�ľ�������
		/// </summary>
		OxyzPointF GISGrid
		{
			get;
			set;
		}

		/// <summary>
		/// ��ͨʵ���GIS����ת��ΪԪ������֮�����״����,�������ǽ���ͨʵ����GIS���ɵĹؼ�
		/// ��ͨʵ�����״���������ߣ�����GDI�����ߺ���������Ⱦ�����پ�����ֱ����״
		/// </summary>
		EntityShape Shape
		{
			get;
		}
		
		/// <summary>
		/// for 3D drawing method
		/// </summary>
		OxyzPointF SpatialGrid
		{
			get;
			set;
		}
		
		Color Color
		{
			get;
			set;
		}
		
	}
	
}
