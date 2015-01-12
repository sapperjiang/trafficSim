
using System;
using System.Drawing;
using System.Collections.Generic;
using SubSys_SimDriving.SysSimContext;

namespace SubSys_SimDriving.TrafficModel
{
	public class HashKeyProvider
	{
		//利用（x,y）计算新存储结构的哈希值，以支持利用x.y快速访问矩阵元素
		public static int GetHashCode(int ix, int iy)
		{
			return ix*1000+iy;//.GetHashCode();//.ToString().GetHashCode().ToString().GetHashCode() + iy.ToString().GetHashCode()).GetHashCode();
		}
		public static int GetHashCode(int iCode)
		{
			return iCode.ToString().GetHashCode();
		}
	}
	
	internal class HashMatrix<T>
	{
		/// <summary>
		/// 最大六个车道，坐标远点是RoadNode的positon
		/// </summary>
		internal readonly int iMaxWidth = SimSettings.iMaxLanes;

		private Dictionary<int, T> hashMat = new Dictionary<int, T>();
		/// <summary>
		/// 判断元胞是否被占用了
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		internal bool IsBlocked(int x, int y)
		{
			return hashMat.ContainsKey(HashKeyProvider.GetHashCode(x, y));
		}
		/// <summary>
		/// 把元宝从o点移动到d点
		/// </summary>
		internal bool Move(Point inXY, Point inXY_D)
		{
			int iHashkey = HashKeyProvider.GetHashCode(inXY.X, inXY.Y);
			T cac;
			if (hashMat.TryGetValue(iHashkey, out cac) == true)
			{
				hashMat.Remove(iHashkey);
				iHashkey = HashKeyProvider.GetHashCode(inXY_D.X, inXY_D.Y);
				if (hashMat.ContainsKey(iHashkey)==true)
				{
					return false;
				}else
				{
					hashMat.Add(iHashkey, cac);
					return true;
				}
			}
			return false;
		}
		internal void Add(int x, int y, T cell)
		{
			//更新行和列的最大索引
			if (Math.Abs(x) > this.iMaxWidth || Math.Abs(y) > this.iMaxWidth)
			{
				throw new ArgumentOutOfRangeException("x或者y 参数超出了默认的最大数值");
			}
			int iHKey = HashKeyProvider.GetHashCode(x, y);
			if (!hashMat.ContainsKey(iHKey))
			{
				hashMat.Add(iHKey, cell);
			}
		}
		internal bool Remove(int x, int y)
		{
			return hashMat.Remove(HashKeyProvider.GetHashCode(x, y));
		}
		
		internal int Count
		{
			get
			{
				return hashMat.Count;
			}
		}

		internal Dictionary<int, T>.ValueCollection Values
		{
			get { return hashMat.Values; }
		}
		#region 枚举器
		/// <summary>
		/// 提供对存储元素的高效遍历
		/// </summary>
		/// <returns></returns>
		internal IEnumerator<T> GetEnumerator()
		{
			return this.hashMat.Values.GetEnumerator();
		}
		#endregion

		internal T this[int index]
		{
			get
			{
				return hashMat[index];
			}
		}

		internal ICollection<int> Keys
		{
			get
			{
				return hashMat.Keys;
			}
		}
	}
}