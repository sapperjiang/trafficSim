

using System;
using System.Collections;
using System.Collections.Generic;

namespace SubSys_SimDriving.TrafficModel
{
    internal abstract class EntityIDManager<T>
    {
        protected T templateMaxID ;
       protected List<T> listIDContainer = new List<T>();
        /// <summary>
        /// 利用泛型之后ID 可以为string类型,由子类进行实现
        /// </summary>
        /// <returns></returns>
        internal abstract T GetUniqueRoadNodeID();
    }
}