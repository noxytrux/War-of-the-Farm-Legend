﻿using UnityEngine;
using System.Collections;

public class Worker : SelectableEntity
{
    public override void Start () 
    {
        base.Start();
        entityType = EntityType.Worker;
	}

    protected override void Update()
    {
        base.Update();
    }
	
	IEnumerator dieCoroutine()
	{
		GetComponent<WorkerAI>().HandleDieAnimation();
		yield return new WaitForSeconds(5.0f); 
		Destroy(transform.gameObject);		
	}

	public override void EntityDied()
	{
		StartCoroutine(dieCoroutine());
	}
}
