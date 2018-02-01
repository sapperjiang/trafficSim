
namespace SubSys_SimDriving.Service
{
    /// <summary>
    /// �۲��߽ӿ�
    /// </summary>
    public interface IService  
	{
        bool IsRunning
         {
             get;
             set;
         }
        void Perform(IEntity tVar);
        void Revoke(IEntity tVar);
        void Attach();
    }

    public abstract class Service : IService
    {
    	/// <summary>
    	/// �������еĿ��ر���
    	/// </summary>
        public static bool IsServiceUp = true;
        private bool _isRunning = true;
        /// <summary>
        /// ���ȫ�ֿ��عر���һ������false
        /// </summary>
        public bool IsRunning
        {
            get { return (_isRunning&&Service.IsServiceUp); }
            set { _isRunning = value; }
        }

        /// <summary>
        /// design to be a switch for a service 
        /// </summary>
        /// <param name="tVar"></param>
        public void Perform(IEntity tVar)
        {
            if (this.IsRunning == true)
            {
                this.SubPerform(tVar);
            }
        }
        protected abstract void SubPerform(IEntity tVar);

        public void Revoke(IEntity tVar)
        {
             if (this.IsRunning == true)
            {
                this.SubRevoke(tVar);
            }
        }
        protected abstract void SubRevoke(IEntity tVar);

        public void Attach()
        {
            throw new System.NotImplementedException();
        }
    }
     
 
}
 
