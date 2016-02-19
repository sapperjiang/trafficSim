/*
 * 由SharpDevelop创建。
 * 用户： sapperjiang
 * 日期: 2016/1/30
 * 时间: 10:51
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using SubSys_SimDriving;
using SubSys_SimDriving.TrafficModel;
using SubSys_MathUtility;
	
namespace test
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void Button1Click(object sender, EventArgs e)
		{
			CP cp = new CP();
			
			BP bp = new BP();
			
			MessageBox.Show(cp.A.b.ToString());
			cp.A.b  =1;
				
			MessageBox.Show(cp.A.b.ToString());
			
			MessageBox.Show(bp.A.b.ToString());
			
			
		}
		void Button2Click(object sender, EventArgs e)
		{
			
			//MobileEntity me = new MobileEntity(new Lane(LaneType.Straight));
			
			OxyzPoint op = new OxyzPoint(28,20,0);
//			op = this..NextPoint(op);
			MessageBox.Show(op._X.ToString()+op._Y.ToString());
			
			
			
//			IFactory ifacotry=new StaticFactory();
//
//			var rnA= ifacotry.Build(new XNodeBuildCmd(new Point(8,20)),EntityType.XNode) as XNode;
//			var rnB = ifacotry.Build(new XNodeBuildCmd(new Point(28, 20)), EntityType.XNode) as XNode;
//			
//			var  way =  new Way(rnA,rnB);/// Way();//new Lane(;
//			RoadNet   roadNetwork = RoadNet.GetInstance();// SubSys_SimDriving.SimContext.GetInstance().;//.sim;SimController.ISimCtx.RoadNet;
//			
//			roadNetwork.AddXNode(rnA);
//			roadNetwork.AddXNode(rnB);
//			roadNetwork.AddWay(way);
//			
//			//roadNetwork.AddXNode(rnC);
//			
//			for (int i = 0; i < 2; i++)//直行
//			{
//				way.AddLane(LaneType.Straight);
//				//re.GetReverse().AddLane(LaneType.Straight);
//			}
//
//			var Testlane =way.Lanes[0];
//			
//			
//			//        lane.Container = new
//
//
//			foreach (var element in Testlane.Shape) {
//				MessageBox.Show(element.ToString());
			//}
			
			//Assert.AreEqual(way.Shape.Start,Testlane.Shape.Start);//
		}
		
		
		class Tets
		{
			public  int b=0  ;
		}
		
		
		class BASS
		{
			protected Tets a;
			
			public Tets A{
				
				get {
					if (this.a == null) {
						return a = new Tets();
					}
					return a;
				}
			}
			
		}
		
		
		class CP:BASS
		{
			public Tets A{
				
				get {
					return base.A;
				}
			}
		}
		
		
		class BP:BASS
		{
			public Tets A{
				
				get {
					return base.A;
				}
			}
			
		}
		
	}
}