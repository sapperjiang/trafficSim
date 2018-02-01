using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Service
{

  
    /// <summary>
    /// Ҳ�������е�ʵ�嶼��EventAttacherLogger����Ҫע�Ტ��ʵ��
    /// </summary>
    public class EventAttachService:Service
    {
        public delegate void EventAttacher(IEntity tVar);
        private EventAttachService() { }//�������޲�������
        private EventAttacher eaAttacher;

        public EventAttachService(EventAttacher ea)
        {
            this.eaAttacher = ea;
        }
        protected override void SubPerform(IEntity tVar)
        {
            if (eaAttacher != null)
            {
                eaAttacher(tVar);//�����ⲿ�¼�
            }
            else 
            {
                throw new System.Exception("EventAttacher Ϊnull");
            }
        }

        protected override void SubRevoke(IEntity tVar)
        {
            this.eaAttacher = null;
        }
    }
	 
}
 
