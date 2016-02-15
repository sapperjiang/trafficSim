using System.Drawing;
using System.Collections.Generic;

using SubSys_SimDriving;
using SubSys_MathUtility;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving
{
	public interface ITrafficEntity
	{
        /// <summary>
        /// 仿真运行的静态数据，交通实体运行的数据环境，包括各种仿真系统运行需要的数据结构
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
        /// 容器交通实体，如道路是路段的容器。路段是车道的容器
        /// </summary>
        ITrafficEntity Container
        {
            get;
            set;
        }

 
        /// <summary>
        /// 交通实体的类型，VMS，Car，TrafficLight 等
        /// </summary>
        EntityType EntityType
        {
            get;
            set;
        }

//        [System.obse]
        OxyzPointF ToVector();
		 
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
        /// 交通实体的元胞坐标系，相对于容器实体或者其他
        /// </summary>
        Point Grid
        {
            get;
            set;
        }

        /// <summary>
        /// 相对于GIS的绝对坐标
        /// </summary>
	    OxyzPointF GISPosition
	    {
            get;
            set;
	    }

        /// <summary>
        /// 交通实体的GIS坐标转换为元胞坐标之后的形状曲线,该属性是将交通实体与GIS集成的关键
        /// 交通实体的形状可以是曲线，利用GDI画曲线函数进行渲染，不再局限于直线形状
        /// </summary>
        EntityShape Shape
        {
            get;
        }
        
            OxyzPoint SpatialGrid
         {
         	get;
         	set;
         }
        
	}
	 
}
 
