#include <ESP8266WiFi.h>
#include <WiFiClientSecure.h> 
#include <ESP8266WebServer.h>
#include <ESP8266HTTPClient.h>
#include <SoftwareSerial.h>

SoftwareSerial Nodemcu_SoftSerial(D2,D3);

const char *ssid = "WiFi name";
const char *password = "WiFi Password";

char c;

bool isDone = false;
String endpoint = "/saveData?";
String dataIn;

const char *host = "airqualityappoj.azurewebsites.net";
const int httpsPort = 443;  //HTTPS= 443 and HTTP = 80

const char fingerprint[] PROGMEM = "0B AE B8 B3 EA 3C 84 9D 87 80 89 12 2C 2B 63 B5 07 B1 06 8A";

void setup() {
  delay(1000);
  Serial.begin(115200);
  Nodemcu_SoftSerial.begin(9600);
  WiFi.mode(WIFI_OFF);  
  delay(1000);
  WiFi.mode(WIFI_STA);      
  
  WiFi.begin(ssid, password);     
  Serial.println("");

  Serial.print("Connecting");
  
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.print("Connected to ");
  Serial.println(ssid);
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP()); 
}

void loop() {
  if(WiFi.status() != WL_CONNECTED){
    WiFi.begin(ssid, password);

    Serial.print("Connecting");

    while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  } 
  }
  while(Nodemcu_SoftSerial.available()>0)
  {
    c = Nodemcu_SoftSerial.read();
    if(c=='\n') {
      isDone = true;
      Serial.println(dataIn);
      break;
      }
    else {dataIn+=c;}  
  }

  if(isDone){ 
  WiFiClientSecure httpsClient;  

  httpsClient.setFingerprint(fingerprint);
  httpsClient.setTimeout(15000);
  delay(1000);
  
  int r=0; //retry counter
  while((!httpsClient.connect(host, httpsPort)) && (r < 30)){
      delay(100);
      Serial.print(".");
      r++;
  }
  if(r==30) {
    Serial.println("Connection failed");
  }
  else {
    Serial.println("Connected to web");
  }
  
  String route = "/saveData?" + dataIn;

  route.trim();
  
  Serial.println(host + route);

  httpsClient.print(String("GET ") + route + " HTTP/1.1\r\n" +
               "Host: " + host + "\r\n" +               
               "Connection: close\r\n\r\n");

  Serial.println(String("POST ") + route + " HTTP/1.1\r\n" +
               "Host: " + host + "\r\n" +               
               "Connection: close\r\n\r\n");

  Serial.println("request sent");

  while (httpsClient.connected()) {
      String line = httpsClient.readStringUntil('\n');
      if (line == "\r") {
        Serial.println("headers received");
        break;
      }
    }

  Serial.println("reply was:");
  Serial.println("==========");
  String line;
  while(httpsClient.available()){        
    line = httpsClient.readStringUntil('\n');  //Read Line by Line
    Serial.println(line); //Print response
  }
  Serial.println("==========");
  Serial.println("closing connection");

  isDone = false;
  dataIn = "";
  }
}
