using System.Drawing;
using System.Collections.Generic;

using SubSys_SimDriving;
using SubSys_MathUtility;
using SubSys_SimDriving.SysSimContext;
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

        MyPoint ToVector();
		 
        int iWidth
        {
            get;
            set;
        }
		int iLength
        {
            get;
            set;
        }
        /// <summary>
        /// ��ͨʵ���Ԫ������ϵ�����������ʵ���������
        /// </summary>
        Point RelativePosition
        {
            get;
            set;
        }

        ///// <summary>
        ///// ���潻ͨʵ�����Ļ����.����GUIʹ��,ע��������ǽṹ������
        ///// </summary>
        //Point scrnPos
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// �����GIS�ľ�������
        /// </summary>
	    MyPoint gisPos
	    {
            get;
            set;
	    }

        /// <summary>
        /// ��ͨʵ���GIS����ת��ΪԪ������֮�����״����,�������ǽ���ͨʵ����GIS���ɵĹؼ�
        /// ��ͨʵ�����״���������ߣ�����GUI�����ߺ���������Ⱦ�����پ�����ֱ����״
        /// </summary>
        EntityShape EntityShape
        {
            get;
        }
	}
	 
}
 
