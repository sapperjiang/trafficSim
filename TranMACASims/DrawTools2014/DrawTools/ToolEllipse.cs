namespace DrawTools
{
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    
    /// <summary>
	/// Ellipse tool
	/// </summary>
	class ToolEllipse : DrawTools.ToolRectangle
	{
		public ToolEllipse()
		{
            Cursor = new Cursor(GetType(), "Ellipse.cur");
		}

        public override void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
            Point p = GetEventPointInArea(drawArea, e);
            AddNewObject(drawArea, new DrawEllipse(p.X, p.Y, 1, 1));
        }

	}
}
