using UnityEngine;
using System.Collections;

public class SoldierAI : BaseEnemyAI {

	public Animator animator;
	private bool stopRushing = false;

	// Use this for initialization
	public override void Start () {
		base.Start();
		
	}
	
	public override void Update () {

		if(stopRushing) RushingToTarget = false;

		base.Update();

        bool moving = GetComponent<SelectableEntity>().isMoving();
		
		animator.SetBool("walking", moving);
		
	}
	 
	public override void HandleAttackAnim (bool attacking)
	{
		animator.SetBool("attacking", attacking);
	}

	public override void HandleDieAnimation()
	{
		animator.SetBool("died",true);
		GetComponent<Entity>().StopAttack();
		stopRushing = true;
	}
}
