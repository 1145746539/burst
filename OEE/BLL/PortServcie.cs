using DevComponents.DotNetBar.Controls;
using OEE.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using OEE.bean;

namespace OEE.BLL
{
    /// <summary>
    /// 串口服务类
    /// </summary>
    class PortServcie
    {
        /// <summary>
        /// 串口
        /// </summary>
        public static SerialPort sp = new SerialPort();
        private static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "para.ini");
        /// <summary>
        /// 参数暂存队列
        /// </summary>
        public static Queue<ParaObject> queueParaBuff = new Queue<ParaObject>(30);
        /// <summary>
        /// 参数暂存对象
        /// </summary>
        public static ParaObject paraBuff = null;
        /// <summary>
        /// 是否读取模式
        /// </summary>
        public static bool IsReadMode = true;

        private ModBusTools modbustool = new ModBusTools();

        public static PortServcie paraServcie = null;
        public PortServcie() { }
        public static PortServcie Install()
        {
            if (paraServcie == null)
                paraServcie = new PortServcie();

            return paraServcie;
        }

        /// <summary>
        /// 从参数文件加载串口参数
        /// </summary>
        /// <returns></returns>
        public SerialPara LoadPara()
        {
            SerialPara serialPara = new SerialPara();
            Type serialType = serialPara.GetType();
            object obj = Activator.CreateInstance(serialType);
            PropertyInfo[] serialArray = serialType.GetProperties();
            foreach (PropertyInfo item in serialArray)
            {
                string name = item.Name;
                string value = INIHelper.Read("Parameter", name, "", filePath);
                item.SetValue(obj, value, null);
            }
            serialPara = obj as SerialPara;
            return serialPara;
        }


        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns></returns>
        public SerialPort OpenPort(SerialPara serialPara)
        {
            if (ParaService.Install().GetPort().Length == 0)
            {
                MessageBox.Show("打开串口错误，本机没有串口！", "警告");
                return null;
            }
            if (!sp.IsOpen)
            {
                try
                {

                    string port = serialPara.Port;
                    string baudRate = serialPara.BaudRate;
                    string dataBits = serialPara.DataBits;
                    string stopBits = serialPara.StopBit;
                    string parity = serialPara.Parity;

                    sp.PortName = port;
                    sp.BaudRate = int.Parse(baudRate);
                    sp.DataBits = int.Parse(dataBits);
                    sp.StopBits = (StopBits)int.Parse(stopBits);
                    switch (parity)             //校验位
                    {
                        case "none":
                            sp.Parity = Parity.None;
                            break;
                        case "odd":
                            sp.Parity = Parity.Odd;
                            break;
                        case "even":
                            sp.Parity = Parity.Even;
                            break;
                    }
                    //sp.ReadTimeout = 1000;
                    //sp.WriteTimeout = 1000;
                    sp.ReceivedBytesThreshold = 1;
                    sp.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
                    sp.Open();
                    return sp;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("串口参数设置错误!" + ex.Message, "错误");
                    return null;
                }
            }
            return sp;

        }

        /// <summary>
        /// 写入Byte数据
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(SerialPort port, string value)
        {
            if (!port.IsOpen)
            {
                WriteLog.WriteOrCreateLog(null, this.GetType().Name + "error--串串口未打开。写入失败：" + value);
            }
            try
            {
                WriteLog.WriteOrCreateLog(null, ParaService.infostring + "--写入串口数据：" + value);
                Byte[] writeByte = TransformString(value);

                port.Write(writeByte, 0, writeByte.Length);

                Thread.Sleep(200);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                WriteLog.WriteOrCreateLog(null, this.GetType().Name + "error--串口写入Byte数据失败：" + ex.Message);
            }
        }


        public bool Close()
        {

            if (sp.IsOpen)
            {
                try
                {
                    sp.Close();
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 将字符串转成byte数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] TransformString(string value)
        {
            byte[] byteBuffer = new byte[value.Length / 2];
            char[] valueChar = value.ToArray();
            for (int i = 0; i < value.Length; i = i + 2)
            {
                int transNum = Convert.ToInt32(valueChar[i].ToString() + value[i + 1].ToString(), 16);
                byteBuffer[i / 2] = Convert.ToByte(transNum);
            }
            return byteBuffer;
        }



        /// <summary>
        /// 串口数据接收
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            string receive = "";
            List<byte> _byteData = new List<byte>();
            while (sp.BytesToRead > 0)
            {
                byte[] readBuffer = new byte[sp.ReadBufferSize + 1];
                int count = sp.Read(readBuffer, 0, sp.ReadBufferSize);
                for (int i = 0; i < count; i++)
                {
                    receive = receive + readBuffer[i].ToString("X2");
                    _byteData.Add(readBuffer[i]);
                }
                Thread.Sleep(50);
            }
            if (!string.IsNullOrEmpty(receive))
            {
                
                WriteLog.WriteOrCreateLog(null, ParaService.infostring + "--收到数据：" + receive);
                //实时解析串口返回数据
                new ModBusTools().ReceiveData(receive);
               
            }
        }


    }

    class SerialPara
    {
        /// <summary>
        /// 端口号
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// 波特率
        /// </summary>
        public string BaudRate { get; set; }
        /// <summary>
        /// 数据位
        /// </summary>
        public string DataBits { get; set; }
        /// <summary>
        /// 停止位
        /// </summary>
        public string StopBit { get; set; }
        /// <summary>
        /// 校检位
        /// </summary>
        public string Parity { get; set; }
    }



}
