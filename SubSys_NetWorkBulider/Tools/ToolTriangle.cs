using System;
using System.Windows.Forms;

namespace SubSys_NetWorkBuilder
{
    /// <summary>
    /// Triangle tool
    /// </summary>
    class ToolTriangle : global::SubSys_NetWorkBuilder.ToolRectangle
    {
        public ToolTriangle()
        {
            Cursor = new Cursor( "Triangle.cur");
        }

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            AddNewObject(drawArea, new DrawTriangle(e.X, e.Y, 1, 1));
        }
    }
}
