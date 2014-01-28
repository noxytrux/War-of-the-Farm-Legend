using UnityEngine;
using System.Collections;

public class AboutMenuSceneManager : MonoBehaviour
{
	public Texture textureAbout;
    public Texture buttonBack;
	
	private int buttonWidth = 150;
	private int buttonHeight = 45;
	private int buttonIndentation = 160;
	private int buttonStartIndentation = 60;
	private int textureWidth = 400;
	private int textureHeight = 200;
	
	void OnGUI()
	{
		GUI.DrawTexture(new Rect((Screen.width / 2) - (textureWidth / 2), (Screen.height / 2) - (buttonStartIndentation * 2), textureWidth, textureHeight), textureAbout);
		
		if (GUI.Button(new Rect((Screen.width / 2) - (buttonWidth / 2), (Screen.height / 2) + buttonIndentation - buttonStartIndentation, buttonWidth, buttonHeight), buttonBack))
		{
			Application.LoadLevel("MainMenu");
		}
	}
}
