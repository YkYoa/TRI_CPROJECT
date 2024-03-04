using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TRI_PROJECT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            String[] Baudrate = { "115200", "200000" };
            cboBaud.Items.AddRange(Baudrate);
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = cboPort.Text;
                serialPort1.BaudRate = int.Parse(cboBaud.Text);
                serialPort1.Open();
                button1.Enabled = false;
                button2.Enabled = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Close();
                button1.Enabled = true;
                button2.Enabled = false;
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        bool FlagStartConfig;
        bool FlagStopConfig;
        private void button3_Click(object sender, EventArgs e)
        {
            if (serialBox.Text.Equals("STARTCONFIG", StringComparison.OrdinalIgnoreCase))
            {
                // Xử lý khi chuỗi là "STARTCONFIG"
                FlagStartConfig = true;
                FlagStopConfig = false;
            }
            if (serialBox.Text.Equals("STOPCONFIG", StringComparison.OrdinalIgnoreCase))
            {
                // Xử lý khi chuỗi là "STARTCONFIG"
                FlagStartConfig = false;
                FlagStopConfig = true;
            }
            serialPort1.WriteLine(serialBox.Text);
        }
        private Tuple<string, string, string> parsestring(string input)
        {
            Match match = Regex.Match(input.Trim(), @"^(\d+)/(\d+)/(\w+)$");
            if(match.Success)
            {
                return Tuple.Create<string, string, string>(match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value);
            }
            return null;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cboPort.DataSource = SerialPort.GetPortNames();
            cboBaud.Text = "115200";
            for (int i = 1; i <= 50; i++)
            {
                bList.Add(i.ToString());
            }
            ID.DataSource = bList;
            
        }
        string selectedData = "";
        //List<string> str = new List<string>();
        //List<List<string>> s_56 = new List<List<string>>();
        private string hex2binary(string hexvalue)
        {
            string binaryval = "";
            binaryval = Convert.ToString(Convert.ToInt32(hexvalue, 16), 2);
            return binaryval;
        }
        private bool IsFloat(string input)
        {
            return float.TryParse(input, out _);
        }
        private bool IsHex(string input)
        {
            return int.TryParse(input, System.Globalization.NumberStyles.HexNumber, null, out _);
        }
        private void ID_SelectedValuesChanged(object sender, EventArgs e)
        {
            if (this.ActiveControl != ID)
                return;
            List<string> str = new List<string>();
            List<List<string>> s_56 = new List<List<string>>();
            if (ID.Text != null && int.TryParse(ID.Text.ToString(), out int selectedID))
            {
                if (selectedID >= 1 && selectedID <= bList.Count)
                {
                    selectedData = bList[selectedID - 1];
                    listBox3.Items.Add(selectedData);
                    string[] parts = selectedData.Split('/');
                    listBox4.Items.Add($" {parts[0]} Received at {DateTime.Now.ToString("HH:mm:ss")}");
                    if (parts.Length >= 2)
                    {
                        foreach (string item in parts)
                        {
                            str.Add(item);
         
                        }
                        
                        for (int i = 1; i < Math.Min(9, str.Count); i++)
                        {
                            List<string> innerList = new List<string>();
                            for (int j = 0; j < 4; j++)
                            {
                                List<string> substring = new List<string>();
                                substring.AddRange(str[i].Split(':'));

                                
                                if (j == 0)
                                {
                                    if (IsHex(substring[j]) == true)
                                    {
                                        innerList.Add(substring[j]);
                                    }
                                }
                                else
                                {
                                    if (IsFloat(substring[j]) == true)
                                    {
                                        innerList.Add(substring[j]);
                                    }
                                }
                                    
                            }
                            s_56.Add(innerList);
                           

                        }


                        string bin = "";
                        bin = hex2binary(s_56[0][0]);
                        string[] myStringArray = bin.PadLeft(12, '0').Select(x => x.ToString()).ToArray();
                        emc_1.Text = myStringArray[0];
                        s1_1.Text = myStringArray[1];
                        s2_1.Text = myStringArray[2];
                        s3_1.Text = myStringArray[3];
                        s4_1.Text = myStringArray[4];
                        s5_1.Text = myStringArray[5];
                        s6_1.Text = myStringArray[6];
                        s7_1.Text = myStringArray[7];
                        s8_1.Text = myStringArray[8];
                        s9_1.Text = myStringArray[9];
                        s10_1.Text = myStringArray[10];
                        s11_1.Text = myStringArray[11];
                        as1_1.Text = s_56[0][1];
                        as2_1.Text = s_56[0][2];
                        as3_1.Text = s_56[0][3];
                        string bin1 = "";
                        bin1 = hex2binary(s_56[1][0]);
                        string[] myStringArray1 = bin1.PadLeft(12, '0').Select(x => x.ToString()).ToArray();
                        emc_2.Text = myStringArray1[0];
                        s1_2.Text = myStringArray1[1];
                        s2_2.Text = myStringArray1[2];
                        s3_2.Text = myStringArray1[3];
                        s4_2.Text = myStringArray1[4];
                        s5_2.Text = myStringArray1[5];
                        s6_2.Text = myStringArray1[6];
                        s7_2.Text = myStringArray1[7];
                        s8_2.Text = myStringArray1[8];
                        s9_2.Text = myStringArray1[9];
                        s10_2.Text = myStringArray1[10];
                        s11_2.Text = myStringArray1[11];
                        as1_2.Text = s_56[1][1];
                        as2_2.Text = s_56[1][2];
                        as3_2.Text = s_56[1][3];
                        string bin2 = "";
                        bin2 = hex2binary(s_56[2][0]);
                        string[] myStringArray2 = bin2.PadLeft(12, '0').Select(x => x.ToString()).ToArray();
                        emc_3.Text = myStringArray2[0];
                        s1_3.Text = myStringArray2[1];
                        s2_3.Text = myStringArray2[2];
                        s3_3.Text = myStringArray2[3];
                        s4_3.Text = myStringArray2[4];
                        s5_3.Text = myStringArray2[5];
                        s6_3.Text = myStringArray2[6];
                        s7_3.Text = myStringArray2[7];
                        s8_3.Text = myStringArray2[8];
                        s9_3.Text = myStringArray2[9];
                        s10_3.Text = myStringArray2[10];
                        s11_3.Text = myStringArray2[11];
                        as1_3.Text = s_56[2][1];
                        as2_3.Text = s_56[2][2];
                        as3_3.Text = s_56[2][3];
                        string bin3 = "";
                        bin3 = hex2binary(s_56[3][0]);
                        string[] myStringArray3 = bin3.PadLeft(12, '0').Select(x => x.ToString()).ToArray();
                        emc_4.Text = myStringArray3[0];
                        s1_4.Text = myStringArray3[1];
                        s2_4.Text = myStringArray3[2];
                        s3_4.Text = myStringArray3[3];
                        s4_4.Text = myStringArray3[4];
                        s5_4.Text = myStringArray3[5];
                        s6_4.Text = myStringArray3[6];
                        s7_4.Text = myStringArray3[7];
                        s8_4.Text = myStringArray3[8];
                        s9_4.Text = myStringArray3[9];
                        s10_4.Text = myStringArray3[10];
                        s11_4.Text = myStringArray3[11];
                        as1_4.Text = s_56[3][1];
                        as2_4.Text = s_56[3][2];
                        as3_4.Text = s_56[3][3];
                        string bin4 = "";
                        bin4 = hex2binary(s_56[4][0]);
                        string[] myStringArray4 = bin4.PadLeft(12, '0').Select(x => x.ToString()).ToArray();
                        emc_5.Text = myStringArray4[0];
                        s1_5.Text = myStringArray4[1];
                        s2_5.Text = myStringArray4[2];
                        s3_5.Text = myStringArray4[3];
                        s4_5.Text = myStringArray4[4];
                        s5_5.Text = myStringArray4[5];
                        s6_5.Text = myStringArray4[6];
                        s7_5.Text = myStringArray4[7];
                        s8_5.Text = myStringArray4[8];
                        s9_5.Text = myStringArray4[9];
                        s10_5.Text = myStringArray4[10];
                        s11_5.Text = myStringArray4[11];
                        as1_5.Text = s_56[4][1];
                        as2_5.Text = s_56[4][2];
                        as3_5.Text = s_56[4][3];
                        string bin5 = "";
                        bin5 = hex2binary(s_56[5][0]);
                        string[] myStringArray5 = bin5.PadLeft(12, '0').Select(x => x.ToString()).ToArray();
                        emc_6.Text = myStringArray5[0];
                        s1_6.Text = myStringArray5[1];
                        s2_6.Text = myStringArray5[2];
                        s3_6.Text = myStringArray5[3];
                        s4_6.Text = myStringArray5[4];
                        s5_6.Text = myStringArray5[5];
                        s6_6.Text = myStringArray5[6];
                        s7_6.Text = myStringArray5[7];
                        s8_6.Text = myStringArray5[8];
                        s9_6.Text = myStringArray5[9];
                        s10_6.Text = myStringArray5[10];
                        s11_6.Text = myStringArray5[11];
                        as1_6.Text = s_56[5][1];
                        as2_6.Text = s_56[5][2];
                        as3_6.Text = s_56[5][3];
                        string bin6 = "";
                        bin6 = hex2binary(s_56[6][0]);
                        string[] myStringArray6 = bin6.PadLeft(12, '0').Select(x => x.ToString()).ToArray();
                        emc_7.Text = myStringArray6[0];
                        s1_7.Text = myStringArray6[1];
                        s2_7.Text = myStringArray6[2];
                        s3_7.Text = myStringArray6[3];
                        s4_7.Text = myStringArray6[4];
                        s5_7.Text = myStringArray6[5];
                        s6_7.Text = myStringArray6[6];
                        s7_7.Text = myStringArray6[7];
                        s8_7.Text = myStringArray6[8];
                        s9_7.Text = myStringArray6[9];
                        s10_7.Text = myStringArray6[10];
                        s11_7.Text = myStringArray6[11];
                        as1_7.Text = s_56[6][1];
                        as2_7.Text = s_56[6][2];
                        as3_7.Text = s_56[6][3];
                        string bin7 = "";
                        bin7 = hex2binary(s_56[7][0]);
                        string[] myStringArray7 = bin7.Select(x => x.ToString()).ToArray();
                        emc_8.Text = myStringArray7[0];
                        s1_8.Text = myStringArray7[1];
                        s2_8.Text = myStringArray7[2];
                        s3_8.Text = myStringArray7[3];
                        s4_8.Text = myStringArray7[4];
                        s5_8.Text = myStringArray7[5];
                        s6_8.Text = myStringArray7[6];
                        s7_8.Text = myStringArray7[7];
                        s8_8.Text = myStringArray7[8];
                        s9_8.Text = myStringArray7[9];
                        s10_8.Text = myStringArray7[10];
                        s11_8.Text = myStringArray7[11];
                        as1_8.Text = s_56[7][1];
                        as2_8.Text = s_56[7][2];
                        as3_8.Text = s_56[7][3];
                    }

                }
            }
        }

        List<string> bList = new List<string>();
        List<string> smallList = new List<string>();
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = serialPort1.ReadLine();
            listBox1.Items.Add(data);
            if (FlagStartConfig == true )
            {
                Tuple<string, string, string> parsedData = parsestring(data);
                if(parsedData != null) 
                {
                    string a = parsedData.Item1;
                    string b = parsedData.Item2;
                    string c = parsedData.Item3;
                    if (a == "1" && int.TryParse(b, out int b1) && b1 >= 1 && b1 <= 50 && c == "OK")
                    {
                        listBox2.Items.Add($"Device join network:{b1}");
                    }
                }
            }
            if(FlagStopConfig == true)  
            {
                for (int i = 1; i <= 50; i++)
                {
                    bList.Add("b" + i.ToString());

                }
                if (data.Length>20&&data.Length<185)
                {
                    string[] smallList = data.Split('/');
                    if (smallList[0] == "2")
                    {
                        if (int.TryParse(smallList[1], out int a) && a >= 1 && a <= 50)
                        {
                            for (int j = 1; j <= 50; j++)
                            {
                                if (a == j)
                                {
                                    bList[j - 1] = string.Join("/", smallList.Skip(1).Take(10));
                                }
                            }
                        }
                    }
                   
                }
                
            }

        }
    }
}
