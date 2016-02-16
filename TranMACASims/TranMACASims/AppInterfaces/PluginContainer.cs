using System;
using System.Collections;

using System.Collections.Generic;
using System.Text;

namespace GISTranSim
{
    class PluginContainer:CollectionBase
    {

        internal PluginContainer()
        {
           // throw new System.NotImplementedException();
        }

        internal PluginContainer(PluginContainer value)
        {
            foreach (IPlugin pi in value)
            {
                this.Add(pi);
            }
        }

        internal PluginContainer(IPlugin[] value)
        {
            foreach (IPlugin pi in value)
            {
                this.Add(pi);
            }
        }

        internal int Add(IPlugin value)
        {
            return this.List.Add(value);// throw new System.NotImplementedException();
        }
        internal IPlugin this[int index]
        {
            get { return this.List[index] as IPlugin;/* return the specified index here */ }
        }

        internal int IndexOf(IPlugin value)
        {
            return this.List.IndexOf(value);// new System.NotImplementedException();
        }

        internal bool Contains(IPlugin value)
        {
            return this.List.Contains(value);// throw new System.NotImplementedException();
        }

        internal void CopyTo(IPlugin[] array, int index)
        {
            this.List.CopyTo(array,index);//IPlugin[] array new System.NotImplementedException();
        }

        internal IPlugin[] ToArray()
        {
            IPlugin[] array = new IPlugin[this.Count];// (this.Count);
            this.CopyTo(array,0);
            return array;
        }

        internal void Insert(int index, IPlugin value)
        {
            this.List.Insert(index, value);// throw new System.NotImplementedException();
        }

        internal void Remove(IPlugin value)
        {
            this.List.Remove(value);// throw new System.NotImplementedException();
        }

        //internal new PluginContainerEnumerator GetEnumerator()
        //{
        //    return new PluginContainerEnumerator(this);
        //}

      
    }
}
