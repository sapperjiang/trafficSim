using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubSys_SimDriving.TrafficModel
{
    public delegate void UpdateHandler();

    public interface IRoadNetWork
    {

        int iCurrTimeStep
        {
            get;
            set;
        }

        event UpdateHandler UpdateCompleted;

        ICollection<RoadEdge> RoadEdges 
        {
            get;
        }
        ICollection<RoadNode> RoadNodes
        {
            get;
        }
        ICollection<RoadLane> RoadLanes
        {
            get;
        }

        void AddRoadNode(RoadNode value);
        RoadNode FindRoadNode(RoadNode value);
        void RemoveRoadNode(RoadNode value);

        void AddRoadEdge(RoadEdge re);
        RoadEdge AddRoadEdge(RoadNode fromRoadNode, RoadNode ToRoadNode);
       
        void RemoveRoadEdge(RoadNode fromRoadNode, RoadNode ToRoadNode);

        RoadEdge FindRoadEdge(RoadNode from, RoadNode to);
        RoadEdge FindRoadEdge(int RoadEdgeHash);

       
    }
}
