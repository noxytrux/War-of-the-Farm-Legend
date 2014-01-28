using UnityEngine;
using System.Collections;

public class RPGPlayerStatistics : MonoBehaviour {
	
	public int level = 1;
	public float defense = 2.0f;
	public float attack = 2.0f;

	public float currentExp = 0.0f;
	public float lastExp = 0.0f;
	public float nextRequiredExp = 100.0f;


	Rect rect = new Rect(0.0f,0.0f,400.0f,40.0f);
	Vector3 expoffset = new Vector3(0.0f, -2.4f, 0.0f); 
	Vector3 lvloffset = new Vector3(0.0f, -2.8f, 0.0f);

	private GameManager gameManager;

	void OnGUI(){

		if (gameManager.GetGameMode() == GameModeType.RPG)
		{

			Vector3 point = Camera.main.WorldToScreenPoint(transform.position + expoffset);
			rect.x = point.x - 42.0f;
			rect.y = point.y - rect.height; 
			GUI.Label(rect, "EXP: " + currentExp + "/" + nextRequiredExp ); 
			
			point = Camera.main.WorldToScreenPoint(transform.position + lvloffset);
			rect.x = point.x - 42.0f;
			rect.y = point.y - rect.height; 
			GUI.Label(rect, "LVL: " + level); 

		}
	}

	public void addExp( float exp ) {

		currentExp += exp;

		if(currentExp > nextRequiredExp) {

			level++;
			currentExp = 0;

			lastExp = nextRequiredExp;
			nextRequiredExp +=  lastExp * 0.6f;

			attack += level / 20.0f;
			defense += level / 30.0f;
		}
	}

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        float expAdded = (float)GetComponent<Entity>().GetExp() - currentExp;
        addExp(expAdded);
	}
}
