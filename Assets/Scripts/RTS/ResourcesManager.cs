using UnityEngine;
using System.Collections;

public class ResourcesManager : MonoBehaviour
{
    public int StartGoldAmount;

    public int Gold { get; private set; }

    public bool SubstractGold(int goldAmount)
    {
        if (HaveEnoughGold(goldAmount))
        {
            Gold -= goldAmount;
            return true;
        }

        return false;
    }

    public void AddGold(int goldAmount)
    {
        Gold += goldAmount;
    }

    public bool HaveEnoughGold(int goldAmount)
    {
        return (Gold - goldAmount) >= 0;
    }

    void Start ()
    {
        Gold = StartGoldAmount;
    }

	void Update () 
    {
	
	}


}
