using SubSys_SimDriving;
using System.Collections.Generic;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 运行态使用的数据结构，主要是roadlane 内部使用
    /// 设计为GUI 驱动 和 数据记录驱动的模型的接口类
    /// </summary>
	public class CACellChain
	{
        private List<CACell> caChain = new List<CACell>();

        /// <summary>
        /// 将元素插入队列末尾,并且更新元素指针
        /// </summary>
        /// <param name="ca"></param>
        public void Enqueue(CACell ca)
        {
            //更新指针
            ca.nextCACell = caChain.Count > 0 ? caChain[0] : null;
            caChain.Add(ca);
        }
        /// <summary>
        /// 返回队列的第一个元素，末尾为队列头
        /// </summary>
        /// <returns></returns>
        public CACell Dequeue()
        {
             CACell caC = null;
             int iLastIndex = caChain.Count - 1;
            if (iLastIndex>=0)
	        {
                caC = caChain[iLastIndex];
	        }
            caChain.RemoveAt(iLastIndex);
            return caC;
        }
        /// <summary>
        /// 返回队列队列末尾的的第一个元素，队列末尾为最近进入的元素,因此依照链表遍历就是从后往前遍历
        /// </summary>
        /// <returns></returns>
        public CACell Peek()
        {
            return caChain.Count > 0 ? caChain[caChain.Count-1] : null;
        }
        public int Count
        {
            get { return caChain.Count; }
        }

	}
	 
}
 
