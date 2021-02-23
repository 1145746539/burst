using OEE.bean;
using OEE.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace OEE.Util
{
    /// <summary>
    /// 读取XML的工具类
    /// </summary>
    internal class ReadXML
    {
        /// <summary>
        /// 当前ATQ程序参数集合
        /// </summary>
        public static List<ParaObject> paraList = new List<ParaObject>();

        /// <summary>
        /// 读取XML
        /// </summary>
        /// <param name="XMLPath"></param>
        /// <param name="Choice">1爆破模式 ， 2泄露模式</param>
        public static void Read_XML_Text(string XMLPath, int Choice)
        {
            try
            {
                paraList.Clear();
                XmlDocument xml = new XmlDocument();
                xml.Load(XMLPath);
                XmlElement documentElement = xml.DocumentElement;
                foreach (XmlNode xmlNode in documentElement)
                {

                    ParaObject para = new ParaObject();
                    XmlElement xe = (XmlElement)xmlNode;
                    try
                    {
                        para.Chinese = xe.GetAttribute("Chinese");
                        para.English = xe.GetAttribute("English");
                        // 得到Book节点的所有子节点
                        XmlNodeList xnl = xe.ChildNodes;
                        para.Read = xnl.Item(0).InnerText;
                        para.Write = xnl.Item(1).InnerText;
                        para.ReadWord = xnl.Item(2).InnerText;
                        para.WriteWord = xnl.Item(3).InnerText;
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteOrCreateLog(null, "error--转换XML节点异常：" + ex.ToString());
                    }

                    switch (Choice)
                    {
                        //爆破模式
                        case 1:
                            string[] Burst = Enum.GetNames(new PatternBurst().GetType());
                            if (Burst.Contains(para.English))
                                paraList.Add(para);
                            break;
                        case 2:
                            string[] Leak = Enum.GetNames(new PatternLeak().GetType());
                            if (Leak.Contains(para.English))
                                paraList.Add(para);
                            break;
                        default:
                            paraList.Add(para);
                            break;
                    }


                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteOrCreateLog(null, "error--读取XML异常：" + ex.ToString());
            }

        }


    }
}
