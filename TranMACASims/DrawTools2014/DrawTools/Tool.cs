using System;
using System.Windows.Forms;
using System.Drawing;


namespace DrawTools
{
	/// <summary>
	/// Base class for all drawing tools
	/// </summary>
	abstract class Tool
	{

        /// <summary>
        /// Left nous button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnMouseDown(DrawArea drawArea, MouseEventArgs e)
        {
        }


        /// <summary>
        /// Mouse is moved, left mouse button is pressed or none button is pressed
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnMouseMove(DrawArea drawArea, MouseEventArgs e)
        {
        }


        /// <summary>
        /// Left mouse button is released
        /// </summary>
        /// <param name="drawArea"></param>
        /// <param name="e"></param>
        public virtual void OnMouseUp(DrawArea drawArea, MouseEventArgs e)
        {
        }

        protected Point GetEventPointInArea(DrawArea drawArea, MouseEventArgs e)
        {
            Point p = new Point(Math.Abs(drawArea.AutoScrollPosition.X) + e.X, Math.Abs(drawArea.AutoScrollPosition.Y) + e.Y);
            string status = "X: " + p.X + ", Y: " + p.Y;
            ((MainForm)drawArea.Parent).SetStatusStrip(status);
            return p;
        }
    }
}
