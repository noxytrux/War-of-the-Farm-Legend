using UnityEngine;
using System.Collections;
using System.Net;

public class ClientServerGUI : MonoBehaviour {
    
    public string connectionIP = "127.0.0.1";
    public int connectionPort = 25001;

    private string currentIP = "";

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), "CurrentIP: " + currentIP);

       connectionIP = GUI.TextField(new Rect(10, 60, 100, 20), connectionIP);

        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            GUI.Label(new Rect(10, 30, 300, 20), "Status: Disconnected");
            if (GUI.Button(new Rect(10, 90, 120, 20), "Client Connect"))
            {
                Network.Connect(connectionIP, connectionPort);
                
            }
            if (GUI.Button(new Rect(10, 120, 120, 20), "Initialize Server"))
            {
                Network.InitializeServer(32, connectionPort, false);
            }
        }
        else if (Network.peerType == NetworkPeerType.Client)
        {
            GUI.Label(new Rect(10, 30, 300, 20), "Status: Connected as Client");
            if (GUI.Button(new Rect(10, 90, 120, 20), "Disconnect"))
            {
                Network.Disconnect(200);
            }
        }
        else if (Network.peerType == NetworkPeerType.Server)
        {
            GUI.Label(new Rect(10, 30, 300, 20), "Status: Connected as Server");
            if (GUI.Button(new Rect(10, 90, 120, 20), "Disconnect"))
            {
                Network.Disconnect(200); 
            }
        }
    }

	// Use this for initialization
	void Start () {
        currentIP = Dns.Resolve(Dns.GetHostName()).AddressList[0].ToString(); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
