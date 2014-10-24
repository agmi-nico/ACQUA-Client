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

namespace MasterProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        // Close Application Button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        int LandmarksNumber = 0, index;
        MeasurementVector[] FinalDelayAndJitterArray;
        MeasurementVector[] FinalBandwidthArray;
        Time[] TimeArray;
        static double CodecRate = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            FinalDelayAndJitterArray = new MeasurementVector[1000];
            FinalBandwidthArray = new MeasurementVector[1000];
            TimeArray = new Time[1000];
            LandmarksNumber = 0;

            if (LandmarksDelayRchTxt.Text == "")
            {
                MessageBox.Show("Choose landmarks, please !", "Error");
                return;
            }
            if (BWFileTxtBox.Text == "")
            {
                MessageBox.Show("Enter file path you want to make your test with, please !", "Error");
                return;
            }
            if (DecisionTreeTxtBox.Text == "")
            {
                MessageBox.Show("No decision tree is chosen, please choose one !", "Error");
                return;
            }

            if (ProbesNumeric.Value == 1)
            {
                DialogResult dr = MessageBox.Show("Jitter cannot be calculated! Only one probe is sent. Do you want to continue?", "Alert", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                    return;
            }

            // Count the number of landmarks in the richTextBox
            foreach (string landmark in LandmarksDelayRchTxt.Lines)
                LandmarksNumber++;

            // Read Codec Rate value from a file
            try
            {
                string text = "120";//File.ReadAllText("C:\\ApplicationProfile\\CodecRate.txt");
                CodecRate = Convert.ToDouble(text);
            }
            catch
            {
                MessageBox.Show("Cannot find Application Configuration file ! Codec rate value is missing.", "Error");
                return;
            }
            System.Console.WriteLine("Starting...");
            Thread t = new Thread(new ThreadStart(Run));
            t.Start();
        }

        int MeanResult = -1;
        int[] QoEPerLanmark;
        void Run()
        {
            QoEPerLanmark = new int[LandmarksNumber];
            int LocalIndex = 0;
            while (true)
            {
                // Get Two-way Delay and Jitter Values 
                FinalDelayAndJitterArray[LocalIndex] = PingSetOfLandmarks();
                if (FinalDelayAndJitterArray[LocalIndex] == null)
                    return;

                // Get Bandwidth (Uploading + Downnloading) Values
                FinalBandwidthArray[LocalIndex] = SendFileToSetOfLandmarks();
                if (FinalBandwidthArray[LocalIndex] == null)
                    return;

                // Calculate Loss Rate LR = max(CR - AB, 0) / AB
                int LossRate;
                try
                {
                    LossRate = (int)(Math.Max((CodecRate - FinalBandwidthArray[LocalIndex].dimension1), 0) /
                                                   FinalBandwidthArray[LocalIndex].dimension1) * 100;
                    if (LossRate > 100) LossRate = 100;
                }
                catch
                {
                    MessageBox.Show("Problem in getting the Codec Rate value, please check file settings !", "Error");
                    return;
                }

                //// Make QoE Estimation
                //// For the Set of Landmarks First
                int LossRatePerLandmark = 0;
                for (int i = 0; i < LandmarksNumber; i++)
                {
                    LossRatePerLandmark = (int)(Math.Max((CodecRate - UploadBWPerLandmarkArray[i]), 0) / UploadBWPerLandmarkArray[i]) * 100;
                    if (LossRatePerLandmark > 100) LossRatePerLandmark = 100;
                    System.Console.WriteLine("QoEPerLanmark {0}", i);
                    QoEPerLanmark[i] = CallerClass.Call(root,                             // Decision-tree Root
                                                        TwoWayDelayPerLandmarkArray[i],   // Delay
                                                        JitterPerLandmarkArray[i],        // Jitter
                                                        UploadBWPerLandmarkArray[i],      // UBandwidth
                                                        DownloadBWPerLandmarkArray[i],    // DBandwidth
                                                        LossRatePerLandmark);             // LossRate
                }
                //// Then for the Mean Values of Measurements
                System.Console.WriteLine("Mean QoE");
                MeanResult = CallerClass.Call(root,                                            // Decision-tree Root
                                              FinalDelayAndJitterArray[LocalIndex].dimension1, // Delay
                                              FinalDelayAndJitterArray[LocalIndex].dimension2, // Jitter
                                              FinalBandwidthArray[LocalIndex].dimension1,      // UBandwidth
                                              FinalBandwidthArray[LocalIndex].dimension2,      // DBandwidth
                                              LossRate);                                       // LossRate

                // Get Full Time
                TimeArray[LocalIndex] = new Time();

                // Draw the Charts
                this.DelayChart.Series["Round-trip Delay"].Points.AddXY(TimeArray[LocalIndex].ToString(), FinalDelayAndJitterArray[LocalIndex].dimension1);
                this.JitterChart.Series["Jitter"].Points.AddXY(TimeArray[LocalIndex].ToString(), FinalDelayAndJitterArray[LocalIndex].dimension2);
                this.UploadBandwidthChart.Series["Upload Bandwidth"].Points.AddXY(TimeArray[LocalIndex].ToString(), FinalBandwidthArray[LocalIndex].dimension1);
                this.DownloadBandwidthChart.Series["Download Bandwidth"].Points.AddXY(TimeArray[LocalIndex].ToString(), FinalBandwidthArray[LocalIndex].dimension2);
                for (int i = 0; i < LandmarksNumber; i++)
                {
                    this.QoEChart.Series["QoE"].Points.AddXY("Landmark " + i + 1, QoEPerLanmark[i]);
                }
                this.MeanQoEChart.Series["Mean QoE"].Points.AddXY(TimeArray[LocalIndex].ToString(), MeanResult);
                LocalIndex++;
                Thread.Sleep((int)PeriodNumeric.Value * 60 * 1000);
                //Thread.Sleep(50000);
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
            byte[] data = new byte[1024];
            int recv;
            Socket host;
            var watch = Stopwatch.StartNew();

            if (ProbesNumeric.Value == 1)
                MoreThanOneCampaign = false;

            try
            {
                host = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
            }
            catch(SocketException e)
            {
                MessageBox.Show("Some problem occured during building the socket !\nPlease, check your account administrative settings.", "Error");
                Console.WriteLine(e.Message);
                return null;
            }
            IPEndPoint iep;
            EndPoint ep;
            ICMP packet;

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
                    // Ping (for one-time) Implementation
                    try
                    {
                        iep = new IPEndPoint(IPAddress.Parse(Landmark), 0);
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

                        host.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000);
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
                            MessageBox.Show("No response from host: " + Landmark +
                                            " Please check the availability of this host or your Internet connection!", "Error");
                            return null;
                        }
                        ICMP response = new ICMP(data, recv);
                    }
                    catch
                    {
                        MessageBox.Show("Problem in pinging host: " + Landmark, "Error");
                        return null;
                    }

                    elapsedMs = watch.ElapsedMilliseconds;
                    _2WD = elapsedMs; // _2WD : Two-way Delay
                    Total2WDPerLandmark += _2WD;
                    TwoWayDelayArray[i] = _2WD;
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

            host.Close();
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
                    ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
                catch
                {
                    MessageBox.Show("Some problem occured during building the socket !\nPlease, check your account administrative settings.", "Error");
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
                    MessageBox.Show("Landmark: " + Landmark + " is not available! Please, try agian later.", "Error");
                    return null;
                }

                // Calculating Uploading Bandwidth
                // Sending the file
                try
                {
                    FileContent = File.ReadAllBytes(fsNew.Name);
                    System.Console.WriteLine("Uploading File...");
                    watch.Restart(); // Start a timer before sending the file
                    ClientSocket.Send(BitConverter.GetBytes(FileContent.Length)); // Send file size
                    ClientSocket.Send(BitConverter.GetBytes(0)); // End of send : send(0)
                    int x = 0;
                    x = ClientSocket.Send(FileContent, 0, FileContent.Length, SocketFlags.None);
                    byte[] confirmation = new byte[1];
                    if (ClientSocket.Receive(confirmation, 0, 1, SocketFlags.None) != 1 || Encoding.UTF8.GetString(confirmation, 0, 1) != "0")
                    {
                        System.Console.WriteLine("Error with upload confirmation");
                    }
                    watch.Stop(); // Stop the timer after sending the file
                    System.Console.WriteLine("Uploaded {0} bytes in {1} miliseconds", x, watch.ElapsedMilliseconds);
                }
                catch
                {
                    MessageBox.Show("Problem in sending the file to the landmark: " + Landmark + " ! Please, try agian later.", "Error");
                    return null;
                }
                // Uploading Bandwidth is
                Double mbpsUP = ((double)FileContent.Length) * 8 / kilo / kilo * 1000 / watch.ElapsedMilliseconds;
                System.Console.WriteLine("Calculated Upload Bandiwidth {0} Mbps", mbpsUP);
                UploadBWPerLandmarkArray[BWindex] = Convert.ToInt32(mbpsUP); // Size of file in Mbits / Time in Sec = Mbps

                // Calculate Downloading Bandwidth
                // Receiving the file
                try
                {
                    offset = 0;
                    FileRec = new byte[FileContent.Length];
                    System.Console.WriteLine("Downloading File...");
                    watch.Restart(); // Start a timer before recieving the file
                    while (offset < FileContent.Length)
                    {
                        BytesRec = ClientSocket.Receive(FileRec);
                        offset += BytesRec;
                    }
                    watch.Stop(); // Stop the timer after receiving the file
                    System.Console.WriteLine("Downloaded {0} bytes in {1} miliseconds", offset, watch.ElapsedMilliseconds);
                }
                catch
                {
                    MessageBox.Show("Error occurred when receiving the file from the landmark: " + Landmark + " !", "Error");
                    return null;
                }
                // Downloading Bandwidth is
                Double mbpsDWN = ((double)FileRec.Length) * 8 / kilo / kilo * 1000 / watch.ElapsedMilliseconds;
                System.Console.WriteLine("Calculated Download Bandiwidth {0} Mbps", mbpsDWN);
                DownloadBWPerLandmarkArray[BWindex] = Convert.ToInt32(mbpsDWN); // Size of file in Mbits / Time in Sec = Mbps
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
        int _10MB = 10 * kilo;
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                if (fs.Length > _10MB) // File size needed to be greater then 10MB
                {
                    BWFileTxtBox.Text = fs.Name;
                    fsNew = fs;
                    fs.Close();
                }
                else
                {
                    MessageBox.Show("Plese choose a file with a size greater than 10MB !", "Error");
                    fs = null;
                    return;
                }
            }
            else
            {
                MessageBox.Show("Error in opening the requested file !", "Error");
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
                    MessageBox.Show("Error in Decision Tree File Format ! Please choose a valid XML file format.", "Error");
                    DecisionTreeFile.Close();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Error in opening the requested Decision Tree file !", "Error");
                return;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        { }

        private void SelectLandmarks_Click(object sender, EventArgs e)
        {
            try
            {
                LandNumber = Convert.ToInt32(LandmarksNo.Text);
            }
            catch
            {
                MessageBox.Show("Please enter the number of landmarks !", "Error");
                return;
            }
            if (LandNumber < 1 || LandNumber >100)
            {
                MessageBox.Show("Landmarks number shall be a value between 1 and 100 !","Error");
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
                    RandomLandmark = AllLandmarks[r.Next(0, 99)];
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
                    MessageBox.Show("Some problem occured during building the socket !\nPlease, check your account administrative settings.", "Error");
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
                    k=0;
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

        private void InitializeLandmarksArray()
        {
            AllLandmarks[0] = "134.76.249.230";
            AllLandmarks[1] = "161.106.240.18";
            AllLandmarks[2] = "193.10.64.35";
            AllLandmarks[3] = "130.192.157.138";
            AllLandmarks[4] = "156.17.10.52";
            AllLandmarks[5] = "192.33.210.16";
            AllLandmarks[6] = "131.188.44.100";
            AllLandmarks[7] = "194.167.254.18";
            AllLandmarks[8] = "158.110.27.116";
            AllLandmarks[9] = "195.148.124.73";
            AllLandmarks[10] = "150.244.58.161";
            AllLandmarks[11] = "193.55.112.41";
            AllLandmarks[12] = "193.136.124.228";
            AllLandmarks[13] = "193.136.166.56";
            AllLandmarks[14] = "212.235.189.115";
            AllLandmarks[15] = "192.42.43.22";
            AllLandmarks[16] = "130.149.49.136";
            AllLandmarks[17] = "194.42.17.121";
            AllLandmarks[18] = "193.138.2.12";
            AllLandmarks[19] = "169.229.50.4";
            AllLandmarks[20] = "193.190.168.49";
            AllLandmarks[21] = "138.246.99.250";
            AllLandmarks[22] = "145.99.179.147";
            AllLandmarks[23] = "212.51.218.237";
            AllLandmarks[24] = "132.65.240.101";
            AllLandmarks[25] = "193.144.21.130";
            AllLandmarks[26] = "138.48.3.201";
            AllLandmarks[27] = "147.102.3.113";
            AllLandmarks[28] = "195.148.124.74";
            AllLandmarks[29] = "157.181.175.248";
            AllLandmarks[30] = "155.185.54.249";
            AllLandmarks[31] = "130.237.50.124";
            AllLandmarks[32] = "132.227.62.121";
            AllLandmarks[33] = "131.254.208.11";
            AllLandmarks[34] = "130.192.157.131";
            AllLandmarks[35] = "130.92.70.254";
            AllLandmarks[36] = "212.199.61.205";
            AllLandmarks[37] = "158.110.27.127";
            AllLandmarks[38] = "128.232.103.202";
            AllLandmarks[39] = "193.167.187.186";
            AllLandmarks[40] = "212.235.189.114";
            AllLandmarks[41] = "131.130.69.164";
            AllLandmarks[42] = "194.42.17.123";
            AllLandmarks[43] = "83.230.127.122";
            AllLandmarks[44] = "145.99.179.146";
            AllLandmarks[45] = "139.91.90.239";
            AllLandmarks[46] = "130.37.193.143";
            AllLandmarks[47] = "130.73.142.87";
            AllLandmarks[48] = "149.156.5.116";
            AllLandmarks[49] = "193.1.170.135";
            AllLandmarks[50] = "143.215.131.206";
            AllLandmarks[51] = "138.48.3.203";
            AllLandmarks[52] = "130.237.50.125";
            AllLandmarks[53] = "147.83.29.234";
            AllLandmarks[54] = "193.136.166.54";
            AllLandmarks[55] = "85.23.168.77";
            AllLandmarks[56] = "195.130.121.204";
            AllLandmarks[57] = "130.104.72.201";
            AllLandmarks[58] = "141.76.45.18";
            AllLandmarks[59] = "195.113.161.13";
            AllLandmarks[60] = "221.199.217.144";
            AllLandmarks[61] = "213.73.40.105";
            AllLandmarks[62] = "193.167.187.185";
            AllLandmarks[63] = "134.151.255.181";
            AllLandmarks[64] = "157.181.175.249";
            AllLandmarks[65] = "130.206.158.140";
            AllLandmarks[66] = "130.83.166.243";
            AllLandmarks[67] = "130.79.48.57";
            AllLandmarks[68] = "193.145.46.242";
            AllLandmarks[69] = "195.130.124.1";
            AllLandmarks[70] = "85.23.168.76";
            AllLandmarks[71] = "194.167.254.19";
            AllLandmarks[72] = "192.41.135.219";
            AllLandmarks[73] = "129.88.70.227";
            AllLandmarks[74] = "194.47.148.170";
            AllLandmarks[75] = "192.38.109.143";
            AllLandmarks[76] = "193.205.215.74";
            AllLandmarks[77] = "147.83.30.164";
            AllLandmarks[78] = "193.166.160.98";
            AllLandmarks[79] = "148.81.140.194";
            AllLandmarks[80] = "139.165.12.212";
            AllLandmarks[81] = "152.66.245.162";
            AllLandmarks[82] = "129.242.19.196";
            AllLandmarks[83] = "80.65.237.10";
            AllLandmarks[84] = "193.1.170.136";
            AllLandmarks[85] = "193.226.19.30";
            AllLandmarks[86] = "128.232.103.203";
            AllLandmarks[87] = "193.138.2.13";
            AllLandmarks[88] = "222.99.146.83";
            AllLandmarks[89] = "222.99.146.81";
            AllLandmarks[90] = "128.36.233.153";
            AllLandmarks[91] = "156.62.231.244";
            AllLandmarks[92] = "133.9.81.164";
            AllLandmarks[93] = "157.92.44.103";
            AllLandmarks[94] = "169.229.50.14";
            AllLandmarks[95] = "128.112.139.42";
            AllLandmarks[96] = "200.129.132.18";
            AllLandmarks[97] = "133.1.74.162";
            AllLandmarks[98] = "142.150.238.13";
            AllLandmarks[99] = "140.109.17.181";
        }
    }
}