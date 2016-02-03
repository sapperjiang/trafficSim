using SubSys_SimDriving;
using System.Collections.Generic;
//using System

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// ����̬ʹ�õ����ݽṹ����Ҫ��roadlane �ڲ�ʹ��
    /// ���ΪGUI����ģ�͵Ľӿ���
    /// </summary>
	public class CellQueue:IEnumerable<Cell>
	{
        private List<Cell> cells = new List<Cell>();
        /// <summary>
        /// ��Ԫ�ز������ĩβ,���Ҹ���Ԫ��ָ��
        /// </summary>
        /// <param name="ca"></param>
        internal virtual void Enqueue(Cell ca)
        {
            int iLastIndex = cells.Count - 1;
            //����ָ��s
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
 
