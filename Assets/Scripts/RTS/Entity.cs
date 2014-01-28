using UnityEngine;
using System.Collections;



public class Entity : NetworkedObject
{
    public GameObject[] EntityWeapons;
    public GameObject DefaultWeapon;
    Weapon CurrentWeapon;

    public int ExpLooted = 10;
    public float fFoodLooted = 5.0f;

    float fRegenerateHpFreq = 5.0f;
    float fLastTimeRegHP = 0.0f;

    [HideInInspector]
    public int Exp = 0;
    public float Health = 100.0f;
    float MaxHealth = 0.0f;

    public float fFood = 0.0f;

    public EntityType entityType;


  	private bool isAlive;


    public virtual void Start()
    {
        MaxHealth = Health;

        InitializeNetwork();

        isAlive = true;

        foreach (GameObject weaponObj in EntityWeapons)
        {
            if (weaponObj.GetComponent<Weapon>() == null)
            {
                Debug.LogError("Object: " + weaponObj.name + "is not weapon");
            }

            if (weaponObj.collider)
                Physics.IgnoreCollision(collider, weaponObj.collider); // boczek

            weaponObj.active = false;
        }

        if (DefaultWeapon != null)
        {
            if (DefaultWeapon.GetComponent<Weapon>() == null)
            {
                Debug.LogError("Object: " + DefaultWeapon.name + "is not weapon");
            }

            DefaultWeapon.active = true;
            CurrentWeapon = DefaultWeapon.GetComponent<Weapon>();
        }
    }

    public void SetWeapon(int iWeaponIndex)
    {
        CurrentWeapon.active = false;
        CurrentWeapon = EntityWeapons[iWeaponIndex].GetComponent<Weapon>();
        CurrentWeapon.active = true;

        SendRPC("SetWeaponIndex", RPCMode.Others, iWeaponIndex);
    }

    public bool CanUseWeapon(int iIndex)
    {
        if (iIndex == 1)
            if (Exp > 60)
                return true;

        if (iIndex == 2)
            if (Exp > 120)
                return true;

        return false;
    }
    public void EntityKillsEnemy(Entity enemyEntity)
    {
        if (IsClient()) // only server can create action
            return;

        Exp += enemyEntity.ExpLooted;
        fFood += enemyEntity.fFoodLooted;

        RPGPlayerStatistics rpgPlayerStatistics = GetComponent<RPGPlayerStatistics>();

        if (rpgPlayerStatistics != null)
        {
            rpgPlayerStatistics.addExp(enemyEntity.ExpLooted);
        }
    }

	public void SubstractHealth(float healthAmount, Entity attackerEntity)
    {
        if (IsClient()) // only server can create action
            return;

        if (attackerEntity == null)
            return;

        Health -= healthAmount;

        if (Health <= 0)
        {
            isAlive = false;
            attackerEntity.EntityKillsEnemy(this);
			EntityDied();

        }
    }

	public virtual void EntityDied()
	{
		Destroy(transform.gameObject);		
	}

    public void AddHealth(int healthAmount)
    {
        if (IsClient()) // only server can create action
            return;

        Health += healthAmount;
    }

    public float GetHealth()
    {
        return Health;
    }

    public int GetExp()
    {
        return Exp;
    }

	public bool IsAlive()
    {
        return isAlive;
    }

    public void Attack(GameObject objectToAttack)
    {
        if (IsClient()) // only server can create action
            return;

        if (CurrentWeapon != null)
        {
            CurrentWeapon.StartAttacking(objectToAttack);
        }
        else
            Debug.LogError("Cant find current weapon!");
    }

    public void StopAttack()
    {
        if (IsClient()) // only server can create action
            return;

        if (CurrentWeapon != null)
            CurrentWeapon.StopAttacking();
    } 

    public void AttackOnce(GameObject objectToAttack)
    {
        if (IsClient()) // only server can create action
            return;

        if (CurrentWeapon != null)
            CurrentWeapon.AttackOnce(objectToAttack);
    }
	protected virtual void Update ()
    {
        if (IsServer())
        {
            if (fFood > 0.0f)
            {
                if (Health < MaxHealth)
                {
                    fLastTimeRegHP += Time.deltaTime;

                    if (fLastTimeRegHP > 1.0f)
                    {
                        fLastTimeRegHP = 0.0f;
                        Health += fRegenerateHpFreq;
                        fFood -= fRegenerateHpFreq;

                        Health = Mathf.Min(Health, MaxHealth);
                        fFood = Mathf.Max(fFood, 0.0f);
                    }
                }
            }

            SendRPC("SetRtsEntityPositionRotation", RPCMode.Others, transform.gameObject.name, transform.position, transform.rotation);

            SendRPC("SetEntityDescData", RPCMode.Others,transform.gameObject.name, Exp, Health,fFood);
        }
	}

    public void SetServerEntityDescData(int iExp, float fHealth,float _fFood)
    {
        Exp = iExp;
        Health = fHealth;
        fFood = _fFood;
    }

	protected override void SynchronizeState()
    {
       // SendRPC("SetRtsEntityPositionRotation", RPCMode.Others, transform.gameObject.name, transform.position,transform.rotation);
     //   SendRPC("SetEntityDescData", RPCMode.Others, transform.gameObject.name, entityDesc.Exp, entityDesc.Health);
      
    }

    void OnDestroy ()
    {
        if (GetComponent<HealtBarScript>() != null)
        {
            Health = 0;
            GetComponent<HealtBarScript>().Update();
        }

        if (IsClient())
            return;

        SendRPC("EntityDestroyed", RPCMode.Others, transform.gameObject.name);
    }

    public void EntityDestroyed()
    {
        Destroy(transform.gameObject);
    }
    // For selection methods

    
}
