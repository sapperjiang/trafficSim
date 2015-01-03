using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.SysSimContext;

namespace SubSys_SimDriving.RoutePlan
{
	internal abstract class Route<T>
	{
        protected List<T> al = new List<T>();

        internal virtual void Remove(T t)
        {
            this.al.Remove(t);
        }
        /// <summary>
        /// 找到当前目标的下一个目标
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        internal virtual T FindNext(T t)
        {
            for (int i = 1; i < al.Count; i++)
            {
                if (t.Equals(al[i - 1]))//两个路段不可能完全相同
                {
                    return al[i];
                }
            }
            return default(T);
        }
        internal virtual T FindPrev(T t)
        {
            for (int i = 1; i < al.Count; i++)
            {
                if (t.Equals(al[i]))//两个路段不可能完全相同
                {
                    return al[i-1];
                }
            }
            return default(T);
        }
	}
	 
}
 
