using UnityEngine;
using System.Collections;

public class BalanceProvider
{
    public class GoldMining
    {
        public float MiningTime { get { return 2; } }
        public int IncreaseAmount { get { return 1; } }
    }

    public class ResourcesPrice
    {
        public int WorkerPrice { get { return 20; } }
        public int WarriorPriceGreen { get { return 40; } }
        public int WarriorPriceYellow { get { return 80; } }
        public int WarriorPriceRed { get { return 120; } }
        public int TowerPrice { get { return 100; } }
    }

    // Singleton impl

    private static BalanceProvider instance;
    public GoldMining goldMining = new GoldMining();
    public ResourcesPrice resourcesPrice = new ResourcesPrice();

    public static BalanceProvider Instance()
    {
        if (instance == null)
        {
            instance = new BalanceProvider();
        }

        return instance;
    }
}
