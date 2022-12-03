#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>

#include "EMGFilters.h"

#define TIMING_DEBUG 1

#define SensorInputPin A0 

EMGFilters myFilter;
SAMPLE_FREQUENCY sampleRate = SAMPLE_FREQ_1000HZ;
NOTCH_FREQUENCY humFreq = NOTCH_FREQ_50HZ;

static int Threshold = 4000;

unsigned long timeStamp;
unsigned long timeBudget;



#ifndef STASSID
#define STASSID "KT_GiGA_2G_Wave2_17CB"
#define STAPSK  "83jd12zj88"
#endif

const char* ssid     = STASSID;
const char* password = STAPSK;

const char* host = "172.30.1.42";
const uint16_t port = 4129;

ESP8266WiFiMulti WiFiMulti;
WiFiClient client;
void setup() {
  // setup for time cost measure
  // using micros()
  myFilter.init(sampleRate, humFreq, true, true, true);
  Serial.begin(115200);
  timeBudget = 1e6 / sampleRate;

  // We start by connecting to a WiFi network
  WiFi.mode(WIFI_STA);
  WiFiMulti.addAP(ssid, password);

  Serial.println();
  Serial.println();
  Serial.print("Wait for WiFi... ");

  while (WiFiMulti.run() != WL_CONNECTED) {
    Serial.print(".");
    delay(500);
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());

  while (!client.connect(host, port)) {
      Serial.println("connection failed");
      Serial.println("wait 5 sec...");
      delay(5000);
    }
  setting_threashold();

  delay(500);
}

void setting_threashold(){
  for(int i =0; i<1000; i++){
    Threshold = max(Threshold, getData()); 
  }
}


void loop() {
  int sensor = getData();
  if(sensor != 0){
    client.println("activate");
    while(getData() != 0);
  }
}

int getData(){
    /*------------start here-------------------*/
    timeStamp = micros();

    int Value = analogRead(SensorInputPin);

    // filter processing
    int DataAfterFilter = myFilter.update(Value);

    int envlope = sq(DataAfterFilter);
    // any value under threshold will be set to zero
    envlope = (envlope > Threshold) ? envlope : 0;

    timeStamp = micros() - timeStamp;
    if (TIMING_DEBUG) {
        // Serial.print("Read Data: "); Serial.println(Value);
        // Serial.print("Filtered Data: ");Serial.println(DataAfterFilter);
        Serial.println(envlope);
        if(envlope > 1000){
          return envlope;
        }
        // Serial.print("Filters cost time: "); Serial.println(timeStamp);
        // the filter cost average around 520 us
    }

    /*------------end here---------------------*/
    // if less than timeBudget, then you still have (timeBudget - timeStamp) to
    // do your work
    delayMicroseconds(500);
    return 0;
}

