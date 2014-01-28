using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {

	private bool disableTrigger;

	void Awake() 
	{

		IgnoreTrigger();
	}

	IEnumerator lockCoroutine()
	{
		disableTrigger = true;
		yield return new WaitForSeconds(0.3f); 
		disableTrigger = false;
	}

	void IgnoreTrigger() 
	{

		StartCoroutine(lockCoroutine());
	}

	void OnTriggerEnter(Collider colider)
	{

	}

	void OnTriggerStay(Collider colider)
	{
		if(disableTrigger) return;
		
		Weapon weapon = GetComponent<Weapon>();

		if(weapon.ParentEntityObject){
			
			Entity ParentEntity = weapon.ParentEntityObject.GetComponent<Entity>();
			Entity rootEntity = colider.gameObject.GetComponent<Entity>();
			
			if (rootEntity && ParentEntity){
				rootEntity.SubstractHealth(weapon.damage, ParentEntity);
			}		
		}
		
		Destroy(gameObject);
	}
}
