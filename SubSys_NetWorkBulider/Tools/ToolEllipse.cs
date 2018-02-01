using System;
using System.Windows.Forms;

namespace SubSys_NetWorkBuilder
{
	/// <summary>
	/// Ellipse tool
	/// </summary>
	class ToolEllipse : global::SubSys_NetWorkBuilder.ToolRectangle
	{
		public ToolEllipse()
		{
            Cursor = new Cursor( "Ellipse.cur");
		}

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawEllipse(e.X, e.Y, 1, 1));
        }
	}
}
