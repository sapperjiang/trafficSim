using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TranMACASims.SubSys_SimDriving.Factory
{
    internal partial class Component1 : Component
    {
        internal Component1()
        {
            InitializeComponent();
        }

        internal Component1(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
