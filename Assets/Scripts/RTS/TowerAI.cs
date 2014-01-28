using UnityEngine;
using System.Collections;

public class TowerAI : BaseEnemyAI
{
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        if (RushTarget == null) // mozliwa sytyuacja gdy ai i my gonimy np. wilka 
        {
            RushingToTarget = false;
            return;
        }

        if (RushingToTarget)
        {
            if (RushTarget != null && RushTarget.transform != null)
            {

                if (Vector3.Distance(transform.position, RushTarget.transform.position) <= 15.0f)
                {
                    GetComponent<Tower>().Shoot(RushTarget.transform.position);
                    HandleAttackAnim(true);
                }
                else
                {
                    HandleAttackAnim(false);
                    GetComponent<Tower>().StopShooting();
                }

                return;
            }
        }
    }

    public override void HandleAttackAnim(bool attacking)
    {
    }
}
