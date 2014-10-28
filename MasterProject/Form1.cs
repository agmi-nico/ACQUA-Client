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
using System.Net.Sockets;
using System.Net;

namespace MasterProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        int LandmarksNumber = 0, index;
        MeasurementVector[] FinalDelayAndJitterArray;
        MeasurementVector[] FinalBandwidthArray;
        Time[] TimeArray;

        private void button1_Click(object sender, EventArgs e)
        {
            int[] series=new int[2];
            series[1] = 2;
            series[0] = 1;
           
            FinalDelayAndJitterArray = new MeasurementVector[1000];
            FinalBandwidthArray = new MeasurementVector[1000];
            TimeArray = new Time[1000];
            LandmarksNumber = 0;

            if (LandmarksDelayRchTxt.Text == "")
            {
                LandNumber = 1;
                BeginLandmarksSelection();
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

            Thread t = new Thread(new ThreadStart(Run));
            System.Console.WriteLine("Starting...");
            t.Start();
        }

        int MeanResult = -1;
        int[] QoEPerLanmark;
        int[] skypeQoEPerLandmark;
        int skypeQoEMeanResult;
        void Run()
        {
            QoEPerLanmark = new int[LandmarksNumber];
            skypeQoEPerLandmark=new int[LandmarksNumber];
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
                int[] LossRate=new int[2];
                LossRate = PingHostLossProb(AllLandmarks[LocalIndex], 100, LocalIndex);

                //// Make QoE Estimation
                //// For the Set of Landmarks First
                int[] LossRatePerLandmark = new int[2];
                for (int i = 0; i < LandmarksNumber; i++)
                {
                    LossRatePerLandmark = PingHostLossProb(AllLandmarks[i], 100,i);
                    QoEPerLanmark[i] = CallerClass.Call(root,                             // Decision-tree Root
                                                        TwoWayDelayPerLandmarkArray[i],   // Delay
                                                        JitterPerLandmarkArray[i],        // Jitter
                                                        UploadBWPerLandmarkArray[i],      // UBandwidth
                                                        DownloadBWPerLandmarkArray[i],    // DBandwidth
                                                        LossRatePerLandmark[0],           // DLossRate
                                                        LossRatePerLandmark[1]);          // ULossRate 
                    skypeQoEPerLandmark[i] = estimatedSkypeQuality(DownloadBWPerLandmarkArray[i], 
                                                                    UploadBWPerLandmarkArray[i], 
                                                                    (int)TwoWayDelayPerLandmarkArray[i] / 2, 
                                                                    (int)TwoWayDelayPerLandmarkArray[i] / 2, 
                                                                    LossRatePerLandmark[0], 
                                                                    LossRatePerLandmark[1]);
                }
                System.Console.WriteLine("Mean QoE");
                System.Console.WriteLine("Mean QoE Delay:{0} Jitter:{1} UBW:{2} DBW:{3} DLR:{4} ULR:{5}",
                                            FinalDelayAndJitterArray[LocalIndex].dimension1,
                                            FinalDelayAndJitterArray[LocalIndex].dimension2,
                                            FinalBandwidthArray[LocalIndex].dimension1,
                                            FinalBandwidthArray[LocalIndex].dimension2,
                                            LossRate[0],
                                            LossRate[1]);
                //// Then for the Mean Values of Measurements

                MeanResult = CallerClass.Call(root,                                            // Decision-tree Root
                                              FinalDelayAndJitterArray[LocalIndex].dimension1, // Delay
                                              FinalDelayAndJitterArray[LocalIndex].dimension2, // Jitter
                                              FinalBandwidthArray[LocalIndex].dimension1,      // UBandwidth
                                              FinalBandwidthArray[LocalIndex].dimension2,      // DBandwidth
                                              LossRate[0],                                     // DLossRate
                                              LossRate[1]);                                    // ULossRate
                skypeQoEMeanResult = estimatedSkypeQuality(FinalBandwidthArray[LocalIndex].dimension2, FinalBandwidthArray[LocalIndex].dimension1, (int)FinalDelayAndJitterArray[LocalIndex].dimension1 / 2, (int)FinalDelayAndJitterArray[LocalIndex].dimension1 / 2, LossRate[0], LossRate[1]);
                // Get Full Time
                TimeArray[LocalIndex] = new Time();

                // Draw the Charts
                this.DelayChart.Series["Round-trip Delay"].Points.AddXY(TimeArray[LocalIndex].ToString(), FinalDelayAndJitterArray[LocalIndex].dimension1);
                this.JitterChart.Series["Jitter"].Points.AddXY(TimeArray[LocalIndex].ToString(), FinalDelayAndJitterArray[LocalIndex].dimension2);
                this.UploadBandwidthChart.Series["Upload Bandwidth"].Points.AddXY(TimeArray[LocalIndex].ToString(), FinalBandwidthArray[LocalIndex].dimension1);
                this.DownloadBandwidthChart.Series["Download Bandwidth"].Points.AddXY(TimeArray[LocalIndex].ToString(), FinalBandwidthArray[LocalIndex].dimension2);

                for (int i = 0; i < LandmarksNumber; i++)
                {
                    this.QoEChart.Series["QoE"].Points.AddXY("Landmark " + i + 1, skypeQoEPerLandmark[i]);
                }
                this.MeanQoEChart.Series["Mean QoE"].Points.AddXY(TimeArray[LocalIndex].ToString(), skypeQoEMeanResult);

                Console.WriteLine("Finished with loss: " + LossRate[0] + ", " + LossRate[1] + "\nbandwidth: " + FinalBandwidthArray[LocalIndex].dimension1 + ", " + FinalBandwidthArray[LocalIndex].dimension2 + "\ndelay: " + TwoWayDelayArray[LocalIndex] + ", " + TwoWayDelayArray[LocalIndex]);
                
                Thread.Sleep((int)PeriodNumeric.Value * 60 * 1000);
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
                float kbpsUP = ((float)FileContent.Length) * 8 / kilo * 1000 / watch.ElapsedMilliseconds;
                System.Console.WriteLine("Calculated Upload Bandiwidth {0} Kbps", kbpsUP);
                UploadBWPerLandmarkArray[BWindex] = Convert.ToInt32(kbpsUP); // Size of file in Kbits / Time in Sec = Kbps
                
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
                    MessageBox.Show("Plese choose a file with a size greater than 2MB !", "Error");
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
                    int randomNumber = r.Next(0, size-1);
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
            AllLandmarks[0] = "10.0.0.1";
        }

        private int getLandmarkNumber()
        {
            int count = 0;
            foreach(String landmark in AllLandmarks){
                if (landmark != null)
                    count++;
            }
            return count;
        }
        //ME
        public static int[] PingHostLossProb(string host, int times,int index)
        {
            int[] losses = new int[2];

            UdpClient udpClient = new UdpClient();
            udpClient.Connect(host, 9050);
            
            Console.WriteLine("Sending udp packets to server");
            for (int i = 0; i < 100; i++)
            {
                Byte[] senddata = Encoding.ASCII.GetBytes(i.ToString());
                udpClient.Send(senddata, senddata.Length);
            }
            Console.WriteLine("Done.");
            
            Console.WriteLine("Receiving udp packets from server");
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            UdpClient udpClient2 = new UdpClient(9050);
            Byte[] receiveBytes = udpClient2.Receive(ref RemoteIpEndPoint);

            string received = Encoding.ASCII.GetString(receiveBytes);


            int uploadLoss = 100 - int.Parse(received);
            Console.WriteLine("Upload Loss= " + uploadLoss + "%");
            Thread.Sleep(5000);

            int downloadReceived = 0;
            //receive all packets for download
            while (true)
            {
                try
                {
                    Byte[] receiveBytes1 = udpClient2.Receive(ref RemoteIpEndPoint);
                    string received1 = Encoding.ASCII.GetString(receiveBytes1);
                    udpClient2.Client.ReceiveTimeout = 5000;
                    if (received1 == "secret")
                        downloadReceived++;
                    Console.WriteLine("RECEIVED packet " + downloadReceived);
                }
                catch
                {
                    break;
                }
            }
            int downloadLoss = 100 - downloadReceived++;
            Console.WriteLine("Upload Loss= " + uploadLoss + "%");
            Console.WriteLine("Download Loss= " + downloadLoss + "%");
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

        private int estimatedSkypeQuality(int downloadBandwidth, int uploadBandwidth, int downloadDelay, int uploadDelay, int downloadLoss, int uploadLoss)
        {
            int estimation=2;

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