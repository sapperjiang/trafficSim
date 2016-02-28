

using System;
using System.Collections;
using System.Collections.Generic;

namespace SubSys_SimDriving.TrafficModel
{
    internal class IntIDManager:EntityIDManager<int>
    {

        internal  IntIDManager() 
        {
            this.templateMaxID = 0;
        }
        
        internal override int GetUniqueRoadNodeID()
        { 
            this.templateMaxID ++;
            this.listIDContainer.Add(this.templateMaxID);
            return this.templateMaxID;
        }
    }
}