using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace TrafficSim
{
    /// <summary>
    /// 解析插件容器中的对象将其放到不同的插件容器中
    /// </summary>
    class ParsePluginCollection
    {
        class Person
        {
            private string Name;

            internal string Name1
            {
                get { return Name; }
                set { Name = value; }
            }

            internal Person()
            {
            }

            internal void Eat(int amount)
            {
            
            }
            
           
        }





        private IDictionary<string, TrafficSim.ICommand> _ICmdContainer;

        internal IDictionary<string, TrafficSim.ICommand> getICmdContainer
        {
            get { return _ICmdContainer; }
        }
        private IDictionary<string, TrafficSim.ITool> _IToolContainer;

        internal IDictionary<string, TrafficSim.ITool> getIToolContainer
        {
            get { return _IToolContainer; }
        }
        private IDictionary<string, TrafficSim.IToolBarDef> _IToolBarDefContainer;

        internal IDictionary<string, TrafficSim.IToolBarDef> getIToolBarDefContainer
        {
            get { return _IToolBarDefContainer; }
        }
        private IDictionary<string, TrafficSim.IMenuDef> _IMenuDefContainer;

        internal IDictionary<string, TrafficSim.IMenuDef> getIMenuDefContainer
        {
            get { return _IMenuDefContainer; }
        }
        private IDictionary<string, TrafficSim.IDockableWndDef> _IDockableWndContainer;

        internal IDictionary<string, TrafficSim.IDockableWndDef> getIDockableWndContainer
        {
            get { return _IDockableWndContainer; }
        }
        private ArrayList _CmdCategory;

        internal ParsePluginCollection()
        {
            this._ICmdContainer = new Dictionary<string, ICommand>();
            this._IDockableWndContainer = new Dictionary<string, IDockableWndDef>();
            this._IMenuDefContainer = new Dictionary<string, IMenuDef>();
            this._IToolContainer = new Dictionary<string, ITool>();
            this._IToolBarDefContainer = new Dictionary<string, IToolBarDef>();
            this._CmdCategory = new ArrayList();

            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// 获取和解析插件集合中的所有对象将其分别装入ICommand，IToll，IToolBar和IMenuDef四个集合中
        /// </summary>
        internal void getPluginArray(PluginContainer pCtner)
        {
            foreach (IPlugin ipi in pCtner)
            {
                ICommand icmd = ipi as ICommand;
                if (icmd != null)
                {
                    this._ICmdContainer.Add(icmd.ToString(),icmd);
                    if (icmd.Category != null && !this._CmdCategory.Contains(icmd.Category))
                    {
                        this._CmdCategory.Add(icmd.Category);
                    }

                    continue;
                }


                ITool itool = ipi as ITool;
                if (itool != null)
                {
                    this._IToolContainer.Add(itool.ToString(),itool);
                    if (itool.Category != null && !this._CmdCategory.Contains(itool.Category))
                    {
                        this._CmdCategory.Add(itool.Category);
                    }
                    continue;
                }


                IToolBarDef itbd = ipi as IToolBarDef;
                if (itbd != null)
                {
                    this._IToolBarDefContainer.Add(itbd.ToString(),itbd);
                    continue;
                }


                IDockableWndDef idwd = ipi as IDockableWndDef;
                if (idwd != null)
                {
                    this._IDockableWndContainer.Add(idwd.ToString(),idwd);
                    continue;
                }

                IMenuDef imd = ipi as IMenuDef;
                if (imd != null)
                {
                    this._IMenuDefContainer.Add(imd.ToString(),imd);
                    continue;
                }



            }
            //throw new System.NotImplementedException();
        }
    }
}
