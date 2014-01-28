using UnityEngine;
using System.Collections;

public class EntityCreator :  NetworkedObject
{
    public GameObject EntitySpawnPoint;

    public GameObject WorkerPrefab;
    public GameObject WarriorGreenPrefab;
    public GameObject WarriorYellowPrefab;
    public GameObject WarriorRedPrefab;
    public GameObject TowerPrefab;

    private ResourcesManager resourcesManager;
    private ResourcesPrices resourcesPrices;
    private TowerSelector towerSelector;

    private int iInstantiateCounter = 0;

    // serwer (rts) zespawnowal enemy - jedyne zadanie to je stworzyc w RPG oraz nadac mu dobra nazwe
    public void ServerCreateRequest(EntityType entityType, string strEntityName, Vector3 vPosition, Quaternion qRotation)
    {
        if (IsServer() == true)
        {
            Debug.LogError("Server (rts) cannot spawn entities without taking gold etc please use Create function");
            return;
        }

        GameObject instantiateObject = InstantiateEntityType(entityType, vPosition, qRotation);

        instantiateObject.transform.parent = transform;
        instantiateObject.name = strEntityName;
    }

    public bool Create(EntityType entityType)
    {
        if (IsServer() == false)
        {
            Debug.LogError("Client cannot spawn enemies, please use ServerCreateRequest if it is from server request");
            return false;
        }

        Vector3 position = EntitySpawnPoint.transform.position;
		Quaternion rotation = EntitySpawnPoint.transform.rotation;
		GameObject instantiateObject = null;

		float randX = Random.Range(-3, 3);
		float randZ = Random.Range(-3, 3);

		Vector3 newPosition = new Vector3(position.x + randX, position.y, position.z + randZ);

		instantiateObject = InstantiateEntityType(entityType, newPosition, rotation);

		if (instantiateObject != null)
		{
			instantiateObject.transform.parent = transform;
			instantiateObject.name = instantiateObject.name + "Ins" + iInstantiateCounter;
			iInstantiateCounter++;

			SendRPC("InstantiateEntity", RPCMode.Others, (int) entityType, instantiateObject.name, newPosition, rotation);
			return true;
		}

        return false;
    }

    private GameObject InstantiateEntityType(EntityType entityType, Vector3 position, Quaternion rotation)
    {
        GameObject instantiateObject = null;
        switch (entityType)
        {
            case EntityType.Worker:
                instantiateObject = (GameObject)Instantiate(WorkerPrefab, position, rotation);
                break;

            case EntityType.WarriorGreen:
                instantiateObject = (GameObject)Instantiate(WarriorGreenPrefab, position, rotation);
                break;

            case EntityType.Tower:
                towerSelector.StartCreating();
                instantiateObject = null;
                break;

            case EntityType.WarriorYellow:
                instantiateObject = (GameObject)Instantiate(WarriorYellowPrefab, position, rotation);
                break;

            case EntityType.WarriorRed:
                instantiateObject = (GameObject)Instantiate(WarriorRedPrefab, position, rotation);
                break;
            default:
                break;
        } 
        
        return instantiateObject;
    }

    void Start ()
    {
        InitializeNetwork();

        resourcesManager = GetComponent<ResourcesManager>();
        resourcesPrices = GetComponent<ResourcesPrices>();
        towerSelector = GetComponent<TowerSelector>();
    }

	void Update () 
    {
        if (IsServer())
        {
        }
	}

    protected override void SynchronizeState()
    {
        //  SendRPC("SetRtsEntityPositionRotation", RPCMode.Others, transform.gameObject.name, transform.position,transform.rotation);
    }
}
