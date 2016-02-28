using System;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving;
using SubSys_SimDriving.Agents;
using SubSys_MathUtility;

namespace SubSys_SimDriving
{
    ///// <summary>
    ///// 工厂模式，可以使用命令模式建造更复杂的功能集合类型
    ///// </summary>
    public interface IFactory 
    {
       // AbstractAgent Build(BuildCommand bc, AgentType et);
       
        StaticEntity Build(OxyzPointF start,OxyzPointF end, EntityType et);
        	
    }

  
}
