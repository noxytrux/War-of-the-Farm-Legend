using UnityEngine;
using System.Collections;

public class ResourcesPrices : MonoBehaviour
{
    public int EntityPrice(EntityType entityType)
    {
        switch (entityType)
        {
            case EntityType.Worker:
                return BalanceProvider.Instance().resourcesPrice.WorkerPrice;

            case EntityType.WarriorGreen:
                return BalanceProvider.Instance().resourcesPrice.WarriorPriceGreen;

            case EntityType.WarriorYellow:
                return BalanceProvider.Instance().resourcesPrice.WarriorPriceYellow;

            case EntityType.WarriorRed:
                return BalanceProvider.Instance().resourcesPrice.WarriorPriceRed;

            case EntityType.Tower:
                return BalanceProvider.Instance().resourcesPrice.TowerPrice;

            default:
                return 0;
        }
    }

    void Start ()
    {
    
    }

	void Update () 
    {
	
	}
}
