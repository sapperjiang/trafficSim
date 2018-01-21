using SubSys_MathUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SubSys_SimDriving.TrafficModel
{
/// <summary>
/// 定义一个无参数的代理，需要的时候可以添加参数
/// </summary>
    public delegate void UpdateHandler();

    public interface IRoadNet
    {

        int iTimePulse
        {
        	
            get;
            set;
        }

        event UpdateHandler Updated;

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
        bool ModifyXNode(XNode old,XNode New);
        //bool BulidXNode(OxyzPointF start, OxyzPointF end, EntityType et);

        Way BulidWay(OxyzPointF start, OxyzPointF end);

        Way BulidWay(Point start, Point end);

        Road BulidRoad(Point start, Point end);
     
        StaticEntity BulidEntity(OxyzPointF start, OxyzPointF end, EntityType et);

        void AddWay(Way re);
       
        void RemoveWay(XNode fromRoadNode, XNode ToRoadNode);

        Way FindWay(XNode from, XNode to);
        Way FindWay(int WayHash);

    }
}
