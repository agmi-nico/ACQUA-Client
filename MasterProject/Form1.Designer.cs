﻿namespace MasterProject
{
    partial class Form1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend7 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea9 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend9 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea10 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend10 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea11 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend11 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea12 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend12 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            // tab 1: delay and jitter
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.JitterChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.DelayChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            // tab 2: bandwidth
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.DownloadBandwidthChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.UploadBandwidthChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            // tab 3 (4?): QoE for the provided decision tree
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.MeanQoEChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.QoEChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            // what are "periodic numeric" and "probes numeric"?
            this.PeriodNumeric = new System.Windows.Forms.NumericUpDown();
            this.ProbesNumeric = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LandmarksDelayRchTxt = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.LandmarksNo = new System.Windows.Forms.TextBox();
            this.SelectLandmarks = new System.Windows.Forms.Button();
            this.radioButtonRandom = new System.Windows.Forms.RadioButton();
            this.radioButtonIntelligent = new System.Windows.Forms.RadioButton();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            // decision tree is here. 
            //TODO: allow more than one decision tree. Open a tab for each decision tree. Save tabs and decision trees in arrays
            this.DecisionTreeTxtBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.BWFileTxtBox = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            // "starting" graphs?
            ((System.ComponentModel.ISupportInitialize)(this.JitterChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelayChart)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DownloadBandwidthChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UploadBandwidthChart)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MeanQoEChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QoEChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PeriodNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProbesNumeric)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // adding all tabs 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(391, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(481, 487);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // drawing tab1 borders and margins
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(473, 461);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Delay";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2 
            // adding jitter and delay charts to groupBox2 (and so to tab1 too)
            this.groupBox2.Controls.Add(this.JitterChart);
            this.groupBox2.Controls.Add(this.DelayChart);
            this.groupBox2.Location = new System.Drawing.Point(3, -1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(464, 487);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;

            int borderWidth = 5;

            // 
            // JitterChart
            // 
            chartArea7.Name = "ChartArea1";
            this.JitterChart.ChartAreas.Add(chartArea7);
            legend7.Name = "Legend1";
            this.JitterChart.Legends.Add(legend7);
            this.JitterChart.Location = new System.Drawing.Point(6, 237);
            this.JitterChart.Name = "JitterChart";
            series7.ChartArea = "ChartArea1";
            series7.Legend = "Legend1";
            series7.Name = "Jitter";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.BorderWidth = borderWidth;
            this.JitterChart.Series.Add(series7);

            this.JitterChart.Size = new System.Drawing.Size(452, 215);
            this.JitterChart.TabIndex = 1;
            this.JitterChart.Text = "chart1";
            // 
            // DelayChart
            // 
            chartArea8.Name = "ChartArea1";
            this.DelayChart.ChartAreas.Add(chartArea8);
            legend8.Name = "Legend1";
            this.DelayChart.Legends.Add(legend8);
            this.DelayChart.Location = new System.Drawing.Point(5, 16);
            this.DelayChart.Name = "DelayChart";

            series8.ChartArea = "ChartArea1";
            series8.Legend = "Legend1";
            series8.Name = "Round-trip Delay";
            series8.YValuesPerPoint = 2; //what's this? 2 values on the Y-axis for each point on the X-Axis? A bar, then?
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series8.BorderWidth = borderWidth;
            this.DelayChart.Series.Add(series8);
            this.DelayChart.Size = new System.Drawing.Size(453, 215);
            this.DelayChart.TabIndex = 0;
            this.DelayChart.Text = "chart1";
            // 
            // tabPage2
            // drawing tab2 borders and margins
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(473, 461);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Bandwidth";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // adding bw charts to groupBox6 (and so to tab2 too)
            this.groupBox6.Controls.Add(this.DownloadBandwidthChart);
            this.groupBox6.Controls.Add(this.UploadBandwidthChart);
            this.groupBox6.Location = new System.Drawing.Point(4, -1);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(463, 456);
            this.groupBox6.TabIndex = 9;
            this.groupBox6.TabStop = false;
            // 
            // DownloadBandwidthChart
            // 
            chartArea9.Name = "ChartArea1";
            this.DownloadBandwidthChart.ChartAreas.Add(chartArea9);
            legend9.Name = "Legend1";
            this.DownloadBandwidthChart.Legends.Add(legend9);
            this.DownloadBandwidthChart.Location = new System.Drawing.Point(6, 237);
            this.DownloadBandwidthChart.Name = "DownloadBandwidthChart";
            series9.ChartArea = "ChartArea1";
            series9.Legend = "Legend1";
            series9.Name = "Download Bandwidth";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series9.BorderWidth = borderWidth;
            this.DownloadBandwidthChart.Series.Add(series9);
            this.DownloadBandwidthChart.Size = new System.Drawing.Size(453, 215);
            this.DownloadBandwidthChart.TabIndex = 1;
            this.DownloadBandwidthChart.Text = "chart3";
            // 
            // UploadBandwidthChart
            // 
            chartArea10.Name = "ChartArea1";
            this.UploadBandwidthChart.ChartAreas.Add(chartArea10);
            legend10.Name = "Legend1";
            this.UploadBandwidthChart.Legends.Add(legend10);
            this.UploadBandwidthChart.Location = new System.Drawing.Point(4, 16);
            this.UploadBandwidthChart.Name = "UploadBandwidthChart";
            series10.ChartArea = "ChartArea1";
            series10.Legend = "Legend1";
            series10.Name = "Upload Bandwidth";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series10.BorderWidth = borderWidth;
            this.UploadBandwidthChart.Series.Add(series10);
            this.UploadBandwidthChart.Size = new System.Drawing.Size(453, 215);
            this.UploadBandwidthChart.TabIndex = 0;
            this.UploadBandwidthChart.Text = "chart2";
            // 
            // tabPage4
            // drawing tab4 borders and margins
            this.tabPage4.Controls.Add(this.groupBox8);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(473, 461);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Skype QoE";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // dding QoE charts to groupBox8 (and so to tab4 too)
            this.groupBox8.Controls.Add(this.MeanQoEChart);
            this.groupBox8.Controls.Add(this.QoEChart);
            this.groupBox8.Location = new System.Drawing.Point(4, -1);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(463, 456);
            this.groupBox8.TabIndex = 9;
            this.groupBox8.TabStop = false;
            // 
            // MeanQoEChart
            // 
            chartArea11.Name = "ChartArea1";
            this.MeanQoEChart.ChartAreas.Add(chartArea11);
            legend11.Name = "Legend1";
            this.MeanQoEChart.Legends.Add(legend11);
            this.MeanQoEChart.Location = new System.Drawing.Point(6, 235);
            this.MeanQoEChart.Name = "MeanQoEChart";
            series11.ChartArea = "ChartArea1";
            series11.Legend = "Legend1";
            series11.Name = "Mean QoE";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series11.BorderWidth = borderWidth;
            this.MeanQoEChart.Series.Add(series11);
            this.MeanQoEChart.Size = new System.Drawing.Size(453, 215);
            this.MeanQoEChart.TabIndex = 1;
            this.MeanQoEChart.Text = "chart5";
            // 
            // QoEChart
            // 
            chartArea12.Name = "ChartArea1";
            this.QoEChart.ChartAreas.Add(chartArea12);
            legend12.Name = "Legend1";
            this.QoEChart.Legends.Add(legend12);
            this.QoEChart.Location = new System.Drawing.Point(4, 16);
            this.QoEChart.Name = "QoEChart";
            series12.ChartArea = "ChartArea1";
            series12.Legend = "Legend1";
            series12.Name = "QoE";
            series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series12.BorderWidth = borderWidth;
            this.QoEChart.Series.Add(series12);
            this.QoEChart.Size = new System.Drawing.Size(453, 215);
            this.QoEChart.TabIndex = 0;
            this.QoEChart.Text = "chart4";
            // 
            // PeriodNumeric
            // what's this???
            this.PeriodNumeric.Location = new System.Drawing.Point(138, 232);
            this.PeriodNumeric.Name = "PeriodNumeric";
            this.PeriodNumeric.Size = new System.Drawing.Size(68, 20);
            this.PeriodNumeric.TabIndex = 12;
            this.PeriodNumeric.Value = new decimal(new int[] {
                                                                1,
                                                                0,
                                                                0,
                                                                0});
            // 
            // ProbesNumeric
            //  what's this???
            this.ProbesNumeric.Location = new System.Drawing.Point(105, 24);
            this.ProbesNumeric.Name = "ProbesNumeric";
            this.ProbesNumeric.Size = new System.Drawing.Size(197, 20);
            this.ProbesNumeric.TabIndex = 11;
            this.ProbesNumeric.Value = new decimal(new int[] {
                                                                4,
                                                                0,
                                                                0,
                                                                0});
            //
            // LEFT PART OF THE WINDOW
            // (EXPERIMENT CONFIGURATION)
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(212, 234);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "min.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 234);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Measurements Period";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LandmarksDelayRchTxt);
            this.groupBox1.Location = new System.Drawing.Point(6, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(190, 207);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Landmarks";
            // 
            // LandmarksDelayRchTxt
            // 
            this.LandmarksDelayRchTxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LandmarksDelayRchTxt.Location = new System.Drawing.Point(6, 18);
            this.LandmarksDelayRchTxt.Name = "LandmarksDelayRchTxt";
            this.LandmarksDelayRchTxt.Size = new System.Drawing.Size(178, 183);
            this.LandmarksDelayRchTxt.TabIndex = 0;
            this.LandmarksDelayRchTxt.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of Probes";
            // 
            // groupBox10
            // "configuration" group (the whole left side of the window)
            this.groupBox10.Controls.Add(this.pictureBox1);
            this.groupBox10.Controls.Add(this.groupBox3);
            this.groupBox10.Controls.Add(this.groupBox13);
            this.groupBox10.Controls.Add(this.groupBox12);
            this.groupBox10.Controls.Add(this.groupBox11);
            this.groupBox10.Controls.Add(this.label7);
            this.groupBox10.Controls.Add(this.groupBox1);
            this.groupBox10.Controls.Add(this.PeriodNumeric);
            this.groupBox10.Controls.Add(this.label9);
            this.groupBox10.Location = new System.Drawing.Point(2, 3);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(383, 452);
            this.groupBox10.TabIndex = 13;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Configuration";
            // 
            // pictureBox1
            // probably the picture with the bolt
            this.pictureBox1.Image = global::MasterProject.Properties.Resources.Utilities;
            this.pictureBox1.Location = new System.Drawing.Point(255, 35);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 57);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox3
            // right column, landmark selection
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.LandmarksNo);
            this.groupBox3.Controls.Add(this.SelectLandmarks);
            this.groupBox3.Controls.Add(this.radioButtonRandom);
            this.groupBox3.Controls.Add(this.radioButtonIntelligent);
            this.groupBox3.Location = new System.Drawing.Point(205, 109);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(169, 117);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Landmarks Selection";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Landmarks No.";
            // 
            // LandmarksNo
            // 
            this.LandmarksNo.Location = new System.Drawing.Point(89, 21);
            this.LandmarksNo.Name = "LandmarksNo";
            this.LandmarksNo.Size = new System.Drawing.Size(63, 20);
            this.LandmarksNo.TabIndex = 21;
            // 
            // SelectLandmarks
            // 
            this.SelectLandmarks.Image = global::MasterProject.Properties.Resources._3351;
            this.SelectLandmarks.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SelectLandmarks.Location = new System.Drawing.Point(31, 68);
            this.SelectLandmarks.Name = "SelectLandmarks";
            this.SelectLandmarks.Size = new System.Drawing.Size(105, 43);
            this.SelectLandmarks.TabIndex = 17;
            this.SelectLandmarks.Text = "Select";
            this.SelectLandmarks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SelectLandmarks.UseVisualStyleBackColor = true;
            this.SelectLandmarks.Click += new System.EventHandler(this.SelectLandmarks_Click);
            // 
            // radioButtonRandom
            // 
            this.radioButtonRandom.AutoSize = true;
            this.radioButtonRandom.Checked = true;
            this.radioButtonRandom.Location = new System.Drawing.Point(15, 45);
            this.radioButtonRandom.Name = "radioButtonRandom";
            this.radioButtonRandom.Size = new System.Drawing.Size(65, 17);
            this.radioButtonRandom.TabIndex = 18;
            this.radioButtonRandom.TabStop = true;
            this.radioButtonRandom.Text = "Random";
            this.radioButtonRandom.UseVisualStyleBackColor = true;
            // 
            // radioButtonIntelligent
            // 
            this.radioButtonIntelligent.AutoSize = true;
            this.radioButtonIntelligent.Location = new System.Drawing.Point(86, 45);
            this.radioButtonIntelligent.Name = "radioButtonIntelligent";
            this.radioButtonIntelligent.Size = new System.Drawing.Size(70, 17);
            this.radioButtonIntelligent.TabIndex = 19;
            this.radioButtonIntelligent.Text = "Intelligent";
            this.radioButtonIntelligent.UseVisualStyleBackColor = true;
            // 
            // groupBox13
            // DECISION TREE goes here
            this.groupBox13.Controls.Add(this.DecisionTreeTxtBox);
            this.groupBox13.Controls.Add(this.label11);
            this.groupBox13.Controls.Add(this.button5);
            this.groupBox13.Location = new System.Drawing.Point(6, 395);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(368, 47);
            this.groupBox13.TabIndex = 10;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "QoE Config.";
            // 
            // DecisionTreeTxtBox
            // 
            this.DecisionTreeTxtBox.Location = new System.Drawing.Point(137, 20);
            this.DecisionTreeTxtBox.Name = "DecisionTreeTxtBox";
            this.DecisionTreeTxtBox.Size = new System.Drawing.Size(129, 20);
            this.DecisionTreeTxtBox.TabIndex = 16;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Choose Decision Tree";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(275, 18);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(87, 23);
            this.button5.TabIndex = 15;
            this.button5.Text = "Browse..";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.BWFileTxtBox);
            this.groupBox12.Controls.Add(this.button4);
            this.groupBox12.Controls.Add(this.label10);
            this.groupBox12.Location = new System.Drawing.Point(6, 329);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(368, 60);
            this.groupBox12.TabIndex = 15;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Choose file to transfer";
            // 
            // BWFileTxtBox
            // 
            this.BWFileTxtBox.Location = new System.Drawing.Point(83, 26);
            this.BWFileTxtBox.Name = "BWFileTxtBox";
            this.BWFileTxtBox.Size = new System.Drawing.Size(183, 20);
            this.BWFileTxtBox.TabIndex = 14;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(275, 24);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(87, 23);
            this.button4.TabIndex = 14;
            this.button4.Text = "Browse..";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 29);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Choose File";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.label1);
            this.groupBox11.Controls.Add(this.label2);
            this.groupBox11.Controls.Add(this.ProbesNumeric);
            this.groupBox11.Location = new System.Drawing.Point(6, 265);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(368, 58);
            this.groupBox11.TabIndex = 14;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Delay Config.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(311, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Probe(s)";
            // 
            // button2
            // 
            this.button2.Image = global::MasterProject.Properties.Resources.Delete1;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.Location = new System.Drawing.Point(277, 461);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(93, 39);
            this.button2.TabIndex = 1;
            this.button2.Text = "Close";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Image = global::MasterProject.Properties.Resources.checkmark;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.Location = new System.Drawing.Point(180, 461);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 39);
            this.button1.TabIndex = 1;
            this.button1.Text = "Run";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // Global window: adding all components
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 511);
            this.Controls.Add(this.groupBox10);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "ACQUA";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.JitterChart)).EndInit(); //why?
            ((System.ComponentModel.ISupportInitialize)(this.DelayChart)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DownloadBandwidthChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UploadBandwidthChart)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MeanQoEChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QoEChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PeriodNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProbesNumeric)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox LandmarksDelayRchTxt;
        private System.Windows.Forms.DataVisualization.Charting.Chart DelayChart;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown PeriodNumeric;
        private System.Windows.Forms.NumericUpDown ProbesNumeric;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DataVisualization.Charting.Chart UploadBandwidthChart;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.DataVisualization.Charting.Chart QoEChart;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataVisualization.Charting.Chart JitterChart;
        private System.Windows.Forms.TextBox DecisionTreeTxtBox;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox BWFileTxtBox;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.DataVisualization.Charting.Chart DownloadBandwidthChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart MeanQoEChart;
        private System.Windows.Forms.RadioButton radioButtonIntelligent;
        private System.Windows.Forms.RadioButton radioButtonRandom;
        private System.Windows.Forms.Button SelectLandmarks;
        private System.Windows.Forms.TextBox LandmarksNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

