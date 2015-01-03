using SubSys_SimDriving;
using System.Collections.Generic;

namespace SubSys_SimDriving.TrafficModel
{
    /// <summary>
    /// ����̬ʹ�õ����ݽṹ����Ҫ��roadlane �ڲ�ʹ��
    /// ���ΪGUI ���� �� ���ݼ�¼������ģ�͵Ľӿ���
    /// </summary>
	public class CACellChain
	{
        private List<CACell> caChain = new List<CACell>();

        /// <summary>
        /// ��Ԫ�ز������ĩβ,���Ҹ���Ԫ��ָ��
        /// </summary>
        /// <param name="ca"></param>
        public void Enqueue(CACell ca)
        {
            //����ָ��
            ca.nextCACell = caChain.Count > 0 ? caChain[0] : null;
            caChain.Add(ca);
        }
        /// <summary>
        /// ���ض��еĵ�һ��Ԫ�أ�ĩβΪ����ͷ
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
        /// ���ض��ж���ĩβ�ĵĵ�һ��Ԫ�أ�����ĩβΪ��������Ԫ��,�����������������ǴӺ���ǰ����
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
 
