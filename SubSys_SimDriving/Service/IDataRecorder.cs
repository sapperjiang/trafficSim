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
    /// ���г������˶��켣��¼��ϣ�����ó�����ϣֵ���ҳ���
    /// </summary>
    public class CarInfoDic : DataRecorder<int, CarTrack>
    {
        public override void Record(int hashCode, CarInfo carInfo)
        {
            CarTrack cid = this.GetElement(hashCode);//���ݳ����Ĺ�ϣ��ȡ����������ʻʱ����Ϣ����������ʱ��仯�Ŀռ�·����
            if (cid == null)//û�иó��򴴽�
            {
                cid = new CarTrack();
                base.Add(hashCode, cid);
            }
            cid.Enqueue(carInfo);
        }
    }
    /// <summary>
    /// ���������������ڲ㣬����һ��·���ϵĳ�����Ϣ������·�ι�ϣ��������
    /// ��Ի������⡣�������ӳ������ĸ������ļ�¼
    /// </summary>
    public class EntityDics : DataRecorder<int, CarInfoDic>
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
 
