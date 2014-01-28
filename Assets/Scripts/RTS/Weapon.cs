using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public GameObject ParentEntityObject;
    private Entity ParentEntity;
	public float damage = 2.0f;


    private bool IsAttacking = false;
    // zapobiega kolizja gdy poprostu bron dotyka przeciwnika nie atakujac go
    public float AttackingEveryTime = 1.0f;
    private float CurrentAttackingTime = 0.0f;

    GameObject ObjectToAttack = null;

	// Use this for initialization
	void Start () 
    {
        ParentEntity = ParentEntityObject.GetComponent<Entity>();

	}
	
	// Update is called once per frame
    void Update()
    {
        CurrentAttackingTime += Time.deltaTime;

        //Debug.Log("Update Weapon from " + transform.gameObject.name + " is attacking " + IsAttacking);


        if (ObjectToAttack == null)
        {
            StopAttacking();
            return;
        }

        if (IsAttacking == true)
        {

          //  Debug.Log(transform.gameObject.name + " attTakuje: " + ObjectToAttack.name);
            if (CurrentAttackingTime > AttackingEveryTime)
            {
                CurrentAttackingTime = 0.0f;

                if (ObjectToAttack == null)
                {
                    StopAttacking();
                    return;
                }

                ObjectToAttack.GetComponent<Entity>().SubstractHealth(damage, ParentEntity);
            } 
        }
	}

    public void StartAttacking(GameObject objToAttack)
    {
        if (IsAttacking == true)
            return;


        IsAttacking = true;
        CurrentAttackingTime = 0.0f;
        ObjectToAttack = objToAttack;

       // Debug.Log(transform.gameObject.name + " attakuje: " + ObjectToAttack.name + " is ataking: " + IsAttacking);
    }

    public void StopAttacking()
    {
        if (ObjectToAttack != null)
        //Debug.Log(transform.gameObject.name + " STOP ATAKING: " + ObjectToAttack.name + " is ataking: " + IsAttacking);
        IsAttacking = false;
        //CurrentAttackingTime = 0.0f;
        ObjectToAttack = null;
    }

    public void AttackOnce(GameObject objToAttack)
    {
        if (CurrentAttackingTime > AttackingEveryTime)
        {
            CurrentAttackingTime = 0.0f;
            objToAttack.GetComponent<Entity>().SubstractHealth(damage, ParentEntity);
           // Debug.Log("UDANY ATAK");
        }
    }
}
