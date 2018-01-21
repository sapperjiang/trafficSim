
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

namespace SubSys_SimDriving.TrafficModel
{
    internal class AStarShortestPath
    {
        private List<asNode> openList = new List<asNode>();//open表 用于存放待考察元素
        private List<asNode> closeList = new List<asNode>();//close表 用于存放当前open表中最小代价元素
        private List<asNode> childList = new List<asNode>();//子节点表

        private char[,] Matrix;
        private int w = 0;
        private int h = 0;

        private asNode s = new asNode();//起始节点
        private asNode e = new asNode();//终止节点

        private const int evalCost_H = 10;//水平代价
        private const int evalCost_X = 14;//斜角代价

        private asNode currentNode;

        internal AStarShortestPath(char[,] _Matrix, int _h, int _w)
        {
            this.Matrix = _Matrix;

            this.w = _w;
            this.h = _h;

            for (int i = 0; i < this.h; i++)
            {
                for (int j = 0; j < this.w; j++)
                {
                    if (this.Matrix[i, j] == 'e')
                    {
                        e._x = j;
                        e._y = i;
                    }
                    else
                    if (this.Matrix[i, j] == 's')
                    {
                        s._x = j;
                        s._y = i;
                    }         
                }
            } 

            currentNode = s;

        }

        internal List<asNode> Anilysize()
        {

            List<asNode> rtn_Path = new List<asNode>();

            s.H = cacu_H(s);
            e.H = cacu_H(e);

            while (currentNode.H != 0)
            {

                loadChildNodes(currentNode);

                if (childList.Count > 0)
                {
                    foreach (asNode n in childList)
                    {
                        openList.Add(n);
                    }
                }

                if (openList.Count > 0)
                {               
                    //openList.Sort(new NodeComparer());

                    closeList.Add(openList[0]);

                    asNode t = openList[0];
                    Matrix[t._y, t._x] = 'r';
               
                    openList.RemoveAt(0);
                }
                else
                {
                    return null;
                }

                loadChildNodes(closeList[closeList.Count - 1]);

                if (childList.Count == 0 && !IsEnd(closeList[closeList.Count - 1]))
                {
                    closeList.RemoveAt(closeList.Count - 1);
                }

                if (closeList.Count != 0)
                {
                    currentNode = closeList[closeList.Count - 1];
                }
                else
                {
                    currentNode = s;
                }            
            }
            
            asNode end_tag=closeList[closeList.Count-1].Parent;

            while (end_tag.Parent!=s)
            {
                rtn_Path.Add(end_tag);
                end_tag = end_tag.Parent;

            }

            rtn_Path.Add(end_tag);

            return rtn_Path;

        }

        private int calc_G(asNode node)//水品代价计算函数
        {
            if (node._x != currentNode._x || node._y != currentNode._y)
            {
                return currentNode.G + evalCost_X;
            }
            else 
            {
                return currentNode.G + evalCost_H; 
            }        
        }

        private int cacu_H(asNode node)//曼哈顿法估算函数(斜角代价计算函数)
        {
            return 10 * (Math.Abs(node._x - e._x) + Math.Abs(node._y - e._y));// ;
        }

        private void loadChildNodes(asNode p)
        {
            childList.Clear();
            
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (!(i == 0 && j == 0))
                    {
                        if ( Matrix[i + p._y, j + p._x] != 'o' && 

                                Matrix[i + p._y, j + p._x] != 'r'  && 

                                Matrix[i + p._y, j + p._x] != 's' )
                        {                            

                            asNode n = new asNode();

                            n._x = p._x + j;
                            n._y = p._y + i;

                            n.G = calc_G(n);
                            n.H = cacu_H(n);

                            n.Parent = currentNode;

                            childList.Add(n);

                        }
                    }

                }
            }

            if (Matrix[p._y, p._x - 1] == 'o')
            {
                delfromChildList(p._x - 1, p._y - 1);
                delfromChildList(p._x - 1, p._y + 1);
            }

            if (Matrix[p._y-1,p._x ] == 'o')
            {
                delfromChildList(p._x - 1, p._y - 1);
                delfromChildList(p._x + 1, p._y - 1);
            }

            if (Matrix[p._y,p._x + 1 ] == 'o')
            {
                delfromChildList(p._x + 1, p._y + 1);
                delfromChildList(p._x + 1, p._y - 1);
            }

            if (Matrix[p._y + 1 , p._x] == 'o')
            {
                delfromChildList(p._x - 1, p._y + 1 );
                delfromChildList(p._x + 1, p._y + 1);
            }

            foreach (asNode n in openList)
            {
                for (int i = 0; i < childList.Count; i++)
                {
                    if (n._x == childList[i]._x && n._y == childList[i]._y)
                    {
                        childList.RemoveAt(i);
                        break;
                    }
                }
            }

        }

        private bool IsEnd(asNode n)
        {
            //。。。。。。
            return false;

         }

        void delfromChildList(int x,int y)
        {
            //。。。。。。
         }

    }
    internal class asNode
    {

        internal int G = 0;
        internal int H = 0;

        internal int _x = 0;
        internal int _y = 0;
        internal asNode Parent;

        internal  asNode()
        {             
            G=0;
            H=0;

            _x=0;
            _y=0;

        }

        internal int F
        {
           get
           {
               return this.G + this.H;
           }
        
        }


        public static asNode operator <(asNode n1, asNode n2)
        {
            return n1.H < n2.H ? n1 : n2;
        }

 
        public static asNode operator >(asNode n1, asNode n2)
        {
            return n1.H > n2.H ? n1 : n2;
        }
        
    }
    internal class PathNode : IComparable<PathNode>
  {
   internal int G;
   internal int H;
   internal int F {
       get{
           return G + H;
       }
   }

      private int GValue(int parent)
    {
        int d = 10;
        return d + parent;
    }
    private int HValue(int x, int y, Point end)
    {
        return (Math.Abs(x - end.X) + Math.Abs(y - end.Y)) * 10;
    }
    internal PathNode Parent;
    internal Point Position;

    internal PathNode(Point pos)
    {
        this.Position = pos;
        this.Parent = null;
        this.G = 0;
        this.H = 0;
    }

    public override string ToString()
    {
        return Position.ToString();
    }

   
    public int CompareTo(PathNode other)
    {
        return F - other.F;
    }

    private List<PathNode> unLockList = new List<PathNode>();
    private Dictionary<string, PathNode> lockList = new Dictionary<string, PathNode>();
    private List<PathNode> path = new List<PathNode>();


    internal List<PathNode> FindPath()
{
   unLockList.Clear();
   lockList.Clear();
   path.Clear();
   //doFindPath();
   path.Reverse();
   return path;
}

    private PathNode __pn;
    private bool isPointInUnlockList(Point src)
{
    __pn = null;
    foreach (PathNode item in unLockList)
    {
        if (item.Position.Equals(src))
        {
            __pn = item;
            return true;
        }

    }
    return false;
}

     private bool canWalkOnIt(Point node)
{
    if (node.X < 0 || node.Y < 0)
        return false;
    //if (node.X > Width - 1 || node.Y > Height - 1)
        return false;
    //return GetNodeValue(node.X, node.Y) >= 0;
}

  }
}