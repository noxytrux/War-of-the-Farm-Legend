using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class BaseEnemyAI : MonoBehaviour {
    public bool RushToRPGPlayer = false;
    public bool RushToRTSPlayer = false;

    protected bool RushingToTarget = false;
    protected GameObject RushTarget = null;
    protected bool LockChassing = false;

	// Use this for initialization
	public virtual void Start () {
      
	}
	
	// Update is called once per frame
    public virtual void Update()
    {
        if (RushTarget == null) // mozliwa sytyuacja gdy ai i my gonimy np. wilka 
        {
            RushingToTarget = false;
            return;
        }

        if (RushingToTarget)
        {
            if (Vector3.Distance(transform.position, RushTarget.transform.position) <= 2.0f)
            {
                GetComponent<Entity>().Attack(RushTarget);
				HandleAttackAnim(true);
            }
            else
            {
                GetComponent<Entity>().StopAttack();
                GetComponent<NavMeshAgent>().destination = RushTarget.transform.position;
				HandleAttackAnim(false);
            }

            return; 
        }
	}

	public virtual void HandleAttackAnim (bool attacking)
	{

	}

	public virtual void HandleDieAnimation()
	{

	}

	IEnumerator lockCoroutine()
	{
		LockChassing = true;
		yield return new WaitForSeconds(1.0f); 
		LockChassing = false;
	}

	public bool isRushing() {
		return RushingToTarget;
	}

	public void cancelRush (){

		RushingToTarget = false;
		GetComponent<Entity>().StopAttack();
		HandleAttackAnim(false);
		StartCoroutine(lockCoroutine());
	}

    void OnTriggerStay(Collider colider)
    {
		if(LockChassing) return;

        bool acceptTrigger = false;

        if (colider.gameObject.tag == "RPGPlayer" && RushToRPGPlayer == true) {
            acceptTrigger = true;
		}

        if (colider.gameObject.tag == "RTSPlayer" && RushToRTSPlayer == true) {
            acceptTrigger = true;
		}

		if (colider.gameObject.tag == "Monster") {
			acceptTrigger = true;
		}

        if (acceptTrigger == false)
            return;

        Debug.Log("GONIE " + colider.gameObject.name);

        RushingToTarget = true;
        RushTarget = colider.gameObject;
    }
}
