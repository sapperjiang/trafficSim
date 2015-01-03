using System;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
	/**
	 * ��һ������AgentName������AgentId���б�
	 * ���ڹ����Ǻܶࡣ��ʹ�ù�ϣ����Щ
	 * �����罻ͨ�ƹ��򣬼Ӽ��ٹ����
	 */
	internal abstract class UpdateAgentChain:ChainBaseClass<Agent>
	{
        internal virtual void AddUpdateAgent(Agent ur)
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
        internal virtual void RemoveUpdateAgent(Agent ur)
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
        internal virtual Agent FindUpdateAgentByName(string strAgentName)
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
 
