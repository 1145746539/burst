namespace OEE
{
    partial class ChartShow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.software = new System.Windows.Forms.ToolStripStatusLabel();
            this.gp_sendManage = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.chart_Sim = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.statusStrip1.SuspendLayout();
            this.gp_sendManage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Sim)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.software});
            this.statusStrip1.Location = new System.Drawing.Point(0, 516);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(916, 26);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // software
            // 
            this.software.Name = "software";
            this.software.Size = new System.Drawing.Size(69, 20);
            this.software.Text = "程序号：";
            // 
            // gp_sendManage
            // 
            this.gp_sendManage.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.gp_sendManage.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.gp_sendManage.Controls.Add(this.chart_Sim);
            this.gp_sendManage.DisabledBackColor = System.Drawing.Color.Empty;
            this.gp_sendManage.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gp_sendManage.Location = new System.Drawing.Point(0, 1);
            this.gp_sendManage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gp_sendManage.Name = "gp_sendManage";
            this.gp_sendManage.Size = new System.Drawing.Size(913, 509);
            // 
            // 
            // 
            this.gp_sendManage.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.gp_sendManage.Style.BackColorGradientAngle = 90;
            this.gp_sendManage.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.gp_sendManage.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_sendManage.Style.BorderBottomWidth = 1;
            this.gp_sendManage.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.gp_sendManage.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_sendManage.Style.BorderLeftWidth = 1;
            this.gp_sendManage.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_sendManage.Style.BorderRightWidth = 1;
            this.gp_sendManage.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.gp_sendManage.Style.BorderTopWidth = 1;
            this.gp_sendManage.Style.CornerDiameter = 4;
            this.gp_sendManage.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.gp_sendManage.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.gp_sendManage.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.gp_sendManage.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.gp_sendManage.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.gp_sendManage.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.gp_sendManage.TabIndex = 6;
            this.gp_sendManage.Text = "图形";
            // 
            // chart_Sim
            // 
            chartArea1.Name = "ChartArea1";
            this.chart_Sim.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart_Sim.Legends.Add(legend1);
            this.chart_Sim.Location = new System.Drawing.Point(4, 4);
            this.chart_Sim.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chart_Sim.Name = "chart_Sim";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart_Sim.Series.Add(series1);
            this.chart_Sim.Size = new System.Drawing.Size(905, 478);
            this.chart_Sim.TabIndex = 0;
            this.chart_Sim.Text = "chart1";
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2010Silver;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))), System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(115)))), ((int)(((byte)(199))))));
            // 
            // ChartShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 542);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.gp_sendManage);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChartShow";
            this.Text = "Chart";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.gp_sendManage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_Sim)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel software;
        private DevComponents.DotNetBar.Controls.GroupPanel gp_sendManage;
        public System.Windows.Forms.DataVisualization.Charting.Chart chart_Sim;
        private DevComponents.DotNetBar.StyleManager styleManager1;

    }
}