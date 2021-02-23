using DevComponents.DotNetBar.Controls;
using OEE.bean;
using OEE.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;

namespace OEE.BLL
{
    internal class ParaService
    {
        /// <summary>
        /// 参数键值对
        /// </summary>
        public static Dictionary<string, string> dicPara = new Dictionary<string, string>();
        /// <summary>
        /// 程序常用参数存储及加载路径
        /// </summary>
        public static readonly string _iniFilePath = Application.StartupPath + "\\" + "Para.ini";
        /// <summary>
        /// Software参数存储及加载路径
        /// </summary>
        public static readonly string _iniSoftwarePath = Application.StartupPath + "\\" + "Software.ini";
        /// <summary>
        /// modbus工具类
        /// </summary>
        private ModBusTools mbTool = new ModBusTools();
        /// <summary>
        /// 主窗体对象
        /// </summary>
        public Form1 form;
        /// <summary>
        /// 显示信息
        /// </summary>
        public static string infostring = "";
        /// <summary>
        /// 软件参数与串口参数
        /// </summary>
        private Parameter hp = new Parameter();
        /// <summary>
        /// 爆破模式参数
        /// </summary>
        private PatternBurst pattern1 = new PatternBurst();
        /// <summary>
        /// leak模式参数
        /// </summary>
        private PatternLeak pattern2 = new PatternLeak();

        private PortServcie pps = null;
        /// <summary>
        /// 读取参数时判断该操作是否已经确定模式了。
        /// </summary>
        public static string ReadMode;
       


        /// <summary>
        /// 读取进度
        /// </summary>
        public static int ReadNum;
        /// <summary>
        /// 读取参数的程序号
        /// </summary>
        public static string CodeIndex;

        private static ParaService ps = null;
        private ParaService() { }

        public static ParaService Install()
        {
            if (ps == null)
            {
                ps = new ParaService();
            }
            return ps;
        }

        /// <summary>
        /// 参数保存到INI
        /// </summary>
        /// <param name="ctrl"></param>
        public void SetInfo(Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                if (c.HasChildren)
                {
                    SetInfo(c);
                }
                if (c is TextBox)
                {
                    TextBox txt = c as TextBox;
                    DisposeINIW(txt.Name, txt.Text);
                }
                if (c is ComboBoxEx) // 控件
                {

                    ComboBoxEx cb = c as ComboBoxEx;
                    DisposeINIW(cb.Name, cb.Text);
                }
            }
        }

        //1.读取参数 -- 根据程序号读取模式
        //2.根据模式。选择对应的枚举。将枚举中所有的字段值去匹配List中功能号。。。
        //串口发送modbus

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port">串口对象</param>
        /// <param name="list">功能集合</param>
        /// <param name="node">程序号(不用减一)</param>
        internal void ReadATQToINI(SerialPort port, string node)
        {
            pps = PortServcie.Install();
            // 第一步 选择程序号
            ModBusTools modbustool = new ModBusTools();
            CodeIndex = node;
            pps.WriteByte(port, modbustool.GetSoftwareID(node));
            string ReadmodeCode = "FF0320150001";
            ReadmodeCode = ReadmodeCode + modbustool.CRC(ReadmodeCode);
            PortServcie.IsReadMode = true;
            pps.WriteByte(port, ReadmodeCode);
           

        }


        /// <summary>
        /// 设置当前参数到ATQ设备
        /// </summary>
        /// <param name="list"> 功能集合 </param>
        /// <param name="groupBox"> 包含需设置的参数 </param>
        /// <param name="Pattern"> 模式 </param>
        /// <param name="node"> 程序号 </param>
        internal void SetATQPara(SerialPort port, List<ParaObject> list, GroupBox groupBox, CheckBox Pattern, string node)
        {

            pps = PortServcie.Install();
            // 第一步 选择程序号
            string ID = mbTool.GetSoftwareID(node);
            infostring = "程序号";
            pps.WriteByte(port, ID);

            //第二步  设置模式 (1) 爆破 or leak
            infostring = "模式";
            bool pattern = LoadPattern(Pattern, node); //爆破 true  leak false

            string command = "";
            if (pattern)
            {
                command = "FF106015000204881300007710"; //burst
                pps.WriteByte(port, command);
            }
            else
            {
                command = "FF106015000204e803000068D5"; // leak
                pps.WriteByte(port, command);
            }
            Form1.form1.AddInformation("设置"+Pattern.Name+"模式参数");
            //第三步 设置各种参数

            foreach (Control c in groupBox.Controls)
            {

                if (c is TextBox)
                {
                    TextBox tb = c as TextBox;
                    string value = tb.Text;
                    double outdu;
                    if (double.TryParse(value, out outdu))
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (tb.Name == list[i].English)
                            {
                                Form1.form1.AddInformation("设置" + list[i].Chinese + "模式参数");
                                //设置进度条到哪了
                                form.progressBar_Setting.Value = i + 1;
                                ParaObject para = list[i];
                                string ateqInfo = mbTool.GetATEQString(false, para.WriteWord, para.Write, outdu);
                                infostring = para.Chinese;
                                //INI文件中值与tb.text不一致 设置到仪器里
                                string checkValue = INIHelper.Read(node, tb.Name, "", _iniSoftwarePath);
                                if (checkValue != value)
                                {
                                    pps.WriteByte(port, ateqInfo);
                                }

                            }
                        }
                    }


                }

            }
            Form1.form1.AddInformation("参数保存在机台成功.");
        }



        /// <summary>
        /// 保存动态的参数
        /// </summary>
        /// <param name="ctr">父容器</param>
        /// <param name="ck">选择模式</param>
        /// <param name="node">程序号</param>
        public void SetSoftware(Control ctr, CheckBox ck, string node)
        {
            string ckStatus = ck.Checked.ToString();
            WriteINI(node, ck.Name, ckStatus);
            foreach (Control c in ctr.Controls)
            {
                if (c is TextBox)
                {
                    TextBox tb = c as TextBox;
                    WriteINI(node, tb.Name, tb.Text.Trim());
                }

            }
            Form1.form1.AddInformation("参数保存在本地成功.");
        }

        public void WriteINI(string node, string key, string value)
        {
            if (!File.Exists(_iniSoftwarePath))
            {
                File.Create(_iniSoftwarePath).Close();
            }

            //爆破模式 与正常模式都可以存储
            bool isTry = Enum.TryParse(key, out pattern1) || Enum.TryParse(key, out pattern2);
            if (isTry)
            {
                INIHelper.Write(node, key, value, _iniSoftwarePath);

            }
        }

        /// <summary>
        /// 加载参数到GroupBox
        /// </summary>
        /// <param name="ctr">父容器</param>
        /// <param name="ck">选择模式</param>
        /// <param name="node">程序号</param>
        /// <param name="IsInitialCheckBox">是否初始化CheckBox</param>
        public void LoadParaToGroupBox(Control ctr, CheckBox ck, string node, bool IsInitialCheckBox)
        {
            if (IsInitialCheckBox)
            {
                ck.Checked = LoadPattern(ck, node);
            }
            foreach (Control c in ctr.Controls)
            {
                if (c is TextBox)
                {
                    TextBox tb = c as TextBox;
                    string value = ReadINI(node, tb.Name);
                    tb.Text = value;
                }

            }
            Form1.form1.AddInformation("参数加载完成.");
        }

        /// <summary>
        /// 加载选择模式 burst or leak
        /// </summary>
        /// <param name="ck"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool LoadPattern(CheckBox ck, string node)
        {
            string readBool = ReadINI(node, ck.Name);
            if (string.IsNullOrEmpty(readBool))
            {
                readBool = "true";
            }
            return Convert.ToBoolean(readBool);
        }

        private string ReadINI(string node, string key)
        {
            string v = null;
            if (!File.Exists(_iniSoftwarePath)) //无文件
            {
                return v;
            }

            //爆破模式 与正常模式都可以读取
            bool isTry = Enum.TryParse(key, out pattern1) || Enum.TryParse(key, out pattern2);
            if (isTry)
            {
                v = INIHelper.Read(node, key, null, _iniSoftwarePath);
            }
            return v;
        }


        /// <summary>
        /// 从INI中读取参数
        /// </summary>
        /// <param name="ctrl"></param>
        public void GetInfo(Control ctrl)
        {

            foreach (Control c in ctrl.Controls)
            {
                if (c.HasChildren)
                {
                    GetInfo(c);
                }
                if (c is TextBox)
                {
                    TextBox txt = c as TextBox;
                    string value = DisposeINIR(txt.Name);
                    txt.Text = value;
                }
                if (c is ComboBoxEx) // 控件
                {
                    ComboBoxEx cb = c as ComboBoxEx;
                    string value = DisposeINIR(cb.Name);
                    cb.Text = value;
                }
            }
        }

        /// <summary>
        /// 写入处理
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void DisposeINIW(string key, string value)
        {
            bool isTry = Enum.TryParse(key, out hp);
            if (isTry)
            {
                INIHelper.Write("Parameter", key, value, _iniFilePath);
                saveDic(key, value);
            }
        }

        /// <summary>
        /// 读取处理
        /// </summary>
        public string DisposeINIR(string key)
        {
            string text = "";
            bool isTry = Enum.TryParse(key, out hp);
            if (isTry)
            {
                text = INIHelper.Read("Parameter", key, "", _iniFilePath);
                saveDic(key, text);
            }
            return text;
        }



        /// <summary>
        /// 检查INI文件
        /// </summary>
        public bool checkINI()
        {

            if (!File.Exists(_iniFilePath))
            {
                File.Create(_iniFilePath);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取本机串口
        /// </summary>
        /// <returns></returns>
        public string[] GetPort()
        {
            string[] str = SerialPort.GetPortNames();
            return str;
        }

        /// <summary>
        /// 保存参数到键值对
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private void saveDic(string name, string value)
        {
            if (dicPara.ContainsKey(name))
            {
                dicPara[name] = value;
            }
            else
            {
                dicPara.Add(name, value);
            }
        }
        /// <summary>
        /// 软件参数与串口参数
        /// </summary>
        private enum Parameter
        {

            Factory,            //厂区
            Building,           //楼栋
            Floor,              //楼层
            Line,               //线体
            Project,            //专案名
            Ename,              //设备名称
            EquipmentID,        //设备编号
            Port,               //串口号
            BaudRate,           //波特率
            DataBits,           //数据位
            StopBit,            //停止位
            Parity,             //校检位
        };





    }

    /// <summary>
    /// 爆破模式参数
    /// </summary>
    internal enum PatternBurst : int
    {
        Pattern,                //模式
        RAMP,                   //斜率
        MEAS_START,             //等待时间
        T_LEVEL,                //Step time in burst test mode
        DUMPTIME,               //泄漏时间
        PRESSUNIT,              //压力单位
        MINFILL,                //最小压力
        MAXFILL,                //最大压力
        STARTFILL,              //开始充气
        DROPPRESS,              //降压范围%
        SETFILL,                //目标压力	
        N_OF_STEPS,             //Step number in burst test mode
    }

    /// <summary>
    /// 正常测试参数
    /// </summary>
    internal enum PatternLeak
    {
        Pattern,                //模式
        FILLTIME,               //充气时间
        STABTIME,               //稳压时间
        TESTTIME,               //测试时间
        DUMPTIME,               //泄漏时间
        PRESSUNIT,              //压力单位
        MINFILL,                //最小压力
        MAXFILL,                //最大压力
        SETFILL,                //目标压力	
        LEAKUNIT,               //泄漏单位	
        TESTFAIL,               //测试失败量 正
        REFFAIL                 //测试失败量 负
    }

}
