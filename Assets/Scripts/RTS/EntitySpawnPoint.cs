using UnityEngine;
using System.Collections;

public class EntitySpawnPoint : MonoBehaviour 
{
	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
