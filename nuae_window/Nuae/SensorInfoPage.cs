using System.Collections.Generic;
using System.Windows.Forms;

namespace Nuae
{
    /// <summary>
    /// 센서 정보 페이지 입니다
    /// </summary>
    public partial class SensorInfoPage : Form
    {
        // 센서정보 페이지에 있는지 없는지 확인하기 위한
        public static bool isInSensorInfoPage = false;

        // SensorInfoPage UI 접근용
        public static SensorInfoPage sensorInfoPage;

        public static List<Label> temperature = new List<Label>();
        public static List<Label> pressure = new List<Label>();
        public static List<Label> humidity = new List<Label>();
        public static List<Label> gasR = new List<Label>();
        public static List<Label> iaq = new List<Label>();
        /// <summary>
        /// 센서 정보를 표시해주는 페이지 입니다
        /// </summary>
        public SensorInfoPage()
        {
            InitializeComponent();
            sensorInfoPage = this;
            if(temperature.Count == 0)
            {
                MakeLabelList();
            }
        }

        /// <summary>
        /// 센서 정보를 표현할 라벨의 리스트를 만듭니다
        /// </summary>
        void MakeLabelList()
        {
            temperature.Add(sensor1_temperature);
            pressure.Add(sensor1_pressure);
            humidity.Add(sensor1_humidity);
            gasR.Add(sensor1_gasR);
            iaq.Add(sensor1_iaq);

            temperature.Add(sensor2_temperature);
            pressure.Add(sensor2_pressure);
            humidity.Add(sensor2_humidity);
            gasR.Add(sensor2_gasR);
            iaq.Add(sensor2_iaq);
        }

        /// <summary>
        /// 센서 정보 페이지를 벗어나면 isInSensorInfoPage를 false로 설정합니다
        /// </summary>
        private void SensorInfoPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            isInSensorInfoPage = false;
        }

        /// <summary>
        /// 센서 정보 페이지가 로드되었을때 저장되어있는 센서 데이터를 표시해 줍니다
        /// </summary>
        private void SensorInfoPage_Load(object sender, System.EventArgs e)
        {
            isInSensorInfoPage = true;

            for (int i = 0; i < temperature.Count; i++)
            {
                temperature[i].Text = Serial.sensors[i].temperature.ToString();
                pressure[i].Text = Serial.sensors[i].pressure.ToString();
                humidity[i].Text = Serial.sensors[i].humidity.ToString();
                gasR[i].Text = Serial.sensors[i].gas.ToString();
                iaq[i].Text = Serial.iaq_levels[i];
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (MainPage.mainPage.sensor_req_timer.Enabled == false)
            {
                if (Serial.serial == null || !Serial.serial.IsOpen)
                {
                    MessageBox.Show("포트가 연결이 되어있지 않습니다");
                    return;
                }
                MainPage.mainPage.sensor_req_timer.Enabled = true;
                MainPage.mainPage.get_sensor_data();
                sensor_info_button.Text = "센서 정보 요청 중단";
            }
            else
            {
                sensor_info_button.Text = "센서 정보 요청 시작";
                MainPage.mainPage.sensor_req_timer.Enabled = false;
            }
        }
    }
}
