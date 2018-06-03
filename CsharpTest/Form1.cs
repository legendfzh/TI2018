using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;

namespace CsharpTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Form.CheckForIllegalCrossThreadCalls = false;
            string[] ArryPort = SerialPort.GetPortNames();
            for (int i = 0; i < ArryPort.Length; i++)
            {
                ListBox.Items.Add(ArryPort[i]);
            }

        }
        private void sendMessage(string Msg,bool to1,bool to2)
        {
            try
            {
                if (to1)
                {
                    serialPort1.WriteLine(Msg);
                    textBox1.AppendText(string.Concat("向小车1发送：", Msg, "\n"));
                }
                if (to2)
                {
                    serialPort3.WriteLine(Msg);
                    textBox1.AppendText(string.Concat("向小车2发送：", Msg, "\n"));
                }

            }
            catch
            {
                MessageBox.Show("发送错误！");
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ListBox.Items.Clear();
            string[] ArryPort = SerialPort.GetPortNames();
            for (int i = 0; i < ArryPort.Length; i++)
            {
                ListBox.Items.Add(ArryPort[i]);
            }
            //string[] ArryInfo = GetHarewareInfo(HardwareEnum.Win32_PnPEntity, "Name");



        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string str = serialPort1.ReadExisting();
            textBox1.AppendText(string.Concat("收到1号小车信息：",str, "\n","上位机并不想处理该信息╭(╯^╰)╮\n"));
        }

        private void button2_Click(object sender, EventArgs e)//连接1小车
        {
            try
            {
                serialPort1.PortName = ListBox.SelectedItem.ToString();
                serialPort1.Open();
                textBox1.AppendText(string.Concat(ListBox.SelectedItem.ToString(),"选手1小车已连接！\n"));
            }
            catch
            {
                MessageBox.Show("连接错误！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            serialPort3.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sendMessage(textBox2.Text,checkBox1.Checked,checkBox2.Checked);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            sendMessage(string.Concat("*TR", textBox3.Text, "*"),true,true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sendMessage("*RU*",true,true);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sendMessage("*ST*",true,true);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                sendMessage(textBox2.Text, checkBox1.Checked, checkBox2.Checked);
                textBox2.Clear();
            }
                
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)//选手1暂停
        {
            textBox1.AppendText(string.Concat("选手1申请暂停！\n"));
            sendMessage("*ST*", true, false);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort3.PortName = ListBox.SelectedItem.ToString();
                serialPort3.Open();
                textBox1.AppendText(string.Concat(ListBox.SelectedItem.ToString(), "选手2小车已连接！\n"));
            }
            catch
            {
                MessageBox.Show("连接错误！");
            }
        }

        private void button10_Click(object sender, EventArgs e)//选手2暂停
        {
            textBox1.AppendText(string.Concat("选手2申请暂停！\n"));
            sendMessage("*ST*", false, true);
        }

        private void serialPort3_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string str = serialPort1.ReadExisting();
            textBox1.AppendText(string.Concat("收到2号小车信息：", str, "\n", "上位机并不想处理该信息╭(╯^╰)╮\n"));
        }
        

        private void button12_Click(object sender, EventArgs e)
        {
            textBox1.AppendText(string.Concat("选手1暂停结束！\n"));
            sendMessage("*RU*", true, false);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.AppendText(string.Concat("选手2暂停结束！\n"));
            sendMessage("*RU*", false, true);
        }


        private static string[] GetHarewareInfo(HardwareEnum hardType, string propKey)
        {
            List<string> strs = new List<string>();
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + hardType))
                {
                    var hardInfos = searcher.Get();
                    foreach (var hardInfo in hardInfos)
                    {
                        if (hardInfo.Properties[propKey].Value != null)
                        {
                            String str = hardInfo.Properties[propKey].Value.ToString();
                            strs.Add(str);
                        }

                    }
                }
                return strs.ToArray();
            }
            catch
            {
                return null;
            }
            finally
            {
                strs = null;
            }
        }//end of func GetHarewareInfo().
        
        public enum HardwareEnum
        {
            // 硬件
            Win32_Processor, // CPU 处理器
            Win32_PhysicalMemory, // 物理内存条
            Win32_Keyboard, // 键盘
            Win32_PointingDevice, // 点输入设备，包括鼠标。
            Win32_FloppyDrive, // 软盘驱动器
            Win32_DiskDrive, // 硬盘驱动器
            Win32_CDROMDrive, // 光盘驱动器
            Win32_BaseBoard, // 主板
            Win32_BIOS, // BIOS 芯片
            Win32_ParallelPort, // 并口
            Win32_SerialPort, // 串口
            Win32_SerialPortConfiguration, // 串口配置
            Win32_SoundDevice, // 多媒体设置，一般指声卡。
            Win32_SystemSlot, // 主板插槽 (ISA & PCI & AGP)
            Win32_USBController, // USB 控制器
            Win32_NetworkAdapter, // 网络适配器
            Win32_NetworkAdapterConfiguration, // 网络适配器设置
            Win32_Printer, // 打印机
            Win32_PrinterConfiguration, // 打印机设置
            Win32_PrintJob, // 打印机任务
            Win32_TCPIPPrinterPort, // 打印机端口
            Win32_POTSModem, // MODEM
            Win32_POTSModemToSerialPort, // MODEM 端口
            Win32_DesktopMonitor, // 显示器
            Win32_DisplayConfiguration, // 显卡
            Win32_DisplayControllerConfiguration, // 显卡设置
            Win32_VideoController, // 显卡细节。
            Win32_VideoSettings, // 显卡支持的显示模式。

            // 操作系统
            Win32_TimeZone, // 时区
            Win32_SystemDriver, // 驱动程序
            Win32_DiskPartition, // 磁盘分区
            Win32_LogicalDisk, // 逻辑磁盘
            Win32_LogicalDiskToPartition, // 逻辑磁盘所在分区及始末位置。
            Win32_LogicalMemoryConfiguration, // 逻辑内存配置
            Win32_PageFile, // 系统页文件信息
            Win32_PageFileSetting, // 页文件设置
            Win32_BootConfiguration, // 系统启动配置
            Win32_ComputerSystem, // 计算机信息简要
            Win32_OperatingSystem, // 操作系统信息
            Win32_StartupCommand, // 系统自动启动程序
            Win32_Service, // 系统安装的服务
            Win32_Group, // 系统管理组
            Win32_GroupUser, // 系统组帐号
            Win32_UserAccount, // 用户帐号
            Win32_Process, // 系统进程
            Win32_Thread, // 系统线程
            Win32_Share, // 共享
            Win32_NetworkClient, // 已安装的网络客户端
            Win32_NetworkProtocol, // 已安装的网络协议
            Win32_PnPEntity,//all device
        }
    }
}


