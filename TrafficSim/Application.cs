using System;
using System.Collections.Generic;
//using ESRI.ArcGIS.Carto;
//using ESRI.ArcGIS.Controls;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace TrafficSim
{
    class TApplication:IApplication
    {
        private string _caption;
        private string _currentTool;
        private DataSet _mainDataSet;
        //private IMapControlDefault _mapControl;
       // private IPageLayoutControlDefault _pageLayoutControl;
        private string _name;
        private Form _mainPlatform;
        private StatusBar _statusBar;
        private bool _visible;
      //  private IMapDocument _document;
        #region hook 成员

        string IApplication.Caption
        {
            get
            {
               return this._caption;
            }
            set
            {
                this._caption = value;
            }
        }

        string IApplication.CurrentTool
        {
            get
            {
                return this._currentTool;
            }
            set
            {
                this._currentTool = value;
            }
        }

        System.Data.DataSet IApplication.MainDataSet
        {
            get
            {
                return this._mainDataSet;// new NotImplementedException();
            }
            set
            {
                this._mainDataSet = value;// throw new NotImplementedException();
            }
        }

        //ESRI.ArcGIS.Controls.IMapControlDefault IApplication.MapControl
        //{
        //    get
        //    {
        //        return this._mapControl;// throw new NotImplementedException();
        //    }
        //    set
        //    {
        //        this._mapControl = value;// new NotImplementedException();
        //    }
        //}

        //ESRI.ArcGIS.Controls.IPageLayoutControlDefault IApplication.PageLayoutControl
        //{
        //    get
        //    {
        //        return this._pageLayoutControl;// throw new NotImplementedException();
        //    }
        //    set
        //    {
        //        this._pageLayoutControl = value;// throw new NotImplementedException();
        //    }
        //}

        string IApplication.Name
        {
            get
            {
                return this._name;// new NotImplementedException();
            }
            set
            {
                this._name = value ;//hrow new NotImplementedException();
            }
        }

        System.Windows.Forms.Form IApplication.MainPlatform
        {
            get
            {
                return this._mainPlatform;// throw new NotImplementedException();
            }
            set
            {
                this._mainPlatform = value;// throw new NotImplementedException();
            }
        }

        System.Windows.Forms.StatusBar IApplication.StatusBar
        {
            get
            {
                return this._statusBar;// throw new NotImplementedException();
            }
            set
            {
                this._statusBar = value;// throw new NotImplementedException();
            }
        }

        bool IApplication.Visible
        {
            get
            {
                return this._visible;//hrow new NotImplementedException();
            }
            set
            {
                this._visible = value;// throw new NotImplementedException();
            }
        }

       


        //internal IMapDocument Document
        //{
        //    get
        //    {
        //        return this._document;// new NotImplementedException();
        //    }
        //    set
        //    {
        //        this._document = value;// throw new NotImplementedException();
        //    }
        //}

        #endregion
    }
}
