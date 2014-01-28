using UnityEngine;
using System.Collections;

public class MainMenuSceneManager : MonoBehaviour
{
	public Texture buttonNewGame;
    public Texture buttonAbout;
    public Texture buttonExit;
	
	private int buttonWidth = 150;
	private int buttonHeight = 45;
	private int buttonIndentation = 60;
	private int buttonStartIndentation = 90;
	
	void OnGUI()
	{
		if (GUI.Button(new Rect((Screen.width / 2) - (buttonWidth / 2), (Screen.height / 2) - buttonStartIndentation, buttonWidth, buttonHeight), buttonNewGame))
		{
			Application.LoadLevel("TestScene");
		}

		if (GUI.Button(new Rect((Screen.width / 2) - (buttonWidth / 2), (Screen.height / 2) + buttonIndentation - buttonStartIndentation, buttonWidth, buttonHeight), buttonAbout))
		{
			Application.LoadLevel("AboutMenu");
		}
		
		if (GUI.Button(new Rect((Screen.width / 2) - (buttonWidth / 2), (Screen.height / 2) + (buttonIndentation * 2) - buttonStartIndentation, buttonWidth, buttonHeight), buttonExit))
		{
			Application.Quit(); // is ignored in the editor
		}
	}
}
