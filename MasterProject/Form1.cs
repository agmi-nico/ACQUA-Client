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
        MeasurementVector[] FinalOWDArray;
        MeasurementVector[] FinalBandwidthArray;
        MeasurementVector[] FinalLossRateArray;

        bool running = false;

        private void runButtonClick(object sender, EventArgs e)
        {
            LandmarksNumber = 0;

            if (decisionTreeTextBox.Text == "")
            {
                MessageBox.Show("Decision Tree missing", "Error");
                return;
            }

            // Count the number of landmarks in the richTextBox
            foreach (string landmark in landmarksListForm.Lines)
                LandmarksNumber++;

            Thread t = new Thread(new ThreadStart(Run));
            System.Console.WriteLine("Starting...");
            if (!running)
            {
                running = true;
                t.Start();
            }
            else
            {
                System.Console.WriteLine("\tAlready Running");
            }
        }

        //int MeanResult = -1;
        int[,] QoEPerLandmark;
        int[,] skypeQoEPerLandmark;
        int[,] secondQoEPerLandmark;
        // TODO save QoE for second tree also
        int max_points_per_graph = 10; //10
        int[] meanQoE;
        int[] skypeQoE;
        int[] secondMeanQoE;
        bool firstQoETabDrawn = false;
        bool secondQoETabDrawn = false;

        void Run2()
        {
            System.Console.WriteLine("\tCalculating QoE with values:");
            int U_BD = 16;
            int U_De = 200;
            int U_LR = 15;
            int D_BD = 16;
            int D_De = 200;
            int D_LR = 15;

            System.Console.WriteLine("\tDelay-U:   \t{0}ms    \tDelay-D:   \t{1}ms", U_De, D_De);
            System.Console.WriteLine("\tBandwith-U:\t{0}Kbit/s\tBandwith-D:\t{1}Kbit/s", U_BD, D_BD);
            System.Console.WriteLine("\tLossRate-U:\t{0}%     \tLossRate-D:\t{1}%     ", U_LR, D_LR);
            // just to compare the above result with the hard-coded tree in the bottom of this file
            int a = CallerClass.Call(root, U_De, D_De, U_BD, D_BD, U_LR, D_LR);
            int b = estimatedSkypeQuality(D_BD, U_BD, D_De, U_De, D_LR, U_LR);
            System.Console.WriteLine("\tSalimCode:\t{0}     \tDecitionTree:\t{1}", b, a);
        }

        void Run()
        {
            // see bit.ly/1qnHcFM
            // Open a tab for first tree, if present
            if (!firstQoETabDrawn && firstTreeSelected)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.tabControl.Controls.Add(this.QoETab);
                });
                firstQoETabDrawn = true;
            }
            // Open a tab for second tree, if present
            if (!secondQoETabDrawn && secondTreeSelected)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.tabControl.Controls.Add(this.secondQoETab);
                });
                secondQoETabDrawn = true;
            }


            LandmarksNumber = 1;
            QoEPerLandmark = new int[max_num_experiments, LandmarksNumber];
            skypeQoEPerLandmark = new int[max_num_experiments, LandmarksNumber];
            secondQoEPerLandmark = new int[max_num_experiments, LandmarksNumber];
            meanQoE = new int[max_num_experiments];
            skypeQoE = new int[max_num_experiments];
            secondMeanQoE = new int[max_num_experiments];
            FinalOWDArray = new MeasurementVector[max_num_experiments];
            FinalBandwidthArray = new MeasurementVector[max_num_experiments];
            FinalLossRateArray = new MeasurementVector[max_num_experiments];
            int LocalIndex = 0;

            setUpMeasurement("10.0.1.1", 9050, 9051, 100);
            Stopwatch totalTime = new Stopwatch();

            int[] measurements = getMeasurements();

            while (running)
            {
                totalTime.Restart();
                // measurements = {UD, DD, UBW, DBW, ULR, DLR}
                measurements = getMeasurements();

                System.Console.WriteLine("\tCalculating QoE with values from Experiment {0}:", experimentID);

                System.Console.WriteLine("O-Delay-U: \t{0}ms    \tO-Delay-D: \t{1}ms    \tSum:{2}", measurements[0], measurements[1], measurements[1] + measurements[0]);
                System.Console.WriteLine("Shift-U:   \t{0}ms    \tShift-D:   \t{1}ms    \tSum:{2}", shift_delay_up, shift_delay_down, shift_delay_up + shift_delay_down);

                measurements[0] -= shift_delay_up;
                measurements[1] -= shift_delay_down;

                System.Console.WriteLine("\tDelay-U:   \t{0}ms    \tDelay-D:   \t{1}ms", measurements[0], measurements[1]);
                System.Console.WriteLine("\tBandwith-U:\t{0}Kbit/s\tBandwith-D:\t{1}Kbit/s", measurements[2], measurements[3]);
                System.Console.WriteLine("\tLossRate-U:\t{0}%     \tLossRate-D:\t{1}%     ", measurements[4], measurements[5]);
                //int lowestRTT = Int32.MaxValue;
                //for (int i = 0; i < 4; i++)
                //{
                //    int RTT = PingHostDelay(host);
                //    if (RTT > -1 && RTT < lowestRTT)
                //    {
                //        lowestRTT = RTT;
                //    }
                //}

                //Console.WriteLine("\tLowest RTT: {0}ms", lowestRTT);

                // Estimate QoE with the provided tree
                if (firstTreeSelected)
                {
                    //Call(Node root, int UD, int DD, int UBW, int DBW, int ULR, int DLR)
                    QoEPerLandmark[LocalIndex, 0] = CallerClass.Call(root, measurements[0], measurements[1], measurements[2], measurements[3], measurements[4], measurements[5]);
                }
                // just to compare the above result with the hard-coded tree in the bottom of this file
                skypeQoEPerLandmark[LocalIndex, 0] = estimatedSkypeQuality(measurements[3], measurements[2], measurements[1], measurements[0], measurements[5], measurements[4]);

                // Estimate QoE with the second provided tree
                if (secondTreeSelected)
                {
                    secondQoEPerLandmark[LocalIndex, 0] = CallerClass.Call(root2, measurements[0], measurements[1], measurements[2], measurements[3], measurements[4], measurements[5]);
                }

                FinalOWDArray[LocalIndex] = new MeasurementVector(measurements[0], measurements[1]);
                FinalBandwidthArray[LocalIndex] = new MeasurementVector(measurements[2], measurements[3]);
                FinalLossRateArray[LocalIndex] = new MeasurementVector(measurements[4], measurements[5]);

                meanQoE[LocalIndex] = QoEPerLandmark[LocalIndex, 0];
                skypeQoE[LocalIndex] = skypeQoEPerLandmark[LocalIndex, 0];
                secondMeanQoE[LocalIndex] = secondQoEPerLandmark[LocalIndex, 0];
                /*
                // Get Full Time
                // This will appear on the x axis...
                TimeArray[LocalIndex] = new Time();
                */
                if (LocalIndex > max_points_per_graph)
                {
                    // for each graph, clear everything and redraw the last 19 points. Add the 20th at the end.
                    // upload OWD
                    this.uploadDelayChart.Series["Upload OWD"].Points.Clear();
                    int xval = 0;
                    for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                    {
                        this.uploadDelayChart.Series["Upload OWD"].Points.AddXY((double)xval, FinalOWDArray[pos].dimension1);
                        xval++;
                    }
                    foreach (System.Windows.Forms.DataVisualization.Charting.DataPoint dp in uploadDelayChart.Series["Upload OWD"].Points)
                    {
                        switch ((int)dp.YValues[0])
                        {
                            case 0: dp.AxisLabel = "No Call"; break;
                            
                        }
                    }

                    //download OWD [ no jitter computation for the moment]
                    this.downloadDelayChart.Series["Download OWD"].Points.Clear();
                    xval = 0;
                    for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                    {
                        this.downloadDelayChart.Series["Download OWD"].Points.AddXY((double)xval, FinalOWDArray[pos].dimension2);
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

                    if (firstTreeSelected)
                    {
                        Console.WriteLine("Plotting new point on first tree");
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
                    Console.WriteLine("Value of secondTreeSelected when plotting:{0}", secondTreeSelected);
                    if (secondTreeSelected)
                    {
                        Console.WriteLine("Plotting new point on second tree");
                        // QoE 
                        for (int i = 0; i < LandmarksNumber; i++)
                        {
                            //this.QoEChart.Series["QoE"].Points.AddXY("Landmark " + i + 1, QoEPerLandmark[LocalIndex, i]); // TODO how does this work?
                            this.secondQoEChart.Series["QoE"].Points.Clear();
                            xval = 0;
                            for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                            {
                                Console.WriteLine("Plotting new point on graph1 of second tree");
                                this.secondQoEChart.Series["QoE"].Points.AddXY((double)xval, secondQoEPerLandmark[pos, i]);
                                xval++;
                            }
                        }
                        // QoE bis
                        this.secondMeanQoEChart.Series["Mean QoE"].Points.Clear();
                        xval = 0;
                        for (int pos = LocalIndex - max_points_per_graph + 1; pos <= LocalIndex; pos++)
                        {
                            Console.WriteLine("Plotting new point on graph2 of second tree");
                            this.secondMeanQoEChart.Series["Mean QoE"].Points.AddXY((double)xval, secondMeanQoE[pos]);
                            xval++;
                        }

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
                    this.uploadDelayChart.Series["Upload OWD"].Points.AddXY(LocalIndex, FinalOWDArray[LocalIndex].dimension1);
                    this.downloadDelayChart.Series["Download OWD"].Points.AddXY(LocalIndex, FinalOWDArray[LocalIndex].dimension2);
                    this.uploadBandwidthChart.Series["Upload Bandwidth"].Points.AddXY(LocalIndex, FinalBandwidthArray[LocalIndex].dimension1);
                    this.downloadBandwidthChart.Series["Download Bandwidth"].Points.AddXY(LocalIndex, FinalBandwidthArray[LocalIndex].dimension2);
                    this.uploadLossRateChart.Series["Upload Loss Rate"].Points.AddXY(LocalIndex, FinalLossRateArray[LocalIndex].dimension1);
                    this.downloadLossRateChart.Series["Download Loss Rate"].Points.AddXY(LocalIndex, FinalLossRateArray[LocalIndex].dimension2);
                    for (int i = 0; i < LandmarksNumber; i++)
                    {
                        this.QoEChart.Series["Hardcoded"].Points.AddXY("Landmark " + i + 1, QoEPerLandmark[LocalIndex, i]);
                    }
                    this.meanQoEChart.Series["Tree"].Points.AddXY(LocalIndex, meanQoE[LocalIndex]);
                    for (int i = 0; i < LandmarksNumber; i++)
                    {
                        this.secondQoEChart.Series["QoE"].Points.AddXY("Landmark " + i + 1, secondQoEPerLandmark[LocalIndex, i]);
                    }
                    this.secondMeanQoEChart.Series["Mean QoE"].Points.AddXY(LocalIndex, secondMeanQoE[LocalIndex]);
                }

                Console.WriteLine("HARDCODED {0}", skypeQoE[LocalIndex]);
                LocalIndex++;
                Console.WriteLine("\tTotal Execution Time {0}", totalTime.ElapsedMilliseconds);
                Thread.Sleep(5000);
            }

            this.Close();
        }


        // Choose Decision Tree (Configuration File)
        Node root;
        FileStream DecisionTreeFile;
        bool firstTreeSelected = false;
        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DecisionTreeFile = new FileStream(openFileDialog1.FileName, FileMode.Open);
                decisionTreeTextBox.Text = DecisionTreeFile.Name;
                DecisionTreeFile.Close();
                root = CallerClass.Load(DecisionTreeFile.Name);
                if (root == null)
                {
                    MessageBox.Show("Error in Decision Tree Format. Please choose a valid XML format.", "Error");
                    DecisionTreeFile.Close();
                    firstTreeSelected = false;
                    return;
                }
                firstTreeSelected = true;
                Console.WriteLine("First tree selected!");
            }
            else
            {
                MessageBox.Show("Error when opening the Decision Tree file", "Error");
                firstTreeSelected = false;
                return;
            }
        }
        Node root2;
        FileStream DecisionTreeFile2;
        bool secondTreeSelected = false;
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DecisionTreeFile2 = new FileStream(openFileDialog1.FileName, FileMode.Open);
                decisionTreeTwoTextBox.Text = DecisionTreeFile2.Name;
                DecisionTreeFile2.Close();
                root2 = CallerClass.Load(DecisionTreeFile2.Name);
                if (root2 == null)
                {
                    MessageBox.Show("Error in Decision Tree Format. Please choose a valid XML format.", "Error");
                    DecisionTreeFile2.Close();
                    secondTreeSelected = false;
                    return;
                }
                secondTreeSelected = true;
                Console.WriteLine("Second Tree selected!");
            }
            else
            {
                MessageBox.Show("Error when opening the Decision Tree file", "Error");
                secondTreeSelected = false;
                return;
            }
        }
        string[] AllLandmarks = new string[100];

        UdpClient udpOut;
        UdpClient udpIn;
        int udpPort;
        int tcpPort;
        string host;
        IPEndPoint RemoteIpEndPoint;
        int number_probes;

        byte[] udpPacketBytes;
        byte[] serverMeasurements = new byte[100];
        static int serverMeasurementsSize = 4 * 4; //delay bandwith loss_rate experimentID [all int]
        static int udpPacketSize = serverMeasurementsSize + 60;
        static int bits_per_packet = 28 * 8 + udpPacketSize * 8; //NOTE: bandwith consider headers of udp packets => ip_headers[20bytes] and udp_headers[8bytes]
        static int experimentID = -1;

        int shift_delay_up = 0;
        int shift_delay_down = 0;
        byte[] udpDelayBytes = new byte[16];
        UdpClient udpDelay;

        private void delayServer()
        {
            udpDelay = new UdpClient(9052);
            udpDelay.Connect("192.168.0.1", 9052);
            udpDelay.Client.ReceiveTimeout = 2000;
        }


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
            delayServer();
        }

        private int[] getMeasurements()
        {
            experimentID++;
            Console.WriteLine("Executing Experiment {0}...", experimentID);

            if (experimentID % 2 == 0)
            {
                int delayID = experimentID;

                Buffer.BlockCopy(BitConverter.GetBytes(delayID), 0, udpDelayBytes, 0, 4);
                long delayTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                Buffer.BlockCopy(BitConverter.GetBytes(delayTime), 0, udpDelayBytes, 4, 8);
                udpDelay.Send(udpDelayBytes, udpDelayBytes.Length);
                bool ok = false;
                try
                {
                    udpDelayBytes = udpDelay.Receive(ref RemoteIpEndPoint);
                    delayTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                    ok = true;
                }
                catch
                {
                    Console.WriteLine("No response");
                }
                if (ok)
                {
                    if (delayID != BitConverter.ToInt32(udpDelayBytes, 0))
                    {
                        Console.WriteLine("Shift Delay Packet Error");
                    }
                    shift_delay_up = BitConverter.ToInt32(udpDelayBytes, 12);
                    shift_delay_down = (int)(delayTime - BitConverter.ToInt64(udpDelayBytes, 4));
                }
            }

            Stopwatch stopwatch = new Stopwatch();
            long timestamp;
            long startTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
            stopwatch.Start();

            int[] ret = new int[6];

            float[] lossRate = new float[2];
            float[] bandwith = new float[2];
            float[] delay = new float[2];
            Console.WriteLine("Sending udp packet train to {0}:{1}...", host, udpPort);
            udpPacketBytes = new byte[udpPacketSize];
            for (int i = 0; i < number_probes; i++)
            {
                timestamp = startTime + stopwatch.ElapsedMilliseconds;
                //Console.WriteLine("Packet {0} timestamp:{1}", i, timestamp);
                Buffer.BlockCopy(BitConverter.GetBytes(i), 0, udpPacketBytes, 0, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(timestamp), 0, udpPacketBytes, 4, 8);
                Buffer.BlockCopy(BitConverter.GetBytes(experimentID), 0, udpPacketBytes, 12, 4);
                udpOut.Send(udpPacketBytes, udpPacketBytes.Length);
            }
            stopwatch.Stop();
            Console.WriteLine("Done.");

            Console.WriteLine("Connecting to {0}:{1} (tcp)...", host, tcpPort);
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

            Console.WriteLine("Waiting for server measurements results...");
            serverMeasurements = new byte[serverMeasurementsSize];
            int x = ClientSocket.Receive(serverMeasurements, 0, serverMeasurementsSize, SocketFlags.None);

            delay[0] = BitConverter.ToInt32(serverMeasurements, 0);
            bandwith[0] = BitConverter.ToInt32(serverMeasurements, 4);
            lossRate[0] = BitConverter.ToInt32(serverMeasurements, 8);

            ClientSocket.Send(BitConverter.GetBytes('0'), 1, SocketFlags.None);

            ClientSocket.Close();
            Console.WriteLine("Server measurements:\n  Delay-U:{0}ms Bandwith-U:{1}Kbits/s LossRate-U:{2}%", delay[0], bandwith[0], lossRate[0]);

            Console.WriteLine("Waiting for udp packet train...");

            udpPacketBytes = new byte[udpPacketSize];

            udpIn.Client.ReceiveTimeout = 0;

            bool probing = true;
            int seq_id;

            //int delay_sum = 0;
            int packets_received = 0;
            //int delay_packets = 0;
            int expID;
            long exec_time = 0;

            Stopwatch watch = new Stopwatch();

            startTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
            stopwatch.Restart();
            long timestamp2 = 0;

            int lowestDelay = Int32.MaxValue;
            int packetDelay = Int32.MaxValue;

            // Console.WriteLine("ID:{3}, Client:{0}, Server:{1} Result:{2}", timestamp2, timestamp, packetDelay, seq_id);
            //long[] clientTimestamp = new long[100];
            //long[] serverTimestamp = new long[100];
            //long[] resultTimestamp = new long[100];

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
                if (packets_received == 0)
                {
                    watch.Start();
                    udpIn.Client.ReceiveTimeout = 5000;
                    Console.WriteLine("Receiving packets...");
                }
                timestamp2 = stopwatch.ElapsedMilliseconds + startTime;
                exec_time = watch.ElapsedMilliseconds;
                seq_id = BitConverter.ToInt32(udpPacketBytes, 0);
                timestamp = BitConverter.ToInt64(udpPacketBytes, 4);

                expID = BitConverter.ToInt32(udpPacketBytes, 12);
                if (expID != experimentID)
                {
                    Console.WriteLine("Experiment ID and Received ID dont match {0}!={1}", experimentID, expID);
                }

                packetDelay = (int)(timestamp2 - timestamp);

                //clientTimestamp[seq_id] = timestamp;
                //serverTimestamp[seq_id] = timestamp2;
                //resultTimestamp[seq_id] = packetDelay;

                //Console.WriteLine("ID:{3}, Client:{0}, Server:{1} Result:{2}", timestamp2, timestamp, packetDelay, seq_id);

                if (lowestDelay > packetDelay)
                {
                    lowestDelay = packetDelay;
                }

                //if (delay_packets < 5)
                //{
                //    delay_sum += packetDelay;
                //    delay_packets++;
                //}

                packets_received++;
            }
            watch.Stop();
            stopwatch.Stop();

            //for (int i = 0; i < number_probes; i++)
            //{
            //    Console.WriteLine("ID:{3}, Client:{0}, Server:{1} Result:{2}", clientTimestamp[i], serverTimestamp[i], resultTimestamp[i], i);
            //}

            Console.WriteLine("Done.");

            //delay[1] = delay_sum / delay_packets;
            delay[1] = lowestDelay;
            bandwith[1] = packets_received * bits_per_packet / exec_time;
            //Console.WriteLine("Upon reception, the burst spanned: {0} ms", exec_time);
            lossRate[1] = 100 * (number_probes - packets_received) / number_probes;
            //Console.WriteLine("\tProbes:{0}, Received:{1} Result:{2}", number_probes, packets_received, lossRate[1]);

            Console.WriteLine("Client measurements:\n  Delay-D:{0}ms Bandwith-D:{1}Kbits/s LossRate-D:{2}%", delay[1], bandwith[1], lossRate[1]);
            //Console.WriteLine("Lowest Delay-D:{0}ms", delay[1]);

            ret[0] = (int)delay[0];
            ret[1] = (int)delay[1];
            ret[2] = (int)bandwith[0];
            ret[3] = (int)bandwith[1];
            ret[4] = (int)lossRate[0];
            ret[5] = (int)lossRate[1];

            Console.WriteLine("Experiment {0} Complete.", experimentID);

            return ret;
        }

        public static int PingHostDelay(string host)
        {
            int RTT = -1;
            //IPAddress address = GetIpFromHost(ref host);
            PingOptions pingOptions = new PingOptions(128, true);
            Ping ping = new Ping();
            byte[] buffer = new byte[32];

            try
            {
                PingReply pingReply = ping.Send(host, 1000, buffer, pingOptions);
                if (pingReply != null)
                {
                    switch (pingReply.Status)
                    {
                        case IPStatus.Success:
                            RTT = (int)pingReply.RoundtripTime;
                            Console.WriteLine("Ping RTT: {0}", RTT);
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

        private void InitializeLandmarksArray()
        {
            AllLandmarks[0] = "10.0.1.1";
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
        private void closeButtonClick(object sender, EventArgs e)
        {
            running = false;
            Console.WriteLine("Finishing experiment and closing ACQUA...");
        }

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

        private void decisionTreeGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void experimentConfigGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void burstSizeUnitLabel_Click(object sender, EventArgs e)
        {

        }

        private void probeSizeUnitLabel_Click(object sender, EventArgs e)
        {

        }

        private void experimentSleepTime_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void decisionTreeSelectionLabel_Click(object sender, EventArgs e)
        {

        }

    }
}