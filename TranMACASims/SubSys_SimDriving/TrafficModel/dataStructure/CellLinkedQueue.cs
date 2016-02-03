using SubSys_SimDriving;
using System.Collections.Generic;
//using System

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 运行态使用的数据结构，主要是roadlane 内部使用
    /// 设计为GUI驱动模型的接口类
    /// </summary>
	public class CellQueue:IEnumerable<Cell>
	{
        private List<Cell> cells = new List<Cell>();
        /// <summary>
        /// 将元素插入队列末尾,并且更新元素指针
        /// </summary>
        /// <param name="ca"></param>
        internal virtual void Enqueue(Cell ca)
        {
            int iLastIndex = cells.Count - 1;
            //更新指针s
            ca.nextCell = cells.Count > 0 ? cells[iLastIndex] : null;
            cells.Add(ca);//添加到队列末尾
        }
        /// <summary>
        /// 返回队列的第一个元素，末尾为队列头，路段结尾处的元素
        /// </summary>
        /// <returns></returns>
        internal virtual Cell Dequeue()
        {
            int iIndex = cells.Count;
            Cell ce = iIndex>0?cells[0]:null;
            if (cells.Count>=2)
            {//切断倒数第二个元素指向倒数第一个元素的链表指针
                cells[1].nextCell=null;
	        }//删除第一个元素
            cells.RemoveAt(0);
            return ce; 
        }
        /// <summary>
        /// 返回队列队列末尾的第一个元素，队列末尾处为最近进入的元素
        /// 因此依照链表遍历就是从后往前遍历
        /// </summary>
        /// <returns></returns>
        internal virtual Cell PeekLast()
        {
            return cells.Count > 0 ? cells[cells.Count - 1] : null;
        }
        /// <summary>
        /// 返回队列列首第一个元素，列首为最新进入的元素
        /// </summary>
        /// <returns></returns>
        internal virtual Cell PeekFirst()
        {
            return cells.Count > 0 ? cells[0] : null;
        }

        public Cell this[int index]
        {
            get { return this.cells[index]; }
        }
        public int Count 
        {
            get { return this.cells.Count; }
        }
        public IEnumerator<Cell> GetEnumerator()
        {
            return this.cells.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.cells.GetEnumerator();
        }
    }
	
}
 
