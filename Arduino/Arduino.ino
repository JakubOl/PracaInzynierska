#include "DHT.h"
#include <SoftwareSerial.h>
#include "MQ131.h"
#include "Air_Quality_Sensor.h"
#include "PMS.h"

#define DHTPIN 2
#define DHTTYPE DHT22
DHT dht(DHTPIN, DHTTYPE);

#define tvocPin 7  // VOC sensor activation

SoftwareSerial espSerial(10, 11);
SoftwareSerial pmsSerial(8, 9);
PMS pms(pmsSerial);
PMS::DATA data;

float humidity;
float temperature;
int o3;
int tvoc;
unsigned long dataTimer3 = 0;
int pm1_0;
int pm2_5;
int pm10_0;

AirQualitySensor sensor(A1);

void setup() {
  
  pinMode(6, OUTPUT);
  pinMode(tvocPin, OUTPUT);

  pmsSerial.begin(9600);
  Serial.begin(57600);
  espSerial.begin(9600); 
  pms.passiveMode(); 
  // DHT22 INIT
  dht.begin();

  // Warming up sensors
  digitalWrite(6, HIGH);        // Ozone sensor
  digitalWrite(tvocPin, HIGH);  // TVOC sensor
  delay(20 * 1000); // delay 20 seconds
  digitalWrite(6, LOW);
  digitalWrite(tvocPin, LOW);

  //   MQ131 INIT
  digitalWrite(tvocPin, HIGH);  // TVOC sensor
  MQ131.begin(6, A0, LOW_CONCENTRATION, 1000000); //
  MQ131.setTimeToRead(20); 
  MQ131.setR0(9000);
   

  // MP503
  Serial.println("Waiting sensor to init...");
  delay(20000);

  if (sensor.init()) {
    Serial.println("MP503 Sensor ready.");
  } else {
    Serial.println("MP503 Sensor ERROR!");
  }
  delay(2000);
}
void loop()
{
  Serial.println("LOOP");
   // Read DHT22 Data
  humidity = dht.readHumidity();
  temperature = dht.readTemperature();

  MQ131.setEnv(temperature, humidity);

  // Read PM  
  pms.wakeUp();
  
  // Read MQ131 Data
  MQ131.sample();
  o3 = MQ131.getO3(PPB);
  o3 = map(o3, -32768, 32768, 10, 1000);
  
  // Read MP503
  digitalWrite(tvocPin, HIGH);
  
  int quality = sensor.slope();
  
  delay(10000); 
  
    // Read TVOC for 5 seconds 
  tvoc = analogRead(A1); 
  tvoc = map(tvoc, 0, 1024, 0, 100);
//  digitalWrite(tvocPin, LOW);
  digitalWrite(tvocPin, LOW);

  // Reading PMS data
  pms.requestRead();
  pmsSerial.listen();
  dataTimer3 = millis();
  while (millis() - dataTimer3 <= 1000) {
    pms.readUntil(data);
    pm1_0 = data.PM_AE_UG_1_0;
    pm2_5 = data.PM_AE_UG_2_5;
    pm10_0 = data.PM_AE_UG_10_0;
  }

  pms.sleep();

  String result = "temperature=" + String(temperature) + "&humidity=" + String(humidity) + "&ozone=" + String(o3) + "&PM_1_0=" + String(pm1_0) + "&PM_2_5=" + String(pm2_5) + "&PM_10_0=" + String(pm10_0) + "&tvoc=" + String(tvoc);
  Serial.println(result);
  espSerial.println(result);


  Serial.println("LOOP END");
  delay(300000);
}
