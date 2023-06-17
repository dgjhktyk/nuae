using GodSharp.SerialPort;
using System;
using System.Windows.Forms;

namespace Nuae
{
    /// <summary>
    /// 포트 연결 페이지
    /// </summary>
    public partial class PortConnectPage : Form
    {
        // 선택된 포트
        string selectedPort = "";

        // pc에 연결된 포트들 담을 변수
        string[] ports;

        public PortConnectPage()
        {
            InitializeComponent();
            ports = GodSerialPort.GetPortNames();
            portComboBox.DataSource = ports;
        }

        private void portConButton_Click(object sender, EventArgs e)
        {
            if(selectedPort == "")
            {
                MessageBox.Show("포트를 선택하세요");
                return;
            }

            if (Serial.serial != null && Serial.serial.IsOpen)
            {
                portConButton.Text = "연결";
                portComboBox.Enabled = true;
                Serial.serial.Close();
                Serial.serial = null;

                return;
            } else
            {
                Serial.SetSerialPort(selectedPort);

                if (!Serial.Open())
                {
                    MessageBox.Show("연결 실패");
                    return;
                }
                else
                {
                    MessageBox.Show("연결 완료");
                    portConButton.Text = "연결 해제";
                    portComboBox.Enabled = false;
                    return;
                }
            }


        }

        /// <summary>
        /// 포트 콤보 박스에서 어떤 포트를 선택 했을 때 호출됩니다
        /// 선택한 포트를 selectedPort에 저장합니다
        /// </summary>
        private void portComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedPort = portComboBox.SelectedItem as string;
        }

        private void portComboBox_MouseClick(object sender, MouseEventArgs e)
        {
            ports = GodSerialPort.GetPortNames();
            portComboBox.DataSource = ports;
        }
    }
}
