using System.Collections;
using System.Collections.Generic;
using SubSys_SimDriving.SysSimContext;

namespace SubSys_SimDriving.SysSimContext
{
    /// <summary>
    /// ������������һά������ϣ��Tvalue�����ͣ���Ҳ��������ά������ϣ��Tvalue�Ǽ��ϣ�
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    internal abstract class StaticSysTable<TKey, TValue>:Dictionary<TKey,TValue>
	{
        //protected Dictionary<TKey, TValue> dicIndexHashTable = new Dictionary<TKey, TValue>();

        internal virtual TValue Get(TKey iKey)
		{
            if (!iKey.Equals(default(TKey)))
	        {
		         TValue outTValue;
                base.TryGetValue(iKey, out outTValue);
                return outTValue;
	        }
            return default(TValue);
		}
        internal virtual void Add(TKey iKey,TValue value)
        {
            if (base.ContainsKey(iKey))
            {
                throw new System.ArgumentException("ӵ�иü�ֵ�����Ѿ�����");
            }
            base.Add(iKey, value);
        }
        internal virtual void Remove(TKey iKey)
        {
            if (!base.ContainsKey(iKey))
            {
                throw new System.ArgumentException("�޷�ɾ��û����ӵĶ���");
            }
            base.Remove(iKey);
        }
    }
	 
}
 
