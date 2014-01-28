using UnityEngine;
using System.Collections;

public class NetworkedObject : MonoBehaviour {

    RPCFunctions NetworkSendingObject = null;
    private NetworkView NetworkSend = null;

    public bool IsServer()
    {

        //zakomentowac Network.connections.Length == 0 gdy nie chcemy grac bez sieci
        return Network.isServer || Network.connections.Length == 0 ;
    }

    public bool IsClient()
    {
        //zakomentowac Network.connections.Length == 0 gdy nie chcemy grac bez sieci
        return Network.isClient;
    }

    protected virtual void SynchronizeState()
    {
        Debug.LogError("Synchronize state is not implemented in " + name);
    }

    protected void SendRPC(string name, RPCMode mode, params object[] p)
    {
        if (Network.peerType != NetworkPeerType.Disconnected )
            NetworkSend.RPC(name, mode, p);
    }

	// Use this for initialization
    protected void InitializeNetwork()
    {
        if (NetworkSendingObject == null)
        {
            GameObject rpcGameObj = GameObject.Find("RPCFunctionsObject");
            if (rpcGameObj == null)
            {
                Debug.Log("Cant find RPCFunctionsObject in NetworkCubeMoving.cs");
            }
            else
            {
                NetworkSendingObject = rpcGameObj.GetComponent<RPCFunctions>();
                NetworkSend = NetworkSendingObject.GetComponent<NetworkView>();

                SynchronizeState();
            }
        }
	}
	
}
