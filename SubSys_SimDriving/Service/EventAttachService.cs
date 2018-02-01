using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.RoutePlan;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving.Service
{

  
    /// <summary>
    /// 也不是所有的实体都有EventAttacherLogger。需要注册并且实现
    /// </summary>
    public class EventAttachService:Service
    {
        public delegate void EventAttacher(IEntity tVar);
        private EventAttachService() { }//不允许无参数构造
        private EventAttacher eaAttacher;

        public EventAttachService(EventAttacher ea)
        {
            this.eaAttacher = ea;
        }
        protected override void SubPerform(IEntity tVar)
        {
            if (eaAttacher != null)
            {
                eaAttacher(tVar);//调用外部事件
            }
            else 
            {
                throw new System.Exception("EventAttacher 为null");
            }
        }

        protected override void SubRevoke(IEntity tVar)
        {
            this.eaAttacher = null;
        }
    }
	 
}
 
