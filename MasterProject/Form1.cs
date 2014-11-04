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
        int LandmarksNumber = 0;
        MeasurementVector[] FinalDelayAndJitterArray;
        MeasurementVector[] FinalBandwidthArray;
        MeasurementVector[] FinalLossRateArray;
        //Time[] TimeArray;

        private void button1_Click(object sender, EventArgs e)
        {
            int[] series = new int[2];
            series[1] = 2;
            series[0] = 1;

            FinalDelayAndJitterArray = new MeasurementVector[max_num_experiments];
            FinalBandwidthArray = new MeasurementVector[max_num_experiments];
            FinalLossRateArray = new MeasurementVector[max_num_experiments];
            //TimeArray = new Time[max_num_experiments];
            LandmarksNumber = 0;

            if (decisionTreeTxtBox.Text == "")
            {
                MessageBox.Show("Decision Tree missing", "Error");
                return;
            }

            if (probesNumber.Value == 1)
            {
                DialogResult dr = MessageBox.Show("Jitter can't be calculated with only one probe. Do you want to continue?", "Alert", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                    return;
            }

            // Count the number of landmarks in the richTextBox
            foreach (string landmark in landmarksListForm.Lines)
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
            LandmarksNumber = 1;
            QoEPerLandmark = new int[max_num_experiments, LandmarksNumber];
            skypeQoEPerLandmark = new int[max_num_experiments, LandmarksNumber];
            meanQoE = new int[max_num_experiments];
            skypeQoE = new int[max_num_experiments];
            FinalDelayAndJitterArray = new MeasurementVector[max_num_experiments];
            FinalBandwidthArray = new MeasurementVector[max_num_experiments];
            int LocalIndex = 0;

            setUpMeasurement("10.0.0.1", 9050, 9051, 100);
            Stopwatch totalTime = new Stopwatch();

            while (true)
            {
                totalTime.Restart();
                int[] measurements = getMeasurements();

                System.Console.WriteLine("\tCalculating QoE with values:");
                System.Console.WriteLine("\tDelay-U:   \t{0}ms    \tDelay-D:   \t{1}ms    \tSum:{2}", measurements[0], measurements[1], measurements[1] + measurements[0]);
                System.Console.WriteLine("\tBandwith-U:\t{0}Kbit/s\tBandwith-D:\t{1}Kbit/s", measurements[2], measurements[3]);
                System.Console.WriteLine("\tLossRate-U:\t{0}%     \tLossRate-D:\t{1}%     ", measurements[4], measurements[5]);

                // Estimate QoE with the provided tree
                QoEPerLandmark[LocalIndex, 0] = CallerClass.Call(root,           // Decision-tree Root
                                                                measurements[0], // Delay
                                                                measurements[1], // Jitter
                                                                measurements[2], // UBandwidth
                                                                measurements[3], // DBandwidth
                                                                measurements[4], // ULossRate
                                                                measurements[5]);// DLossRate 
                // just to compare the above result with the hard-coded tree in the bottom of this file
                skypeQoEPerLandmark[LocalIndex, 0] = estimatedSkypeQuality(measurements[3],
                                                                measurements[2],
                                                                measurements[0],
                                                                measurements[0],
                                                                measurements[4],
                                                                measurements[5]);

                FinalDelayAndJitterArray[LocalIndex] = new MeasurementVector(measurements[0], measurements[1]);
                FinalBandwidthArray[LocalIndex] = new MeasurementVector(measurements[2], measurements[3]);
                FinalLossRateArray[LocalIndex] = new MeasurementVector(measurements[4], measurements[5]);

                meanQoE[LocalIndex] = QoEPerLandmark[LocalIndex, 0];
                skypeQoE[LocalIndex] = skypeQoEPerLandmark[LocalIndex, 0];

                /*
                // Get Full Time
                // This will appear on the x axis...
                TimeArray[LocalIndex] = new Time();
                */
                if (LocalIndex > max_points_per_graph)
                {
                    // for each graph, clear everything and redraw the last 19 points. Add the 20th at the end.
                    //delay
                    this.delayChart.Series["Upload Delay"].Points.Clear();
                    int xval = 0;
                    for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                    {
                        this.delayChart.Series["Upload Delay"].Points.AddXY((double)xval, FinalDelayAndJitterArray[pos].dimension1);
                        xval++;
                    }


                    //jitter
                    this.jitterChart.Series["Download Delay"].Points.Clear();
                    xval = 0;
                    for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                    {
                        this.jitterChart.Series["Download Delay"].Points.AddXY((double)xval, FinalDelayAndJitterArray[pos].dimension2);
                        xval++;
                    }

                    // upload loss rate
                    this.uploadLossRateChart.Series["Upload Loss Rate"].Points.Clear();
                    xval = 0;
                    for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                    {
                        this.uploadLossRateChart.Series["Upload Loss Rate"].Points.AddXY((double)xval, FinalLossRateArray[pos].dimension1);
                        xval++;
                    }

                    // download loss rate
                    this.downloadLossRateChart.Series["Download Loss Rate"].Points.Clear();
                    xval = 0;
                    for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                    {
                        this.downloadLossRateChart.Series["Download Loss Rate"].Points.AddXY((double)xval, FinalLossRateArray[pos].dimension2);
                        xval++;
                    }

                    // upload bw
                    this.uploadBandwidthChart.Series["Upload Bandwidth"].Points.Clear();
                    xval = 0;
                    for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                    {
                        this.uploadBandwidthChart.Series["Upload Bandwidth"].Points.AddXY((double)xval, FinalBandwidthArray[pos].dimension1);
                        xval++;
                    }

                    // download bw
                    this.downloadBandwidthChart.Series["Download Bandwidth"].Points.Clear();
                    xval = 0;
                    for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                    {
                        this.downloadBandwidthChart.Series["Download Bandwidth"].Points.AddXY((double)xval, FinalBandwidthArray[pos].dimension2);
                        xval++;
                    }

                    // QoE 
                    for (int i = 0; i < LandmarksNumber; i++)
                    {
                        //this.QoEChart.Series["QoE"].Points.AddXY("Landmark " + i + 1, QoEPerLandmark[LocalIndex, i]); // TODO how does this work?
                        this.QoEChart.Series["Hardcoded"].Points.Clear();
                        xval = 0;
                        for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                        {
                            this.QoEChart.Series["Hardcoded"].Points.AddXY((double)xval, QoEPerLandmark[pos, i]);
                            xval++;
                        }
                    }
                    // QoE bis
                    this.meanQoEChart.Series["Tree"].Points.Clear();
                    xval = 0;
                    for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                    {
                        this.meanQoEChart.Series["Tree"].Points.AddXY((double)xval, meanQoE[pos]);
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
                    this.delayChart.Series["Upload Delay"].Points.AddXY(LocalIndex, FinalDelayAndJitterArray[LocalIndex].dimension1);
                    this.jitterChart.Series["Download Delay"].Points.AddXY(LocalIndex, FinalDelayAndJitterArray[LocalIndex].dimension2);
                    this.uploadBandwidthChart.Series["Upload Bandwidth"].Points.AddXY(LocalIndex, FinalBandwidthArray[LocalIndex].dimension1);
                    this.downloadBandwidthChart.Series["Download Bandwidth"].Points.AddXY(LocalIndex, FinalBandwidthArray[LocalIndex].dimension2);
                    this.uploadLossRateChart.Series["Upload Loss Rate"].Points.AddXY(LocalIndex, FinalLossRateArray[LocalIndex].dimension1);
                    this.downloadLossRateChart.Series["Download Loss Rate"].Points.AddXY(LocalIndex, FinalLossRateArray[LocalIndex].dimension2);
                    for (int i = 0; i < LandmarksNumber; i++)
                    {
                        this.QoEChart.Series["Hardcoded"].Points.AddXY("Landmark " + i + 1, QoEPerLandmark[LocalIndex, i]);
                    }
                    this.meanQoEChart.Series["Tree"].Points.AddXY(LocalIndex, meanQoE[LocalIndex]);
                }

                Console.WriteLine("HARDCODED {0}", skypeQoE[LocalIndex]);
                LocalIndex++;
                //Thread.Sleep((int)PeriodNumeric.Value * 60 * 1000); // 
                Console.WriteLine("Total Execution Time {0}", totalTime.ElapsedMilliseconds);
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
                decisionTreeTxtBox.Text = DecisionTreeFile.Name;
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

            landmarksListForm.Clear();
            // Fill the Landmarks RichTextBox
            for (int i = 0; i < ChosenLandmarks.Count; i++)
            {
                if (i != ChosenLandmarks.Count - 1)
                {
                    landmarksListForm.Text += ChosenLandmarks[i] + "\n";
                }
                else
                {
                    landmarksListForm.Text += ChosenLandmarks[i];
                }
            }
        }

        UdpClient udpOut;
        UdpClient udpIn;
        int udpPort;
        int tcpPort;
        string host;
        IPEndPoint RemoteIpEndPoint;
        int number_probes;

        private void setUpMeasurement(string host_, int udpPort_, int tcpPort_, int number_probes_)
        {
            udpPort = udpPort_;
            tcpPort = tcpPort_;
            host = host_;
            udpOut = new UdpClient();
            udpOut.Connect(host, udpPort);
            udpIn = new UdpClient(udpPort);
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            number_probes = number_probes_;
        }

        Byte[] udpPacketBytes;
        Byte[] serverMeasurements = new Byte[100];
        static int udpPacketSize = 12;
        static int bits_per_packet = 28 * 8 + udpPacketSize * 8; //NOTE: bandwith consider headers of udp packets => ip_headers[20bytes] and udp_headers[8bytes]
        static int serverMeasurementsSize = 4 * 3; //delay bandwith loss_rate [all int]

        private int[] getMeasurements()
        {
            Console.WriteLine("Executing Measurements...");

            Stopwatch stopwatch = new Stopwatch();
            long timestamp;
            long startTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
            stopwatch.Start();

            int[] ret = new int[6];

            float[] lossRate = new float[2];
            float[] bandwith = new float[2];
            float[] delay = new float[2];
            Console.WriteLine("Sending udp packet train...");
            for (int i = 0; i < number_probes; i++)
            {
                timestamp = startTime + stopwatch.ElapsedMilliseconds;
                //Console.WriteLine("Packet {0} timestamp:{1}", i, timestamp);
                udpPacketBytes = new Byte[udpPacketSize];
                Buffer.BlockCopy(BitConverter.GetBytes(i), 0, udpPacketBytes, 0, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(timestamp), 0, udpPacketBytes, 4, 8);
                udpOut.Send(udpPacketBytes, udpPacketBytes.Length);
            }
            stopwatch.Stop();
            Console.WriteLine("Done.");

            Console.WriteLine("Connecting to server (tcp)...");
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
            serverMeasurements = new Byte[serverMeasurementsSize];
            int x = ClientSocket.Receive(serverMeasurements, 0, serverMeasurementsSize, SocketFlags.None);

            delay[0] = BitConverter.ToInt32(serverMeasurements, 0);
            bandwith[0] = BitConverter.ToInt32(serverMeasurements, 4);
            lossRate[0] = BitConverter.ToInt32(serverMeasurements, 8);

            ClientSocket.Send(BitConverter.GetBytes('0'), 1, SocketFlags.None);

            ClientSocket.Close();
            Console.WriteLine("Server measurements Delay-U:{0}ms Bandwith-U:{1}Kbits/s LossRate-U:{2}%", delay[0], bandwith[0], lossRate[0]);

            Console.WriteLine("Waiting for udp packet train...");
            udpPacketBytes = new Byte[udpPacketSize];

            udpIn.Client.ReceiveTimeout = 0;

            bool probing = true;
            int seq_id;

            int delay_sum = 0;
            int packets_received = 0;
            long exec_time = 0;

            Stopwatch watch = new Stopwatch();

            startTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
            stopwatch.Restart();
            long timestamp2 = 0;

            while (probing)
            {
                try
                {
                    udpPacketBytes = udpIn.Receive(ref RemoteIpEndPoint);
                }
                catch
                {
                    probing = false;
                    break;
                }
                if (exec_time == 0)
                {
                    udpIn.Client.ReceiveTimeout = 5000;
                    watch.Start();
                    Console.WriteLine("Receiving packets...");
                }
                exec_time = watch.ElapsedMilliseconds;
                timestamp2 = stopwatch.ElapsedMilliseconds + startTime;

                seq_id = BitConverter.ToInt32(udpPacketBytes, 0);
                timestamp = BitConverter.ToInt64(udpPacketBytes, 4);
                delay_sum += (int)(timestamp2 - timestamp);
                packets_received++;
            }
            watch.Stop();
            stopwatch.Stop();

            Console.WriteLine("Done.");

            delay[1] = delay_sum / packets_received;
            bandwith[1] = packets_received * bits_per_packet / exec_time;
            lossRate[1] = 100 * (number_probes - packets_received) / number_probes;

            Console.WriteLine("Client measurements Delay-D:{0}ms Bandwith-D:{1}Kbits/s LossRate-D:{2}%", delay[1], bandwith[1], lossRate[1]);

            ret[0] = (int)delay[0];
            ret[1] = (int)delay[1];
            ret[2] = (int)bandwith[0];
            ret[3] = (int)bandwith[1];
            ret[4] = (int)lossRate[0];
            ret[5] = (int)lossRate[1];

            Console.WriteLine("Measurements Complete.");

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