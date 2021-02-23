using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEE.bean
{
    /// <summary>
    /// 测漏仪对象
    /// </summary>
    class Detector
    {
        /// <summary>
        /// 爆破模式
        /// </summary>
        public bool _IsBuRst = false;

        public string ID { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 测漏仪状态值
        /// </summary>
        public int _Connect = 1;

        /// <summary>
        /// 侧漏仪状态
        /// </summary>
        public string Status
        {
            get
            {
                ConnectStatus value = (ConnectStatus)_Connect;
                return value.ToString();
            }
        }

        /// <summary>
        /// 实时结果
        /// </summary>
        public string TResult { get; set; }

        /// <summary>
        /// 最终结果
        /// </summary>
        public string LastResult { get; set; }

        /// <summary>
        /// 最终结果 bool值 
        /// </summary>
        public bool BoolLastResukt { get; set; }

        /// <summary>
        /// 气密仪忙碌
        /// </summary>
        public bool Busy { get; set; }

        /// <summary>
        /// 气密仪测试计时器
        /// </summary>
        public Stopwatch sw_TestDealy = new Stopwatch();

    }
    enum ConnectStatus : int
    {
        //"准备", "充气", "稳压", "测试", "泄气"
        空闲 = 1,
        准备,              //准备2
        充气,             //3
        稳压,             //4
        测试,             //5
        保压,             //6
        泄气,             //7
    };
}
