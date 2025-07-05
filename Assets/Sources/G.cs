using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using fs_minicar_control_lib_win10;

public class G : MonoBehaviour
{
    UdpCommunication udpComm;

    public Slider uiInputX;
    public Slider uiInputY;


    string connectedAddress = null;


    // Rate limiting variables
    private float timeSinceLastSend = 0f;
    private const float sendInterval = 1f / 30f; // 30 FPS


    void Start()
    {
        int d = Class1.Add(3, 4);
        Debug.Log("3 + 4 = " + d);

        udpComm = new UdpCommunication();
        udpComm.OnPrintLog += message => Debug.Log(message);
        udpComm.OnReceiveCommandPacket += (packet) =>
        {
            // Log the received command packet
            Debug.Log($"Just Received command packet: {packet}");

            if (packet.Command.Equals("DISCOVERY"))
            {
                Debug.Log($"DISCOVERY Received");
                connectedAddress = packet.Address;
            }
            else if (packet.Command.Equals("ACK"))
            {
                Debug.Log($"ACK Received");
            }
            else
            {
                Debug.Log($"packet.Command: {packet.Command} DISCOVERY");
                Debug.Log($"packet.Command  {packet.Command.Equals("DISCOVERY")}");
            }
        };

        // Start listening for packets
        _ = udpComm.ListenForPacketsAsync();

        // Example of sending a packet
        // udpComm.SendPacket("A1 B2 C3");

        // udpComm.Close();
    }

    void Update()
    {
        timeSinceLastSend += Time.deltaTime; // Accumulate time since last send

        if (connectedAddress != null && timeSinceLastSend >= sendInterval)
        {
            // Reset the timer
            timeSinceLastSend = 0f;

            int inputX = (int)uiInputX.value;
            int inputY = (int)uiInputY.value;
            byte[] responseData = {
                (byte)inputX, // X input
                (byte)inputY  // Y input
            };

            udpComm.SendPacket(
                connectedAddress,
                responseData
            );
        }
    }


    void OnApplicationQuit()
    {
        udpComm.Close();
    }
}
