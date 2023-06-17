using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Nuae
{
    public partial class LedControlPage : Form
    {

        /// <summary>
        /// ID:
        /// LED 제어기의 번호 입니다
        /// 현재 0x3031 (01h)로 고정해놓고 해당 ui부분은 보이지 않게 해놓기로 했습니다
        /// 
        /// 각 변수의 정보는
        /// [LPEC] LED 제어기 통신 프로토콜.pdf에서 확인 하세요
        /// </summary>
        short ID = 0x3031;
        short COMMAND = 0x3036;
        int DATA_ADDRESS;
        int DATA;
        short LRC_CHK;
        int brightness;

        public LedControlPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// LRC 체크섬 생성
        /// 자세한 정보는 [LPEC] LED 제어기 통신 프로토콜.pdf를 확인 하세요
        /// </summary>
        /// <returns></returns>
        short MakeLRCCHK()
        {
            char[] id_arr = new char[2] { (char)(ID >> 8), (char)(ID&0xFF) };
            char[] command_arr2 = new char[2] { (char)(COMMAND >> 8), (char)(COMMAND & 0xFF) };
            char[] data_address_high_arr = new char[2] { (char)(DATA_ADDRESS >> (8 * 3)), (char)((DATA_ADDRESS >> (8 * 2)) & 0xFF)};
            char[] data_address_low_arr = new char[2] { (char)((DATA_ADDRESS >> (8 * 1)) & 0xFF), (char)(DATA_ADDRESS & 0xFF) };
            char[] data_high_arr = new char[2] { (char)((DATA >> 8 * 3)), (char)((DATA >> 8 * 2)&0xFF)};
            char[] data_low_arr = new char[2] { (char)((DATA >> 8 * 1) & 0xFF), (char)(DATA & 0xFF) };

            string id = new string(id_arr);
            string command = new string(command_arr2);
            string data_address_high = new string(data_address_high_arr);
            string data_address_low = new string(data_address_low_arr);
            string data_high = new string(data_high_arr);
            string data_low = new string(data_low_arr);

            int sum = int.Parse(id, System.Globalization.NumberStyles.HexNumber) + int.Parse(command, System.Globalization.NumberStyles.HexNumber)
                + int.Parse(data_address_high, System.Globalization.NumberStyles.HexNumber) + int.Parse(data_address_low, System.Globalization.NumberStyles.HexNumber)
                + int.Parse(data_high, System.Globalization.NumberStyles.HexNumber) + int.Parse(data_low, System.Globalization.NumberStyles.HexNumber);

            short ret = (short)((sum^0xFF) + 1);
            string lrc_chk = ret.ToString("X");
            short lrcchk = 0;
            lrcchk |= (short)(lrc_chk[0] << 8);
            lrcchk |= (short)(lrc_chk[1]);
            return lrcchk;
        }

        /// <summary>
        /// LED 제어 데이터를 보냅니다
        /// </summary>
        private void ledDataSendButton_Click(object sender, EventArgs e)
        {
            if (Serial.serial == null || !Serial.serial.IsOpen)
            {
                MessageBox.Show("통신 없음. 메인보드에 연결하세요");
                return;
            }

            if (!UserInputCheck(DATA_ADDRESS))
                return;
            
            string hex_brightness = brightness.ToString("x4");  //  4자릿수 hex코드로 변환

            DATA = hex_brightness[0] << 8 * 3;
            DATA |= hex_brightness[1] << 8 * 2;
            DATA |= hex_brightness[2] << 8 * 1;
            DATA |= hex_brightness[3];

            LRC_CHK = MakeLRCCHK();

            List<byte> packet = new List<byte>();
            packet.Add(Constants.CMD_LED_CONTROL); // cmd

            packet.Add(Constants.STX_LED);
            packet.Add((byte)(ID >> 8)); // 현재 고정값
            packet.Add((byte)(ID & 0xFF));
            packet.Add((byte)(COMMAND >> 8)); // 고정값
            packet.Add((byte)(COMMAND & 0xFF));
            packet.Add((byte)(DATA_ADDRESS >> 8 * 3)); // DATA_ADDRESS,  BLUE = 0x31303031, RED = 0x31303032, UV = 0x31303033
            packet.Add((byte)(DATA_ADDRESS >> 8 * 2));
            packet.Add((byte)(DATA_ADDRESS >> 8 * 1));
            packet.Add((byte)(DATA_ADDRESS));
            packet.Add((byte)(DATA >> 8 * 3)); // DATA : 0~100의 값
            packet.Add((byte)(DATA >> 8 * 2));
            packet.Add((byte)(DATA >> 8 * 1));
            packet.Add((byte)(DATA));
            packet.Add((byte)((LRC_CHK >> 8) & 0xFF)); // 문서에 나와있는 체크섬 방식에 따라 계산된 값
            packet.Add((byte)(LRC_CHK & 0xFF));
            packet.Add((byte)(Constants.END_LED >> 8)); // 고정값
            packet.Add((byte)(Constants.END_LED & 0xFF));
            
            Serial.saved_led_control_data = packet;
            Serial.transmit_data(packet);
            MainPage.mainPage.led_data_timer.Enabled = true;
        }

        /// <summary>
        /// 제어기 ID 선택하는 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IDComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            short selected_id = short.Parse((string)IDComboBox.SelectedItem);
            string id;
            if (selected_id < 10)
            {
                id = "0";
            }
            else
            {
                id = "";
            }
            id += selected_id.ToString();
            ID = (short)(id[0] << 8);
            ID |= (short)id[1];
        }
        #region LED 채널(색깔) 선택 라디오 버튼
        private void blue_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            DATA_ADDRESS = Constants.BLUE;
        }

        private void red_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            DATA_ADDRESS = Constants.RED;
        }

        private void uv_radioButton_CheckedChanged(object sender, EventArgs e)
        {
            DATA_ADDRESS = Constants.UV;
        }
        #endregion

        /// <summary>
        /// 사용자 입력을 확인 합니다
        /// </summary>
        /// <param name="data_addr">LED 채널 (색깔)</param>
        /// <returns></returns>
        public bool UserInputCheck(int data_addr)
        {
            switch(data_addr)
            {
                case Constants.BLUE:
                    if (blue_brightness_text_box.Text == "")
                    {
                        MessageBox.Show("밝기를 입력하세요 (0~100)");
                        return false;
                    }
                    brightness = int.Parse(blue_brightness_text_box.Text);
                    break;
                case Constants.RED:
                    if (red_brightness_text_box.Text == "")
                    {
                        MessageBox.Show("밝기를 입력하세요 (0~100)");
                        return false;
                    }
                    brightness = int.Parse(red_brightness_text_box.Text);
                    break;
                case Constants.UV:
                    if (uv_brightness_text_box.Text == "")
                    {
                        MessageBox.Show("밝기를 입력하세요 (0~100)");
                        return false;
                    }
                    brightness = int.Parse(uv_brightness_text_box.Text);
                    break;
            }
            if(brightness < 0 || brightness > 100)
            {
                MessageBox.Show("0~100 사이의 값을 입력하세요");
                return false;
            }
            return true;
        }
    }
}
