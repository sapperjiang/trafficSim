
using System;
using System.Drawing;
using SubSys_MathUtility;
using System.Collections.Generic;
using SubSys_SimDriving;

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
	
	public class HashMatrix:LinkedList<MobileEntity>
	{
		/// <summary>
		/// 最大六个车道，坐标远点是RoadNode的positon
		/// </summary>
		internal readonly int iMaxWidth = SimSettings.iMaxLanes;
		
		/// <summary>
		/// to get if a point within a xnode is occupied by a mobile's shape point
		/// </summary>
		private Dictionary<int, MobileEntity> hashMat = new Dictionary<int, MobileEntity>();
		
		//private Dictionary<int, MobileEntity> hashMat = new Dictionary<int, MobileEntity>();
		/// <summary>
		/// 判断元胞是否被占用了
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		internal bool IsOccupied(OxyzPoint opP)
		{
			
//			var mobileNode = base.First;
//			//update mobile on a lane one by one
//			while(mobileNode!=null) {
//				var mobile = mobileNode.Value;
//				//mobile is possibaly be deleted
//
//				if (mobile.IsMoved==true) {
//					//switch off
//					mobile.IsMoved = false;
//
//					foreach (var prevShap in mobile.PrevShape) {
//						int iKey = prevShap.GetHashCode();
//						if (this.hashMat.ContainsKey(iKey)==true) {
//							this.hashMat.Remove(iKey);
//						}
//
//					}
//
//					foreach (var Shap in mobile.Shape) {
//						int iKey = Shap.GetHashCode();
//						if (this.hashMat.ContainsKey(iKey)==false) {
//							this.hashMat.Add(iKey,mobile);
//						}
//					}
//
//				}
//				mobileNode = mobileNode.Next;
//			}
//
//			return this.hashMat.ContainsKey(opP.GetHashCode());

			//-----------------------------------------------------------
			var mobileNode = base.First;
			//update mobile on a lane one by one
				while(mobileNode!=null) {
				var mobile = mobileNode.Value;
				//	mobile is possibaly be deleted
				foreach (var Shap in mobile.Shape) {
					if (Shap.Equals(opP)) {
						return true;
					}
				}
				mobileNode = mobileNode.Next;
			}
			
			return false;
			//-----------------------------------------------------------
		}
		
		internal void Add(MobileEntity mobile)
		{
			base.AddFirst(mobile);
			
//			int iKey ;
//			foreach (var shap in mobile.Shape) {
//				iKey = shap.GetHashCode();//struct
//				if (hashMat.ContainsKey(iKey)==false)
//				{
//					//as long as a point is occupied by a mobile ,tag this mobile
//					hashMat.Add(iKey,mobile);
//				}
//			}
		}

		internal bool Remove(MobileEntity mobile)
		{
			bool b= base.Remove(mobile);
			
//			int iKey ;
//			//when removed ,a mobiles' prevShape is within a hashMat
//			foreach (var shap in mobile.PrevShape) {
//				iKey = shap.GetHashCode();
//				//as long as a point is occupied by a mobile ,tag this mobile
//				if (hashMat.ContainsKey(iKey)==true) {
//					hashMat.Remove(iKey);
//				}
//			}
			return b;
		}
		
		internal int Count
		{
			get
			{
				return base.Count;
			}
		}

		internal Dictionary<int, MobileEntity>.ValueCollection Values
		{
			get { return hashMat.Values; }
		}
		#region 枚举器
		/// <summary>
		/// 提供对存储元素的高效遍历
		/// </summary>
		/// <returns></returns>
		public IEnumerator<MobileEntity> GetEnumerator()
		{
			return base.GetEnumerator();
		}
		#endregion


//		internal ICollection<int> Keys
//		{
//			get
//			{
//				return hashMat.Keys;
//			}
//		}
	}
	
//	public class HashMatrix<T>
//	{
//		/// <summary>
//		/// 最大六个车道，坐标远点是RoadNode的positon
//		/// </summary>
//		internal readonly int iMaxWidth = SimSettings.iMaxLanes;
//
//		private List<T> lsMat = new List<T>();
//		private Dictionary<int, T> hashMat = new Dictionary<int, T>();
//		/// <summary>
//		/// 判断元胞是否被占用了
//		/// </summary>
//		/// <param name="x"></param>
//		/// <param name="y"></param>
//		/// <returns></returns>
//		internal bool IsBlocked(int x, int y)
//		{
//			return hashMat.ContainsKey(HashKeyProvider.GetHashCode(x, y));
//		}
//
//		internal bool ContainsKey(int iKey)
//		{
//			return this.hashMat.ContainsKey(iKey);
//		}
//		internal void Add(T mobile)
//		{
//			int iHKey = mobile.GetHashCode();//.GetHashCode();
//			if (!hashMat.ContainsKey(iHKey))
//			{
//				lsMat.Add(mobile);
//				hashMat.Add(iHKey,mobile);
//
//			}
//
//			//System.Diagnostics.Debug.Assert(this.hashMat.Count == this.lsMat.Count);
//
//		}
//
//		internal bool Remove(T mobile)
//		{
//			this.lsMat.Remove(mobile);
//			int iKey = mobile.GetHashCode();
//			return hashMat.Remove(mobile.GetHashCode());
//		}
//
//		internal int Count
//		{
//			get
//			{
//				return hashMat.Count;
//				System.Diagnostics.Debug.Assert(this.hashMat.Count == this.lsMat.Count);
//			}
//		}
//
//		internal Dictionary<int, T>.ValueCollection Values
//		{
//			get { return hashMat.Values; }
//		}
//		#region 枚举器
//		/// <summary>
//		/// 提供对存储元素的高效遍历
//		/// </summary>
//		/// <returns></returns>
//		public IEnumerator<T> GetEnumerator()
//		{
//			return this.hashMat.Values.GetEnumerator();
//		}
//		#endregion
//
//		internal T this[int index]
//		{
//			get
//			{
//				return lsMat[index];
//			}
//		}
//
//		internal ICollection<int> Keys
//		{
//			get
//			{
//				return hashMat.Keys;
//			}
//		}
//	}
}