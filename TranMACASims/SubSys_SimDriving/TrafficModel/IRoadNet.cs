using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubSys_SimDriving.TrafficModel
{
/// <summary>
/// 定义一个无参数的代理，需要的时候可以添加参数
/// </summary>
    public delegate void UpdateHandler();
    
//    public delegate void M

    public interface IRoadNet
    {

        int iCurrTimeStep
        {
        	
            get;
            set;
        }

        event UpdateHandler Updated;
//        event 

        ICollection<Way> Ways 
        {
            get;
        }
        ICollection<XNode> XNodes
        {
            get;
        }
        ICollection<Lane> Lanes
        {
            get;
        }

        void AddXNode(XNode value);
        XNode FindXNode(XNode value);
        void RemoveXNode(XNode value);

        void AddWay(Way re);
        Way AddWay(XNode fromRoadNode, XNode ToRoadNode);
       
        void RemoveWay(XNode fromRoadNode, XNode ToRoadNode);

        Way FindWay(XNode from, XNode to);
        Way FindWay(int RoadEdgeHash);

       
    }
}
