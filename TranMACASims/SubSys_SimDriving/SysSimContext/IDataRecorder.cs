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
    /// 抽象类
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
    /// 最内层哈希表，保存车辆的信息。利用车辆哈希进行索引
    /// </summary>
    public class CarInfoDic : DataRecorder<int, CarInfoQueue>
    {
        public override void Record(int tk, CarInfo ciItem)
        {
            CarInfoQueue cid = this.GetElement(tk);//依据车辆的哈希获取到车辆的形式信息队列
            if (cid == null)//没有该车则创建
            {
                cid = new CarInfoQueue();
                base.Add(tk, cid);
            }
            cid.Enqueue(ciItem);
        }
    }
    /// <summary>
    /// 哈表索引树，次内层，保存一个路段上的车辆信息，利用路段哈希进行索引
    /// 针对换道问题。可以增加车辆在哪个车道的记录
    /// </summary>
    public class EntityDic : DataRecorder<int, CarInfoDic>
    {
        public override void Record(int tk, CarInfo ciItem)
        {
            CarInfoDic cid = this.GetElement(tk);//获取到的是一个路段上所有车的的字典信息
            if (cid == null)//没有该路段则创建
            {
                cid = new CarInfoDic();
                this.Add(tk, cid);//添加字典
            }
            cid.Record(ciItem.iCarHashCode,ciItem);
        }
    }
   
}
 
