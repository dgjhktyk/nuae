#include <Wire.h>
#include <SPI.h>
#include <stdio.h>
#include <Adafruit_Sensor.h>
#include "Adafruit_BME680.h"
#include "uart_protocol.h"

#define SEALEVELPRESSURE_HPA (1013.25)

// 센서
Adafruit_BME680 bme; // I2C

uint8_t  buf[MAX_SERIAL_BUF];
uint16_t head;
uint16_t tail;
char packet[PACKET_SIZE];

byte sensor_number = 0;
float temperature, humidity;
uint32_t pressure, gas_resistance;

void setup() {
  // PC와의 연결
  Serial.begin(115200);

  // 메인보드와의 연결
  Serial2.begin(115200, SERIAL_8N1, 19, 18);
  
  if (!bme.begin()) 
  {
    Serial.println("bme begin 실패");
    return;
  }

  bme.setTemperatureOversampling(BME680_OS_8X);
  bme.setHumidityOversampling(BME680_OS_2X);
  bme.setPressureOversampling(BME680_OS_4X);
  bme.setIIRFilterSize(BME680_FILTER_SIZE_3);
  bme.setGasHeater(320, 150); // 320*C for 150 ms
}

void loop() {

  unsigned long endTime = bme.beginReading();
  
  if (endTime == 0) {
    return;
  }

  delay(50); // This represents parallel work.

  if (!bme.endReading()) {
    return;
  }
  
  // 센서 데이터
  temperature = bme.readTemperature();
  humidity = bme.readHumidity();
  pressure = bme.readPressure() / 100.0F;
  gas_resistance = bme.gas_resistance / 1000.0;

  uint8_t* data = (uint8_t *)malloc(16);
  memcpy(data, &temperature, 4);
  memcpy(data+4, &humidity, 4);
  memcpy(data+8, &pressure, 4);
  memcpy(data+12, &gas_resistance, 4);

  transmit_data(SENSOR, data, 16);
  delay(1920);
}

void transmit_data(uint8_t cmd, uint8_t* data, uint32_t len)
{
  uint16_t crc = 0;
  uint32_t inx = 0;
  uint8_t* sendData;
  uint8_t* cmdData;
  uint16_t dataLen;
  uint16_t sendLen;
  uint16_t sendDataLen;

  dataLen = (uint16_t)(len + SIZE_CMD);
  sendLen = len + SIZE_PKT_DATA; 

  sendDataLen = sendLen;
  sendData = (uint8_t*)malloc(sendDataLen);
  memset(sendData, 0, sendLen);
  sendData[inx++] = STX;
  sendData[inx++] = (uint8_t)(dataLen);
  sendData[inx++] = (uint8_t)(dataLen >> 8);
  sendData[inx++] = (uint8_t)(dataLen);
  sendData[inx++] = (uint8_t)(dataLen >> 8);
  sendData[inx++] = cmd;

  memcpy(&sendData[inx], data, len);
  inx += len;

  cmdData = (uint8_t*)malloc(dataLen);
  memset(cmdData, 0, dataLen);
  memcpy(cmdData, sendData + IDX_CMD, dataLen);
  crc = crc16_ccitt(cmdData, dataLen);
  sendData[inx++] = (crc & 0xFF00) >> 8;
  sendData[inx++] = (crc & 0x00FF);
  
  sendData[inx++] = ETX;


  send_uart_packet(sendData, sendDataLen);

  free(cmdData);
  free(sendData);
}

void send_uart_packet(uint8_t* sendPacket, uint16_t sendDataLen) 
{
  for(int i = 0; i < sendDataLen; i++)
  {
    Serial.printf("%d\n",sendPacket[i]);
  }
  Serial2.write(sendPacket, sendDataLen);  
}

uint16_t crc16_ccitt(uint8_t *buf, int len)
{
	int counter;
	unsigned short crc = 0;
	for( counter = 0; counter < len; counter++)
		crc = (crc<<8) ^ crc16tab[((crc>>8) ^ *(uint8_t *)buf++)&0x00FF];
	return crc;
}
