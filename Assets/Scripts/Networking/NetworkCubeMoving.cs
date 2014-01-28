using UnityEngine;
using System.Collections;

public class NetworkCubeMoving : NetworkedObject {
	// Use this for initialization
    void Start()
    {
        InitializeNetwork();
    }

    protected override void SynchronizeState()
    {
        SendRPC("SetPosition", RPCMode.Others, transform.gameObject.name, transform.position);
    }

	// Update is called once per frame
	void Update ()
    {
        if (IsServer())
        {
            Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            float speed = 5;
            transform.Translate(speed * moveDir * Time.deltaTime);

            SendRPC("SetPosition", RPCMode.Others, transform.gameObject.name, transform.position);
        }
	}
   
}
