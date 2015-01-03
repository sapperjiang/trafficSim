using SubSys_SimDriving;
using System.Collections.Generic;
//using System

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// 运行态使用的数据结构，主要是roadlane 内部使用
    /// 设计为GUI 驱动 和 数据记录驱动的模型的接口类
    /// </summary>
	internal class CellChain:IEnumerable<Cell>
	{
        private List<Cell> cells = new List<Cell>();

        /// <summary>
        /// 将元素插入队列末尾,并且更新元素指针
        /// </summary>
        /// <param name="ca"></param>
        internal virtual void Enqueue(Cell ca)
        {
            int iLastIndex = cells.Count - 1;
            //更新指针
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

        #region 原本打算自己写枚举器的逻辑，现在可以直接使用list自己带的
        //private Cell _cell;
        //private int Index;//
        //internal Cell Current
        //{
        //    get { return this.cells; }
        //}
        ///// <summary>
        ///// 从队列头开始往队尾遍历
        ///// </summary>
        ///// <returns></returns>
        //internal virtual bool MoveNext()
        //{
        //    if ((this.version == list._version) && (this.index < list._size))
        //    {
        //        this.current = list._items[this.index];
        //        this.index++;
        //        return true;
        //    }
        //    return this.MoveNextRare();

        //}
        //internal virtual bool ResetToQueue()
        //{
        //    int iCellCount = this.cells.Count;
        //    if (iCellCount > 0)
        //    {
        //        this._cell = iCellCount > 0 ? this.cells[iCellCount - 1] : null;
        //    }
        //}
        //internal int Count
        //{
        //    get { return cells.Count; }
        //}
        #endregion

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
 
