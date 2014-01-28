using UnityEngine;
using System.Collections;

public class HealtBarScript : MonoBehaviour {

    [HideInInspector]
	public float hp;
     [HideInInspector]
	private float maxhp;
     [HideInInspector]
	public float healtBarMaxSize;
     [HideInInspector]
	private int healtBarWidth;
	public GameObject myHealtBar;
     [HideInInspector]
	private GameObject myhb;
     [HideInInspector]
	public float offsetY = 0.0f;

    private Entity ParentEntityObject = null;

	// Use this for initialization
	void Start () {
		healtBarWidth = (int)healtBarMaxSize;
		myhb = (GameObject)Instantiate(myHealtBar,transform.position,transform.rotation) as GameObject;


        ParentEntityObject = GetComponent<Entity>();

        maxhp = ParentEntityObject.GetHealth();
	}

    
	// Update is called once per frame
	public void Update () {

        if (ParentEntityObject != null)
        {
            hp = ParentEntityObject.GetHealth();
        }

        if (Camera.main == null || myhb == null || transform == null)
            return;

		myhb.transform.position = Camera.main.WorldToViewportPoint(transform.position);
		Vector3 pos = myhb.transform.position;

		pos.x -= 0.05f;
		pos.y += 0.05f + offsetY;

		myhb.transform.position = pos;
		myhb.transform.localScale = Vector3.zero;

		float healtPercentage = hp/maxhp;
		if(healtPercentage < 0.0f) { healtPercentage = 0.0f; }
		if(healtPercentage > 100.0f ) { healtPercentage = 100.0f; }
	
		healtBarWidth = (int)(healtPercentage * healtBarMaxSize);
		myhb.guiTexture.pixelInset = new Rect(10, 10, healtBarWidth, 5);

 	}
}
