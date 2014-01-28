using UnityEngine;
using System.Collections;

public class NavBehoviour : NetworkedObject 
{
    private NavMeshAgent navMeshAgent;
    public GameObject[] NavTargets;

    private Vector3 nextDestination;
    private int nextIndex = 0;

	void Start ()
	{
        InitializeNetwork();

	    NavTargets = GameObject.FindGameObjectsWithTag("NavigationPointTag");

        Vector3 position = NavTargets[0].transform.position;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = position;
        nextDestination = position;
	}

	void Update ()
    {
        if (IsServer() == false)
            return;

        if (Vector3.Distance(transform.position, nextDestination) <= 1f)
        {
            if (nextIndex == NavTargets.Length - 1)
            {
                nextIndex = 0;
            }
            else
            {
                nextIndex++;
            }

            nextDestination = NavTargets[nextIndex].transform.position;
        }

        navMeshAgent.destination = nextDestination;
	}


    protected override void SynchronizeState()
    {
    }
}
