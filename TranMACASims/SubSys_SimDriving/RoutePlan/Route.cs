using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.SysSimContext;

namespace SubSys_SimDriving.RoutePlan
{
	public abstract class Route<T>
	{
        protected List<T> routeList = new List<T>();

        public virtual void Remove(T t)
        {
            this.routeList.Remove(t);
        }
        public virtual void Add(T t)
        {
            this.routeList.Add(t);
        }
        /// <summary>
        /// 找到当前目标的下一个目标
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        internal virtual T FindNext(T t)
        {
            for (int i = 1; i < routeList.Count; i++)
            {
                if (t.Equals(routeList[i - 1]))//两个路段不可能完全相同
                {
                    return routeList[i];
                }
            }
            return default(T);
        }
        internal virtual T FindPrev(T t)
        {
            for (int i = 1; i < routeList.Count; i++)
            {
                if (t.Equals(routeList[i]))//两个路段不可能完全相同
                {
                    return routeList[i-1];
                }
            }
            return default(T);
        }
	}
	 
}
 
