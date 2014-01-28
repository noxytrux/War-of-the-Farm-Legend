using UnityEngine;
using System.Collections;

public class HUDDisplayer : MonoBehaviour
{
    public Texture warriorSwordTexture;
    public Texture warriorHammerTexture;
    public Texture warriorAxeTexture;

    public Texture warriorHammerTextureLock;
    public Texture warriorAxeTextureLock;

	public Texture workerTexture;
    public Texture warriorGreenTexture;
    public Texture warriorYellowTexture;
    public Texture warriorRedTexture;
	public Texture towerTexture;
	public Texture goldTexture;
	public int textureWidth; // one size because this is square
	public int hudIndentation;
	public int underButtonIndentation; // for width and height
	public int underButtonWidth;
	public int underButtonHeight;
	public int inButtonWidthIndentation;
	public int inButtonHeightIndentation;
	public int buttonIndentation; // between other buttons (in horizontal)

	private int hudPositionX;
	private int hudPositionY;
	private int workerButtonClickCount;
    private int warriorGreenButtonClickCount;
    private int warriorYellowButtonClickCount;
    private int warriorRedButtonClickCount;
	private int towerButtonClickCount;
	private float entityCreateOn;
	private float entityCreateSpeed;
	private GameManager gameManagerScript;
	private ResourcesManager resourcesManagerScript;
	private int workerPrice;
    private int warriorGreenPrice;
    private int warriorYellowPrice;
    private int warriorRedPrice;
	private int towerPrice;

    GameObject rpgPlayerObject = null;
	void OnGUI()
	{
        if (gameManagerScript.GetGameMode() == GameModeType.RPG)
        {
            if (rpgPlayerObject == null)
                rpgPlayerObject = GameObject.Find("RPG_PLAYER");
            
            float fButtonsYPosition = hudPositionY ;

            Entity playerEntity = rpgPlayerObject.GetComponent<Entity>();

            GUI.Label(new Rect(
                hudPositionX ,
                hudPositionY - 50,
                100,
                200),
                "Exp: " + playerEntity.GetExp());

            // Pierwszy złożony button (Worker)
            if (GUI.Button(new Rect(hudPositionX, fButtonsYPosition, textureWidth, textureWidth), warriorSwordTexture))
            {
                playerEntity.SetWeapon(0);
            }

            // Drugi złożony button (Warrior)
            if (playerEntity.CanUseWeapon(1))
            {
                if (GUI.Button(new Rect(hudPositionX + textureWidth + buttonIndentation, fButtonsYPosition, textureWidth, textureWidth), warriorHammerTexture))
                {
                    playerEntity.SetWeapon(1);
                }
            }
            else
            {
                GUI.Button(new Rect(hudPositionX + textureWidth + buttonIndentation, fButtonsYPosition, textureWidth, textureWidth), warriorHammerTextureLock);
            }

            // Trzeci złożony button (Warrior)
            if (playerEntity.CanUseWeapon(2))
            {
                if (GUI.Button(new Rect(hudPositionX + ((textureWidth + buttonIndentation) * 2), fButtonsYPosition, textureWidth, textureWidth), warriorAxeTexture))
                {
                    playerEntity.SetWeapon(2);
                }
            }
            else
            {
                GUI.Button(new Rect(hudPositionX + ((textureWidth + buttonIndentation) * 2), fButtonsYPosition, textureWidth, textureWidth), warriorAxeTextureLock);
            }
        }else
		if (gameManagerScript.GetGameMode() == GameModeType.RTS)
		{
            if (!workerTexture || !warriorGreenTexture || !warriorYellowTexture || !warriorRedTexture  || !towerTexture)
			{
				Debug.LogError("Assign all expected Textures in the inspector.");
				return;
			}
			
			// Pierwszy złożony button (Worker)
			if (GUI.Button(new Rect(hudPositionX, hudPositionY, textureWidth, textureWidth), workerTexture))
			{
				if (resourcesManagerScript.HaveEnoughGold(workerPrice))
				{
					resourcesManagerScript.SubstractGold(workerPrice);
					++workerButtonClickCount;
					entityCreateOn = Time.time;
					entityCreateOn += entityCreateSpeed;
				}
			}
			
			if (workerButtonClickCount > 0 && Time.time > entityCreateOn)
			{
				GameObject.Find("RTS_EntityCreator").GetComponent<EntityCreator>().Create(EntityType.Worker);
				--workerButtonClickCount;
				entityCreateOn = Time.time;
				entityCreateOn += entityCreateSpeed;
			}

			GUI.Label(new Rect(
				hudPositionX + underButtonIndentation,
				hudPositionY + textureWidth + underButtonIndentation,
				underButtonWidth,
				underButtonHeight),
				workerPrice.ToString());

			GUI.Label(new Rect(
				hudPositionX + inButtonWidthIndentation,
				hudPositionY + textureWidth - inButtonHeightIndentation,
				underButtonWidth,
				underButtonHeight),
				workerButtonClickCount.ToString());
				
			// Drugi złożony button (Warrior)
			if (GUI.Button(new Rect(hudPositionX + textureWidth + buttonIndentation, hudPositionY, textureWidth, textureWidth), warriorGreenTexture))
			{
				if (resourcesManagerScript.HaveEnoughGold(warriorGreenPrice))
				{
					resourcesManagerScript.SubstractGold(warriorGreenPrice);
					++warriorGreenButtonClickCount;
					entityCreateOn = Time.time;
					entityCreateOn += entityCreateSpeed;
				}
			}
			
			if (warriorGreenButtonClickCount > 0 && Time.time > entityCreateOn)
			{
                GameObject.Find("RTS_EntityCreator").GetComponent<EntityCreator>().Create(EntityType.WarriorGreen);
				--warriorGreenButtonClickCount;
				entityCreateOn = Time.time;
				entityCreateOn += entityCreateSpeed;
			}

			GUI.Label(new Rect(
				hudPositionX + underButtonIndentation + textureWidth + buttonIndentation,
				hudPositionY + textureWidth + underButtonIndentation,
				underButtonWidth,
				underButtonHeight),
			    warriorGreenPrice.ToString());

			GUI.Label(new Rect(
				hudPositionX + inButtonWidthIndentation + textureWidth + buttonIndentation,
				hudPositionY + textureWidth - inButtonHeightIndentation,
				underButtonWidth,
				underButtonHeight),
			    warriorGreenButtonClickCount.ToString());

            // Trzeci złożony button (Warrior)
            if (GUI.Button(new Rect(hudPositionX + ((textureWidth + buttonIndentation) * 2), hudPositionY, textureWidth, textureWidth), warriorYellowTexture))
            {
                if (resourcesManagerScript.HaveEnoughGold(warriorYellowPrice))
                {
                    resourcesManagerScript.SubstractGold(warriorYellowPrice);
                    ++warriorYellowButtonClickCount;
                    entityCreateOn = Time.time;
                    entityCreateOn += entityCreateSpeed;
                }
            }

            if (warriorYellowButtonClickCount > 0 && Time.time > entityCreateOn)
            {
                GameObject.Find("RTS_EntityCreator").GetComponent<EntityCreator>().Create(EntityType.WarriorYellow);
                --warriorYellowButtonClickCount;
                entityCreateOn = Time.time;
                entityCreateOn += entityCreateSpeed;
            }

            GUI.Label(new Rect(
                hudPositionX + underButtonIndentation + ((textureWidth + buttonIndentation) * 2),
                hudPositionY + textureWidth + underButtonIndentation,
                underButtonWidth,
                underButtonHeight),
                warriorYellowPrice.ToString());

            GUI.Label(new Rect(
                hudPositionX + inButtonWidthIndentation + ((textureWidth + buttonIndentation) * 2),
                hudPositionY + textureWidth - inButtonHeightIndentation,
                underButtonWidth,
                underButtonHeight),
                warriorYellowButtonClickCount.ToString());

            // Czwarty złożony button (Warrior)
            if (GUI.Button(new Rect(hudPositionX + ((textureWidth + buttonIndentation) * 3), hudPositionY, textureWidth, textureWidth), warriorRedTexture))
            {
                if (resourcesManagerScript.HaveEnoughGold(warriorRedPrice))
                {
                    resourcesManagerScript.SubstractGold(warriorRedPrice);
                    ++warriorRedButtonClickCount;
                    entityCreateOn = Time.time;
                    entityCreateOn += entityCreateSpeed;
                }
            }

            if (warriorRedButtonClickCount > 0 && Time.time > entityCreateOn)
            {
                GameObject.Find("RTS_EntityCreator").GetComponent<EntityCreator>().Create(EntityType.WarriorRed);
                --warriorRedButtonClickCount;
                entityCreateOn = Time.time;
                entityCreateOn += entityCreateSpeed;
            }

            GUI.Label(new Rect(
                hudPositionX + underButtonIndentation + ((textureWidth + buttonIndentation) * 3),
                hudPositionY + textureWidth + underButtonIndentation,
                underButtonWidth,
                underButtonHeight),
                warriorRedPrice.ToString());

            GUI.Label(new Rect(
                hudPositionX + inButtonWidthIndentation + ((textureWidth + buttonIndentation) * 3),
                hudPositionY + textureWidth - inButtonHeightIndentation,
                underButtonWidth,
                underButtonHeight),
                warriorRedButtonClickCount.ToString());

			// Piaty złożony button (Tower)
			if (GUI.Button(new Rect(hudPositionX + ((textureWidth + buttonIndentation) * 4), hudPositionY, textureWidth, textureWidth), towerTexture))
			{
				if (resourcesManagerScript.HaveEnoughGold(towerPrice))
				{
					resourcesManagerScript.SubstractGold(towerPrice);
					++towerButtonClickCount;
					entityCreateOn = Time.time;
					entityCreateOn += entityCreateSpeed;
				}
			}
			
			if (towerButtonClickCount > 0 && Time.time > entityCreateOn)
			{
				GameObject.Find("RTS_EntityCreator").GetComponent<EntityCreator>().Create(EntityType.Tower);
				--towerButtonClickCount;
				entityCreateOn = Time.time;
				entityCreateOn += entityCreateSpeed;
			}

			GUI.Label(new Rect(
				hudPositionX + underButtonIndentation + ((textureWidth + buttonIndentation) * 4),
				hudPositionY + textureWidth + underButtonIndentation,
				underButtonWidth,
				underButtonHeight),
			    towerPrice.ToString());

			GUI.Label(new Rect(
				hudPositionX + inButtonWidthIndentation + ((textureWidth + buttonIndentation) * 4),
				hudPositionY + textureWidth - inButtonHeightIndentation,
				underButtonWidth,
				underButtonHeight),
			    towerButtonClickCount.ToString());
				
			// Pierwszy label (Gold)
			if (!goldTexture)
			{
				Debug.LogError("Assign a Texture in the inspector.");
				return;
			}
			
			GUI.DrawTexture(new Rect(hudPositionX, hudPositionY - goldTexture.height - 50, textureWidth, textureWidth), goldTexture);
			GUI.Label(new Rect(hudPositionX + 10, hudPositionY + goldTexture.height - 140, 35, 25), resourcesManagerScript.Gold.ToString());
		}
	}

	private void Start()
	{
		hudPositionX = hudIndentation;
		hudPositionY = Screen.height - workerTexture.height - hudIndentation - underButtonHeight;
		workerButtonClickCount = 0;
        warriorGreenButtonClickCount = 0;
        warriorYellowButtonClickCount = 0;
        warriorRedButtonClickCount = 0;
		towerButtonClickCount = 0;
		entityCreateOn = 0.0f;
		entityCreateSpeed = 2.0f;
		gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
		resourcesManagerScript = GameObject.Find("RTS_EntityCreator").GetComponent<ResourcesManager>();
		workerPrice = BalanceProvider.Instance().resourcesPrice.WorkerPrice;
        warriorGreenPrice = BalanceProvider.Instance().resourcesPrice.WarriorPriceGreen;
        warriorYellowPrice = BalanceProvider.Instance().resourcesPrice.WarriorPriceYellow;
        warriorRedPrice = BalanceProvider.Instance().resourcesPrice.WarriorPriceRed;
		towerPrice = BalanceProvider.Instance().resourcesPrice.TowerPrice;
	}
}
