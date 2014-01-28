using UnityEngine;
using System.Collections;

public class TowerSelector : NetworkedObject
{
    public GameObject TowerPrefab;
    public GameObject Castle;
    public float MaxTowerDistance;

    private int iTowerCreatedIndex = 0; // for unique names
    public void StartCreating()
    {
        startCreating = true;
    }

    private bool startCreating;
    private Vector3 circleCenter;

    void Start()
    {
        InitializeNetwork();

        circleCenter = Castle.transform.position;
        startCreating = false;
    }

    protected override void SynchronizeState()
    {
        //  SendRPC("InstantiateEntityTower", RPCMode.Others, transform.gameObject.name, transform.position);
    }

    public void ServerCreateRequest(string strEntityName, Vector3 vPosition, Quaternion qRotation)
    {
        if (IsServer() == true)
        {
            Debug.LogError("Server (rts) cannot spawn Towen entities without taking gold etc please use StartCreating function");
            return;
        }

        GameObject instantiateObject = (GameObject)Instantiate(TowerPrefab, vPosition, qRotation);

        instantiateObject.transform.parent = transform;
        instantiateObject.name = strEntityName;
    }

    void Update()
    {
        if (startCreating)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (Vector3.Distance(circleCenter, hit.point) < MaxTowerDistance)
                    {
                        GameObject instantiateObject = (GameObject)Instantiate(TowerPrefab, hit.point, TowerPrefab.transform.rotation);

                        instantiateObject.transform.parent = transform;
                        instantiateObject.name = instantiateObject.name + "Ins" + iTowerCreatedIndex;
                        iTowerCreatedIndex++;

                        SendRPC("InstantiateEntityTower", RPCMode.Others, instantiateObject.name, instantiateObject.transform.position,
                            instantiateObject.transform.rotation);

                    }
                    else
                    {
                        // tutaj jkis dzwięki czy coś że nie można postawić wierzyczki
                    }
                }

                startCreating = false;
            }

        }
    }
}
