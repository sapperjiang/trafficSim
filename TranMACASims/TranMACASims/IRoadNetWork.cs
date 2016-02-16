using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubSys_SimDriving.TrafficModel
{
    public interface IRoadNetWork
    {
        void AddRoadNode(RoadNode value);
        void RemoveRoadNode(RoadNode value);
        void AddRoadEdge(RoadNode fromRoadNode, RoadNode ToRoadNode);
       
        void RemoveEdge(RoadNode fromRoadNode, RoadNode ToRoadNode);
    
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
