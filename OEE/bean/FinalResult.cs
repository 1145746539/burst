using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OEE.Util;

namespace OEE.bean
{
    /// <summary>
    /// 最终测试结果类
    /// </summary>
    public class FinalResult
    {
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
        /// 报警状态
        /// </summary>
        public string _alarm;

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

        ModBusTools modtool = new ModBusTools();
        /// <summary>
        /// 程序号
        /// </summary>
        public string CodeIndex
        {
            get
            {
                if (_codeindex == null)
                {
                    return "-9999";
                }
                string index = (modtool.GetNumber(_codeindex) + 1).ToString();
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
        /// 测试状态
        /// </summary>
        public string TestStatus
        {
            get
            {
                //有alarm时显示
                if (modtool.GetNumber(_alarm) != 0)
                {
                    return "NG";
                }
                string status = "unused";
                string status_temp = Convert.ToString(Convert.ToInt32(_teststatus, 16), 2).PadLeft(16, '0');
                //leak模式显示
                if (status_temp[status_temp.Length - 1] == '1')
                {
                    status = "通过";

                }
                if (status_temp[status_temp.Length - 2] == '1')
                {
                    status = "fail part, maximum flow reject";

                }
                if (status_temp[status_temp.Length - 3] == '1')
                {
                    status = "fail part, minimum flow reject";
                }
                if (status_temp[status_temp.Length - 4] == '1')
                {
                    status = "Alarm";
                }

                //if ( _testmode == "0500") //爆破模式显示
                //{
                //    switch (status)
                //    {
                //        case "通过":
                //            status = "NG";
                //            break;
                //        default:
                //            status = "OK";
                //            break;
                //    }
                //}



                return status;
            }

        }
        /// <summary>
        /// 报警状态
        /// </summary>
        public string Alarm
        {
            get
            {
                int index = modtool.GetNumber(_alarm);
                string output = Enum.GetName(typeof(AlarmCode), index);
                if (output == null)
                {
                    return "未知错误";
                }
                return output;
            }
        }
        /// <summary>
        /// 压力值
        /// </summary>
        public string PressureValue
        {
            get
            {
                double temp = (double)modtool.GetNumber(_pressurevalue) / 1000;
                return temp.ToString();
            }
        }
        /// <summary>
        /// 压力单位
        /// </summary>
        public string PressureUnit
        {
            get
            {
                int index = modtool.GetNumber(_pressureunit) / 1000;
                return Enum.GetName(typeof(Unit), index);
            }
        }
        /// <summary>
        /// 泄漏值
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
        /// 泄露单位
        /// </summary>
        public string LeakUnit
        {
            get
            {
                int index = modtool.GetNumber(_leakunit) / 1000;
                return Enum.GetName(typeof(Unit), index);
            }
        }

        /// <summary>
        /// 测试结果 OK or NG
        /// </summary>
        public string Result
        {
            get
            {
                //有alarm时显示
                if (modtool.GetNumber(_alarm) != 0)
                {
                    return "NG";
                }
                string _result = "NG";
                string status_temp = Convert.ToString(Convert.ToInt32(_teststatus, 16), 2).PadLeft(16, '0');
                if (_testmode != null)
                {
                    if (status_temp[status_temp.Length - 1] == '1')
                    {
                        _result = "OK";
                    }

                    //switch (_testmode)
                    //{
                    //    case "0100":    //"Leak"
                    //        if (status_temp[status_temp.Length - 1] == '1')
                    //        {
                    //            _result = "OK";
                    //        }
                    //        break;
                    //    case "0500":    //Burst 爆破模式下 检测结果判定与leak相反
                    //        if (status_temp[status_temp.Length - 1] != '1')
                    //        {
                    //            _result = "OK";
                    //        }
                    //        break;
                    //}
                }
                return _result;
            }
        }

        /// <summary>
        /// 产品名称号
        /// </summary>
        public String Product { get; set; }

        /// <summary>
        /// 此数据的状态。
        /// 用于软件数据处理过程中，判断该数据是否正确转换
        /// </summary>
        public string DataState { get; set; }
    }
}
