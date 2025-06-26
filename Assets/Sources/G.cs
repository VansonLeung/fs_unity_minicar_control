using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using fs_minicar_control_lib_win10;

public class G : MonoBehaviour
{
    UdpCommunication udpComm;

    void Start()
    {
        int d = Class1.Add(3, 4);
        Debug.Log("3 + 4 = " + d);

        udpComm = new UdpCommunication();

        // Start listening for packets
        _ = udpComm.ListenForPacketsAsync();

        // Example of sending a packet
        // udpComm.SendPacket("A1 B2 C3");

        // udpComm.Close();
    }

    void Update()
    {

    }

    void OnApplicationQuit()
    {
        udpComm.Close();
    }    
}
