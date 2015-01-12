using System;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
	/**
	 * 有一个储存AgentName或者是AgentId的列表，
	 * 由于规则不是很多。不使用哈希表，这些
	 * 规则如交通灯规则，加减速规则等
	 */
	internal abstract class UpdateAgentChain:AbstractChain<AbstractAgent>
	{
        internal virtual void AddUpdateAgent(AbstractAgent ur)
        {
            if (ur != null)
            {
                base.Add(ur);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        internal virtual void RemoveUpdateAgent(AbstractAgent ur)
        {
            if (ur != null)
            {
                base.Remove(ur);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        internal virtual AbstractAgent FindUpdateAgentByName(string strAgentName)
        {
            if (strAgentName != null)
            {
                return null;
            }else
            {
                throw new ArgumentNullException();
            }
        }

	}
	 
}
 
