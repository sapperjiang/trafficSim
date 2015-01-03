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
        /// �ҵ���ǰĿ�����һ��Ŀ��
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        internal virtual T FindNext(T t)
        {
            for (int i = 1; i < al.Count; i++)
            {
                if (t.Equals(al[i - 1]))//����·�β�������ȫ��ͬ
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
                if (t.Equals(al[i]))//����·�β�������ȫ��ͬ
                {
                    return al[i-1];
                }
            }
            return default(T);
        }
	}
	 
}
 
