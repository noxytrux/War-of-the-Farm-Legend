using UnityEngine;
using System.Collections;

public class Tower : SelectableEntity
{
    public GameObject ArrowPrefab;
    public Vector3 vdir;
	private bool isShooting = false;
	private Vector3 targetPos;
	private float maxShootTimeInterval = 1.0f;
	private float localDelta = 0.0f;

    public void Shoot(Vector3 position)
    {
		isShooting = true;
		targetPos = position;
    }

	public void StopShooting()
	{
		isShooting = false;
		localDelta = 0.0f;
	}

    public override void Start ()
    {
        base.Start();
	    entityType = EntityType.Tower;
    }

    protected override void Update()
    {
        base.Update();

		if(isShooting) {
			localDelta += Time.deltaTime;
		
			if( localDelta > maxShootTimeInterval){
				localDelta = 0.0f;

				Vector3 direction = targetPos - gameObject.transform.position;
				direction.Normalize();
				
				GameObject arrow = (GameObject)Instantiate(ArrowPrefab, gameObject.transform.position + new Vector3(0.0f,2.0f,0.0f), new Quaternion(0,0,0,1));
				
				if(arrow.collider) {
					Physics.IgnoreCollision(GetComponent<Entity>().gameObject.collider, arrow.collider);
				}

				arrow.rigidbody.AddRelativeForce(direction * 1000.0f);
				arrow.GetComponent<Weapon>().ParentEntityObject = gameObject;

			}

		}
    }
}
