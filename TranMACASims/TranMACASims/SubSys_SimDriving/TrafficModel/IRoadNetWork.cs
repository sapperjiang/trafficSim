using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubSys_SimDriving.TrafficModel
{
    internal interface IRoadNetWork
    {
        ICollection RoadEdges 
        {
            get;
        }
        ICollection RoadNodes
        {
            get;
        }

        void AddRoadNode(RoadNode value);
        RoadNode FindRoadNode(RoadNode value);
        void RemoveRoadNode(RoadNode value);

        void AddRoadEdge(RoadEdge re);
        void AddRoadEdge(RoadNode fromRoadNode, RoadNode ToRoadNode);
       
        void RemoveRoadEdge(RoadNode fromRoadNode, RoadNode ToRoadNode);
    
        RoadEdge FindRoadEdge(RoadNode from, RoadNode to);

        int RoadNodeCount
        {
            get;
        }

        int RoadEdgeCount
        {
            get;
   
        }
    }
}
