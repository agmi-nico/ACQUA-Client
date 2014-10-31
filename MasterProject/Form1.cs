using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Net.NetworkInformation;

namespace MasterProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        int max_num_experiments = 10000;
        int LandmarksNumber = 0, index;
        MeasurementVector[] FinalDelayAndJitterArray;
        MeasurementVector[] FinalBandwidthArray;
        //Time[] TimeArray;

        private void button1_Click(object sender, EventArgs e)
        {
            int[] series = new int[2];
            series[1] = 2;
            series[0] = 1;

            FinalDelayAndJitterArray = new MeasurementVector[max_num_experiments];
            FinalBandwidthArray = new MeasurementVector[max_num_experiments];
            //TimeArray = new Time[max_num_experiments];
            LandmarksNumber = 0;

            if (LandmarksDelayRchTxt.Text == "")
            {
                LandNumber = 1;
                BeginLandmarksSelection();
            }
            if (BWFileTxtBox.Text == "")
            {
                MessageBox.Show("File for data transfer missing", "Error");
                return;
            }
            if (DecisionTreeTxtBox.Text == "")
            {
                MessageBox.Show("Decision Tree missing", "Error");
                return;
            }

            if (ProbesNumeric.Value == 1)
            {
                DialogResult dr = MessageBox.Show("Jitter can't be calculated with only one probe. Do you want to continue?", "Alert", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                    return;
            }

            // Count the number of landmarks in the richTextBox
            foreach (string landmark in LandmarksDelayRchTxt.Lines)
                LandmarksNumber++;

            Thread t = new Thread(new ThreadStart(Run));
            System.Console.WriteLine("Starting...");
            t.Start();
        }

        //int MeanResult = -1;
        int[,] QoEPerLandmark;
        int[,] skypeQoEPerLandmark;
        int max_points_per_graph = 10;
        int[] meanQoE;
        int[] skypeQoE;

        void Run()
        {
            QoEPerLandmark = new int[max_num_experiments, LandmarksNumber];
            skypeQoEPerLandmark = new int[max_num_experiments, LandmarksNumber];
            meanQoE = new int[max_num_experiments];
            skypeQoE = new int[max_num_experiments];
            int LocalIndex = 0;
            while (true)
            {
                // Compute delay and jitter 
                FinalDelayAndJitterArray[LocalIndex] = PingSetOfLandmarks();
                if (FinalDelayAndJitterArray[LocalIndex] == null)
                    return;
                // Compute upload and download bandwidth (or rather, the goodput)
                FinalBandwidthArray[LocalIndex] = SendFileToSetOfLandmarks();
                if (FinalBandwidthArray[LocalIndex] == null)
                    return;
                int download_probing_rate = (int)FinalBandwidthArray[LocalIndex].dimension1 / 2;
                //asdf (??)
                int upload_probing_rate = (int)FinalBandwidthArray[LocalIndex].dimension2 / 2;
                // Compute loss rate
                int[] LossRate = new int[2];
                int probing_duration = 5;
                //LossRate = PingHostLossProb(AllLandmarks[LocalIndex], LocalIndex, upload_probing_rate, download_probing_rate, probing_duration);
                //// Make QoE Estimation
                //// For the Set of Landmarks First
                float[] LossRatePerLandmark = new float[2];
                for (int i = 0; i < LandmarksNumber; i++)
                {
                    LossRatePerLandmark = PingHostLossProb(AllLandmarks[i], i, upload_probing_rate, download_probing_rate, probing_duration);
                    // Estimate QoE with the provided tree
                    QoEPerLandmark[LocalIndex, i] = CallerClass.Call(root,                             // Decision-tree Root
                                                        TwoWayDelayPerLandmarkArray[i],   // Delay
                                                        JitterPerLandmarkArray[i],        // Jitter
                                                        UploadBWPerLandmarkArray[i],      // UBandwidth
                                                        DownloadBWPerLandmarkArray[i],    // DBandwidth
                                                        (int)LossRatePerLandmark[0],           // DLossRate
                                                        (int)LossRatePerLandmark[1]);          // ULossRate 
                    // just to compare the above result with the hard-coded tree in the bottom of this file
                    skypeQoEPerLandmark[LocalIndex, i] = estimatedSkypeQuality(DownloadBWPerLandmarkArray[i],
                                                                    UploadBWPerLandmarkArray[i],
                                                                    (int)TwoWayDelayPerLandmarkArray[i] / 2,
                                                                    (int)TwoWayDelayPerLandmarkArray[i] / 2,
                                                                    (int)LossRatePerLandmark[0],
                                                                    (int)LossRatePerLandmark[1]);
                    LossRate[0] += (int)LossRatePerLandmark[0];
                    LossRate[1] += (int)LossRatePerLandmark[1];
                }
                //LossRate is the average loss rate per landmark
                LossRate[0] /= LandmarksNumber;
                LossRate[1] /= LandmarksNumber;

                System.Console.WriteLine("Mean QoE");
                //TwoWayDelayArray[LocalIndex]
                System.Console.WriteLine("Mean QoE Delay:{0} Jitter:{1} UBW:{2} DBW:{3} DLR:{4} ULR:{5}",
                                            (int)FinalDelayAndJitterArray[LocalIndex].dimension1 / 2,
                                            (int)FinalDelayAndJitterArray[LocalIndex].dimension2 / 2,
                                            (int)FinalBandwidthArray[LocalIndex].dimension1,
                                            (int)FinalBandwidthArray[LocalIndex].dimension2,
                                            (int)LossRate[0],
                                            (int)LossRate[1]);
                //// Then for the Mean Values of Measurements

                meanQoE[LocalIndex] = CallerClass.Call(root,                                            // Decision-tree Root
                                             (int)FinalDelayAndJitterArray[LocalIndex].dimension1 / 2, // Delay
                                              (int)FinalDelayAndJitterArray[LocalIndex].dimension2 / 2, // Jitter
                                              (int)FinalBandwidthArray[LocalIndex].dimension1,      // UBandwidth
                                              (int)FinalBandwidthArray[LocalIndex].dimension2,      // DBandwidth
                                              (int)LossRate[0],                                     // DLossRate
                                              (int)LossRate[1]);                                    // ULossRate
                skypeQoE[LocalIndex] = estimatedSkypeQuality((int)FinalBandwidthArray[LocalIndex].dimension2,
                                                            (int)FinalBandwidthArray[LocalIndex].dimension1,
                                                            (int)FinalDelayAndJitterArray[LocalIndex].dimension1 / 2,
                                                            (int)FinalDelayAndJitterArray[LocalIndex].dimension1 / 2,
                                                            (int)LossRate[0],
                                                            (int)LossRate[1]);
                /*
                // Get Full Time
                // This will appear on the x axis...
                TimeArray[LocalIndex] = new Time();
                */
                if (LocalIndex > max_points_per_graph)
                {
                    // for each graph, clear everything and redraw the last 19 points. Add the 20th at the end.
                    //delay
                    this.DelayChart.Series["Round-trip Delay"].Points.Clear();
                    int xval = 0;
                    for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                    {
                        this.DelayChart.Series["Round-trip Delay"].Points.AddXY((double)xval, FinalDelayAndJitterArray[pos].dimension1);
                        xval++;
                    }


                    //jitter
                    this.JitterChart.Series["Jitter"].Points.Clear();
                    xval = 0;
                    for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                    {
                        this.JitterChart.Series["Jitter"].Points.AddXY((double)xval, FinalDelayAndJitterArray[pos].dimension2);
                        xval++;
                    }

                    // upload bw
                    this.UploadBandwidthChart.Series["Upload Bandwidth"].Points.Clear();
                    xval = 0;
                    for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                    {
                        this.UploadBandwidthChart.Series["Upload Bandwidth"].Points.AddXY((double)xval, FinalBandwidthArray[pos].dimension1);
                        xval++;
                    }

                    // download bw
                    this.DownloadBandwidthChart.Series["Download Bandwidth"].Points.Clear();
                    xval = 0;
                    for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                    {
                        this.DownloadBandwidthChart.Series["Download Bandwidth"].Points.AddXY((double)xval, FinalBandwidthArray[pos].dimension2);
                        xval++;
                    }

                    // QoE 
                    for (int i = 0; i < LandmarksNumber; i++)
                    {
                        //this.QoEChart.Series["QoE"].Points.AddXY("Landmark " + i + 1, QoEPerLandmark[LocalIndex, i]); // TODO how does this work?
                        this.QoEChart.Series["QoE"].Points.Clear();
                        xval = 0;
                        for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                        {
                            this.QoEChart.Series["QoE"].Points.AddXY((double)xval, QoEPerLandmark[pos, i]);
                            xval++;
                        }
                    }
                    // QoE bis
                    this.MeanQoEChart.Series["Mean QoE"].Points.Clear();
                    xval = 0;
                    for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                    {
                        this.MeanQoEChart.Series["Mean QoE"].Points.AddXY((double)xval, meanQoE[pos]);
                        xval++;
                    }
                }
                // Draw the Charts
                // I have to:
                /*
                 * For each chart:
                 * check whether the series has more than n pairs of numbers,
                 * if it doesn't, just add the current pair of points
                 * if it does, you have to extract all points, emove the first point and decrease by 1 the x-value of all the remaining points, and plot them
                 * Then, you add the current point
                 * 
                 * 
                 * 
                 * /
                */
                //this.DelayChart.Series["Round-trip Delay"] = AddPointToFixedSizeCollection((double) TimeArray[LocalIndex], FinalDelayAndJitterArray[LocalIndex].dimension1, this.DelayChart.Series["Round-trip Delay"], max_points_per_graph);
                //this.DelayChart.Series["Round-trip Delay"].Points.RemoveAt
                //System.Windows.Forms.[] plotted_data = new Array[this.DelayChart.Series["Round-trip Delay"].Points.Count];
                //this.DelayChart.Series["Round-trip Delay"].Points.CopyTo(plotted_data, 0);
                else
                {
                    this.DelayChart.Series["Round-trip Delay"].Points.AddXY(LocalIndex, FinalDelayAndJitterArray[LocalIndex].dimension1);
                    this.JitterChart.Series["Jitter"].Points.AddXY(LocalIndex, FinalDelayAndJitterArray[LocalIndex].dimension2);
                    this.UploadBandwidthChart.Series["Upload Bandwidth"].Points.AddXY(LocalIndex, FinalBandwidthArray[LocalIndex].dimension1);
                    this.DownloadBandwidthChart.Series["Download Bandwidth"].Points.AddXY(LocalIndex, FinalBandwidthArray[LocalIndex].dimension2);
                    // TODO: Loss Rate!!! We need a tab with the loss rate (upstream and downstream!)
                    for (int i = 0; i < LandmarksNumber; i++)
                    {
                        this.QoEChart.Series["QoE"].Points.AddXY("Landmark " + i + 1, QoEPerLandmark[LocalIndex, i]);
                    }
                    this.MeanQoEChart.Series["Mean QoE"].Points.AddXY(LocalIndex, meanQoE[LocalIndex]);
                }

                Console.WriteLine("HardCoded calculation:{0}", skypeQoE[LocalIndex]);
                LocalIndex++;
                //Thread.Sleep((int)PeriodNumeric.Value * 60 * 1000); // sleep for 6 seconds * what ???
            }
        }

        // Delay and Jitter Measurement
        long elapsedMs = 0;
        double Total2WDPerLandmark = 0, Total2WD = 0, _2WD = 0;
        double TotalJitterPerLandmark = 0, TotalJitter = 0;
        int[] TwoWayDelayPerLandmarkArray;
        int[] JitterPerLandmarkArray;
        int MeanTwowayDelay = 0, MeanJitter = 0;
        double[] TwoWayDelayArray;
        bool MoreThanOneCampaign = true;

        public MeasurementVector PingSetOfLandmarks()
        {
            index = 0;
            Total2WD = 0;
            Total2WDPerLandmark = 0;

            if (ProbesNumeric.Value == 1)
                MoreThanOneCampaign = false;

            TwoWayDelayPerLandmarkArray = new int[LandmarksNumber];
            JitterPerLandmarkArray = new int[LandmarksNumber];

            // Ping each landmark
            foreach (string Landmark in LandmarksDelayRchTxt.Lines)
            {
                Total2WDPerLandmark = 0;
                TwoWayDelayArray = new double[(int)ProbesNumeric.Value];
                // Ping selected landmark n times (ProbesNumeric.Value)
                for (int i = 0; i < ProbesNumeric.Value; i++)
                {
                    int RTT = PingHostDelay(Landmark);

                    Total2WDPerLandmark += RTT;
                    TwoWayDelayArray[i] = RTT;
                }

                // Jitter Per Landmark
                if (MoreThanOneCampaign)
                {
                    TotalJitterPerLandmark = 0;
                    for (int i = 0; i < TwoWayDelayArray.Length - 1; i++)
                        TotalJitterPerLandmark += Math.Abs((TwoWayDelayArray[i + 1] - TwoWayDelayArray[i]));
                    JitterPerLandmarkArray[index] = (int)(TotalJitterPerLandmark / (double)ProbesNumeric.Value);
                }

                // 2WD value Per Landmark
                TwoWayDelayPerLandmarkArray[index] = (int)Total2WDPerLandmark / (int)ProbesNumeric.Value;

                index++;
            }

            // Average Jitter value over number of landmarks
            if (MoreThanOneCampaign)
            {
                for (int i = 0; i < LandmarksNumber; i++)
                    TotalJitter += JitterPerLandmarkArray[i];

                MeanJitter = (int)(TotalJitter / LandmarksNumber); // This is the final-average-value of Jitter
            }
            else
            {
                MeanJitter = 0;
                JitterChart.Visible = false;
            }

            // Average OWD value over number of landmarks
            for (int i = 0; i < LandmarksNumber; i++)
                Total2WD += TwoWayDelayPerLandmarkArray[i];

            MeanTwowayDelay = (int)(Total2WD / LandmarksNumber); // This is the final-average-value of One-way Delay

            return new MeasurementVector(MeanTwowayDelay, MeanJitter);
        }

        // Bandwidth Measurement
        int[] UploadBWPerLandmarkArray;
        int[] DownloadBWPerLandmarkArray;
        Socket ClientSocket;
        byte[] FileContent;
        byte[] FileRec;
        int BWindex = 0;
        static int kilo = 1000;
        FileStream fsNew;
        public MeasurementVector SendFileToSetOfLandmarks()
        {
            var watch = Stopwatch.StartNew();
            int offset = 0;
            int BytesRec;
            UploadBWPerLandmarkArray = new int[LandmarksNumber];
            DownloadBWPerLandmarkArray = new int[LandmarksNumber];
            BWindex = 0;
            // Measure Uploading and Downloading Bandwidth per each landmark
            foreach (string Landmark in LandmarksDelayRchTxt.Lines)
            {
                // Creating the socket and the target IPEndPoint object
                try
                {
                    ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                }
                catch
                {
                    MessageBox.Show("A problem occurred when creating the socket!\nPlease, check your account administrative settings.", "Error");
                    return null;
                }
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(Landmark), 51254);
                // Connecting to the Landmark
                try
                {
                    ClientSocket.Connect((EndPoint)iep);
                }
                catch
                {
                    MessageBox.Show("Landmark: " + Landmark + " is not available.", "Error");
                    return null;
                }

                // Calculating Uploading Bandwidth
                // Sending the file
                byte[] confirmation;
                try
                {
                    confirmation = new byte[1];
                    if (ClientSocket.Receive(confirmation, 0, 1, SocketFlags.None) != 1 || Encoding.UTF8.GetString(confirmation, 0, 1) != "0")
                    {
                        System.Console.WriteLine("Error with start upload confirmation");
                    }
                    FileContent = File.ReadAllBytes(fsNew.Name);
                    System.Console.WriteLine("Uploading File...");

                    watch.Restart(); // Start a timer before sending the file
                    ClientSocket.Send(BitConverter.GetBytes(FileContent.Length)); // Send file size
                    ClientSocket.Send(BitConverter.GetBytes(0)); // End of send : send(0)
                    int x = 0;
                    confirmation = new byte[1];
                    x = ClientSocket.Send(FileContent, 0, FileContent.Length, SocketFlags.None);
                    if (ClientSocket.Receive(confirmation, 0, 1, SocketFlags.None) != 1 || Encoding.UTF8.GetString(confirmation, 0, 1) != "0")
                    {
                        System.Console.WriteLine("Error with upload confirmation");
                    }
                    watch.Stop(); // Stop the timer after sending the file
                    System.Console.WriteLine("Uploaded {0} bytes in {1} ms", x, watch.ElapsedMilliseconds);
                }
                catch
                {
                    MessageBox.Show("Problem when sending the file to the landmark: " + Landmark + "! Please, try again later.", "Error");
                    return null;
                }
                // Upload bandwidth is...
                float kbpsUP = ((float)FileContent.Length) * 8 / kilo * 1000 / watch.ElapsedMilliseconds;// TODO add parenthesis to make the formula clear
                System.Console.WriteLine("Calculated Upload Bandwidth {0} Kbps", kbpsUP);
                UploadBWPerLandmarkArray[BWindex] = Convert.ToInt32(kbpsUP); // Size of file in Kbits / Time in Sec = Kbps

                // Calculate Downloading Bandwidth
                // Receiving the file
                try
                {
                    offset = 0;
                    FileRec = new byte[FileContent.Length];
                    System.Console.WriteLine("Downloading file...");
                    watch.Restart(); // Start a timer before receiving the file
                    while (offset < FileContent.Length)
                    {
                        BytesRec = ClientSocket.Receive(FileRec);
                        offset += BytesRec;
                    }
                    watch.Stop(); // Stop the timer after receiving the file
                    System.Console.WriteLine("Downloaded {0} bytes in {1} ms", offset, watch.ElapsedMilliseconds);
                }
                catch
                {
                    MessageBox.Show("Error occurred when receiving the file from the landmark: " + Landmark + " !", "Error");
                    return null;
                }
                // Download bandwidth is...
                float kbpsDWN = ((float)FileRec.Length) * 8 / kilo * 1000 / watch.ElapsedMilliseconds;
                System.Console.WriteLine("Calculated Download Bandiwidth {0} Kbps", kbpsDWN);
                DownloadBWPerLandmarkArray[BWindex] = Convert.ToInt32(kbpsDWN); // Size of file in Kbits / Time in Sec = Kbps

                BWindex++;
                ClientSocket.Close();
            }

            // Averaging over number of landmarks
            int TotalUploadBandwidth = 0;
            int TotalDownloadBandwidth = 0;
            for (int i = 0; i < LandmarksNumber; i++)
            {
                TotalDownloadBandwidth += DownloadBWPerLandmarkArray[i];
                TotalUploadBandwidth += UploadBWPerLandmarkArray[i];
            }
            // This vector represents the final-average-value of both upload and download bandwidth
            return new MeasurementVector((int)(TotalUploadBandwidth / LandmarksNumber), (int)(TotalDownloadBandwidth / LandmarksNumber));
        }

        // Browse File to Use for Bandwidth Measurement
        FileStream fs;
        int _MB = 1 * kilo * kilo;
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                if (fs.Length > _MB) // File size needed to be greater then 10MB
                {
                    BWFileTxtBox.Text = fs.Name;
                    fsNew = fs;
                    fs.Close();
                }
                else
                {
                    MessageBox.Show("Please choose a file with a size greater than 2MB !", "Error");
                    fs = null;
                    return;
                }
            }
            else
            {
                MessageBox.Show("Error when opening the requested file !", "Error");
                return;
            }
        }

        // Choose Decision Tree (Configuration File)
        Node root;
        FileStream DecisionTreeFile;
        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DecisionTreeFile = new FileStream(openFileDialog1.FileName, FileMode.Open);
                DecisionTreeTxtBox.Text = DecisionTreeFile.Name;
                DecisionTreeFile.Close();
                root = CallerClass.Load(DecisionTreeFile.Name);
                if (root == null)
                {
                    MessageBox.Show("Error in Decision Tree Format. Please choose a valid XML format.", "Error");
                    DecisionTreeFile.Close();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Error when opening the Decision Tree file", "Error");
                return;
            }
        }

        private void SelectLandmarks_Click(object sender, EventArgs e)
        {
            try
            {
                LandNumber = Convert.ToInt32(LandmarksNo.Text);
            }
            catch
            {
                MessageBox.Show("Please enter the number of landmarks", "Error");
                return;
            }
            if (LandNumber < 1 || LandNumber > 100)
            {
                MessageBox.Show("Landmarks number must be a value between 1 and 100 !", "Error");
                return;
            }
            Thread t = new Thread(new ThreadStart(BeginLandmarksSelection));
            t.Start();
        }

        string[] AllLandmarks = new string[100];
        ArrayList ChosenLandmarks = new ArrayList();
        ArrayList LandmarksFirstSet = new ArrayList();
        ArrayList LandmarksSecondSet = new ArrayList();
        ArrayList LandmarksthirdSet = new ArrayList();
        int LandNumber;
        static Random r;
        private static byte[] data;
        public void BeginLandmarksSelection()
        {
            IPEndPoint iep;
            EndPoint ep;
            ICMP packet;
            Socket host;
            byte[] data = new byte[1024];
            int recv;
            var watch = Stopwatch.StartNew();
            long MS, RTD = 0;

            // initialize the AllLandmarks array
            InitializeLandmarksArray();

            string RandomLandmark = "";
            ChosenLandmarks.Clear();
            // Select landmarks randomly
            if (radioButtonRandom.Checked)
            {
                int i = 0;
                r = new Random();
                while (i < LandNumber)
                {
                    int size = getLandmarkNumber();
                    int randomNumber = r.Next(0, size - 1);
                    RandomLandmark = AllLandmarks[randomNumber];
                    if (!ChosenLandmarks.Contains(RandomLandmark))
                    {
                        ChosenLandmarks.Add(RandomLandmark);
                        i++;
                    }
                }
            }
            else if (radioButtonIntelligent.Checked) // Select landmarks intelligently according to RTD order
            {
                try
                {
                    host = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
                }
                catch
                {
                    MessageBox.Show("A problem occurred when creating the socket !\n Check your administrative settings.", "Error");
                    return;
                }

                for (int i = 0; i < AllLandmarks.Length; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        // Ping all landmarks and sort them in three lists according to RTD value (Each landmark 4 times)
                        try
                        {
                            iep = new IPEndPoint(IPAddress.Parse(AllLandmarks[i]), 0);
                            ep = (EndPoint)iep;
                            packet = new ICMP();
                            packet.Type = 0x08;
                            packet.Code = 0x00;
                            packet.Checksum = 0;
                            Buffer.BlockCopy(BitConverter.GetBytes((short)1), 0, packet.Message, 0, 2);
                            Buffer.BlockCopy(BitConverter.GetBytes((short)1), 0, packet.Message, 2, 2);
                            data = Encoding.ASCII.GetBytes("test packet");
                            Buffer.BlockCopy(data, 0, packet.Message, 4, data.Length);
                            packet.MessageSize = data.Length + 4;
                            int packetsize = packet.MessageSize + 4;

                            UInt16 chcksum = packet.getChecksum();
                            packet.Checksum = chcksum;

                            host.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 2000);
                            watch = Stopwatch.StartNew(); // Start a timer before ping
                            host.SendTo(packet.getBytes(), packetsize, SocketFlags.None, iep);
                            try
                            {
                                data = new byte[1024];
                                recv = host.ReceiveFrom(data, ref ep);
                                watch.Stop(); // Stop timer after ping
                            }
                            catch (SocketException)
                            {
                                //MessageBox.Show("No response from host: " + AllLandmarks[i] +
                                //                " Please check the availability of this host or your Internet connection!", "Error");
                                continue;
                            }
                            ICMP response = new ICMP(data, recv);
                        }
                        catch
                        {
                            MessageBox.Show("Problem in pinging host: " + AllLandmarks[i], "Error");
                            return;
                        }
                        MS = watch.ElapsedMilliseconds;
                        RTD += MS; // RTD : Two-way Delay
                    }

                    RTD = RTD / 4;

                    if (RTD < 80)
                        LandmarksFirstSet.Add(AllLandmarks[i]);
                    else if (RTD >= 80 && RTD < 200)
                        LandmarksSecondSet.Add(AllLandmarks[i]);
                    else if (RTD >= 200)
                        LandmarksthirdSet.Add(AllLandmarks[i]);
                }

                // Filling the chosen landmarks
                ChosenLandmarks.Clear();
                r = new Random();
                int k = 0;
                while (ChosenLandmarks.Count != LandNumber)
                {
                    k = 0;
                    while (k < Convert.ToInt32(LandNumber * LandmarksFirstSet.Count / 100))
                    {
                        if (ChosenLandmarks.Count == LandNumber)
                            break;
                        else
                            try
                            {
                                RandomLandmark = LandmarksFirstSet[r.Next(0, LandmarksFirstSet.Count)] as string;
                                if (!ChosenLandmarks.Contains(RandomLandmark))
                                {
                                    ChosenLandmarks.Add(RandomLandmark);
                                    k++;
                                }
                            }
                            catch
                            { continue; }
                    }
                    k = 0;
                    while (k < Convert.ToInt32(LandNumber * LandmarksSecondSet.Count / 100))
                    {
                        if (ChosenLandmarks.Count == LandNumber)
                            break;
                        else
                            try
                            {
                                RandomLandmark = LandmarksSecondSet[r.Next(0, LandmarksSecondSet.Count)] as string;
                                if (!ChosenLandmarks.Contains(RandomLandmark))
                                {
                                    ChosenLandmarks.Add(RandomLandmark);
                                    k++;
                                }
                            }
                            catch
                            { continue; }
                    }
                    k = 0;
                    while (k < Convert.ToInt32(LandNumber * LandmarksthirdSet.Count / 100))
                    {
                        if (ChosenLandmarks.Count == LandNumber)
                            break;
                        else
                            try
                            {
                                RandomLandmark = LandmarksthirdSet[r.Next(0, LandmarksthirdSet.Count)] as string;
                                if (!ChosenLandmarks.Contains(RandomLandmark))
                                {
                                    ChosenLandmarks.Add(RandomLandmark);
                                    k++;
                                }
                            }
                            catch
                            { continue; }
                    }
                }
            }

            LandmarksDelayRchTxt.Clear();
            // Fill the Landmarks RichTextBox
            for (int i = 0; i < ChosenLandmarks.Count; i++)
            {
                if (i != ChosenLandmarks.Count - 1)
                {
                    LandmarksDelayRchTxt.Text += ChosenLandmarks[i] + "\n";
                }
                else
                {
                    LandmarksDelayRchTxt.Text += ChosenLandmarks[i];
                }
            }
        }

        UdpClient udpOut;
        UdpClient udpIn;
        IPEndPoint RemoteIpEndPoint;
        private void setUpMeasurement(string host, int udpPort, int tcpPort)
        {
            udpOut = new UdpClient();
            udpOut.Connect(host, udpPort);
            udpIn = new UdpClient(udpPort);
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }

        Byte[] udpPacketBytes; 
        Byte[] serverMeasurements = new Byte[100];
        int udpPacketSize = 10;
        private int[] getMeasurements(string host, int probing_duration, int number_probes, int udpPort, int tcpPort)
        {
            int[] ret = new int[8];

            float[] lossRate = new float[2];
            float[] bandwith = new float[2];
            float[] delay = new float[2];

            Console.WriteLine("Sending udp packet train...");
            for (int i = 0; i < number_probes; i++)
            {
                ushort id = (ushort)i;
                var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
                udpPacketBytes = new Byte[udpPacketSize];
                Buffer.BlockCopy(BitConverter.GetBytes(id), 0, udpPacketBytes, 0, 2);
                Buffer.BlockCopy(BitConverter.GetBytes(timeSpan.TotalMilliseconds), 0, udpPacketBytes, 2, 8);
                udpOut.Send(udpPacketBytes, udpPacketBytes.Length);
            }
            Console.WriteLine("Done.");

            Console.WriteLine("Connecting to server [tcp]...");
            Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            bool connected = false;
            while (!connected)
            {
                try
                {
                    ClientSocket.Connect(host, tcpPort);
                    connected = true;
                }
                catch
                {
                    connected = false;
                    Console.WriteLine("Error connecting to {0}:{1}", host, tcpPort);
                }
            }

            Console.WriteLine("Waiting for server measurements...");
            serverMeasurements = new Byte[4];
            int x = ClientSocket.Receive(serverMeasurements, 0, 4, SocketFlags.None);
            //TODO separar los valores del array de bytes
            //delay[0]=
            //bandwith[0]=
            //lossRate[0]=
            ClientSocket.Close();
            //Console.WriteLine("Server response delay:{0} upload-bandwith:{1} lossRate:{2}", delay[0], bandwith[1], lossRate[2]);

            Console.WriteLine("Waiting for udp packet train...");
            udpPacketBytes = new Byte[udpPacketSize];

            int seqId;
            long timestamp;

            udpIn.Client.ReceiveTimeout = 5000;

            int packetsReceived = 0;
            // Loss Rate in the download direction
            //receive all packets for download
            bool probing = true;
            while (probing)
            {
                try
                {
                    udpPacketBytes = udpIn.Receive(ref RemoteIpEndPoint);
                    //TODO parse [id][timestamp] values in udPakcetBytes

                    //TODO use timestamp to measure "delay"
                    packetsReceived++;
                }
                catch
                {
                    probing = false;
                }
            }
            //TODO use packet count to measure packet loss
            //TODO use initial and end time for "bandwith" NOTE: bandwith must consider headers of udp packets => ip_headers[20bytes] and udp_headers[8bytes]

            return ret;
        }

        private void InitializeLandmarksArray()
        {
            AllLandmarks[0] = "10.0.0.1";
        }

        private int getLandmarkNumber()
        {
            int count = 0;
            foreach (String landmark in AllLandmarks)
            {
                if (landmark != null)
                    count++;
            }
            return count;
        }

        //probing_rate [bytes/sec]
        //probing_duration [seconds]
        public static float[] PingHostLossProb(string host, int index, int upload_probing_rate, int download_probing_rate, int probing_duration)
        {
            float[] losses = new float[2];

            //client to send
            UdpClient udpClient = new UdpClient();
            udpClient.Connect(host, 9050);

            // Loss Rate in the upload direction
            // ******* Upstream *******
            UdpClient udpClient2 = new UdpClient(9050);
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            // Size of probes at the application level
            int probe_size = 10;//bytes
            int upload_pps = upload_probing_rate / probe_size;
            int download_pps = download_probing_rate / probe_size;
            int number_probes_up = upload_pps * probing_duration;
            int number_probes_down = download_pps * probing_duration;
            Console.WriteLine("Sending {0} udp packets to server at {1} pps ({2} Kbps)", number_probes_up, upload_pps, upload_probing_rate * 8 / 1000);
            //number_probes_up must be smaller than ushort.MaxValue (range is [0, 65535])
            /* TODO (not urgent) Here we should check whether with the desired sending rate (in bits/s) and probing duration
               the resulting number of probes is less than 65535. If it is, we should keep the sending rate (in bits/s)
               constant and reduce the duration of the experiment accordingly.
            */
            // Creating probes and sending them
            for (int i = 0; i < number_probes_up; i++)
            {
                ushort id = (ushort)i;
                var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
                Byte[] timestamp = BitConverter.GetBytes(timeSpan.TotalMilliseconds);
                Byte[] senddata = new Byte[10];
                Buffer.BlockCopy(BitConverter.GetBytes(id), 0, senddata, 0, 2);
                Buffer.BlockCopy(timestamp, 0, senddata, 2, 8);
                udpClient.Send(senddata, senddata.Length);
                Thread.Sleep(1000 / upload_pps);
            }
            Console.WriteLine("Done.");

            // = udpClient2.Receive(ref RemoteIpEndPoint);

            // Receiving details about the upstream experiment and sending info on the downstream one on a separate TCP socket
            Byte[] receivedBytes;
            Console.WriteLine("Connecting to server [tcp]");
            Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            bool connected = false;
            while (!connected)
            {
                try
                {
                    ClientSocket.Connect(host, 9051);
                    connected = true;
                }
                catch
                {
                    connected = false;
                    Console.WriteLine("Error connecting to {0}:{1}", host, 9051);
                }
            }
            Console.WriteLine("Waiting for count of udp packets received by the server");
            receivedBytes = new Byte[4];
            int x = ClientSocket.Receive(receivedBytes, 0, 4, SocketFlags.None);
            int server_received_probes = BitConverter.ToInt32(receivedBytes, 0);

            Console.WriteLine("Server received {0} packets", server_received_probes);
            float uploadLoss = 100f * (number_probes_up - server_received_probes) / number_probes_up;

            Console.WriteLine("Sending download test info to server number_probes:{0}", number_probes_down);
            //send number_probes_down, download_pps
            ClientSocket.Send(BitConverter.GetBytes(number_probes_down));
            ClientSocket.Send(BitConverter.GetBytes(1000 / download_pps));

            Console.WriteLine("Closing tcp connection");
            ClientSocket.Close();
            // ******* Downstream *******
            Console.WriteLine("Waiting udp packets from server");
            udpClient2.Client.ReceiveTimeout = 5000;

            int downloadReceived = 0;
            // Loss Rate in the download direction
            //receive all packets for download
            bool probing = true;
            while (probing)
            {
                try
                {
                    receivedBytes = udpClient2.Receive(ref RemoteIpEndPoint);
                    //handle info [2 bytes id][8 bytes timestamp]
                    string received1 = Encoding.ASCII.GetString(receivedBytes);
                    downloadReceived++;
                    //if (downloadReceived % 200 == 0)
                    //{
                    //    Console.Write("{0} ", downloadReceived / 200);
                    //}
                }
                catch
                {
                    probing = false;
                }
            }
            Console.WriteLine("Received {0} udp packets", downloadReceived);

            float downloadLoss = 100f * (number_probes_down - downloadReceived) / number_probes_down;

            Console.WriteLine("Upload Loss: " + uploadLoss + "%");
            Console.WriteLine("Download Loss: " + downloadLoss + "%");
            udpClient.Close();
            udpClient2.Close();
            losses[1] = uploadLoss;
            losses[0] = downloadLoss;
            return losses;
        }

        public static int PingHostDelay(string host)
        {
            int RTT = 0;
            IPAddress address = GetIpFromHost(ref host);
            PingOptions pingOptions = new PingOptions(128, true);
            Ping ping = new Ping();
            byte[] buffer = new byte[32];

            try
            {
                PingReply pingReply = ping.Send(address, 1000, buffer, pingOptions);
                if (!(pingReply == null))
                {
                    switch (pingReply.Status)
                    {
                        case IPStatus.Success:
                            RTT = (int)pingReply.RoundtripTime;
                            break;
                        default:
                            Console.WriteLine("Ping failed: {0}", pingReply.Status.ToString());
                            break;
                    }
                }
            }
            catch (PingException ex)
            {
                Console.WriteLine("Connection Error: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection Error: {0}", ex.Message);
            }

            return RTT;
        }
        private static IPAddress GetIpFromHost(ref string host)
        {
            //variable to hold our error message (if something fails)
            string errMessage = string.Empty;

            //IPAddress instance for holding the returned host
            IPAddress address = null;

            //wrap the attempt in a try..catch to capture
            //any exceptions that may occur
            try
            {
                //get the host IP from the name provided
                address = Dns.GetHostEntry(host).AddressList[0];
            }
            catch (SocketException ex)
            {
                //some DNS error happened, return the message
                errMessage = string.Format("DNS Error: {0}", ex.Message);
            }
            return address;
        }
        // Close Application Button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /*
        public System.Windows.Forms.DataVisualization.Charting.DataPointCollection AddPointToFixedSizeCollection(double x_val, double y_val, System.Windows.Forms.DataVisualization.Charting.DataPointCollection aCollection, int max_size)
        {
            if (aCollection.Count > max_size)
            {
                // remove oldest point and shift the x value of all remaining points to the left
                aCollection.RemoveAt(0);
                for (int i = 0; i < aCollection.Count; i++)
                {
                    double point_x = aCollection[i].XValue - 1.0;
                    double[] point_y = aCollection[i].YValues;
                    aCollection[i].SetValueXY(point_x, point_y);
                }
            }
            aCollection.AddXY(x_val, y_val);
            return aCollection;
        }
        */
        /*
        private double[] keepFixedSizeList(double[] aList, int maxSize)
        {
            if (aList.Length > maxSize)
            {
                for (int i = 1; i < aList.Length; i++)
                {
                    aPoint = aList[i];
                    aList[i - 1] = aPoint;
                }
            }

            return aList;
        }
        */

        private int estimatedSkypeQuality(int downloadBandwidth, int uploadBandwidth, int downloadDelay, int uploadDelay, int downloadLoss, int uploadLoss)
        {
            int estimation = 2;

            if (downloadBandwidth > 1078 && downloadDelay <= 94) estimation = 3;

            if (uploadBandwidth > 1903 && downloadBandwidth > 1078) estimation = 3;

            if (downloadBandwidth <= 1078 && downloadDelay <= 665 && uploadLoss > 0 && uploadLoss <= 2 && downloadLoss > 0 && downloadLoss <= 2) estimation = 3;

            if (downloadBandwidth <= 12) estimation = 0;

            if (uploadBandwidth <= 14 && uploadDelay <= 854) estimation = 0;

            if (downloadBandwidth <= 723 && uploadDelay <= 400 && downloadDelay > 481 && uploadLoss > 36 && uploadLoss <= 46 && downloadLoss > 22 && downloadLoss <= 45) estimation = 0;

            if (uploadBandwidth <= 18 && downloadDelay > 417 && downloadDelay <= 665) estimation = 0;

            if (uploadDelay > 464 && uploadDelay <= 615 && uploadLoss <= 27 && downloadLoss > 43) estimation = 0;

            if (uploadLoss > 27 && uploadLoss <= 46 && downloadLoss > 45) estimation = 0;

            if (downloadBandwidth <= 848 && uploadDelay > 479 && uploadLoss > 47) estimation = 0;

            if (uploadLoss <= 27 && downloadLoss > 48) estimation = 0;

            if (uploadDelay <= 479 && uploadLoss > 46) estimation = 1;

            if (uploadBandwidth > 14 && uploadBandwidth <= 17) estimation = 1;

            if (downloadDelay > 992) estimation = 1;

            if (uploadBandwidth > 17 && downloadBandwidth > 12 && uploadDelay > 400 && uploadLoss > 27 && downloadLoss <= 44) estimation = 1;

            if (uploadBandwidth > 17 && uploadBandwidth <= 1903 && downloadBandwidth > 12 && downloadDelay > 94 && uploadLoss <= 27 && downloadLoss <= 9) estimation = 2;

            if (uploadDelay <= 400 && uploadLoss > 27 && uploadLoss <= 46 && downloadLoss <= 22) estimation = 2;

            if (uploadBandwidth > 30 && downloadBandwidth > 12 && uploadLoss <= 27 && downloadLoss > 9 && downloadLoss <= 43) estimation = 2;

            if (uploadBandwidth > 17 && downloadBandwidth > 12 && uploadDelay > 400 && uploadLoss <= 28 && downloadLoss <= 35) estimation = 2;

            return estimation;
        }
    }
}