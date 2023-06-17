using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Nuae
{
    public partial class MainPage : Form
    {
        // Active Form
        private Form activeForm = null;

        // 외부에서 UI 접근용
        public static MainPage mainPage;

        /* 카메라 페이지와 센서 페이지는 만들어 놓은 페이지를 계속 사용 합니다 */
        public static CameraPage cameraPage;
        public static SensorInfoPage sensorPage = new SensorInfoPage();

        // 시리얼
        public static Serial serial;

        // led data timer counter
        public static int cnt = 0;
        //  통신 보낼 센서 보드 Num 
        private byte g_taget_sensor_board_num = 0;

        // 용량 제한
        public static long size_limit = 16106127360; // 15기가

        public MainPage()
        {
            InitializeComponent();
            
            serial = new Serial(); 

            mainPage = this;
            cameraPage = new CameraPage();

            SettingPage.SizeLimitFile();
            SettingPage.SaveLocationFile();

            Folders.SetCamFolderPath();
            Folders.SetDateFolderPath();
            Folders.CreateFolders();

            Camera.CreateVideoResolutionInfoTxT();
            openChildForm(new PortConnectPage());
            
            Camera.SearchCamera();
            CameraPage.cameraPage.SetVideoSourcePlayer();
            Camera.VspStart();
        }

        /// <summary>
        /// childForm을 MainPage의 contentPanel에 표시해줍니다
        /// </summary>
        /// <param name="childForm"></param>
        private void openChildForm(Form childForm)
        {
            // cameraPage, sensorPage는 처음에 만든 창을 계속 쓰기 위하여
            if(activeForm != cameraPage && activeForm != sensorPage)
            {
                if (activeForm != null) activeForm.Close();
            }
            
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            contentPanel.Controls.Add(childForm);
            contentPanel.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();
        }

        // 포트 연결 페이지
        private void portConPageButton_Click(object sender, EventArgs e)
        {
            openChildForm(new PortConnectPage());
        }

        // CMD_LED_CONTROL 제어 페이지
        private void LedPageButton_Click(object sender, EventArgs e)
        {
            openChildForm(new LedControlPage());
        }

        // 카메라 페이지
        private void cameraPageButton_Click(object sender, EventArgs e)
        {
            openChildForm(cameraPage);
        }

        // 센서 정보 페이지
        private void sensorPageButton_Click(object sender, EventArgs e)
        {
            openChildForm(sensorPage);
        }

        /// <summary>
        /// 프로그램 종료
        /// </summary>
        private void MainPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            Camera.CameraFinish();
            Camera.CloseVSP();
            Camera.CloseFiles();
        }

        /// <summary>
        /// 녹화중인 카메라의 개수를 구합니다
        /// </summary>
        /// <returns>녹화중인 카메라의 개수</returns>
        public int GetNumberOfRecordingCamera()
        {
            int ret = 0;
            foreach(bool recording in Camera.recordings)
            {
                if (recording)
                    ret++;
            }
            return ret;
        }

        /// <summary>
        /// 설정된 주기마다 호출되어
        /// 영상 파일 완성 (CloseFiles())
        /// 어떤 카메라 폴더가 설정한 용량을 넘어가면 가장 이전 날짜의 폴더 삭제 (CheckOverSized())
        /// 다시 영상 파일 생성 (OpenFiles())
        /// 다시 녹화 시작 (RecordStart())을 수행합니다
        /// </summary>
        private void saveTimer_Tick(object sender, EventArgs e)
        {
            Camera.RecordStop();
            Camera.CloseFiles();
            CheckOverSized();
            Folders.SetDateFolderPath();
            Camera.OpenFiles();
            Camera.BringingUpRecords();
        }

        /// <summary>
        /// 카메라 폴더 중에 일정 용량을 초과한 폴더를 찾고
        /// 초과한 폴더가 있다면 해당 카메라 폴더 내부에서 가장 빠른 날짜의 폴더를 삭제합니다
        /// </summary>
        public static void CheckOverSized()
        {
            List<int> overSizedCamFolders = new List<int>();
            long size;

            for(int i = 0; i < Camera.numberOfCamera; i++)
            {
                size = GetDirectorySize(Paths.camFolders[i]);
                if (size > size_limit)
                    overSizedCamFolders.Add(i);
            }
            if (overSizedCamFolders.Count == 0)
                return;

            foreach(int i in overSizedCamFolders)
            {
                GetEarliestDateFolderAndDeleteThat(Paths.camFolders[i]);
            }  
        }

        /// <summary>
        /// path 폴더의 용량을 구합니다
        /// </summary>
        /// <param name="path">폴더 경로</param>
        /// <returns></returns>
        public static long GetDirectorySize(string path)
        {
            long size = 0;
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            
            foreach (FileInfo fi in dirInfo.GetFiles("*", SearchOption.AllDirectories))
            {
                size += fi.Length;
            }
            return size;
        }

        /// <summary>
        /// path 폴더 안에서 가장 빠른 날짜의 폴더를 찾고 삭제합니다
        /// </summary>
        /// <param name="path">Cam Folder의 경로 입니다</param>
        public static void GetEarliestDateFolderAndDeleteThat(string path)
        {
            string earliest_date = "";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            DirectoryInfo[] dirs = dirInfo.GetDirectories();

            List<int> dirNames = new List<int>();
            foreach(DirectoryInfo dinfo in dirs)
            {
                dirNames.Add(int.Parse(dinfo.Name));
            }
            earliest_date = dirNames.Min().ToString();
            Directory.Delete(path + @"\" + earliest_date, true);
        }

        /// <summary>
        /// 윈도우의 메시지 입니다
        /// 여기서는 usb 분리, 결합 메시지를 확인하고 있습니다
        /// </summary>
        /// <param name="m">윈도우의 메시지</param>
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            // Listen for operating system messages.
            switch (m.Msg)
            {
                // The WM_ACTIVATEAPP message occurs when the application
                // becomes the active application or becomes inactive.
                case 0x0219:
                    // usb 분리, 결합
                    if (Camera.isCameraAdded() == 1)
                    {
                        // 추가
                        Camera.SearchCamera();
                        CameraPage.cameraPage.SetVideoSourcePlayer();
                        Camera.VspStart();
                    }
                    else if (Camera.isCameraAdded() == 0)
                    {
                        // 분리
                        int disConCamNumber = Camera.whoIsDisconnected();
                        Camera.recordings[disConCamNumber] = false;
                        Camera.CamWriters[disConCamNumber].Close();
                        CameraPage.cameraPage.Invoke((MethodInvoker)delegate ()
                        {
                            switch (disConCamNumber + 1)
                            {
                                case 1:
                                    CameraPage.cameraPage.cam1RecordStartButton.Enabled = true;
                                    CameraPage.cameraPage.cam1RecordStartButton.BackgroundImage = Properties.Resources.start_button;
                                    break;
                                case 2:
                                    CameraPage.cameraPage.cam2RecordStartButton.Enabled = true;
                                    CameraPage.cameraPage.cam2RecordStartButton.BackgroundImage = Properties.Resources.start_button;
                                    break;
                                case 3:
                                    CameraPage.cameraPage.cam3RecordStartButton.Enabled = true;
                                    CameraPage.cameraPage.cam3RecordStartButton.BackgroundImage = Properties.Resources.start_button;
                                    break;
                                case 4:
                                    CameraPage.cameraPage.cam4RecordStartButton.Enabled = true;
                                    CameraPage.cameraPage.cam4RecordStartButton.BackgroundImage = Properties.Resources.start_button;
                                    break;
                                case 5:
                                    CameraPage.cameraPage.cam5RecordStartButton.Enabled = true;
                                    CameraPage.cameraPage.cam5RecordStartButton.BackgroundImage = Properties.Resources.start_button;
                                    break;
                                case 6:
                                    CameraPage.cameraPage.cam6RecordStartButton.Enabled = true;
                                    CameraPage.cameraPage.cam6RecordStartButton.BackgroundImage = Properties.Resources.start_button;
                                    break;
                            }
                        });
                    }
                    else if (Camera.isCameraAdded() == -1)
                    {
                        // 알수 없는 오류
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// LED 제어 데이터를 보내고
        /// 반환값이 들어올 때까지 보낸 데이터를 다시 보내기 위한 타이머 입니다
        /// 처음에 보낼때 cnt는 0이고
        /// 보낸 후 이 타이머가 호출되기 전에 반환값이 제대로 들어오면 수신하는 부분에서 타이머를 종료합니다
        /// 그런데 값이 안들어 왔다면 이 타이머의 주기 (현재 3초) 안에 다시 여기를 들어와서 방금 보낸 제어 데이터를 다시 보냅니다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void led_data_timer_Tick(object sender, EventArgs e)
        {
            if(cnt == 1)
            {
                // 제대로된 반환값이나 틀린 반환값, 노이즈 이중에 아무것도 안옴
                // LED 제어부에서 틀리게 데이터를 받았고 그래서 아무것도 안보낸 때임
                // 재전송
                Serial.transmit_data(Serial.saved_led_control_data);
                cnt = 0;
            }
            cnt++;
        }

        /**
         * Sensor Data Request Timer Tick Fun
         */
        private void sensor_req_timer_tick(object sender, EventArgs e)
        {
            Console.WriteLine("pc_get_timer_Tick");
            get_sensor_data();
        }

        /**
         * Main 보드에 데이터 요청
         * Cmd - CMD_GET_SENSOR_DATA_MAINBOARD
         * param - 요청 할 디바이스 Num
         */
        public void get_sensor_data()
        {
            List<byte> packet = new List<byte>();
            packet.Add(Constants.CMD_GET_SENSOR_DATA_MAINBOARD); // cmd
            packet.Add((byte)(g_taget_sensor_board_num + 1));   // 요청할 센서 Num  *** 1 is Memory Index Numbering Offset *** 
            Serial.transmit_data(packet);

            g_taget_sensor_board_num++;    //   target sensor board num 0~SENSOR_TOTAL_COUNT 번갈아가며 데이터 요청 
            g_taget_sensor_board_num %= Constants.SENSOR_TOTAL_COUNT; //  SENSOR_TOTAL_COUNT 까지 target senosr num 증가 
            Console.WriteLine("g_taget_sensor_board_num : " + g_taget_sensor_board_num);
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            openChildForm(new SettingPage());
        }
    }
}
