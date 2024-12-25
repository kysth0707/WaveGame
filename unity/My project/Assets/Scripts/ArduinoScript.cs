using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoScript : MonoBehaviour
{
    public SerialPort MyArduino;
    [SerializeField] string portName = "COM15";
    [SerializeField] WaveScript WS;

    int PL1Current = 0;
    int PL2Current = 0;
    int PL1Last = -99;
    int PL2Last = -99;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            MyArduino = new SerialPort(portName, 9600);
            MyArduino.ReadTimeout = 200;
            MyArduino.Open();
        }
        catch
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            string receivedData = MyArduino.ReadExisting();
            if (receivedData.Length >= 4)
            {
                string[] splitData = receivedData.Split('\n')[1].Split(' ');

                if (splitData[0][0] == '1')
                {
                    PL1Current = 1;
                }
                else if (splitData[1][0] == '1')
                {
                    PL1Current = -1;
                }
                else
                {
                    PL1Current = 0;
                }


                if (splitData[2][0] == '1')
                {
                    PL2Current = -1;
                }
                else if (splitData[3][0] == '1')
                {
                    PL2Current = 1;
                }
                else
                {
                    PL2Current = 0;
                }
            }
        }
        catch
        {
            
        }

        if (PL1Current != PL1Last)
        {
            if (PL1Current == 1)
            {
                WS.addP1WaveUp();
            }
            else if (PL1Current == -1)
            {
                WS.addP1WaveDown();
            }
            PL1Last = PL1Current;
        }

        if (PL2Current != PL2Last)
        {
            if (PL2Current == 1)
            {
                WS.addP2WaveUp();
            }
            else if (PL2Current == -1)
            {
                WS.addP2WaveDown();
            }
            PL2Last = PL2Current;
        }
    }
}
