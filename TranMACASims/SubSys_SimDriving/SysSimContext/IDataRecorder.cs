using System.Collections;
using System.Collections.Generic;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;

namespace SubSys_SimDriving
{
    public interface IDataRecorder<Tkey, Tvalue> : IDictionary<Tkey, Tvalue>
    {
        Tvalue GetElement(Tkey tk);
        void Record(Tkey tk, CarInfo ciItem);
    }
    /// <summary>
    /// ������
    /// </summary>
    public abstract class DataRecorder<Tkey, Tvalue> : Dictionary<Tkey, Tvalue>, IDataRecorder<Tkey, Tvalue>
    {
        public virtual Tvalue GetElement(Tkey tk)
        {   
            Tvalue outRecorder;
            this.TryGetValue(tk,out outRecorder);
            return outRecorder;
        }
        public abstract void Record(Tkey tk, CarInfo ciItem);
       
    }

    /// <summary>
    /// ���ڲ��ϣ�����泵������Ϣ�����ó�����ϣ��������
    /// </summary>
    public class CarInfoDic : DataRecorder<int, CarInfoQueue>
    {
        public override void Record(int tk, CarInfo ciItem)
        {
            CarInfoQueue cid = this.GetElement(tk);//���ݳ����Ĺ�ϣ��ȡ����������ʽ��Ϣ����
            if (cid == null)//û�иó��򴴽�
            {
                cid = new CarInfoQueue();
                base.Add(tk, cid);
            }
            cid.Enqueue(ciItem);
        }
    }
    /// <summary>
    /// ���������������ڲ㣬����һ��·���ϵĳ�����Ϣ������·�ι�ϣ��������
    /// ��Ի������⡣�������ӳ������ĸ������ļ�¼
    /// </summary>
    public class EntityDic : DataRecorder<int, CarInfoDic>
    {
        public override void Record(int tk, CarInfo ciItem)
        {
            CarInfoDic cid = this.GetElement(tk);//��ȡ������һ��·�������г��ĵ��ֵ���Ϣ
            if (cid == null)//û�и�·���򴴽�
            {
                cid = new CarInfoDic();
                this.Add(tk, cid);//����ֵ�
            }
            cid.Record(ciItem.iCarHashCode,ciItem);
        }
    }
   
}
 
