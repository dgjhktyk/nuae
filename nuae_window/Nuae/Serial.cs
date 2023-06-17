using GodSharp.SerialPort;
using System;
using System.Collections.Generic;
using System.Drawing;

using System.Windows.Forms;

namespace Nuae
{
    public class Serial
    {
        public static GodSerialPort serial;

        #region 센서 데이터 관련
        // 제어 데이터를 보낸 후 그 데이터를 저장해놓기 위한 변수
        public static List<byte> saved_led_control_data = new List<byte>();

        // 제어 데이터를 담을 변수
        public static List<byte> packet = new List<byte>();

        // 센서 정보 담을 변수들
        static float hum_score, gas_score;
        static float hum_reference = 40;
        static float gas_reference = 250000;

        // 센서 정보를 바탕으로 판별되는 실내 공기질 등급, 점수
        public static string iaq_level;
        public static float iaq_score;

        // 센서 정보를 받기 위한 변수
        public static Sensor sensor = new Sensor();

        // 받은 센서 데이터를 합치기 위한 센서 리스트
        public static List<Sensor> sensors = new List<Sensor>(2) { new Sensor(), new Sensor() };

        // iaq(실내 공기질) 저장용 리스트
        public static List<string> iaq_levels = new List<string>(2) { "0", "0" };

        // 센서 번호를 담을 변수
        static int sensor_idx = 0;
        #endregion

        /// <summary>
        /// 데이터를 보냅니다
        /// </summary>
        /// <param name="bytes">데이터</param>
        public static void SendData(byte[] bytes)
        {
            if(serial != null)
            {
                serial.Write(bytes);
            }
        }

        /// <summary>
        /// 시리얼 포트를 설정합니다
        /// </summary>
        /// <param name="port"></param>
        public static void SetSerialPort(string port)
        {
            serial = new GodSerialPort(port, 115200, 0);
        }

        /// <summary>
        /// 시리얼 포트를 오픈 합니다
        /// </summary>
        /// <returns></returns>
        public static bool Open()
        {
            serial.UseDataReceived(true, (sp, bytes) =>
            {
                DataReceived(bytes);
            });
            if (!serial.Open())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 데이터가 들어오는 부분 입니다
        /// </summary>
        /// <param name="bytes"></param>
        public static void DataReceived(byte[] bytes)
        {
            if (bytes == null)
                return;
            bool recv_end = false;
            byte recv_step = 0;
            ushort data_chk = 0;
            ushort data_chk2 = 0;
            ushort dataLen = 0;
            ushort dataLen2 = 0;
            ushort dataLenTmp = 0;
            ushort crc_chk = 0;
            ushort crc;

            ushort serRxLen = (ushort)bytes.Length;

            if (serRxLen > 0)
            {
                for (int i = 0; i < serRxLen; i++)
                {
                    if (recv_step == 0)
                    {
                        // STX
                        if (bytes[i] == Constants.STX)
                        {
                            recv_step = 1;
                            dataLen = 0;
                            data_chk = 0;
                            continue;
                        }
                    }
                    else if (recv_step == 1)
                    {
                        if (data_chk == 0)
                        {
                            // data 길이 하위 1바이트
                            dataLen = bytes[i];
                            ++data_chk;
                            continue;
                        }
                        else
                        {
                            // data 길이 상위 1바이트
                            dataLen = (ushort)(dataLen | (bytes[i] << 8));
                            dataLenTmp = dataLen;
                            packet.Clear();
                            data_chk = 0;
                            recv_step = 2;
                            continue;
                        }
                    }
                    else if (recv_step == 2)
                    {
                        if (data_chk2 == 0)
                        {
                            // data 길이 하위 1바이트
                            dataLen2 = bytes[i];
                            ++data_chk2;
                            continue;
                        }
                        else
                        {
                            // data 길이 상위 1바이트
                            dataLen2 = (ushort)(dataLen2 | (bytes[i] << 8));
                            data_chk2 = 0;
                            if (dataLen != dataLen2)
                            {
                                recv_step = 0;
                                dataLen2 = 0;
                                dataLen = 0;
                            }
                            else
                            {
                                recv_step = 3;
                            }
                            continue;
                        }
                    }
                    else if (recv_step == 3)
                    {
                        // data
                        packet.Add(bytes[i]);
                        --dataLenTmp;
                        if (dataLenTmp == 0)
                        {
                            recv_step = 4;
                            continue;
                        }
                    }
                    else if (recv_step == 4)
                    {
                        // crc
                        packet.Add(bytes[i]);
                        ++crc_chk;
                        if (crc_chk >= 2)
                        {
                            crc_chk = 0;
                            recv_step = 5;
                            continue;
                        }
                    }
                    else if (recv_step == 5)
                    {
                        if (bytes[i] == Constants.ETX)
                        {
                            recv_end = true;
                        }
                        else
                        {
                            dataLen = 0;
                            dataLenTmp = 0;
                        }
                        recv_step = 0;
                    }
                }
                if (recv_end == true)
                {
                    crc = crc16_ccitt(packet);
                    if (crc == 0) // crc ok
                    {
                        byte cmd = packet[0];
                        packet.RemoveAt(0);
                        packet.RemoveAt(packet.Count-1); // crc 제거
                        packet.RemoveAt(packet.Count-1); // crc 제거
                        cmd_process(cmd, packet);
                    }
                    else
                    {
                        // NACK
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 프런티어의 프로토콜에 따라 cmd별 데이터를 처리하는 부분 입니다.
        /// </summary>
        /// <param name="cmd">cmd</param>
        /// <param name="data"></param>
        public static void cmd_process(byte cmd, List<byte> data)
        {
            switch (cmd)
            {
                case Constants.CMD_GET_SENSOR_DATA_MAINBOARD:
                    sensor.sensor_id = data[0];
                    sensor.temperature = BitConverter.ToSingle(data.GetRange(1, 4).ToArray() ,0);
                    sensor.humidity = BitConverter.ToSingle(data.GetRange(5, 4).ToArray(), 0);
                    sensor.pressure = BitConverter.ToUInt32(data.GetRange(9, 4).ToArray(), 0);
                    sensor.gas = BitConverter.ToUInt32(data.GetRange(13, 4).ToArray(), 0);

                    iaq_score = GetIAQScore();

                    iaq_level = DetermineIAQLevel(iaq_score);

                    sensor_idx = sensor.sensor_id - 1;
 
                    sensors[sensor_idx].sensor_id = sensor.sensor_id;
                    sensors[sensor_idx].temperature = sensor.temperature;
                    sensors[sensor_idx].pressure = sensor.pressure;
                    sensors[sensor_idx].humidity = sensor.humidity;
                    sensors[sensor_idx].gas = sensor.gas;

                    iaq_levels[sensor_idx] = iaq_level;

                    // 현재 센서정보 페이지 안에 있다면 들어오는 데이터를 표시해 줍니다
                    if (SensorInfoPage.isInSensorInfoPage)
                    {
                        SensorInfoPage.sensorInfoPage.Invoke((MethodInvoker)delegate ()
                        {
                            SensorInfoPage.temperature[sensor_idx].Text = sensor.temperature + " ℃";
                            SensorInfoPage.humidity[sensor_idx].Text = sensor.humidity + " %";
                            SensorInfoPage.pressure[sensor_idx].Text = sensor.pressure + " hPa";
                            SensorInfoPage.gasR[sensor_idx].Text = sensor.gas + " kOhms";
                            SensorInfoPage.iaq[sensor_idx].Text = iaq_level;
                            SensorInfoPage.iaq[sensor_idx].ForeColor = DetermineIAQLabelColor(iaq_level);

                        });
                    }
                    break;
                case Constants.CMD_LED_CONTROL:
                    if(MainPage.mainPage.led_data_timer.Enabled == true)
                    {
                        // 메인보드에서 LED 제어부로부터 제대로된 반환값을 받고 그거를 pc로 보내준것
                        // 별다른 작업 안해줘도 됨, 보낼때 실행시킨 타이머만 종료시킴
                        MainPage.mainPage.led_data_timer.Enabled = false;
                        MainPage.cnt = 0;
                    }
                    break;
            }
        }

        public static ushort crc16_ccitt(List<byte> packet)
        {
            ushort crc = 0;
            foreach (byte b in packet)
            {
                crc = (ushort)((crc << 8) ^ Constants.crc16tab[((crc >> 8) ^ b) & 0x00FF]);
            }
            return crc;
        }

        /// <summary>
        /// 센서 데이터를 바탕으로 IAQ (실내 공기질) 점수를 만듭니다
        /// </summary>
        /// <returns></returns>
        static float GetIAQScore()
        {
            float current_humidity = sensor.humidity;
            if (current_humidity >= 38 && current_humidity <= 42)
                hum_score = (float)0.25 * 100; // Humidity +/-5% around optimum 
            else
            { 
                //sub-optimal
                if (current_humidity < 38)
                    hum_score = (float)0.25 / hum_reference * current_humidity * 100;
                else
                {
                    hum_score = (float)((-0.25 / (100 - hum_reference) * current_humidity) + 0.416666) * 100;
                }
            }

            //Calculate gas contribution to IAQ index
            float gas_lower_limit = 5000;   // Bad air quality limit
            float gas_upper_limit = 50000;  // Good air quality limit 
            if (gas_reference > gas_upper_limit) gas_reference = gas_upper_limit;
            if (gas_reference < gas_lower_limit) gas_reference = gas_lower_limit;
            gas_score = (float)(0.75 / (gas_upper_limit - gas_lower_limit) * gas_reference - (gas_lower_limit * (0.75 / (gas_upper_limit - gas_lower_limit)))) * 100;

            //Combine results for the final IAQ index value (0-100% where 100% is good quality air)
            float air_quality_score = hum_score + gas_score;
            return air_quality_score;
        }

        /// <summary>
        /// IAQ (실내 공기질) 점수를 바탕으로 IAQ (실내 공기질) 레벨을 결정합니다
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        static String DetermineIAQLevel(float score)
        {
            String IAQ_text = "";
            score = (100 - score) * 5;
            if (score >= 301) IAQ_text += "Extremely polluted";
            else if (score >= 251 && score <= 300) IAQ_text += "Severely polluted";
            else if (score >= 201 && score <= 250) IAQ_text += "Heavily polluted";
            else if (score >= 151 && score <= 200) IAQ_text += "Moderately polluted";
            else if (score >= 101 && score <= 150) IAQ_text += "Lightly polluted";
            else if (score >= 51 && score <= 100) IAQ_text += "Good";
            else if (score <= 50) IAQ_text += "Excellent";
            return IAQ_text;
        }
        /// <summary>
        /// IAQ (실내 공기질) 레벨에 따라 색깔을 결정하고 반환합니다
        /// </summary>
        /// <param name="IAQ_level">IAQ (실내 공기질) 레벨</param>
        /// <returns>
        /// color : 센서정보 페이지에서 실내 공기질로 표시되는 텍스트의 색깔 입니다
        /// </returns>
        static Color DetermineIAQLabelColor(string IAQ_level)
        {
            Color color = new Color();
            if(IAQ_level == "Excellent")
            {
                color = Color.FromArgb(1,228,0);
            }
            else if(IAQ_level == "Good")
            {
                color = Color.FromArgb(146, 209, 79);
            }
            else if (IAQ_level == "Lightly polluted")
            {
                color = Color.FromArgb(255, 255, 1);
            }
            else if (IAQ_level == "Moderately polluted")
            {
                color = Color.FromArgb(255, 126, 0);
            }
            else if (IAQ_level == "Heavily polluted")
            {
                color = Color.FromArgb(254, 0, 0);
            }
            else if (IAQ_level == "Severely polluted")
            {
                color = Color.FromArgb(152, 0, 75);
            }
            else if (IAQ_level == "Extremely polluted")
            {
                color = Color.FromArgb(102, 51, 0);
            }
            return color;
        }

        public static void transmit_data(List<byte> data)
        {
            ushort dataLen = (ushort)data.Count;

            List<byte> packet = new List<byte>();

            packet.Add(Constants.STX);                            
            packet.Add((byte)(dataLen));                   
            packet.Add((byte)(dataLen >> 8));              
            packet.Add((byte)(dataLen));                   
            packet.Add((byte)(dataLen >> 8));              

            packet.AddRange(data);

            ushort crc = crc16_ccitt(data);

            packet.Add((byte)(crc >> 8));                     
            packet.Add((byte)(crc & 0xFF));                   
            packet.Add(Constants.ETX);                           

            // 전송
            SendData(packet.ToArray());
        }
    }
}

