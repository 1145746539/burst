using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OEE.Util;


namespace OEE.bean
{
    /// <summary>
    /// 保存读取实时数据值的类
    /// </summary>
    public class RealTimeData
    {
        ModBusTools modtool = new ModBusTools();

        /// <summary>
        /// 程序号字段
        /// </summary>
        public string _codeindex;

        /// <summary>
        /// 测试模式字段
        /// </summary>
        public string _testmode;

        /// <summary>
        /// 仪器测试状态字段
        /// </summary>
        public string _teststatus;

        /// <summary>
        /// 测试阶段字段
        /// </summary>
        public string _testprocess;

        /// <summary>
        /// 测试压力字段
        /// </summary>
        public string _pressurevalue;

        /// <summary>
        /// 压力单位字段
        /// </summary>
        public string _pressureunit;

        /// <summary>
        /// 泄漏量字段
        /// </summary>
        public string _leakvalue;

        /// <summary>
        /// 泄漏量单位字段
        /// </summary>
        public string _leakunit;

        /// <summary>
        /// 程序号
        /// </summary>
        public string CodeIndex
        {
            get
            {
                string index = "";
                if (_codeindex == null)
                {
                    return "-9999";
                }
                index = (modtool.GetNumber(_codeindex)+1).ToString();
                return index;
            }
        }
        /// <summary>
        /// 测试模式
        /// </summary>
        public string TestMode
        {
            get
            {
                string testmode = "-9999";
                if (_testmode != null)
                {
                    switch (_testmode)
                    {
                        case "0100":
                            testmode = "Leak";
                            break;
                        case "0500":
                            testmode = "Burst";
                            break;
                    }
                }
                return testmode;
            }
        }

        /// <summary>
        /// 仪器测试状态
        /// </summary>
        public string TestStatus
        {
            get
            {
                string status = "-9999";
                if (_teststatus == null)
                {
                    return status;
                }
                string status_temp = Convert.ToString(Convert.ToInt32(_teststatus,16), 2).PadLeft(16, '0');
                //burst  8000测试中   8028空闲 8001结束
                //leak 8000测试中 8002 泄气 8022空闲
                switch (_testmode)
                {
                    case "0100": //Leak
                        if (_teststatus == "8022") //空闲
                        {
                            status = "Cycle_End";
                        }
                        else
                        {
                            if (status_temp[0] == '1')
                                status = "通过";
                            else
                                status = "Alarm";
                        }
                        break;
                    case "0500": //Burst
                        //if (status_temp[5] == '1' || status_temp[15] == '1') //正常结束

                        if (_teststatus == "8001" || _teststatus == "8028") //正常结束 || 空闲
                        {
                            status = "Cycle_End";
                        }
                        else
                        {
                            if (status_temp[0] == '1')
                                status = "通过";
                            else
                                status = "Alarm";
                        }

                        break;
                }

                
                return status;
            }
        }
        /// <summary>
        /// 测试阶段
        /// </summary>
        public string TestProcess
        {
            get
            {
                string process = null;
                if (_testprocess != null)
                {
                    int index = modtool.GetNumber(_testprocess.PadRight(8, '0'));
                    process = Enum.GetName(typeof(Step), index);
                }
                if (process == null)
                {
                    WriteLog.WriteOrCreateLog(@"D:\equipment\logtestprocess", "测试阶段" + _testprocess);
                }
               

                return process == null ? "-9999" : process;
            }
        }
        /// <summary>
        /// 测试压力
        /// </summary>
        public string PressureValue
        {
            get
            {
                double temp = (double)modtool.GetNumber(_pressurevalue) / 1000;
                return temp.ToString(); ;
            }
        }
        /// <summary>
        /// 压力单位
        /// </summary>
        public string PressUnit
        {
            get
            {
                int index = modtool.GetNumber(_pressureunit) / 1000;
                string name = Enum.GetName(typeof(Unit), index);
                return name == null ? "-9999" : name;
            }
        }
        /// <summary>
        /// 泄漏量值
        /// </summary>
        public string LeakValue
        {
            get
            {
                double temp = (double)modtool.GetNumber(_leakvalue) / 1000;
                return temp.ToString();
            }
        }
        /// <summary>
        /// 泄漏量单位
        /// </summary>
        public string LeakUnit
        {
            get
            {
                int index = modtool.GetNumber(_leakunit) / 1000;
                string name = Enum.GetName(typeof(Unit), index);
                return name == null ? "-9999" : name;
            }
        }
        /// <summary>
        /// 此数据的状态。
        /// 用于软件数据处理过程中，判断该数据是否正确转换
        /// </summary>
        public string DataState { get; set; }
    }
}
