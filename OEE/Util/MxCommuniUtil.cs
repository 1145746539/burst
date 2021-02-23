using ActUtlTypeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OEE.Util
{
    public class MxCommuniUtil
    {
        private ActUtlType actUtlType = null;
        /// <summary>
        /// 是否打开连接
        /// </summary>
        private bool _isOpen = false;
        private int stationNumber = 1;  //默认0
        public string message = null;
        private static MxCommuniUtil mxUtil;
        private MxCommuniUtil() { }

        public static MxCommuniUtil Instance()
        {
            if (mxUtil == null)
            {
                return new MxCommuniUtil();
            }
            return mxUtil;
        }

        /// <summary>
        /// 第一步 打开PLC连接
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            lock (this)
            {
                try
                {
                    if (_isOpen == true)
                    {
                        return true;
                    }
                    if (actUtlType == null)
                    {
                        actUtlType = new ActUtlType();
                        actUtlType.ActLogicalStationNumber = stationNumber;
                    }
                    int result = actUtlType.Open();
                    if (result != 0)
                    {
                        message = "PLC连接失败,错误代码：" + (uint)result;
                        return false;
                    }
                    else
                    {
                        _isOpen = true;
                        message = "PLC连接成功";
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    _isOpen = false;
                    message = "error:\n" + ex.Message;
                    return false;
                }
            }

        }

        /// <summary>
        /// 软元件的批量读取->连续 (4 字节数据 )
        /// </summary>
        public bool ReadDeviceBlock(string blockName, int size, out int[] lData)
        {
            lock (this)
            {
                lData = new int[size];
                try
                {
                    int result = actUtlType.ReadDeviceBlock(blockName, size, out lData[0]);
                    if (0 == result)
                    {
                        message = "从PLC地址" + blockName + "开始读取" + size + "位成功";
                        return true;
                    }
                    else
                    {
                        message = "从PLC地址" + blockName + "开始读取" + size + "失败，错误代码：\n" + (uint)result;
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    _isOpen = false;
                    message = "error:\n" + ex.Message;
                    return false;
                }
                finally
                {
                    Console.WriteLine(message);
                }
            }

        }

        /// <summary>
        /// 随机读取
        /// </summary>
        /// <param name="szDevice"></param>
        /// <param name="size"></param>
        /// <param name="lData"></param>
        /// <returns></returns>
        public bool ReadDeviceRandom(string szDevice, int size, out int[] lData)
        {
            lock (this)
            {
                lData = new int[size];
                try
                {
                    int result = actUtlType.ReadDeviceRandom(szDevice, size, out lData[0]);
                    if (0 == result)
                    {
                        message = "随即读取PLC地址" + szDevice + "\n" + size + "位成功";
                        return true;
                    }
                    else
                    {
                        message = "随即读取PLC地址" + szDevice + "\n" + size + "失败，错误代码：\n" + (uint)result;
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    _isOpen = false;
                    message = "error:\n" + ex.Message;
                    return false;
                }
                finally
                {
                    Console.WriteLine(message);
                }
            }

        }


        /// <summary>
        /// 软元件的单个读取 (4 字节数据 )
        /// </summary>
        /// <param name="device"></param>
        /// <param name="lData"></param>
        /// <returns></returns>
        public bool GetDevice(string device, out int lData)
        {
            lock (this)
            {
                try
                {
                    int result = actUtlType.GetDevice(device, out lData);
                    if (0 == result)
                    {
                        message = "PLC地址" + device + "读取成功,结果：" + lData;
                        return true;
                    }
                    else
                    {
                        message = "PLC地址" + device + "读取失败，错误代码：" + (uint)result;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    message = "error:\n" + ex.Message;
                    lData = -1;
                    return false;
                }
                finally
                {
                    //Console.WriteLine(message);
                }
            }
        }
        public bool SetDevice(string device, int lData)
        {
            try
            {
                int RCode = actUtlType.SetDevice(device, lData);
                if (0 == RCode)
                {
                    message = "PLC地址" + device + "设置成功,结果：" + lData;
                    return true;
                }
                else
                {
                    message = "PLC地址" + device + "读取成功,结果：" + lData;
                    return false;
                }
            }
            catch (Exception ex)
            {
                _isOpen = false;
                return false;
            }
        }


        public bool ClosePLC()
        {
            lock (this)
            {
                try
                {
                    if (_isOpen == false)
                    {
                        return true;
                    }
                    int result = actUtlType.Close();
                    if (result != 0)
                    {
                        message = "PLC关闭失败,错误代码：" + (uint)result;
                        return false;
                    }
                    else
                    {
                        _isOpen = false;
                        message = "PLC关闭成功";
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    _isOpen = true;
                    message = "error:\n" + ex.Message;
                    return false;
                }
            }


        }
    }

}
