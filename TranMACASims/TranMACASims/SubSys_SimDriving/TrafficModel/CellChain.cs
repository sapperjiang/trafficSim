using SubSys_SimDriving;
using System.Collections.Generic;
//using System

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// ����̬ʹ�õ����ݽṹ����Ҫ��roadlane �ڲ�ʹ��
    /// ���ΪGUI ���� �� ���ݼ�¼������ģ�͵Ľӿ���
    /// </summary>
	internal class CellChain:IEnumerable<Cell>
	{
        private List<Cell> cells = new List<Cell>();

        /// <summary>
        /// ��Ԫ�ز������ĩβ,���Ҹ���Ԫ��ָ��
        /// </summary>
        /// <param name="ca"></param>
        internal virtual void Enqueue(Cell ca)
        {
            int iLastIndex = cells.Count - 1;
            //����ָ��
            ca.nextCell = cells.Count > 0 ? cells[iLastIndex] : null;
            cells.Add(ca);//��ӵ�����ĩβ
        }
        /// <summary>
        /// ���ض��еĵ�һ��Ԫ�أ�ĩβΪ����ͷ��·�ν�β����Ԫ��
        /// </summary>
        /// <returns></returns>
        internal virtual Cell Dequeue()
        {
            int iIndex = cells.Count;
            Cell ce = iIndex>0?cells[0]:null;
            if (cells.Count>=2)
            {//�жϵ����ڶ���Ԫ��ָ������һ��Ԫ�ص�����ָ��
                cells[1].nextCell=null;
	        }//ɾ����һ��Ԫ��
            cells.RemoveAt(0);
            return ce; 
        }
        /// <summary>
        /// ���ض��ж���ĩβ�ĵ�һ��Ԫ�أ�����ĩβ��Ϊ��������Ԫ��
        /// �����������������ǴӺ���ǰ����
        /// </summary>
        /// <returns></returns>
        internal virtual Cell PeekLast()
        {
            return cells.Count > 0 ? cells[cells.Count - 1] : null;
        }
        /// <summary>
        /// ���ض������׵�һ��Ԫ�أ�����Ϊ���½����Ԫ��
        /// </summary>
        /// <returns></returns>
        internal virtual Cell PeekFirst()
        {
            return cells.Count > 0 ? cells[0] : null;
        }

        #region ԭ�������Լ�дö�������߼������ڿ���ֱ��ʹ��list�Լ�����
        //private Cell _cell;
        //private int Index;//
        //internal Cell Current
        //{
        //    get { return this.cells; }
        //}
        ///// <summary>
        ///// �Ӷ���ͷ��ʼ����β����
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
 
