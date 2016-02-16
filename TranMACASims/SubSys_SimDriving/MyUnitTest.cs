/*
 * 由SharpDevelop创建。
 * 用户： sapperjiang
 * 日期: 2016/2/5
 * 时间: 12:12
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using NUnit.Framework;
using SubSys_SimDriving.TrafficModel;




namespace SubSys_SimDriving
{
	[TestFixture]
	public class MyUnitTest
	{
		[Test]
		public void TestMethod()
		{
			// TODO: Add your test.
			
//			EntityShape es = new EntityShape();
//
//			OxyzPoint first=  new OxyzPoint(8,20,0);
//
//			OxyzPoint last=  new OxyzPoint(10,20,0);
//
//			es.Add(first);
			////			es.Add(0
//			es.Add(new OxyzPoint(9,20,0));
//			//es.Add(new OxyzPoint(9,20,0));
//
//			es.Add(last);
//
//			int i  = es.GetIndex(first);
//			Assert.AreEqual(0,i,"RIGHT");
//
//			Assert.AreEqual(2, es.GetIndex(last),"RIGHT");


			IFactory iabstractFacotry=new StaticFactory();
			
			
			
			int iRoadWidth = 6;
			int iBase = 2;
			var rnA= iabstractFacotry.Build(new XNodeBuildCmd(new Point(8,20)),EntityType.XNode) as XNode;
			var rnB = iabstractFacotry.Build(new XNodeBuildCmd(new Point(28, 20)), EntityType.XNode) as XNode;
			
			var  way =  new Way(rnA,rnB);/// Way();//new Lane(;
			RoadNet   roadNetwork = RoadNet.GetInstance();// SubSys_SimDriving.SimContext.GetInstance().;//.sim;SimController.ISimCtx.RoadNet;
			
			roadNetwork.AddXNode(rnA);
			roadNetwork.AddXNode(rnB);
			roadNetwork.AddWay(way);
			
			//roadNetwork.AddXNode(rnC);
			
			for (int i = 0; i < 2; i++)//直行
			{
				way.AddLane(LaneType.Straight);
				//re.GetReverse().AddLane(LaneType.Straight);
			}

			var Testlane =way.Lanes[0];
			
			
			//        lane.Container = new


			Assert.AreEqual(way.Shape.Start,Testlane.Shape.Start);//
			//Assert.AreEqual(
			//Assert.
			//System.Console.WriteLine(i.ToString());
		}
	}
}
