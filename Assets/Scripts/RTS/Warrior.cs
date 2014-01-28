using UnityEngine;
using System.Collections;

public class Warrior : SelectableEntity
{

	public override void Start ()
    {
        base.Start();

        entityType = EntityType.WarriorGreen;
	}
    protected override void Update()
    {
        base.Update();
    }

	IEnumerator dieCoroutine()
	{
		GetComponent<SoldierAI>().HandleDieAnimation();
		yield return new WaitForSeconds(5.0f); 
		Destroy(transform.gameObject);		
	}
	
	public override void EntityDied()
	{
		StartCoroutine(dieCoroutine());
	}

}
