using System;

namespace Nuae
{
    /// <summary>
    /// 센서 정보를 담을 클래스 입니다
    /// </summary>
    public class Sensor
    {
        public byte sensor_id;
        public float temperature;
        public float humidity;
        public UInt32 pressure;
        public UInt32 gas;
        public string iaq;

        public Sensor()
        {
            sensor_id = 0;
            temperature = 0;
            humidity = 0;
            pressure = 0;
            gas = 0;
            iaq = "-";
        }
    }
}
