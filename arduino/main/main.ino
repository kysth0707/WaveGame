#define PIN1 3
#define PIN2 4
#define PIN3 5
#define PIN4 6

void setup() {
    pinMode(PIN1, INPUT);
    pinMode(PIN2, INPUT);
    pinMode(PIN3, INPUT);
    pinMode(PIN4, INPUT);
    Serial.begin(9600);
}

void loop() {
    char buff[20];
    sprintf(buff, "%d %d %d %d", !digitalRead(PIN1), !digitalRead(PIN2), !digitalRead(PIN3), !digitalRead(PIN4));
    Serial.println(buff);
}
