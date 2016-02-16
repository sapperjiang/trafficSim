using System;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving;
using SubSys_SimDriving.Agents;

namespace SubSys_SimDriving
{
    ///// <summary>
    ///// 工厂模式，可以使用命令模式建造更复杂的功能集合类型
    ///// </summary>
    public interface IFactory 
    {
        AbstractAgent Build(BuildCommand bc, AgentType et);
       
        TrafficEntity Build(BuildCommand bc, EntityType et);
        
   //     MobileEntity Build(BuildCommand bc, EntityType etype);
        	
    }

    public class AgentFactory : IFactory
    {
        public AbstractAgent Build(BuildCommand bc, AgentType et)
        {
            switch (et)
            {
                case AgentType.DecelerateAgent:
                    throw new NotImplementedException();
                    //break;
                case AgentType.CollisionAvoidingAgent:
                    throw new NotImplementedException();
                    //break;
                case AgentType.LaneShiftAgent:
                    throw new NotImplementedException();
                    //break;
                //case AgentType.SpeedUpDownAgent:
                //    return new SpeedUpDownAgent();
                    //break;
                case AgentType.SignalLightAgent:
                    return new SignalLightAgent();
                    //break;
                default:
                    throw new NotImplementedException();
                    //break;
            }
        }
     
        public TrafficEntity Build(BuildCommand bc, EntityType et)
        {
            throw new NotImplementedException();
        }
    }
  
   
}
