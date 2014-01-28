using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RPCFunctions : MonoBehaviour {

    List<GameObject> NetworkRegisteredObjects;

	// Use this for initialization
	void Start () {
        NetworkRegisteredObjects = new List<GameObject>();
	}

    void RegisterObject(GameObject objReg)
    {
#if UNITY_EDITOR
        if (NetworkRegisteredObjects.Contains(objReg))
            Debug.LogError("Object already exist: " + objReg.name);
#endif

        NetworkRegisteredObjects.Add(objReg);
    }

    void UnregisterObject(GameObject objReg)
    {
#if UNITY_EDITOR
        if (NetworkRegisteredObjects.Contains(objReg) == false)
            Debug.LogError("Object doesnt exist: " + objReg.name);
#endif
        NetworkRegisteredObjects.Remove(objReg);
    }

	// Update is called once per frame
	void Update () {
	
	}

    [RPC]
    void SetPosition(string strObjectName, Vector3 vPosition)
    {
        GameObject searchObject = GameObject.Find(strObjectName);
        if (searchObject == null)
        {
            Debug.Log("Cant find " + strObjectName + " to set NETWORK position");
        }
        else
        {
            searchObject.transform.position = vPosition;
        }
    }

    [RPC]
    void SetRtsEntityPositionRotation(string strObjectName, Vector3 vPosition,Quaternion qRotation)
    {
        GameObject searchObject = GameObject.Find(strObjectName);
        if (searchObject == null)
        {
            Debug.Log("Cant find " + strObjectName + " to set NETWORK position");
        }
        else
        {
            searchObject.transform.position = vPosition;
            searchObject.transform.rotation = qRotation;
        }
    }

    [RPC]
    void SetEntityDescData(string strObjectName,int iExp, float fHealth,float fFood)
    {
        GameObject searchObject = GameObject.Find(strObjectName);
        if (searchObject == null)
        {
            Debug.Log("Cant find " + strObjectName + " to set NETWORK position");
        }
        else
        {
            if (searchObject.GetComponent<Entity>() == null)
                return;

            searchObject.GetComponent<Entity>().SetServerEntityDescData(iExp, fHealth, fFood);
        }
    }

    [RPC]
    void InstantiateEntity(int iEntityType, string strEntityName,Vector3 vPosition,Quaternion qRotation)
    {
        GameObject searchObject = GameObject.Find("RTS_EntityCreator");
        if (searchObject == null)
        {
            Debug.Log("Cant find " + "RTS_EntityCreator" + " to InstantiateEntity");
        }
        else
        {
            if (searchObject.GetComponent<EntityCreator>() == null)
                return;
            searchObject.GetComponent<EntityCreator>().ServerCreateRequest((EntityType)iEntityType, strEntityName, vPosition, qRotation);
        }
    }

     [RPC]
    void InstantiateEntityTower(string strEntityName, Vector3 vPosition, Quaternion qRotation)
    {
        GameObject searchObject = GameObject.Find("RTS_EntityCreator");
        if (searchObject == null)
        {
            Debug.Log("Cant find " + "RTS_EntityCreator" + " to Instantiate Tower Entity");
        }
        else
        {
            if (searchObject.GetComponent<TowerSelector>() == null)
                return;
            searchObject.GetComponent<TowerSelector>().ServerCreateRequest(strEntityName, vPosition, qRotation);
        }
    }

    [RPC]
    void SetRPGCameraTransform(Vector3 vPosition,Quaternion qOrientation)
     {
         GameObject searchObject = GameObject.Find("RPGCamera");
         if (searchObject == null)
         {
             Debug.Log("Cant find " + "RPGCamera" + " to Setup RPG camera");
         }
         else
         {
             if (searchObject.GetComponent<RPGCamera>() == null)
                 return;
             searchObject.GetComponent<RPGCamera>().SetRPGCameraTransform(vPosition, qOrientation);
         }
     }

    [RPC]
    void EntityDestroyed(string strEntityName)
    {
        GameObject searchObject = GameObject.Find(strEntityName);
        if (searchObject == null)
        {
            Debug.Log("Cant find " + strEntityName + " to destroy entity");
        }
        else
        {
            if (searchObject.GetComponent<Entity>() == null)
                return;
            searchObject.GetComponent<Entity>().EntityDestroyed();
        }
    }

    [RPC]
    void SetPlayerTriggerAnimState(string strTriggerState)
    {
        GameObject searchObject = GameObject.Find("RPG_PLAYER");
        if (searchObject == null)
        {
            Debug.Log("Cant find " + "RPG_PLAYER" + " to trigger anim entity");
        }
        else
        {
            if (searchObject.GetComponent<RPGPlayerController>() == null)
                return;

            searchObject.GetComponent<RPGPlayerController>().SetPlayerAnimStateTrigger(strTriggerState);
        }
    }
    // FROM CLIENT TO SERVER FUNCTIONS

    [RPC]
    void SetWeaponIndex(int iIndex)
    {
        GameObject searchObject = GameObject.Find("RPG_PLAYER");
        if (searchObject == null)
        {
            Debug.Log("Cant find " + "RPG_PLAYER" + " to set index weapon");
        }
        else
        {
            if (searchObject.GetComponent<Entity>() == null)
                return;

            searchObject.GetComponent<Entity>().SetWeapon(iIndex);
        }
    }

    [RPC]
    void ClientToServerPlayerInput(float _fMouseXAxis, float _v, float _h, bool _bMouseButtonUp)
     {
         GameObject searchObject = GameObject.Find("RPG_PLAYER");
         if (searchObject == null)
         {
             Debug.Log("Cant find " + "RPG_PLAYER" + " to send Input data");
         }
         else
         {
             if (searchObject.GetComponent<RPGPlayerController>() == null)
                 return;
             searchObject.GetComponent < RPGPlayerController>().ClientToServerPlayerInput(_fMouseXAxis, _v, _h, _bMouseButtonUp);
         }
     }
    
}
