using UnityEngine;
using System.Collections;

public class WorkerAI : BaseEnemyAI {

	public Animator animator;
	private bool stopRushing = false;

	public override void Start () {
		base.Start();

	} 
	
	public override void Update () {

		if(stopRushing) RushingToTarget = false;

		base.Update();

		bool moving = GetComponent<Worker>().isMoving();

		animator.SetBool("walking", moving);

		if(moving){

			animator.SetBool("digging", false);
		}
	}

	IEnumerator digCoroutine()
	{
		yield return new WaitForSeconds(0.5f); 
		animator.SetBool("digging", true);
	}


	public void StartDiging() {
		Debug.Log("Wydobywam!");
		StartCoroutine(digCoroutine());
	}

	public override void HandleDieAnimation()
	{
		animator.SetBool("died",true);
		stopRushing = true;
	}
}
