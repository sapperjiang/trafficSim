using SubSys_SimDriving;
using SubSys_SimDriving.UpdateRule;
using System.Collections.Generic;
using System;

namespace SubSys_SimDriving
{
	public abstract class RoadEntity : TrafficEntity
	{
        /// <summary>
        /// 元胞车辆的宽度是6
        /// </summary>
        protected static int iCellWidth = 6;

        public int iWidth4Test = 120;

        public int iWidth;
		 
		public int iLength;


        /// <summary>
        /// 存储边上定义的异步更新的规则
        /// </summary>
        public AsynchronicUpdateRuleChain asynRuleChain;

        /// <summary>
        /// 存储边上定义的同步更新的规则
        /// </summary>
        public SynchronicUpdateRuleChain synRuleChain;

        /// <summary>
        /// 调用所有的访问者，进行内部元胞的更新
        /// </summary>
        public abstract void UpdateStatus();
        //{
        //    //更新异步消息
        //    for (int i = 0; i < this.asynRuleChain.Count; i++)
        //    {
        //        UpdateRule.UpdateRule visitorRule = this.asynRuleChain[i];
        //        visitorRule.VisitUpdate(this);//.VisitUpdate();
        //    }
        //    ////更新同步消息
        //    //foreach (UpdateRule.UpdateRule item in this.synRuleChain)
        //    //{
        //    //    if (item != null)
        //    //    {
        //    //        item.Update();//visitor.visit 一个规则就是一个访问者，很多的访问者
        //    //    }
        //    //}
        //}

        /// <summary>
        ///RoadEdge是item ，rule是visitor 相当于item.accept(visitor)
        /// </summary>
        /// <param name="ur"></param>
        public void AcceptSynchronicRule(UpdateRule.UpdateRule ur)
        {
            if (ur != null)
            {
                //添加到仿真上下文
                this.SimDrivingContext._updateRuleHashTable.Add(ur.GetHashCode(), ur);

                this.synRuleChain.Add(ur);
            }
            else
            {
                throw new ArgumentNullException("空的更新规则");
            }
        }

        public void AcceptAsynchronicRule(UpdateRule.UpdateRule ur)
        {
            if (ur != null)
            {
                //添加到仿真上下文
                this.SimDrivingContext._updateRuleHashTable.Add(ur.GetHashCode(), ur);
                this.asynRuleChain.Add(ur);
            }
            else
            {
                throw new ArgumentNullException("空的更新规则");
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
		 
	}
	 
}
 
