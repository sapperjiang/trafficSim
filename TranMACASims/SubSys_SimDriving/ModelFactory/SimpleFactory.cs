using System;
using System.Collections.Generic;
using SubSys_SimDriving.TrafficModel;
using SubSys_SimDriving.SysSimContext;
using SubSys_SimDriving.Agents;

namespace SubSys_SimDriving.ModelFactory
{
    ///// <summary>
    ///// 工厂模式，可以使用命令模式建造更复杂的功能集合类型
    ///// </summary>
    public interface IAbstractFactory 
    {
        Agent BuildAgent(BuildCommand bc, AgentType et);
        TrafficEntity BuildEntity(BuildCommand bc, EntityType et);
    }

    public class AgentFactory : IAbstractFactory
    {
        public Agent BuildAgent(BuildCommand bc, AgentType et)
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
        public TrafficEntity BuildEntity(BuildCommand bc, EntityType et)
        {
            throw new NotImplementedException();
        }
    }
    
  
   
}
