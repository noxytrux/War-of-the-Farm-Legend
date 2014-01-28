using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class GoldMinePoint : MonoBehaviour
{
    private bool mining;
    private float elapsed;
    private float miningTime;

    private ResourcesManager resourcesManager;
    private IList<string> registers = new List<string>();

    public void OnTriggerExit(Collider collider)
    {
        if (collider != null && collider.gameObject != null)
        {
            if (collider.gameObject.transform != null)
            {
                Transform colliderParent = collider.gameObject.transform.parent;

                if (colliderParent != null)
                {
                    if (registers.Any())
                    {
                        if (registers.Any(x => x == colliderParent.name))
                        {
                            registers.Remove(colliderParent.name);
                        }
                    }
                }
            }
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider != null && collider.gameObject != null)
        {
            if (collider.gameObject.transform != null)
            {
                Transform colliderParent = collider.gameObject.transform.parent;

                if (colliderParent != null)
                {
                    Entity entity = colliderParent.GetComponent<Entity>();

                    if (entity != null && entity.entityType == EntityType.Worker)
                    {
                        if(registers.All(x => x != colliderParent.name))
                            registers.Add(colliderParent.name);

						colliderParent.GetComponent<WorkerAI>().StartDiging();
						colliderParent.LookAt(transform.position);
                    }
                }
            }
        }
    }

    void Start ()
    {
        mining = false;
        elapsed = 0f;

        miningTime = BalanceProvider.Instance().goldMining.MiningTime;
        resourcesManager = GameObject.Find("RTS_EntityCreator").GetComponent<ResourcesManager>();
    }

	void Update ()
	{
	    if (registers.Any())
	    {
	        elapsed += Time.deltaTime;

	        if (elapsed >= miningTime)
	        {
	            int factor = registers.Count();

	            resourcesManager.AddGold(BalanceProvider.Instance().goldMining.IncreaseAmount * factor);

	            elapsed = 0f;
	        }
	    }
	}
}
