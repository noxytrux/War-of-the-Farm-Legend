using UnityEngine;
using System.Collections;

public class RPGCamera : NetworkedObject {
    public GameObject RPGPlayer = null;


    private FreeMoementMotor cameraMotor;
	// Use this for initialization
	void Start () {

        InitializeNetwork();

        if (RPGPlayer == null)
            Debug.LogError("RPG Player are not signed to rpg camera");

        cameraMotor = RPGPlayer.GetComponent<FreeMoementMotor>();

        //transform.position = RPGPlayer.transform.position;
	}

    protected override void SynchronizeState()
    {
    }

    public void SetRPGCameraTransform(Vector3 vPositon, Quaternion qOrientation)
    {
        if (IsServer())
        {
            Debug.LogError("This function should not be called as server, only recived by client!");
            return;
        }

        transform.position = vPositon;
        transform.rotation = qOrientation;
    }

	// Update is called once per frame
	void Update () {

        if (IsServer())
        {
            if (RPGPlayer != null)
            {
                Vector3 camPos = RPGPlayer.transform.position - cameraMotor.facingDirection*5f;
                camPos.y += 2.5f;
                Vector3 playerPos = RPGPlayer.transform.position;
                playerPos.y = camPos.y;

                transform.position = camPos;
                transform.LookAt(playerPos);


                SendRPC("SetRPGCameraTransform", RPCMode.Others, transform.position, transform.rotation);
            }
        }
        else
        {
        }

        if (GetComponent<Camera>().enabled == false)
            return;

       

       
        
	}
}
