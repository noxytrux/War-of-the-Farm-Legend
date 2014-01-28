using UnityEngine;
using System.Collections;

public class SelectableEntity : Entity
{
    bool IsSelected = false;
	private NavMeshAgent navMeshAgent;

	public bool isMoving () {

		if (Vector3.Distance(transform.position, navMeshAgent.destination) <= 1f)
		{
			return false;
		}

		return true;
	}

	public override void Start ()
    {
        base.Start();
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

    protected override void Update() 
	{
        base.Update();
	}

    public void OnSelect()
    {
        IsSelected = true;
        renderer.material.color = Color.blue;
    }

    public void OnUnSelect()
    {
        IsSelected = false;
        renderer.material.color = Color.white;
    }

	public override void EntityDied()
	{
		base.EntityDied();
	}

    public void GoTo()
    {
        if (IsSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                navMeshAgent.destination = hit.point;

				if( GetComponent<BaseEnemyAI>().isRushing() ){
					Debug.Log("Cancel RUSH");
					GetComponent<BaseEnemyAI>().cancelRush();
				}
            }
        }
    }
}
