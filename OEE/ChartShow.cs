using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.Diagnostics;
using OEE.Util;


namespace OEE
{
    /// <summary>
    /// 图形显示窗口
    /// </summary>
    public partial class ChartShow : Form
    {
       
       

        public ChartShow()
        {
            InitializeComponent();
            InitChart(chart_Sim, "仿真显示表");
            Control.CheckForIllegalCrossThreadCalls = false;
        }


        /// <summary>
        /// 初始化表格
        /// </summary>
        /// <param name="chart">图形对象</param>
        /// <returns></returns>
        public Chart InitChart(Chart chart,string value)
        {
            
            chart.ChartAreas.Clear();
            chart.Series.Clear();
            //chart.Series[0].Points.Clear();
            //定义图表区域 设置图标显示样式
            ChartArea chartA = new ChartArea("CA");
            chartA.AxisX.Minimum = 0;
            chartA.AxisX.Interval = 1; //X轴间距 
            chartA.AxisX.MajorGrid.LineColor = Color.Silver; //灰色
            chartA.AxisY.Minimum = 0;
            //chartA.AxisY.Maximum = 3; //定义为变量
            chartA.AxisY.MajorGrid.LineColor = Color.Silver; //灰色

            //定义储存和显示点的容器
            Series series = new Series("气压");
            series.Color = Color.Red;
            series.ChartType = SeriesChartType.Spline;

            chart.Titles.Clear();
            chart.Titles.Add("simulation"); //simulation 仿真  realTime
            chart.Titles[0].Text = value;
            chart.Titles[0].ForeColor = Color.Blue;
            chart.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);

            chart.ChartAreas.Add(chartA);
            chart.Series.Add(series);


            return chart;

        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="X_value">X点坐标param>
        /// <param name="Y_value">Y点坐标</param>
        /// <returns></returns>
        public Queue<PointXY> AddData( Queue<PointXY> queueData,double X_value, double Y_value)
        {
            Console.WriteLine(X_value + "----" + Y_value);
            queueData.Enqueue(new PointXY() { X = X_value, Y = Y_value });

            return queueData;
        }

        /// <summary>
        /// 刷新chart
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="que"></param>
        public void ShowChart(Chart chart, Queue<PointXY> que)
        {

            chart.Series[0].Points.Clear();
            for (int i = 0; i < que.Count; i++)
            {
                PointXY point = que.ElementAt(i);
                chart.Series[0].Points.AddXY(point.X, point.Y);
            }

        }

        
        
    }

    public class PointXY
    {
        internal double X;
        internal double Y;
    }
}
