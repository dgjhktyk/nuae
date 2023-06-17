#include <SoftwareSerial.h>
#include "uart_protocol.h"

uint8_t buf[MAX_SERIAL_BUF];
uint8_t packet[PACKET_SIZE];
uint16_t head = 0;
uint16_t tail = 0;
int len = 0;

int StrToHex(char str[]);

// LED 제어함과의 연결 (RS-485)
SoftwareSerial RS485;

void setup() {
  // PC와의 연결
  Serial.begin(115200);
  
  // 센서보드1과의 연결
  Serial1.begin(115200, SERIAL_8N1, 33, 32);

  // 센서보드2과의 연결
  Serial2.begin(115200, SERIAL_8N1, 19, 18);

  // RS-485
  RS485.begin(9600, SWSERIAL_8N1, 14, 25);
}

void loop() {
  if(Serial.available() > 0)
  {
    process(0);
  }
  
  if(Serial1.available() > 0)
  {
    process(1);
  }
  
  if(Serial2.available() > 0)
  {
    process(2);
  }

  if(RS485.available() > 0)
  {
    buf[tail] = RS485.read();
    // 0x0A : LED 통신 프로토콜의 맨 마지막 값
    if(buf[tail++] == 0x0A)
    {
      led_data_process();
    }
  }
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

void transmit_data(uint8_t cmd)
{
  uint16_t crc = 0;
  uint32_t inx = 0;
  uint8_t* sendData;
  uint8_t* cmdData;
  uint16_t dataLen;
  uint16_t sendLen;
  uint16_t sendDataLen;

  dataLen = SIZE_CMD;
  sendLen = SIZE_PKT_DATA; 

  sendDataLen = sendLen;
  sendData = (uint8_t*)malloc(sendDataLen);
  memset(sendData, 0, sendLen);
  sendData[inx++] = STX;
  sendData[inx++] = (uint8_t)(dataLen);
  sendData[inx++] = (uint8_t)(dataLen >> 8);
  sendData[inx++] = (uint8_t)(dataLen);
  sendData[inx++] = (uint8_t)(dataLen >> 8);
  sendData[inx++] = cmd;

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

void send_uart_packet(uint8_t* sendPacket, uint16_t sendDataLen) {
  Serial.write(sendPacket, sendDataLen); 
}

/*
센서 데이터 확인

uint8_t sensor_number : 센서 번호
*/
void process(uint8_t sensor_number)
{ 
  bool recv_end = false;
  uint8_t cmd;
  uint8_t recv_step  = 0;
  uint16_t data_chk = 0;
  uint16_t data_chk2 = 0;
  uint16_t inx = 0;
  uint16_t dataLen = 0;
  uint16_t dataLen2 = 0;
  uint16_t dataLenTmp = 0;
  uint16_t crc_chk = 0;
  uint16_t crc = 0;
  
  if(recv_callback(sensor_number) == true)
  {
    uint16_t serHead = 0;
    uint16_t serTail = 0;
    uint16_t serRxLen = 0;
    serHead = head;
    serTail = tail;  
    if (serHead != serTail) 
    {
      if (serHead <= serTail)          
        serRxLen = serTail - serHead;
      else
        serRxLen = serTail + MAX_SERIAL_BUF - serHead;      

      if (serRxLen)
      {
        for (int i=0; i<serRxLen; i++) 
        {
          if (recv_step == 0) {
            if (inx == 0 && buf[head+i] == STX) { 
              recv_step = 1;
              dataLen = 0;
              data_chk = 0;
              inx = 0;
              continue;
            }
          }
          else if (recv_step == 1) {
            if (data_chk == 0) 
            {
              dataLen = buf[head+i];
              ++data_chk;
              continue;
            }
            else {
              dataLen = dataLen | (buf[head+i] << 8); 
              dataLenTmp = dataLen;
              memset(packet, 0, sizeof(packet));
              data_chk = 0;
              recv_step = 2;
              continue;
            }
            
          }
          else if (recv_step == 2) {      
          if (data_chk2 == 0) 
          {
            dataLen2 = buf[head+i]; 
            ++data_chk2;
            continue;
          }
          else {
            dataLen2 = dataLen2 | (buf[head+i] << 8);
            data_chk2 = 0;
            if (dataLen != dataLen2) { 
              recv_step = 0;
              dataLen2 = 0;
              dataLen = 0;
            }
            else{
              recv_step = 3;
            } 
            continue;
          }
         }
          else if (recv_step == 3) {
            packet[inx++] = buf[head+i];  
            --dataLenTmp;                 
            if (dataLenTmp == 0) {
              recv_step = 4;
              continue;
            }
          }
          else if (recv_step == 4) {
            packet[inx++] = buf[head+i];     
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
            if (buf[head+i] == ETX) {        
              recv_end = true;
            }
            else {
              dataLen = 0;
              dataLenTmp = 0;
            }
            recv_step = 0;
            inx = 0;
          }
        }
        if (recv_end == true) 
        {
          // dataLen = 데이터 길이 + 1 (cmd)
          crc = crc16_ccitt(&packet[0], dataLen + 2);
          if (crc == 0) // Crc OK
          {
            cmd = packet[0];
            uint8_t* data = (uint8_t *)malloc(dataLen); // data에는 센서번호 + 데이터가 들어갈 것입니다
            memcpy(data, packet, dataLen);
            if(cmd == SENSOR)
            {
              data[0] = sensor_number; // 센서번호 추가
              cmd_process(cmd, data, dataLen);
            }
            else if(cmd == LED)
            {
              cmd_process(cmd, &data[1], dataLen-1);
            }
          }
          else {
            //send NACK
            buf[0] = NCK;
          }
          dataLen = 0;
          inx = 0;
        }
      }
    }
    head = 0;
    tail = 0;
  } 
}

/*
process(uint8_t sensor_number)에서 확인된 데이터를
cmd에 따라 처리 합니다

cmd : SENSOR, LED
data : 데이터
len : 데이터의 길이
*/
void cmd_process(uint8_t cmd, uint8_t* data, uint16_t len)
{
  switch (cmd) {
    case SENSOR:
      transmit_data(SENSOR, data, len);
      break;
    case LED:
      RS485.write(data, len);
      break;
  }
}

/*
해당 센서 번호의 센서 보드로부터 데이터를 모두 받았는지 확인합니다

uint8_t sensor_number : 센서 번호
*/
bool recv_callback(uint8_t sensor_number)
{
  //Serial.println("recv_callback");
  bool ret = false;
  reicve_uart_packet(&ret, sensor_number);
  return ret;
}

/*
PC, 1번 센서, 2번 센서로부터 데이터를 받았다면
ret을 true로 설정 합니다
*/
void reicve_uart_packet(bool * ret, uint8_t sensor_number) 
{ 
  while (1)
  {
    if(sensor_number == 0) // PC
    {
      if(Serial.available() > 0)
      {
        buf[tail] = Serial.read();
        tail++;
        *ret = true;       
      }
      else
      {
        return;
      }
    }
    else if(sensor_number == 1) // 1번 센서
    {
      if (Serial1.available() > 0)
      {
        buf[tail] = Serial1.read();
        tail++;
        *ret = true;
      }   
      else
      {
        return;
      }
    }
    else if(sensor_number == 2) // 2번 센서
    {
      if (Serial2.available() > 0)
      {
        buf[tail] = Serial2.read();
        tail++;
        *ret = true;
      }
      else
      {
        return;
      }
    }
  }
}
uint16_t crc16_ccitt(uint8_t *buf, int len)
{
	int counter;
	unsigned short crc = 0;
	for( counter = 0; counter < len; counter++)
    //crc = (crc<<8) ^ crc16tab[((crc>>8) ^ buf[counter]) &0x00FF];
		crc = (crc<<8) ^ crc16tab[((crc>>8) ^ *(uint8_t *)buf++)&0x00FF];
	return crc;
}

/* 
 제어기로부터 받는 반환 값을 확인하는 부분 입니다
*/
void led_data_process()
{
  int len = tail;

  char ID[3] = {'0','0','\0'};
  char COMMAND[3] = {'0','0','\0'};
  char DATA_ADDRESS_FRONT[3] = {'0','0','\0'};
  char DATA_ADDRESS_BACK[3] = {'0','0','\0'};
  char DATA_FRONT[3] = {'0','0','\0'};
  char DATA_BACK[3] = {'0','0','\0'};
  char LRC_CHK[3] = {'0','0','\0'};
  short end = 0;
  int recv_step = 0;

  for(int i = 0; i < len; i++)
  {
    // STX, ID, COMMAND, DATA ADDRESS, DATA
    if(recv_step == 0)
    {
      if(buf[i] == STX_LED)
      {
        recv_step = 1;
      }
    }
    else if(recv_step == 1)
    {
      ID[i-1] = (char)buf[i];
      if(i == 2)
      {
        recv_step = 2;
      }
    }
    else if(recv_step == 2)
    {
      COMMAND[i-3] = (char)buf[i];
      if(i == 4)
      {
        recv_step = 3;
      }
    }
    else if(recv_step == 3)
    {
      DATA_ADDRESS_FRONT[i-5] = (char)buf[i];
      if(i == 6)
      {
        recv_step = 4;
      }
    }
    else if(recv_step == 4)
    {
      DATA_ADDRESS_BACK[i-7] = (char)buf[i];
      if(i == 8)
      {
        recv_step = 5;
      }
    }
    else if(recv_step == 5)
    {
      DATA_FRONT[i-9] = (char)buf[i];
      if(i == 10)
      {
        recv_step = 6;
      }
    }
    else if(recv_step == 6)
    {
      DATA_BACK[i-11] = (char)buf[i];
      if(i == 12)
      {
        recv_step = 7;
      }
    }
    else if(recv_step == 7)
    {
      LRC_CHK[i-13] = (char)buf[i];
      if(i == 14)
      {
        recv_step = 8;
      }
    }
    else if(recv_step == 8)
    {
      if(i == 15)
      {
        end = buf[i] << 8;
      }
      else if(i == 16)
      {
        end |= buf[i];
      }
    }
  }

    // END가 발견 안됨
    if(end != END)
    {
      head = 0;
      tail = 0;
      return;
    }

    int idh, cmdh, data_addr_f_h, data_addr_b_h, data_f_h, data_b_h, sum, lrc_chk = 0;

    // 16진수법으로 표기된 문자열을 정수로 변환합니다
    idh = StrToHex(ID);
    cmdh = StrToHex(COMMAND);
    data_addr_f_h = StrToHex(DATA_ADDRESS_FRONT);
    data_addr_b_h = StrToHex(DATA_ADDRESS_BACK);
    data_f_h = StrToHex(DATA_FRONT);
    data_b_h = StrToHex(DATA_BACK);

    sum = idh + cmdh + data_addr_f_h + data_addr_b_h + data_f_h + data_b_h;
    lrc_chk = (sum ^ 0xFF) + 1;

    int LRCCHK_IN_PACKET = StrToHex(LRC_CHK);
    
    // 체크섬 실패
    if(LRCCHK_IN_PACKET != lrc_chk)
    {
      head = 0;
      tail = 0;
      return;
    }

    // pc로 전송
    transmit_data(LED, buf, len);

    head = 0;
    tail = 0;
}

/*
 StrToHex(char str[])
 16진수법으로 표기된 문자열을 정수로 변환합니다
*/
int StrToHex(char str[])
{
  return (int) strtol(str, 0, 16);
}
