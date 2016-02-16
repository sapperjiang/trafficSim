using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GISTranSim
{
    class ItemDef:IItemdef
    {
        private bool _group;
        private string _ID;
        private long _subtype;
        #region itemDef 成员


        internal bool Group
        {
            get
            {
                return this._group;// new NotImplementedException();
            }
            set
            {
                this._group = value;// throw new NotImplementedException();
            }
        }

        internal string ID
        {
            get
            {
                return this._ID;/// throw new NotImplementedException();
            }
            set
            {
                this._ID = value;// throw new NotImplementedException();
            }
        }

        internal long Subtype
        {
            get
            {
                return this._subtype;// throw new NotImplementedException();
            }
            set
            {
                this._subtype = value;// throw new NotImplementedException();
            }
        }

        #endregion

        bool IItemdef.Group
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        string IItemdef.ID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        long IItemdef.Subtype
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
