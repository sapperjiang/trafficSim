using SubSys_SimDriving;
using System.Collections;
using System.Collections.Generic;

namespace SubSys_SimDriving
{  
    /// <summary>
    /// ���зǹ�ϣ��Ļ����ͣ����ʹ�ø����ڲ���list�������ݣ�Ӧ���ɵ��������
    /// ����������ͬ��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractChain<T> : ICollection<T>, ICollection,IEnumerable<T>
    {
        protected List<T> listChain = new List<T>();

        public T this[int index]
        {
            get
            { return this.listChain[index]; }
            set
            {
                this.listChain[index] = value;
            }
        }
        public void Add(T item)
        {
            this.listChain.Add(item);// throw new System.NotImplementedException();
        }

        public void Clear()
        {
            listChain.Clear();
        }

        public bool Contains(T item)
        {
            return listChain.Contains(item);
        }
        [System.Obsolete("û��ʵ�ֵķ���")]
        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public int Count
        {
            get
            {
                return listChain.Count;
            }
        }

        public void Insert(int index, T item)
        {
            this.listChain.Insert(index, item);
        }
        public bool Remove(T item)
        {
            return listChain.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < listChain.Count; ++i)  
            {
                yield return listChain[i];  
            }  
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return listChain.GetEnumerator();
            //throw new System.NotImplementedException();
        }

        [System.Obsolete("û��ʵ�ֵķ���")]
        public void CopyTo(System.Array array, int index)
        {
            //listChain.CopyTo(array, 0); 
            //throw new System.NotImplementedException();
        }

        public bool IsSynchronized
        {
            get
            {// return listChain.issy
                throw new System.NotImplementedException();
            }
        }

        public object SyncRoot
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new System.NotImplementedException(); }
        }


        bool ICollection<T>.IsReadOnly
        {
            get { throw new System.NotImplementedException(); }
        }
    }
    
}
 
