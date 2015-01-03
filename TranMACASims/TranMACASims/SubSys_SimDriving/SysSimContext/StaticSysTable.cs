using System.Collections;
using System.Collections.Generic;
using SubSys_SimDriving.SysSimContext;

namespace SubSys_SimDriving.SysSimContext
{
    /// <summary>
    /// 可以做用作的一维索引哈希表（Tvalue是类型），也可以做二维索引哈希表（Tvalue是集合）
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
                throw new System.ArgumentException("拥有该键值对象已经存在");
            }
            base.Add(iKey, value);
        }
        internal virtual void Remove(TKey iKey)
        {
            if (!base.ContainsKey(iKey))
            {
                throw new System.ArgumentException("无法删除没有添加的对象");
            }
            base.Remove(iKey);
        }
    }
	 
}
 
