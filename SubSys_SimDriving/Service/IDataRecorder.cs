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
    /// 所有车辆的运动轨迹记录哈希表，利用车辆哈希值查找车辆
    /// </summary>
    public class CarInfoDic : DataRecorder<int, CarTrack>
    {
        public override void Record(int hashCode, CarInfo carInfo)
        {
            CarTrack cid = this.GetElement(hashCode);//依据车辆的哈希获取到车辆的行驶时空信息（车辆的随时间变化的空间路径）
            if (cid == null)//没有该车则创建
            {
                cid = new CarTrack();
                base.Add(hashCode, cid);
            }
            cid.Enqueue(carInfo);
        }
    }
    /// <summary>
    /// 哈表索引树，次内层，保存一个路段上的车辆信息，利用路段哈希进行索引
    /// 针对换道问题。可以增加车辆在哪个车道的记录
    /// </summary>
    public class EntityDics : DataRecorder<int, CarInfoDic>
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
 
