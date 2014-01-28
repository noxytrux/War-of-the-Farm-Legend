using UnityEngine;
using System.Collections;

public class Enemy : SelectableEntity {

	public override void Start ()
	{
		base.Start();	
		entityType = EntityType.Enemy;
	}

	protected override void Update()
	{
		base.Update();
	}
	
	IEnumerator dieCoroutine()
	{
		GetComponent<EnemyAI>().HandleDieAnimation();
		yield return new WaitForSeconds(5.0f); 
		Destroy(transform.gameObject);		
	}
	
	public override void EntityDied()
	{
		StartCoroutine(dieCoroutine());
	}
}
