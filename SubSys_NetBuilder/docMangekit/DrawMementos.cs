#region Using directives

using System;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Drawing;
using System.Security.Permissions;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Reflection;


#endregion

namespace SubSys_NetWorkBuilder
{
    using DrawList = List<DrawObject>;

    /// <summary>
    /// List of graphic objects
    /// </summary>
    [Serializable]
   public class GraphicsList : ISerializable
    {
        #region Members

        private DrawList graphics;

        private const string entryCount = "Count";
        private const string entryType = "Type";

        #endregion


        #region Constructor
        public GraphicsList()
        {
            graphics = new DrawList();
        }

        #endregion

        #region Serialization Support

        protected GraphicsList(SerializationInfo info, StreamingContext context)
        {
            graphics = new DrawList();

            int n = info.GetInt32(entryCount);
            string typeName;
            DrawObject drawObject;

            for (int i = 0; i < n; i++)
            {
                typeName = info.GetString(
                    String.Format(CultureInfo.InvariantCulture,
                        "{0}{1}",
                    entryType, i));

                drawObject = (DrawObject)Assembly.GetExecutingAssembly().CreateInstance(
                    typeName);

                drawObject.LoadFromStream(info, i);

                graphics.Add(drawObject);
            }

        }

        /// <summary>
        /// Save object to serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(entryCount, graphics.Count);

            int i = 0;

            foreach (DrawObject o in graphics)
            {
                info.AddValue(
                    String.Format(CultureInfo.InvariantCulture,
                        "{0}{1}",
                        entryType, i),
                    o.GetType().FullName);

                o.SaveToStream(info, i);

                i++;
            }
        }

        #endregion

        #region Other functions

        public void Draw(Graphics g)
        {
            int n = graphics.Count;
            DrawObject o;

            // Enumerate list in reverse order to get first
            // object on the top of Z-order.
            for (int i = n - 1; i >= 0; i--)
            {
                o = graphics[i];

                o.Draw(g);

                if (o.Selected == true)
                {
                    o.DrawTracker(g);
                }
            }
        }

        /// <summary>
        /// Dump (for debugging)
        /// </summary>
        public void Dump()
        {
            Trace.WriteLine("");

            foreach ( DrawObject o in graphics )
            {
                o.Dump();
            }
        }

        /// <summary>
        /// Clear all objects in the list
        /// </summary>
        /// <returns>
        /// true if at least one object is deleted
        /// </returns>
        public bool Clear()
        {
            bool result = (graphics.Count > 0);
            graphics.Clear();
            return result;
        }

        /// <summary>
        /// Count and this [nIndex] allow to read all graphics objects
        /// from GraphicsList in the loop.
        /// </summary>
        public int Count
        {
            get
            {
                return graphics.Count;
            }
        }

        public DrawObject First
        {
            get { return this[0]; }
        }


        public DrawObject this[int index]
        {
            get
            {
                if (index < 0 || index >= graphics.Count)
                    return null;

                return graphics[index];
            }
        }


        /// <summary>
        /// SelectedCount and GetSelectedObject allow to read
        /// selected objects in the loop
        /// </summary>
        public int SelectionCount
        {
            get
            {
                int n = 0;

                foreach (DrawObject o in Selection)
                {
                    n++;
                }

                return n;
            }
        }


        /// <summary>
        /// Returns INumerable object which may be used for enumeration
        /// of selected objects.
        /// 
        /// Note: returning IEnumerable<DrawObject> breaks CLS-compliance
        /// (assembly CLSCompliant = true is removed from AssemblyInfo.cs).
        /// To make this program CLS-compliant, replace 
        /// IEnumerable<DrawObject> with IEnumerable. This requires
        /// casting to object at runtime.
        /// </summary>
        /// <value></value>
        public IEnumerable<DrawObject> Selection
        {
            get
            {
                foreach (DrawObject o in graphics)
                {
                    if (o.Selected)
                    {
                        yield return o;
                    }
                }
            }
        }

        public void Add(DrawObject obj)
        {
            // insert to the top of z-order
            graphics.Insert(0, obj);
        }

        public void AddFirst(DrawObject obj)
        {
            // insert to the top of z-order
            graphics.Insert(0, obj);
            //this.Dirty = true;
        }
        /// <summary>
        /// Insert object to specified place.
        /// Used for Undo.
        /// </summary>
        public void Insert(int index, DrawObject obj)
        {
            if ( index >= 0  && index < graphics.Count )
            {
                graphics.Insert(index, obj);
            }
        }

        /// <summary>
        /// Replace object in specified place.
        /// Used for Undo.
        /// </summary>
        public void Replace(int index, DrawObject obj)
        {
            if (index >= 0 && index < graphics.Count)
            {
                graphics.RemoveAt(index);
                graphics.Insert(index, obj);
            }
        }

        /// <summary>
        /// Remove object by index.
        /// Used for Undo.
        /// </summary>
        public void RemoveAt(int index)
        {
            graphics.RemoveAt(index);
        }

        /// <summary>
        /// Delete last added object from the list
        /// (used for Undo operation).
        /// </summary>
        public void DeleteLastAddedObject()
        {
            if ( graphics.Count > 0 )
            {
                graphics.RemoveAt(0);
            }
        }

        public void SelectInRectangle(Rectangle rectangle)
        {
            UnselectAll();

            foreach (DrawObject o in graphics)
            {
                if (o.IntersectsWith(rectangle))
                    o.Selected = true;
            }

        }

        public void UnselectAll()
        {
            foreach (DrawObject o in graphics)
            {
                o.Selected = false;
            }
        }

        public void SelectAll()
        {
            foreach (DrawObject o in graphics)
            {
                o.Selected = true;
            }
        }

        /// <summary>
        /// Delete selected items
        /// </summary>
        /// <returns>
        /// true if at least one object is deleted
        /// </returns>
        public bool DeleteSelection()
        {
            bool result = false;

            int n = graphics.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)graphics[i]).Selected)
                {
                    graphics.RemoveAt(i);
                    result = true;
                }
            }

            return result;
        }


        /// <summary>
        /// Move selected items to front (beginning of the list)
        /// </summary>
        /// <returns>
        /// true if at least one object is moved
        /// </returns>
        public bool MoveSelectionToFront()
        {
            int n;
            int i;
            DrawList tempList;

            tempList = new DrawList();
            n = graphics.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for (i = n - 1; i >= 0; i--)
            {
                if ((graphics[i]).Selected)
                {
                    tempList.Add(graphics[i]);
                    graphics.RemoveAt(i);
                }
            }

            // Read temporary list in direct order and insert every item
            // to the beginning of the source list
            n = tempList.Count;

            for (i = 0; i < n; i++)
            {
                graphics.Insert(0, tempList[i]);
            }

            return (n > 0);
        }

        /// <summary>
        /// Move selected items to back (end of the list)
        /// </summary>
        /// <returns>
        /// true if at least one object is moved
        /// </returns>
        public bool MoveSelectionToBack()
        {
            int n;
            int i;
            DrawList tempList;

            tempList = new DrawList();
            n = graphics.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for (i = n - 1; i >= 0; i--)
            {
                if ((graphics[i]).Selected)
                {
                    tempList.Add(graphics[i]);
                    graphics.RemoveAt(i);
                }
            }

            // Read temporary list in reverse order and add every item
            // to the end of the source list
            n = tempList.Count;

            for (i = n - 1; i >= 0; i--)
            {
                graphics.Add(tempList[i]);
            }

            return (n > 0);
        }

        /// <summary>
        /// Get properties from selected objects and fill GraphicsProperties instance
        /// </summary>
        /// <returns></returns>
        private GraphicsProperties GetProperties()
        {
            GraphicsProperties properties = new GraphicsProperties();

            bool bFirst = true;

            int firstColor = 0;
            int firstPenWidth = 1;

            bool allColorsAreEqual = true;
            bool allWidthAreEqual = true;

            foreach (DrawObject o in Selection)
            {
                if (bFirst)
                {
                    firstColor = o.Color.ToArgb();
                    firstPenWidth = o.PenWidth;
                    bFirst = false;
                }
                else
                {
                    if (o.Color.ToArgb() != firstColor)
                        allColorsAreEqual = false;

                    if (o.PenWidth != firstPenWidth)
                        allWidthAreEqual = false;
                }
            }


            if (allColorsAreEqual)
            {
                properties.Color = Color.FromArgb(firstColor);
            }

            if (allWidthAreEqual)
            {
                properties.PenWidth = firstPenWidth;
            }

            return properties;
        }

        /// <summary>
        /// Apply properties for all selected objects.
        /// Returns TRue if at least one property is changed.
        /// </summary>
        private bool ApplyProperties(GraphicsProperties properties)
        {
            bool changed = false;

            foreach (DrawObject o in graphics)
            {
                if (o.Selected)
                {
                    if (properties.Color.HasValue)
                    {
                        if (o.Color != properties.Color.Value)
                        {
                            o.Color = properties.Color.Value;
                            DrawObject.LastUsedColor = properties.Color.Value;
                            changed = true;
                        }
                    }

                    if (properties.PenWidth.HasValue)
                    {
                        if (o.PenWidth != properties.PenWidth.Value)
                        {
                            o.PenWidth = properties.PenWidth.Value;
                            DrawObject.LastUsedPenWidth = properties.PenWidth.Value;
                            changed = true;
                        }
                    }
                }
            }

            return changed;
        }

        /// <summary>
        /// Show Properties dialog. Return true if list is changed
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public bool ShowPropertiesDialog(DrawArea parent)
        {
            if (SelectionCount < 1)
                return false;

            GraphicsProperties properties = GetProperties();
            PropertiesDialog dlg = new PropertiesDialog();
            dlg.Properties = properties;

            CommandChangeState c = new CommandChangeState(this);

            if (dlg.ShowDialog(parent) != DialogResult.OK)
                return false;

            if ( ApplyProperties(properties) )
            {
                c.NewState(this);
                parent.AddCommandToHistory(c);
            }

            return true;
        }

        #endregion

    }
}
