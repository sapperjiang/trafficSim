using System;

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using SubSys_MathUtility;
using SubSys_Graphics;

namespace SubSys_NetworkBuilder
{
    /// <summary>
    /// 被观察者：
    /// </summary>
    public abstract class Subject
    {
        List<Observer> observers = new List<Observer>();
        internal virtual void Notify()
        {
            foreach (var observer in observers)
            {
                observer.Action(this);
            }
        }
    }

    public abstract class Observer
    {
        internal void Action(Subject su)
        {
            //do something
        }
    }

}
