namespace MasterProject
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea jitterChartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend jitterLegend = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series jitterSeries = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea delayChartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend delayLegend = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series delaySeries = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea uploadLossRateChartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend uploadLossRateLegend = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series uploadLossRateSeries = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea downloadLossRateChartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend downloadLossRateLegend = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series downloadLossRateSeries = new System.Windows.Forms.DataVisualization.Charting.Series();


            System.Windows.Forms.DataVisualization.Charting.ChartArea downloadBandwidthChartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend downloadBandwidthLegend = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series downloadBandwidthSeries = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea uploadBandwidthChartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend uploadBandwidthLegend = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series uploadBandwidthSeries = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea meanQoEChartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend meanQoELegend = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series meanQoESeries = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea QoEChartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend QoELegend = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series QoESeries = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabControl = new System.Windows.Forms.TabControl();
            // tab 1: delay and jitter
            this.jitterAndDelayTab = new System.Windows.Forms.TabPage();
            this.jitterAndDelayGroupBox = new System.Windows.Forms.GroupBox();
            this.jitterChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.delayChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            // tab 2: bandwidth
            this.bandwidthTab = new System.Windows.Forms.TabPage();
            this.bandwidthGroupBox = new System.Windows.Forms.GroupBox();
            this.downloadBandwidthChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.uploadBandwidthChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            // tab 3: loss rate
            this.lossRateTab = new System.Windows.Forms.TabPage();
            this.lossRateGroupBox = new System.Windows.Forms.GroupBox();
            this.uploadLossRateChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.downloadLossRateChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            // tab 4: QoE for the provided decision tree
            this.QoETab = new System.Windows.Forms.TabPage();
            this.QoEGroupBox = new System.Windows.Forms.GroupBox();
            this.meanQoEChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.QoEChart = new System.Windows.Forms.DataVisualization.Charting.Chart();

            this.experimentSleepTime = new System.Windows.Forms.NumericUpDown();
            this.probesNumber = new System.Windows.Forms.NumericUpDown();
            this.measurementPeriodUnitLabel = new System.Windows.Forms.Label();
            this.measurementPeriodLabel = new System.Windows.Forms.Label();
            this.landmarksListGroupBox = new System.Windows.Forms.GroupBox();
            this.landmarksListForm = new System.Windows.Forms.RichTextBox();
            this.probesNumberSelectionLabel = new System.Windows.Forms.Label();
            this.configurationGroupBox = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.landmarkSelectionGroupBox = new System.Windows.Forms.GroupBox();
            this.landmarksNumberLabel = new System.Windows.Forms.Label();
            this.LandmarksNo = new System.Windows.Forms.TextBox();
            this.SelectLandmarks = new System.Windows.Forms.Button();
            this.radioButtonRandom = new System.Windows.Forms.RadioButton();
            this.radioButtonIntelligent = new System.Windows.Forms.RadioButton();
            this.decisionTreeGroupBox = new System.Windows.Forms.GroupBox();
            // decision tree is here. 
            //TODO: allow more than one decision tree. Open a tab for each decision tree. Save tabs and decision trees in arrays
            this.decisionTreeTxtBox = new System.Windows.Forms.TextBox();
            this.decisionTreeSelectionLabel = new System.Windows.Forms.Label();
            this.decisionTreeSelectionButton = new System.Windows.Forms.Button();
            this.fileToTransferGroupBox = new System.Windows.Forms.GroupBox();
            this.BWFileTxtBox = new System.Windows.Forms.TextBox();
            this.fileToTransferSelectionButton = new System.Windows.Forms.Button();
            this.fileToTransferSelectionLabel = new System.Windows.Forms.Label();
            this.delayConfigGroupBox = new System.Windows.Forms.GroupBox();
            this.probesNumberLabel = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.jitterAndDelayTab.SuspendLayout();
            this.jitterAndDelayGroupBox.SuspendLayout();
            // "starting" graphs?
            ((System.ComponentModel.ISupportInitialize)(this.jitterChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delayChart)).BeginInit();
            this.bandwidthTab.SuspendLayout();
            this.bandwidthGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.downloadBandwidthChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uploadBandwidthChart)).BeginInit();
            this.lossRateTab.SuspendLayout();
            this.lossRateGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.downloadLossRateChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uploadLossRateChart)).BeginInit();
            this.QoETab.SuspendLayout();
            this.QoEGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.meanQoEChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QoEChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.experimentSleepTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.probesNumber)).BeginInit();
            this.landmarksListGroupBox.SuspendLayout();
            this.configurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.landmarkSelectionGroupBox.SuspendLayout();
            this.decisionTreeGroupBox.SuspendLayout();
            this.fileToTransferGroupBox.SuspendLayout();
            this.delayConfigGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // adding all tabs 
            this.tabControl.Controls.Add(this.jitterAndDelayTab);
            this.tabControl.Controls.Add(this.lossRateTab);
            this.tabControl.Controls.Add(this.bandwidthTab);
            this.tabControl.Controls.Add(this.QoETab);
            this.tabControl.Location = new System.Drawing.Point(391, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(481, 487);
            this.tabControl.TabIndex = 0;

            // CAREFUL: Naming all components with a string (e.g. someComponent.name = "myName";) is only useful
            // if you want to select that component with its assigned name. Otherwise, it's useless.
            // I kept the name assignment everywhere, I just made sure the name reflected the purpose of the component.

            // 
            // tab_delay_and_jitter
            // drawing tab1 borders and margins
            this.jitterAndDelayTab.Controls.Add(this.jitterAndDelayGroupBox);
            this.jitterAndDelayTab.Location = new System.Drawing.Point(4, 22);
            this.jitterAndDelayTab.Name = "jitterAndDelayTab";
            this.jitterAndDelayTab.Padding = new System.Windows.Forms.Padding(3);
            this.jitterAndDelayTab.Size = new System.Drawing.Size(473, 461);
            this.jitterAndDelayTab.TabIndex = 0;
            this.jitterAndDelayTab.Text = "Delay";
            this.jitterAndDelayTab.UseVisualStyleBackColor = true;
            // 
            // jitterAndDelayGroupBox 
            // adding jitter and delay charts to groupBox2 (and so to tab1 too)
            this.jitterAndDelayGroupBox.Controls.Add(this.jitterChart);
            this.jitterAndDelayGroupBox.Controls.Add(this.delayChart);
            this.jitterAndDelayGroupBox.Location = new System.Drawing.Point(3, -1);
            this.jitterAndDelayGroupBox.Name = "jitterAndDelayGroupBox";
            this.jitterAndDelayGroupBox.Size = new System.Drawing.Size(464, 487);
            this.jitterAndDelayGroupBox.TabIndex = 7;
            this.jitterAndDelayGroupBox.TabStop = false;

            int borderWidth = 5;
            // 
            // DelayChart
            // 
            delayChartArea.Name = "delayChartArea";
            delayChartArea.AxisX.Title = "";
            delayChartArea.AxisY.Title = "One-Way Delay (ms)";
            this.delayChart.ChartAreas.Add(delayChartArea);
            delayLegend.Name = "delaySeriesLegend";
            this.delayChart.Legends.Add(delayLegend);
            this.delayChart.Location = new System.Drawing.Point(5, 16);
            this.delayChart.Name = "delayChart";

            delaySeries.ChartArea = "delayChartArea";
            delaySeries.Legend = "delaySeriesLegend";
            delaySeries.Name = "Round-trip Delay";
            delaySeries.YValuesPerPoint = 2; //what's this? 2 values on the Y-axis for each point on the X-Axis? A bar, then?
            delaySeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            delaySeries.BorderWidth = borderWidth;
            this.delayChart.Series.Add(delaySeries);
            this.delayChart.Size = new System.Drawing.Size(453, 215);
            this.delayChart.TabIndex = 0;
            this.delayChart.Text = "delayChart";
            // 
            // JitterChart
            // 
            jitterChartArea.Name = "jitterChartArea";
            jitterChartArea.AxisX.Title = "";
            jitterChartArea.AxisY.Title = "Jitter (ms)";
            this.jitterChart.ChartAreas.Add(jitterChartArea);
            jitterLegend.Name = "jitterLegend";
            this.jitterChart.Legends.Add(jitterLegend);
            this.jitterChart.Location = new System.Drawing.Point(6, 237);
            this.jitterChart.Name = "jitterChart";
            jitterSeries.ChartArea = "jitterChartArea";
            jitterSeries.Legend = "jitterLegend";
            jitterSeries.Name = "Jitter";
            jitterSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            jitterSeries.BorderWidth = borderWidth;
            this.jitterChart.Series.Add(jitterSeries);

            this.jitterChart.Size = new System.Drawing.Size(452, 215);
            this.jitterChart.TabIndex = 1;
            this.jitterChart.Text = "jitterChart";

            // 
            // loss rate tab
            // drawing borders and margins
            this.lossRateTab.Controls.Add(this.lossRateGroupBox);
            this.lossRateTab.Location = new System.Drawing.Point(4, 22);
            this.lossRateTab.Name = "lossRateTab";
            this.lossRateTab.Padding = new System.Windows.Forms.Padding(3);
            this.lossRateTab.Size = new System.Drawing.Size(473, 461);
            this.lossRateTab.TabIndex = 1;
            this.lossRateTab.Text = "Loss Rate";
            this.lossRateTab.UseVisualStyleBackColor = true;
            // 
            // loss rate group box
            this.lossRateGroupBox.Controls.Add(this.downloadLossRateChart);
            this.lossRateGroupBox.Controls.Add(this.uploadLossRateChart);
            this.lossRateGroupBox.Location = new System.Drawing.Point(4, -1);
            this.lossRateGroupBox.Name = "lossRateGroupBox";
            this.lossRateGroupBox.Size = new System.Drawing.Size(463, 456);
            //this.lossRateGroupBox.TabIndex = 0;
            this.lossRateGroupBox.TabStop = false;
            // 
            // DownloadLossRateChart
            // 
            downloadLossRateChartArea.Name = "downloadLossRateChartArea";
            downloadLossRateChartArea.AxisX.Title = "";
            downloadLossRateChartArea.AxisY.Title = "Download Loss Rate (%)";
            this.downloadLossRateChart.ChartAreas.Add(downloadLossRateChartArea);
            downloadLossRateLegend.Name = "downloadLossRateLegend";
            this.downloadLossRateChart.Legends.Add(downloadLossRateLegend);
            this.downloadLossRateChart.Location = new System.Drawing.Point(6, 237);
            this.downloadLossRateChart.Name = "downloadLossRateChart";
            downloadLossRateSeries.ChartArea = "downloadLossRateChartArea";
            downloadLossRateSeries.Legend = "downloadLossRateLegend";
            downloadLossRateSeries.Name = "Download Loss Rate";
            downloadLossRateSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            downloadLossRateSeries.BorderWidth = borderWidth;
            this.downloadLossRateChart.Series.Add(downloadLossRateSeries);
            this.downloadLossRateChart.Size = new System.Drawing.Size(453, 215);
            this.downloadLossRateChart.TabIndex = 1;
            this.downloadLossRateChart.Text = "downloadLossRateChartArea";
            // 
            // UploadLossRateChart
            // 
            uploadLossRateChartArea.Name = "uploadLossRateChartArea";
            uploadLossRateChartArea.AxisX.Title = "";
            uploadLossRateChartArea.AxisY.Title = "Upload Loss Rate (%)";
            this.uploadLossRateChart.ChartAreas.Add(uploadLossRateChartArea);
            uploadLossRateLegend.Name = "uploadLossRateLegend";
            this.uploadLossRateChart.Legends.Add(uploadLossRateLegend);
            this.uploadLossRateChart.Location = new System.Drawing.Point(4, 16);
            this.uploadLossRateChart.Name = "uploadLossRateChart";
            uploadLossRateSeries.ChartArea = "uploadLossRateChartArea";
            uploadLossRateSeries.Legend = "uploadLossRateLegend";
            uploadLossRateSeries.Name = "Upload Loss Rate";
            uploadLossRateSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            uploadLossRateSeries.BorderWidth = borderWidth;
            this.uploadLossRateChart.Series.Add(uploadLossRateSeries);
            this.uploadLossRateChart.Size = new System.Drawing.Size(453, 215);
            this.uploadLossRateChart.TabIndex = 0;
            this.uploadLossRateChart.Text = "uploadLossRateChart";


            // 
            // tab_bandwidth
            // drawing tab2 borders and margins
            this.bandwidthTab.Controls.Add(this.bandwidthGroupBox);
            this.bandwidthTab.Location = new System.Drawing.Point(4, 22);
            this.bandwidthTab.Name = "bandwidthTab";
            this.bandwidthTab.Padding = new System.Windows.Forms.Padding(3);
            this.bandwidthTab.Size = new System.Drawing.Size(473, 461);
            this.bandwidthTab.TabIndex = 1;
            this.bandwidthTab.Text = "Bandwidth";
            this.bandwidthTab.UseVisualStyleBackColor = true;
            // 
            // bandwidth group box
            // adding bw charts to groupBox6 (and so to tab2 too)
            this.bandwidthGroupBox.Controls.Add(this.downloadBandwidthChart);
            this.bandwidthGroupBox.Controls.Add(this.uploadBandwidthChart);
            this.bandwidthGroupBox.Location = new System.Drawing.Point(4, -1);
            this.bandwidthGroupBox.Name = "bandwidthGroupBox";
            this.bandwidthGroupBox.Size = new System.Drawing.Size(463, 456);
            this.bandwidthGroupBox.TabIndex = 9;
            this.bandwidthGroupBox.TabStop = false;
            // 
            // DownloadBandwidthChart
            // 
            downloadBandwidthChartArea.Name = "downloadBandwidthChartArea";
            downloadBandwidthChartArea.AxisX.Title = "";
            downloadBandwidthChartArea.AxisY.Title = "Download Bandwidth (Kbs)";
            this.downloadBandwidthChart.ChartAreas.Add(downloadBandwidthChartArea);
            downloadBandwidthLegend.Name = "downloadBandwidthLegend";
            this.downloadBandwidthChart.Legends.Add(downloadBandwidthLegend);
            this.downloadBandwidthChart.Location = new System.Drawing.Point(6, 237);
            this.downloadBandwidthChart.Name = "downloadBandwidthChart";
            downloadBandwidthSeries.ChartArea = "downloadBandwidthChartArea";
            downloadBandwidthSeries.Legend = "downloadBandwidthLegend";
            downloadBandwidthSeries.Name = "Download Bandwidth";
            downloadBandwidthSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            downloadBandwidthSeries.BorderWidth = borderWidth;
            this.downloadBandwidthChart.Series.Add(downloadBandwidthSeries);
            this.downloadBandwidthChart.Size = new System.Drawing.Size(453, 215);
            this.downloadBandwidthChart.TabIndex = 1;
            this.downloadBandwidthChart.Text = "downloadBandwidthChartArea";
            // 
            // UploadBandwidthChart
            // 
            uploadBandwidthChartArea.Name = "uploadBandwidthChartArea";
            uploadBandwidthChartArea.AxisX.Title = "";
            uploadBandwidthChartArea.AxisY.Title = "Upload Bandwidth (Kbs)";
            this.uploadBandwidthChart.ChartAreas.Add(uploadBandwidthChartArea);
            uploadBandwidthLegend.Name = "uploadBandwidthLegend";
            this.uploadBandwidthChart.Legends.Add(uploadBandwidthLegend);
            this.uploadBandwidthChart.Location = new System.Drawing.Point(4, 16);
            this.uploadBandwidthChart.Name = "uploadBandwidthChart";
            uploadBandwidthSeries.ChartArea = "uploadBandwidthChartArea";
            uploadBandwidthSeries.Legend = "uploadBandwidthLegend";
            uploadBandwidthSeries.Name = "Upload Bandwidth";
            uploadBandwidthSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            uploadBandwidthSeries.BorderWidth = borderWidth;
            this.uploadBandwidthChart.Series.Add(uploadBandwidthSeries);
            this.uploadBandwidthChart.Size = new System.Drawing.Size(453, 215);
            this.uploadBandwidthChart.TabIndex = 0;
            this.uploadBandwidthChart.Text = "uploadBandwidthChart";
            // 
            // QoE tab
            // drawing tab4 borders and margins
            this.QoETab.Controls.Add(this.QoEGroupBox);
            this.QoETab.Location = new System.Drawing.Point(4, 22);
            this.QoETab.Name = "QoETab";
            this.QoETab.Padding = new System.Windows.Forms.Padding(3);
            this.QoETab.Size = new System.Drawing.Size(473, 461);
            this.QoETab.TabIndex = 3;
            this.QoETab.Text = "Skype QoE";
            this.QoETab.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // dding QoE charts to groupBox8 (and so to tab4 too)
            this.QoEGroupBox.Controls.Add(this.meanQoEChart);
            this.QoEGroupBox.Controls.Add(this.QoEChart);
            this.QoEGroupBox.Location = new System.Drawing.Point(4, -1);
            this.QoEGroupBox.Name = "QoEGroupBox";
            this.QoEGroupBox.Size = new System.Drawing.Size(463, 456);
            this.QoEGroupBox.TabIndex = 9;
            this.QoEGroupBox.TabStop = false;

            // 
            // QoEChart (one value per landmark)
            // TODO: one LINE per landmark
            // 
            QoEChartArea.Name = "QoEChartArea";
            QoEChartArea.AxisX.Title = "";
            QoEChartArea.AxisY.Title = "QoE";
            this.QoEChart.ChartAreas.Add(QoEChartArea);
            QoELegend.Name = "QoELegend";
            this.QoEChart.Legends.Add(QoELegend);
            this.QoEChart.Location = new System.Drawing.Point(4, 16);
            this.QoEChart.Name = "QoEChart";
            QoESeries.ChartArea = "QoEChartArea";
            QoESeries.Legend = "QoELegend";
            QoESeries.Name = "QoE";
            QoESeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            QoESeries.BorderWidth = borderWidth;
            this.QoEChart.Series.Add(QoESeries);
            this.QoEChart.Size = new System.Drawing.Size(453, 215);
            this.QoEChart.TabIndex = 0;
            this.QoEChart.Text = "QoEChart";

            // 
            // MeanQoEChart
            // 
            meanQoEChartArea.Name = "meanQoEChartArea";
            meanQoEChartArea.AxisX.Title = "";
            meanQoEChartArea.AxisY.Title = "QoE";
            this.meanQoEChart.ChartAreas.Add(meanQoEChartArea);
            meanQoELegend.Name = "meanQoELegend";
            this.meanQoEChart.Legends.Add(meanQoELegend);
            this.meanQoEChart.Location = new System.Drawing.Point(6, 235);
            this.meanQoEChart.Name = "meanQoEChart";
            meanQoESeries.ChartArea = "meanQoEChartArea";
            meanQoESeries.Legend = "meanQoELegend";
            meanQoESeries.Name = "Mean QoE";
            meanQoESeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            meanQoESeries.BorderWidth = borderWidth;
            this.meanQoEChart.Series.Add(meanQoESeries);
            this.meanQoEChart.Size = new System.Drawing.Size(453, 215);
            this.meanQoEChart.TabIndex = 1;
            this.meanQoEChart.Text = "meanQoEChart";

            // 
            // PeriodNumeric
            this.experimentSleepTime.Location = new System.Drawing.Point(138, 232);
            this.experimentSleepTime.Name = "experimentSleepTime";
            this.experimentSleepTime.Size = new System.Drawing.Size(68, 20);
            this.experimentSleepTime.TabIndex = 12;
            this.experimentSleepTime.Value = new decimal(new int[] {
                                                                1,
                                                                0,
                                                                0,
                                                                0});
            // 
            // ProbesNumeric
            //  what's this???
            this.probesNumber.Location = new System.Drawing.Point(105, 24);
            this.probesNumber.Name = "probesNumber";
            this.probesNumber.Size = new System.Drawing.Size(197, 20);
            this.probesNumber.TabIndex = 11;
            this.probesNumber.Value = new decimal(new int[] {
                                                                4,
                                                                0,
                                                                0,
                                                                0});
            //
            // LEFT PART OF THE WINDOW
            // (EXPERIMENT CONFIGURATION)

            // 
            // measurementPeriodUnitLabel
            // 
            this.measurementPeriodUnitLabel.AutoSize = true;
            this.measurementPeriodUnitLabel.Location = new System.Drawing.Point(212, 234);
            this.measurementPeriodUnitLabel.Name = "measurementPeriodUnitLabel";
            this.measurementPeriodUnitLabel.Size = new System.Drawing.Size(26, 13);
            this.measurementPeriodUnitLabel.TabIndex = 10;
            this.measurementPeriodUnitLabel.Text = "min.";
            // 
            // measurementPeriodLabel
            // 
            this.measurementPeriodLabel.AutoSize = true;
            this.measurementPeriodLabel.Location = new System.Drawing.Point(12, 234);
            this.measurementPeriodLabel.Name = "measurementPeriodLabel";
            this.measurementPeriodLabel.Size = new System.Drawing.Size(109, 13);
            this.measurementPeriodLabel.TabIndex = 8;
            this.measurementPeriodLabel.Text = "Measurements Period";
            // 
            // landmarksListGroupBox
            // 
            this.landmarksListGroupBox.Controls.Add(this.landmarksListForm);
            this.landmarksListGroupBox.Location = new System.Drawing.Point(6, 19);
            this.landmarksListGroupBox.Name = "landmarksListGroupBox";
            this.landmarksListGroupBox.Size = new System.Drawing.Size(190, 207);
            this.landmarksListGroupBox.TabIndex = 6;
            this.landmarksListGroupBox.TabStop = false;
            this.landmarksListGroupBox.Text = "Landmarks";
            // 
            // LandmarksDelayRchTxt
            // 
            this.landmarksListForm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.landmarksListForm.Location = new System.Drawing.Point(6, 18);
            this.landmarksListForm.Name = "landmarksListForm";
            this.landmarksListForm.Size = new System.Drawing.Size(178, 183);
            this.landmarksListForm.TabIndex = 0;
            this.landmarksListForm.Text = "";
            // 
            // probesNumberSelectionLabel
            // 
            this.probesNumberSelectionLabel.AutoSize = true;
            this.probesNumberSelectionLabel.Location = new System.Drawing.Point(7, 26);
            this.probesNumberSelectionLabel.Name = "probesNumberSelectionLabel";
            this.probesNumberSelectionLabel.Size = new System.Drawing.Size(92, 13);
            this.probesNumberSelectionLabel.TabIndex = 1;
            this.probesNumberSelectionLabel.Text = "Number of Probes";
            // 
            // configurationGroupBox
            // "configuration" group (the whole left side of the window)
            this.configurationGroupBox.Controls.Add(this.pictureBox1);
            this.configurationGroupBox.Controls.Add(this.landmarkSelectionGroupBox);
            this.configurationGroupBox.Controls.Add(this.decisionTreeGroupBox);
            this.configurationGroupBox.Controls.Add(this.fileToTransferGroupBox);
            this.configurationGroupBox.Controls.Add(this.delayConfigGroupBox);
            this.configurationGroupBox.Controls.Add(this.measurementPeriodLabel);
            this.configurationGroupBox.Controls.Add(this.landmarksListGroupBox);
            this.configurationGroupBox.Controls.Add(this.experimentSleepTime);
            this.configurationGroupBox.Controls.Add(this.measurementPeriodUnitLabel);
            this.configurationGroupBox.Location = new System.Drawing.Point(2, 3);
            this.configurationGroupBox.Name = "configurationGroupBox";
            this.configurationGroupBox.Size = new System.Drawing.Size(383, 452);
            this.configurationGroupBox.TabIndex = 13;
            this.configurationGroupBox.TabStop = false;
            this.configurationGroupBox.Text = "Configuration";
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
            // landmarkSelectionGroupBox
            // right column, landmark selection
            this.landmarkSelectionGroupBox.Controls.Add(this.landmarksNumberLabel);
            this.landmarkSelectionGroupBox.Controls.Add(this.LandmarksNo);
            this.landmarkSelectionGroupBox.Controls.Add(this.SelectLandmarks);
            this.landmarkSelectionGroupBox.Controls.Add(this.radioButtonRandom);
            this.landmarkSelectionGroupBox.Controls.Add(this.radioButtonIntelligent);
            this.landmarkSelectionGroupBox.Location = new System.Drawing.Point(205, 109);
            this.landmarkSelectionGroupBox.Name = "landmarkSelectionGroupBox";
            this.landmarkSelectionGroupBox.Size = new System.Drawing.Size(169, 117);
            this.landmarkSelectionGroupBox.TabIndex = 22;
            this.landmarkSelectionGroupBox.TabStop = false;
            this.landmarkSelectionGroupBox.Text = "Landmarks Selection";
            // 
            // landmarksNumberLabel
            // 
            this.landmarksNumberLabel.AutoSize = true;
            this.landmarksNumberLabel.Location = new System.Drawing.Point(10, 24);
            this.landmarksNumberLabel.Name = "landmarksNumberLabel";
            this.landmarksNumberLabel.Size = new System.Drawing.Size(79, 13);
            this.landmarksNumberLabel.TabIndex = 20;
            this.landmarksNumberLabel.Text = "Landmarks No.";
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
            this.decisionTreeGroupBox.Controls.Add(this.decisionTreeTxtBox);
            this.decisionTreeGroupBox.Controls.Add(this.decisionTreeSelectionLabel);
            this.decisionTreeGroupBox.Controls.Add(this.decisionTreeSelectionButton);
            this.decisionTreeGroupBox.Location = new System.Drawing.Point(6, 395);
            this.decisionTreeGroupBox.Name = "decisionTreeGroupBox";
            this.decisionTreeGroupBox.Size = new System.Drawing.Size(368, 47);
            this.decisionTreeGroupBox.TabIndex = 10;
            this.decisionTreeGroupBox.TabStop = false;
            this.decisionTreeGroupBox.Text = "QoE Config.";
            // 
            // DecisionTreeTxtBox
            // 
            this.decisionTreeTxtBox.Location = new System.Drawing.Point(137, 20);
            this.decisionTreeTxtBox.Name = "decisionTreeTxtBox";
            this.decisionTreeTxtBox.Size = new System.Drawing.Size(129, 20);
            this.decisionTreeTxtBox.TabIndex = 16;
            // 
            // decisionTreeSelectionLabel
            // 
            this.decisionTreeSelectionLabel.AutoSize = true;
            this.decisionTreeSelectionLabel.Location = new System.Drawing.Point(7, 25);
            this.decisionTreeSelectionLabel.Name = "decisionTreeSelectionLabel";
            this.decisionTreeSelectionLabel.Size = new System.Drawing.Size(112, 13);
            this.decisionTreeSelectionLabel.TabIndex = 2;
            this.decisionTreeSelectionLabel.Text = "Choose Decision Tree";
            // 
            // decisionTreeSelectionButton
            // 
            this.decisionTreeSelectionButton.Location = new System.Drawing.Point(275, 18);
            this.decisionTreeSelectionButton.Name = "decisionTreeSelectionButton";
            this.decisionTreeSelectionButton.Size = new System.Drawing.Size(87, 23);
            this.decisionTreeSelectionButton.TabIndex = 15;
            this.decisionTreeSelectionButton.Text = "Browse...";
            this.decisionTreeSelectionButton.UseVisualStyleBackColor = true;
            this.decisionTreeSelectionButton.Click += new System.EventHandler(this.button5_Click);
            // 
            // fileToTransferGroupBox
            // 
            this.fileToTransferGroupBox.Controls.Add(this.BWFileTxtBox);
            this.fileToTransferGroupBox.Controls.Add(this.fileToTransferSelectionButton);
            this.fileToTransferGroupBox.Controls.Add(this.fileToTransferSelectionLabel);
            this.fileToTransferGroupBox.Location = new System.Drawing.Point(6, 329);
            this.fileToTransferGroupBox.Name = "fileToTransferGroupBox";
            this.fileToTransferGroupBox.Size = new System.Drawing.Size(368, 60);
            this.fileToTransferGroupBox.TabIndex = 15;
            this.fileToTransferGroupBox.TabStop = false;
            this.fileToTransferGroupBox.Text = "Choose file to transfer";
            // 
            // BWFileTxtBox
            // 
            this.BWFileTxtBox.Location = new System.Drawing.Point(83, 26);
            this.BWFileTxtBox.Name = "BWFileTxtBox";
            this.BWFileTxtBox.Size = new System.Drawing.Size(183, 20);
            this.BWFileTxtBox.TabIndex = 14;
            // 
            // fileToTransferSelectionButton
            // 
            this.fileToTransferSelectionButton.Location = new System.Drawing.Point(275, 24);
            this.fileToTransferSelectionButton.Name = "fileToTransferSelectionButton";
            this.fileToTransferSelectionButton.Size = new System.Drawing.Size(87, 23);
            this.fileToTransferSelectionButton.TabIndex = 14;
            this.fileToTransferSelectionButton.Text = "Browse...";
            this.fileToTransferSelectionButton.UseVisualStyleBackColor = true;
            // 
            // fileToTransferSelectionLabel
            // 
            this.fileToTransferSelectionLabel.AutoSize = true;
            this.fileToTransferSelectionLabel.Location = new System.Drawing.Point(7, 29);
            this.fileToTransferSelectionLabel.Name = "fileToTransferSelectionLabel";
            this.fileToTransferSelectionLabel.Size = new System.Drawing.Size(62, 13);
            this.fileToTransferSelectionLabel.TabIndex = 1;
            this.fileToTransferSelectionLabel.Text = "Choose File";
            // 
            // delayConfigGroupBox (configuration for ping test)
            // 
            this.delayConfigGroupBox.Controls.Add(this.probesNumberLabel);
            this.delayConfigGroupBox.Controls.Add(this.probesNumberSelectionLabel);
            this.delayConfigGroupBox.Controls.Add(this.probesNumber);
            this.delayConfigGroupBox.Location = new System.Drawing.Point(6, 265);
            this.delayConfigGroupBox.Name = "delayConfigGroupBox";
            this.delayConfigGroupBox.Size = new System.Drawing.Size(368, 58);
            this.delayConfigGroupBox.TabIndex = 14;
            this.delayConfigGroupBox.TabStop = false;
            this.delayConfigGroupBox.Text = "Delay Config.";
            // 
            // probesNumberLabel
            // 
            this.probesNumberLabel.AutoSize = true;
            this.probesNumberLabel.Location = new System.Drawing.Point(311, 26);
            this.probesNumberLabel.Name = "probesNumberLabel";
            this.probesNumberLabel.Size = new System.Drawing.Size(46, 13);
            this.probesNumberLabel.TabIndex = 9;
            this.probesNumberLabel.Text = "Probe(s)";
            
            // 
            // button1
            // 
            this.okButton.Image = global::MasterProject.Properties.Resources.checkmark;
            this.okButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.okButton.Location = new System.Drawing.Point(180, 461);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(81, 39);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "Run";
            this.okButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.closeButton.Image = global::MasterProject.Properties.Resources.Delete1;
            this.closeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.closeButton.Location = new System.Drawing.Point(277, 461);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(93, 39);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // Global window: adding all components
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 511);
            this.Controls.Add(this.configurationGroupBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "ACQUA";
            this.tabControl.ResumeLayout(false);
            this.jitterAndDelayTab.ResumeLayout(false);
            this.jitterAndDelayGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.jitterChart)).EndInit(); //why?
            ((System.ComponentModel.ISupportInitialize)(this.delayChart)).EndInit();
            this.bandwidthTab.ResumeLayout(false);
            this.bandwidthGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.downloadBandwidthChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uploadBandwidthChart)).EndInit();
            this.lossRateTab.ResumeLayout(false);
            this.lossRateGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.downloadLossRateChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uploadLossRateChart)).EndInit();
            this.QoETab.ResumeLayout(false);
            this.QoEGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.meanQoEChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QoEChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.experimentSleepTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.probesNumber)).EndInit();
            this.landmarksListGroupBox.ResumeLayout(false);
            this.configurationGroupBox.ResumeLayout(false);
            this.configurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.landmarkSelectionGroupBox.ResumeLayout(false);
            this.landmarkSelectionGroupBox.PerformLayout();
            this.decisionTreeGroupBox.ResumeLayout(false);
            this.decisionTreeGroupBox.PerformLayout();
            this.fileToTransferGroupBox.ResumeLayout(false);
            this.fileToTransferGroupBox.PerformLayout();
            this.delayConfigGroupBox.ResumeLayout(false);
            this.delayConfigGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage bandwidthTab;
        private System.Windows.Forms.TabPage lossRateTab;
        private System.Windows.Forms.TabPage jitterAndDelayTab;
        private System.Windows.Forms.TabPage QoETab;
        private System.Windows.Forms.Label probesNumberSelectionLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.GroupBox landmarksListGroupBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.GroupBox jitterAndDelayGroupBox;
        private System.Windows.Forms.RichTextBox landmarksListForm;
        private System.Windows.Forms.DataVisualization.Charting.Chart delayChart;
        private System.Windows.Forms.Label measurementPeriodUnitLabel;
        private System.Windows.Forms.Label measurementPeriodLabel;
        private System.Windows.Forms.NumericUpDown experimentSleepTime;
        private System.Windows.Forms.NumericUpDown probesNumber;
        private System.Windows.Forms.GroupBox bandwidthGroupBox;
        private System.Windows.Forms.DataVisualization.Charting.Chart uploadBandwidthChart;
        private System.Windows.Forms.GroupBox lossRateGroupBox;
        private System.Windows.Forms.DataVisualization.Charting.Chart uploadLossRateChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart downloadLossRateChart;
        private System.Windows.Forms.GroupBox QoEGroupBox;
        private System.Windows.Forms.DataVisualization.Charting.Chart QoEChart;
        private System.Windows.Forms.GroupBox configurationGroupBox;
        private System.Windows.Forms.GroupBox delayConfigGroupBox;
        private System.Windows.Forms.Label probesNumberLabel;
        private System.Windows.Forms.GroupBox fileToTransferGroupBox;
        private System.Windows.Forms.Label fileToTransferSelectionLabel;
        private System.Windows.Forms.GroupBox decisionTreeGroupBox;
        private System.Windows.Forms.Label decisionTreeSelectionLabel;
        private System.Windows.Forms.DataVisualization.Charting.Chart jitterChart;
        private System.Windows.Forms.TextBox decisionTreeTxtBox;
        private System.Windows.Forms.Button decisionTreeSelectionButton;
        private System.Windows.Forms.TextBox BWFileTxtBox;
        private System.Windows.Forms.Button fileToTransferSelectionButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart downloadBandwidthChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart meanQoEChart;
        private System.Windows.Forms.RadioButton radioButtonIntelligent;
        private System.Windows.Forms.RadioButton radioButtonRandom;
        private System.Windows.Forms.Button SelectLandmarks;
        private System.Windows.Forms.TextBox LandmarksNo;
        private System.Windows.Forms.Label landmarksNumberLabel;
        private System.Windows.Forms.GroupBox landmarkSelectionGroupBox;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

