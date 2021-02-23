using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEE.bean
{
    class ParaObject
    {
        /// <summary>
        /// 中文解释
        /// </summary>
        public string Chinese { get; set; }

        /// <summary>
        /// 英文ID
        /// </summary>
        public string English { get; set; }

        /// <summary>
        /// 写入地址
        /// </summary>
        public string Write { get; set; }

        /// <summary>
        /// 读取地址
        /// </summary>
        public string Read { get; set; }

        /// <summary>
        /// 读取字数
        /// </summary>
        public string ReadWord { get; set; }

        /// <summary>
        /// 写入字数
        /// </summary>
        public string WriteWord { get; set; }


    }

    class SimTestPara
    {
        public string Pattern{get;set;}                //模式
        public string FILLTIME{get;set;}               //充气时间
        public string STABTIME{get;set;}               //稳压时间
        public string TESTTIME{get;set;}               //测试时间
        public string DUMPTIME{get;set;}               //泄漏时间
        public string PRESSUNIT{get;set;}              //压力单位
        public string MINFILL{get;set;}                //最小压力
        public string MAXFILL{get;set;}                //最大压力
        public string SETFILL{get;set;}                //目标压力	
        public string LEAKUNIT{get;set;}               //泄漏单位	
        public string TESTFAIL{get;set;}               //测试失败量 正
        public string REFFAIL{get;set;}
        public string RAMP{get;set;}                   //斜率
        public string MEAS_START{get;set;}             //等待时间
        public string T_LEVEL{get;set;}                //Step time in burst test mode
        public string STARTFILL{get;set;}              //开始充气
        public string DROPPRESS{get;set;}              //降压范围%
        public string N_OF_STEPS{get;set;}
    }
}
