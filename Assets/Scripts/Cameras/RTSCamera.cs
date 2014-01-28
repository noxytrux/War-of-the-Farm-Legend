using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour {
    public float fCameraMoveSpeed = 7.0f;

    public float boundary = 50;
    private Rect bottomBorder;
    private Rect topBorder;
    private Rect leftBorder;
    private Rect rightBorder;

	// Use this for initialization
	void Start () {
        bottomBorder = new Rect(0, 0, Screen.width, boundary);
        topBorder = new Rect(0, Screen.height - boundary, Screen.width, boundary);
        leftBorder = new Rect(0, 0, boundary,Screen.height);
        rightBorder = new Rect(Screen.width - boundary, 0, boundary,Screen.height );
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 vDir = Vector2.zero;

        Vector3 vNewPos = transform.position;
        
        vDir.x = Input.GetAxis("Horizontal");
        if (vDir.x != 0.0f)
            vDir.x = Mathf.Sign(vDir.x);

        vDir.y = -Input.GetAxis("Vertical");
        if (vDir.y != 0.0f)
            vDir.y = Mathf.Sign(vDir.y);


        if (topBorder.Contains(Input.mousePosition))
        {
            vDir.y = -1.0f;
        }

        if (bottomBorder.Contains(Input.mousePosition))
        {
            vDir.y = 1.0f;
        }

        if (leftBorder.Contains(Input.mousePosition))
        {
            vDir.x = -1.0f;
        }

        if (rightBorder.Contains(Input.mousePosition))
        {
            vDir.x = 1.0f;
        }


     
        if (vDir.x != 0.0f && vDir.y != 0.0f)
        {
            if (Mathf.Sign(vDir.x) == Mathf.Sign(vDir.y))
            {
                vDir /= 2.0f; // 1/1 = 0.5/0.5
            }
            else
            {

               // vDir /= 2.0f; // 1/-1 = 0.5/-0.5
                // hacki zeby to dobrze wygladalo z perspektywy kamery - predkosc
            }
        }
        else
        {
            //1/0
        }
        //Debug.Log(vDir); 
        
        vNewPos += transform.right * fCameraMoveSpeed * Time.deltaTime * vDir.x;
        vNewPos += transform.TransformDirection(transform.forward) * fCameraMoveSpeed * Time.deltaTime * vDir.y;

        transform.position = vNewPos;
	}
}
