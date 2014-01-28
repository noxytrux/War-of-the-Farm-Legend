using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public GameObject RPGCamera = null;
    public GameObject RTSCamera = null;

    public GameModeType GameMode = GameModeType.RPG;

	public GameModeType GetGameMode()
	{
		return GameMode;
	}

	void EnableRPGCamera()
    {
        //temporary
        GameMode = GameModeType.RPG;

        RPGCamera.GetComponent<Camera>().enabled = true;
        RTSCamera.GetComponent<Camera>().enabled = false;

        RPGCamera.tag = "MainCamera";
        RTSCamera.tag = "Untagged";
    }

    void EnableRTSCamera()
    {
        //temporary
        GameMode = GameModeType.RTS;

        RPGCamera.GetComponent<Camera>().enabled = false;
        RTSCamera.GetComponent<Camera>().enabled = true;

        RTSCamera.tag = "MainCamera";
        RPGCamera.tag = "Untagged";
    }

	void Start () 
	{
		if (RPGCamera == null || RTSCamera == null)
			Debug.LogError("Cameras are not assigned to GameManager!");
		
		RPGCamera.GetComponent<Camera>().enabled = false;
		RTSCamera.GetComponent<Camera>().enabled = false;
		
		if (GameMode == GameModeType.RPG)
			EnableRPGCamera();
		else
			EnableRTSCamera();
	}

	void Update () 
    {
	    // tempowo do czasu stworzenia menu
        if (Network.isServer && GameMode == GameModeType.RPG)
        {
            EnableRTSCamera(); 
        }
        else if (Network.isClient && GameMode == GameModeType.RTS)
        {
            EnableRPGCamera(); 
        }
	}
}
