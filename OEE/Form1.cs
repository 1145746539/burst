#define xxc

using DevComponents.DotNetBar;
using OEE.bean;
using OEE.BLL;
using OEE.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace OEE
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 计时器
        /// </summary>
        public static Stopwatch m_StopWatch = new Stopwatch();
        /// <summary>
        /// 串口服务类
        /// </summary>
        private PortServcie m_PortParaServcie = new PortServcie();
        /// <summary>
        /// 串口对象
        /// </summary>
        public SerialPort port = new SerialPort();
        /// <summary>
        /// 爆破模式参数
        /// </summary>
        private PatternBurst pattern1 = new PatternBurst();
        /// <summary>
        /// 正常测试参数
        /// </summary>
        private PatternLeak pattern2 = new PatternLeak();
        /// <summary>
        /// 实时刷新区控件
        /// </summary>
        public List<Control> listConReal = new List<Control>();
        /// <summary>
        /// 最后结果显示控件
        /// </summary>
        public List<Control> listConLast = new List<Control>();
        /// <summary>
        /// 实时数据队列
        /// </summary>
        public static Queue<RealTimeData> queueReal = new Queue<RealTimeData>();
        /// <summary>
        /// 最后测试结果队列
        /// </summary>
        public static Queue<FinalResult> queueLast = new Queue<FinalResult>();
        /// <summary>
        /// 图表显示队列
        /// </summary>
        public Queue<PointXY> queueData = new Queue<PointXY>(4000);
        /// <summary>
        /// 图形显示窗口
        /// </summary>
        private ChartShow cshow = new ChartShow();
        /// <summary>
        /// 是否发送实时数据指令
        /// </summary>
        public static bool isSend = false;
        /// <summary>
        /// 是否清空实时显示区
        /// </summary>
        public static bool isClear = false;
        /// <summary>
        /// modbus通讯
        /// </summary>
        private ModBusTools mbtool = new ModBusTools();
        /// <summary>
        /// PLC启动标志位 1=》启动
        /// </summary>
        private int data;
        /// <summary>
        /// mx链接PLC类
        /// </summary>
        public MxCommuniUtil mx = null;
        /// <summary>
        /// 产量
        /// </summary>
        private double input = 0;
        /// <summary>
        /// OK数
        /// </summary>
        private double in_OK = 0;
        /// <summary>
        /// NG数
        /// </summary>
        private double in_NG = 0;
        /// <summary>
        /// 是否刷新侧漏仪器参数
        /// </summary>
        public static bool isRefresh = false;

        /// <summary>
        /// 时间暂存字段
        /// </summary>
        private string baseTime;
        
        private ChartShow chart;

        /// <summary>
        ///进度条下标
        /// </summary>
        private int SetIndex = 0;

        public static Form1 form1;

        public Form1()
        {
            InitializeComponent();
            InitializeGroupBox(false, 0);
            InitializeCB_Software();
            cshow.InitChart(View_Burst, "实时显示表");
            Control.CheckForIllegalCrossThreadCalls = false;
            form1 = this;

        }

        /// <summary>
        /// 生成程序选择框
        /// </summary>
        private void InitializeCB_Software()
        {
            for (int i = 1; i <= 128; i++)
            {
                cb_Software.Items.Add(i.ToString());
            }
        }

        /// <summary>
        /// 动态实例化 groupBox 控件中的内容
        /// </summary>
        private void InitializeGroupBox(bool Ischoice, int choice)
        {

            ReadXML.Read_XML_Text("address_F.xml", choice);
            List<ParaObject> obj = ReadXML.paraList;
            int width = gb_Software.Size.Width; //GroupBox 宽
            int avg = width / 3; //GroupBox 高
            // isChoice= fasle 未选择程序号的模式,显示全部
            string[] value = null;
            double sum;
            int round = 0;
            if (Ischoice)
            {
                gb_Software.Controls.Clear();
                //选择了程序号 根据模式显示参数
                switch (Pattern.Checked)
                {
                    //爆破模式
                    case true:
                        value = Enum.GetNames(pattern1.GetType());
                        sum = SumEnumPcs(pattern1) - 1; //减去程序号
                        round = (int)Math.Round(sum / 3 + 0.33);
                        Pattern.Checked = true;
                        break;
                    //正常测试模式
                    case false:
                        value = Enum.GetNames(pattern2.GetType());
                        sum = SumEnumPcs(pattern2) - 1;
                        round = (int)Math.Round(sum / 3 + 0.33);
                        Pattern.Checked = false;
                        break;
                }
            }
            else
            {
                sum = obj.Count;
                round = (int)Math.Round(sum / 3 + 0.33);
            }
            for (int n = 0; n < round; n++)
            {
                int startY = n * 30;

                for (int i = 0; i < 3; i++)
                {
                    int startX = i * avg;
                    //选好程序模式下
                    if (Ischoice)
                    {
                        //防止下标越界
                        if (n * 3 + i <= obj.Count - 1 && value != null && value.Contains(obj[n * 3 + i].English))
                        {
                            //生成label 与 TextBox
                            InitialLabelAndTextBox(gb_Software, obj, i, n, startX, startY);
                        }
                    }
                    else
                    {
                        //防止下标越界
                        if (n * 3 + i <= obj.Count - 1)
                        {
                            //生成label 与 TextBox
                            InitialLabelAndTextBox(gb_Software, obj, i, n, startX, startY);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 产生label控件与textbox控件
        /// </summary>
        /// <param name="box"></param>
        /// <param name="obj"></param>
        /// <param name="i"></param>
        /// <param name="n"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        private void InitialLabelAndTextBox(GroupBox box, List<ParaObject> obj, int i, int n, int startX, int startY)
        {
            Label lb = new Label();
            // AutoSize = true,
            lb.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))); //设置字体
            lb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter; //文字居中
            lb.Location = new System.Drawing.Point(startX + 10, startY + 30);
            lb.Name = "lb_" + obj[n * 3 + i].English;
            lb.Size = new System.Drawing.Size(100, 20);
            lb.Text = obj[n * 3 + i].Chinese + ":";
            box.Controls.Add(lb);

            System.Windows.Forms.TextBox tb = new System.Windows.Forms.TextBox();
            tb.Location = new System.Drawing.Point(startX + 110, startY + 30);
            tb.Multiline = true;
            tb.Name = obj[n * 3 + i].English;
            tb.Size = new System.Drawing.Size(100, 20);
            box.Controls.Add(tb);

        }

        /// <summary>
        /// 统计枚举中值个数
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public int SumEnumPcs(Enum @enum)
        {
            string[] value = Enum.GetNames(@enum.GetType());
            return value.Count();
        }

        /// <summary>
        /// 窗口加载前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //加载备注
            lb_Remark.Text = INIHelper.Read(tb_code.Text,"REMARK","",ParaService._iniSoftwarePath);
            //界面TabControl控件位置调整（隐藏TabPage头）
            this.tabCtlMainMenu.Appearance = TabAppearance.FlatButtons;
            this.tabCtlMainMenu.ItemSize = new Size(0, 1);
            for (int i = 0; i < 7; i++)
            {
                this.tabCtlMainMenu.TabPages[i].Text = null;
            }

            this.tabCtlMainMenu.SizeMode = TabSizeMode.Fixed;

            this.tabControlHome.Appearance = TabAppearance.FlatButtons;
            this.tabControlHome.ItemSize = new Size(0, 1);
            this.tabControlHome.TabPages[0].Text = null;
            this.tabControlHome.SizeMode = TabSizeMode.Fixed;

            this.tabCTLPara.Appearance = TabAppearance.FlatButtons;
            this.tabCTLPara.ItemSize = new Size(0, 1);
            this.tabCTLPara.TabPages[0].Text = null;
            this.tabCTLPara.SizeMode = TabSizeMode.Fixed;

            //链接PLC
            mx = MxCommuniUtil.Instance();
            mx.Open();

            GetGBControl();

            //串口连接
            SerialPara m_SerialPara = m_PortParaServcie.LoadPara();
            port = m_PortParaServcie.OpenPort(m_SerialPara);

            //从INI文件中获取当天产量
            GETToDayYield();

        }
        /// <summary>
        /// INI文件中获取当天产量
        /// </summary>
        private void GETToDayYield()
        {
            string nowTime = DateTime.Now.ToString("yyyyMMdd");
            baseTime = INIHelper.Read("Yield", "time", "0", ParaService._iniFilePath);
            if (nowTime != baseTime)
            {
                baseTime = nowTime;
                input = 0;
                in_OK = 0;
                in_NG = 0;
            }
            else
            {
                input = Convert.ToDouble(INIHelper.Read("Yield", "input", "0", ParaService._iniFilePath));
                in_OK = Convert.ToDouble(INIHelper.Read("Yield", "in_OK", "0", ParaService._iniFilePath));
                in_NG = Convert.ToDouble(INIHelper.Read("Yield", "in_NG", "0", ParaService._iniFilePath));
            }
            ShowToDayYield();
        }

        public void ShowToDayYield()
        {
            lb_GB_Yield_Time.Text = DateTime.Now.ToString("yyyy-MM-dd");
            tb_GB_yield_input.Text = input.ToString();
            tb_GB_yield_OK.Text = in_OK.ToString();
            tb_GB_yield_NG.Text = in_NG.ToString();
            if (in_OK == 0)
                tb_GB_yield_percentage.Text = "0%";
            else
                tb_GB_yield_percentage.Text = ((in_OK / input) * 100) + "%";
        }

        /// <summary>
        /// 保存当天产量到INI文件
        /// </summary>
        private void SetToDayYield(DateTime time)
        {
            INIHelper.DeleteSection("Yield", ParaService._iniFilePath);
            INIHelper.Write("Yield", "time", time.ToString("yyyyMMdd"), ParaService._iniFilePath);
            INIHelper.Write("Yield", "input", input.ToString(), ParaService._iniFilePath);
            INIHelper.Write("Yield", "in_OK", in_OK.ToString(), ParaService._iniFilePath);
            INIHelper.Write("Yield", "in_NG", in_NG.ToString(), ParaService._iniFilePath);
        }

        private void bt_Click(object sender, EventArgs e)
        {
            LabelX bt = sender as LabelX;
            switch (bt.Name)
            {
                case "btn_Home":
                    //加载备注
                    lb_Remark.Text = INIHelper.Read(tb_code.Text, "REMARK", "", ParaService._iniSoftwarePath);
                    MainMenuPageChange(bt, Home);
                    break;
                case "btn_DataAna":
                    MainMenuPageChange(bt, DataAna);
                    break;
                case "btn_Manual":
                    MainMenuPageChange(bt, Manual);
                    break;
                case "btn_MStatus":
                    MainMenuPageChange(bt, MachinStatus);
                    break;
                case "btn_ParaSet":
                    MainMenuPageChange(bt, ParaSet);
                    ParaService.Install().GetInfo(this.tabCTLPara);
                    break;
                case "btn_Alarm":
                    MainMenuPageChange(bt, Alarm);
                    break;
            }

        }

        private void MainMenuPageChange(LabelX labelX, TabPage PageName)
        {
            //主界面按钮背景置灰
            btn_Home.BackColor = Color.Gainsboro;
            btn_DataAna.BackColor = Color.Gainsboro;
            btn_Manual.BackColor = Color.Gainsboro;
            btn_MStatus.BackColor = Color.Gainsboro;
            btn_ParaSet.BackColor = Color.Gainsboro;
            btn_Alarm.BackColor = Color.Gainsboro;
            btn_Login.BackColor = Color.Gainsboro;
            //主界面选中按钮背景置绿
            labelX.BackColor = Color.DarkSeaGreen;
            //显示对应按钮的用户界面（使用TabControl 的page实现）
            tabCtlMainMenu.SelectedTab = PageName;
        }

        private void bt_OEEPara_Click(object sender, EventArgs e)
        {
            LabelX lab = sender as LabelX;
            switch (lab.Name)
            {
                case "bt_OEEPara":
                    ParaSetPageChange(lab, Para);
                    break;
                case "bt_Serial":
                    ParaSetPageChange(lab, SerialPortPara);
                    break;

            }

        }

        private void ParaSetPageChange(LabelX labelX, TabPage PageName)
        {
            //参数设置菜单内容背景置灰
            bt_OEEPara.BackColor = Color.Gainsboro;
            bt_Serial.BackColor = Color.Gainsboro;
            //参数设置菜单选中按钮背景置绿
            labelX.BackColor = Color.Turquoise;
            //显示对应按钮的用户界面（使用TabControl 的page实现）
            tabCTLPara.SelectedTab = PageName;

        }


        private void btn_Display_Click_1(object sender, EventArgs e)
        {
            MainMenuPageChange(btn_Display, Display);
        }

        /// <summary>
        /// 保存参数按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, EventArgs e)
        {
            ParaService ppService = ParaService.Install();
            ppService.form = this;
            Control con = sender as Control;
            string name = con.Name;
            switch (name)
            {
                case "SaveSerialPort":
                    ppService.SetInfo(this.SerialPortPara);
                    break;
                case "SaveEquipment":
                    ppService.SetInfo(this.EquipmentInfo);
                    break;
                case "bt_Software":
                    string node = cb_Software.Text;
                    Form1.form1.AddInformation("保存"+node+"号程序参数.");
                    progressBar_Setting.Maximum = gb_Software.Controls.Count / 2;
                    progressBar_Setting.Visible = true;
                    INIHelper.Write(cb_Software.Text,"REMARK", tb_remark.Text, ParaService._iniSoftwarePath);
                    ppService.SetATQPara(port, ReadXML.paraList, this.gb_Software, Pattern, node);
                    ppService.SetSoftware(this.gb_Software, Pattern, node);
                    progressBar_Setting.Visible = false;
                    break;
            }
            MessageBox.Show("保存成功", "提示");

        }

        /// <summary>
        /// 程序号下拉 or 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshGroupBox(object sender, EventArgs e)
        {
            tb_remark.Text = INIHelper.Read(cb_Software.Text,"REMARK", "", ParaService._iniSoftwarePath);

            if ((Control)sender is ComboBox)
            {
                //读取设备中的参数，并保存在INI中。
                ParaService.Install().ReadATQToINI(port, (cb_Software.SelectedIndex + 1).ToString());
                Pattern.Checked = ParaService.Install().LoadPattern(Pattern, cb_Software.Text.Trim());
                //根据参数文档中 pattern 来选择模式
                if (Pattern.Checked)
                    InitializeGroupBox(true, 1); //爆破 burst
                else
                    InitializeGroupBox(true, 2); //泄漏 leak
                gb_Software.Refresh();
                //设置参数读取进度条的最大值
                progressBar_Setting.Maximum = ReadXML.paraList.Count;
            }
            else
            {
                //根据参数文档中 pattern 来选择模式
                if (Pattern.Checked)
                    InitializeGroupBox(true, 1); //爆破 burst
                else
                    InitializeGroupBox(true, 2); //泄漏 leak
                gb_Software.Refresh();

                ParaService.Install().LoadParaToGroupBox(gb_Software, Pattern, cb_Software.Text.Trim(), false);
            }

        }

        /// <summary>
        /// 获取控件集合用于实时与最终结果显示
        /// </summary>
        private void GetGBControl()
        {

            foreach (Control item in GB_Real.Controls)
            {
                if (item.Name.Contains("showR"))
                {
                    listConReal.Add(item);
                }
            }
            foreach (Control item in GB_Last.Controls)
            {
                if (item.Name.Contains("showL"))
                {
                    listConLast.Add(item);
                }
            }

        }

        /// <summary>
        /// 实时状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Real_Tick(object sender, EventArgs e)
        {
            //启动
            mx.GetDevice("M385", out data);
            if (data == 1)
            {
                AddInformation("爆破机台启动");
                mx.SetDevice("M385", 0);
                StartTest();
            }



            if (queueReal.Count > 0) //实时结果
            {

                RealTimeData rData = queueReal.Dequeue();
                double Nowtime = m_StopWatch.ElapsedMilliseconds / 1000.0;
                cshow.AddData(queueData, Nowtime, Convert.ToDouble(rData.PressureValue));
                cshow.ShowChart(View_Burst, queueData);
                for (int i = 0; i < listConReal.Count; i++)
                {
                    string[] realString = listConReal[i].Name.Split('_');
                    Type tp = rData.GetType();
                    PropertyInfo p = tp.GetProperty(realString[1]);
                    string value = p.GetValue(rData, null).ToString();

                    listConReal[i].Text = value;
                }

                
            }

            if (queueLast.Count > 0) //最终结果
            {
                FinalResult finalResult = queueLast.Dequeue();


                for (int i = 0; i < listConLast.Count; i++)
                {
                    string[] lastString = listConLast[i].Name.Split('_');
                    Type tp = finalResult.GetType();
                    PropertyInfo p = tp.GetProperty(lastString[1]);
                    string value = p.GetValue(finalResult, null).ToString();
                    listConLast[i].Text = value;

                }
                Last_LValueAndUnit.Text = finalResult.LeakValue + finalResult.LeakUnit;
                Last_PValueAndUnit.Text = finalResult.PressureValue + finalResult.PressureUnit;
                ExcelUtil excelUtil = ExcelUtil.Instance();
                finalResult.Product = tb_product.Text;
                excelUtil.SaveData(finalResult);

                switch (finalResult.Result)
                {
                    case "OK":
                        mx.SetDevice("M386", 1);
                        in_OK++;
                        break;
                    case "NG":
                        mx.SetDevice("M387", 1);
                        in_NG++;
                        break;
                }
                ShowToDayYield();
            }

            if (isClear) //清空
            {
                isClear = false;
                ClearGBReal();
            }

            if (PortServcie.queueParaBuff.Count > 0) //暂存区有数据 =》 发送 =》 解析数据设置到INI文件
            {
                PortServcie.paraBuff = PortServcie.queueParaBuff.Dequeue();
                SetIndex++;
                ProcessBarDisplay(SetIndex);
                string address = PortServcie.paraBuff.Read;
                string readword = PortServcie.paraBuff.ReadWord;
                string temp1 = "FF03" + address + readword;
                PortServcie.IsReadMode = false;
                m_PortParaServcie.WriteByte(port, temp1 + mbtool.CRC(temp1));

                if (PortServcie.queueParaBuff.Count == 0)
                {
                    SetIndex = 0;
                    ProcessBarDisplay(progressBar_Setting.Maximum +1);
                    
                }

            }

            if (isRefresh) //刷新仪器参数
            {
                isRefresh = false;
                ParaService.Install().LoadParaToGroupBox(gb_Software, Pattern, cb_Software.Text.Trim(), true);
            }
        }

        /// <summary>
        /// 清空实时显示控件
        /// </summary>
        public void ClearGBReal()
        {
            foreach (Control item in listConReal)
            {
                item.Text = "--";
            }
        }

        /// <summary>
        /// 保存Excel
        /// </summary>
        /// <param name="obj"></param>
        private void SaveExcel(object obj)
        {

        }

        /// <summary>
        /// 定时发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Refesh_Tick(object sender, EventArgs e)
        {
            if (isSend)
            {
                PortServcie.Install().WriteByte(PortServcie.sp, "FF030030000D91DE"); //实时参数
            }
        }

        /// <summary>
        /// 测试区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BT_Test(object sender, EventArgs e)
        {

            Button bt = sender as Button;
            try
            {
                int codeIndex = Convert.ToInt32(tb_code.Text.Trim());
                switch (bt.Name)
                {
                    case "bt_add":
                        if ((codeIndex + 1) < 128)
                        {
                            tb_code.Text = (codeIndex + 1).ToString();
                            lb_TestShow.Visible = false;
                        }
                        else
                        {
                            lb_TestShow.Visible = true;
                            lb_TestShow.Text = "超出仪器最大程序号";
                        }
                        break;
                    case "bt_cut":
                        if ((codeIndex - 1) < 1)
                        {
                            lb_TestShow.Visible = true;
                            lb_TestShow.Text = "小于仪器最小程序号";

                        }
                        else
                        {
                            tb_code.Text = (codeIndex - 1).ToString();
                            lb_TestShow.Visible = false;
                        }
                        break;
                    case "bt_sf_Start": //启动测试

                        StartTest();
                        break;
                    case "bt_restor": //复位

                        break;

                }
            }
            catch (Exception ex)
            {
                lb_TestShow.Visible = true;
                lb_TestShow.Text = ex.Message;
            }

        }

        /// <summary>
        /// 启动测试
        /// </summary>
        private void StartTest()
        {
            AddInformation("开始测试..");
            int codeIndex = Convert.ToInt32(tb_code.Text.Trim());
            AddInformation("启动"+codeIndex+"号程序.");
            for (int i = 0; i < listConLast.Count; i++) //清空最后结果显示区
                listConLast[i].Text = "——";
            Last_LValueAndUnit.Text = "——";
            Last_PValueAndUnit.Text = "——";

            queueData.Clear();
            mbtool.ResetFifo();
            PortServcie.Install().WriteByte(PortServcie.sp, mbtool.GetSoftwareIDForStart(codeIndex.ToString())); //程序号
            PortServcie.Install().WriteByte(PortServcie.sp, "FF050001FF00C824"); //启动
            isSend = true; //发送实时数据
            m_StopWatch.Start();

            string nTime = DateTime.Now.ToString("yyyyMMdd");
            if (nTime != baseTime)
            {
                baseTime = nTime;
                input = 1;
                in_OK = 0;
                in_NG = 0;
            }
            else
            {
                input++;
            }
            ShowToDayYield();
        }


        public static Object GetProperties<T>(T obj, string prop_name)
        {
            try
            {
                if (obj == null || prop_name == null) return "";
                Type type = obj.GetType();
                foreach (System.Reflection.PropertyInfo property in type.GetProperties())
                {
                    if (prop_name.Equals(property.Name))
                    {
                        Object value = null;
                        value = property.GetValue(obj, null);
                        return value;
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        private void ck_mode_CheckedChanged(object sender, EventArgs e)
        {
            bt_sf_Start.Visible = ck_mode.Checked;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //保存当日的产量
            SetToDayYield(DateTime.Now);
        }
        /// <summary>
        /// 生成模拟图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_Sim_Click(object sender, EventArgs e)
        {

            Control.ControlCollection all = gb_Software.Controls;
            SimTestPara testpara = new SimTestPara();
            List<TextBox> AllText = new List<TextBox>();
            ChartShow SimChart = new ChartShow();
            Queue<PointXY> SimData = new Queue<PointXY>();
            foreach (Control item in all)
            {
                if (item is TextBox)
                {
                    TextBox box = item as TextBox;
                    AllText.Add(box);
                }
            }

            Type type = testpara.GetType();
            PropertyInfo[] AllVar = type.GetProperties();
            foreach (PropertyInfo item in AllVar)
            {
                foreach (TextBox box in AllText)
                {

                    if (item.Name == box.Name)
                    {
                        item.SetValue(testpara, box.Text, null);
                    }
                }
            }
            try
            {
                if (Pattern.Checked)
                {
                    int step = int.Parse(testpara.N_OF_STEPS);
                    double start = double.Parse(testpara.STARTFILL);
                    double realtime = 0;
                    double realfill = start;
                    double set = double.Parse(testpara.SETFILL);
                    double rise = (set - start) / step;
                    double stay = double.Parse(testpara.T_LEVEL);
                    double risetime = double.Parse(testpara.RAMP);
                    double dump = double.Parse(testpara.DUMPTIME);
                    for (int i = 0; i < step; i++)
                    {
                        SimChart.AddData(SimData, realtime, realfill);
                        realfill += rise;
                        realtime += risetime;
                        SimChart.AddData(SimData, realtime, realfill);
                        realtime += stay;
                    }
                    SimChart.AddData(SimData, realtime, realfill);
                    SimChart.AddData(SimData, realtime + dump, 0);
                }
                else
                {
                    double fill = double.Parse(testpara.SETFILL);
                    double time = (double.Parse(testpara.FILLTIME) + double.Parse(testpara.STABTIME) + double.Parse(testpara.TESTTIME));
                    double maxtime = time + double.Parse(testpara.DUMPTIME);
                    SimChart.AddData(SimData, 0, fill);//起始点
                    SimChart.AddData(SimData, time, fill);//转折点
                    SimChart.AddData(SimData, maxtime, 0);
                }
                SimChart.ShowChart(SimChart.chart_Sim, SimData);
                SimChart.chart_Sim.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                SimChart.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("数据不完整");
            }


        }
        /// <summary>
        /// 显示进度条
        /// </summary>
        /// <param name="max"></param>
        /// <param name="value"></param>
        public void ProcessBarDisplay(int value)
        {

            progressBar_Setting.Visible = true;
            int max = progressBar_Setting.Maximum;
            if (max > value)
            {
                progressBar_Setting.Value = value;
            }
            else
            {
                progressBar_Setting.Value = 0;
                progressBar_Setting.Visible = false;
            }
        }

        /// <summary>
        /// 程序号改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_code_TextChanged(object sender, EventArgs e)
        {
            TextBox box = sender as TextBox;
            lb_Remark.Text = INIHelper.Read(box.Text,"REMARK","",ParaService._iniSoftwarePath);
        }

        /// <summary>
        /// 将发生的消息发送到左边的消息框中
        /// </summary>
        /// <param name="value"></param>
        public void AddInformation(string value)
        {
            string txt = string.Format("{0}:{1}\r\n",DateTime.Now.ToString("HH:MM:ss"),value);
            tb_Message.AppendText(txt);
        }
    }
}

