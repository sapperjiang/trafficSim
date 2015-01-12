using System;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Agents
{
	/**
	 * ��һ������AgentName������AgentId���б�
	 * ���ڹ����Ǻܶࡣ��ʹ�ù�ϣ����Щ
	 * �����罻ͨ�ƹ��򣬼Ӽ��ٹ����
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
 
