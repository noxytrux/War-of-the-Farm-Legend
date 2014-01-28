using UnityEngine;
using System.Collections;

public class RPGPlayerController : NetworkedObject {

    public Transform mainCameraTransform;

	public float Healt = 100.0f;

	public float xSpeed = 150.0f;

    public Animator AttackFireAnimator;

	// Private memeber data
    private FreeMoementMotor motor;
	private Quaternion screenMovementSpace;
	private Vector3 screenMovementForward;
	private Vector3 screenMovementRight;

	private float x = 0.0f;

	enum CharacterState {
		Idle = 0,
		Walking = 1,
		Trotting = 2,
		Running = 3,
		Jumping = 4,
		StrafingLeft = 5,
		StrafingRight = 6,
	}

	private CharacterState _characterState;

	//MOVEMENT
	private float moveSpeed = 0.0f;
	private float walkSpeed = 5.0f;
	private float trotSpeed = 5.0f;

	private float speedSmoothing = 10.0f;
	private float trotAfterSeconds = 3.0f;
	private float walkTimeStart = 0.0f;
	private float speedMultipler = 1.0f;

	private bool isMoving = false;
	private bool movingBack = false;

	public Animator anim;

    void OnDestroy()
    {
        Application.LoadLevel("GameOver");
    }

	void Awake () {
	}

	void Start () {

        InitializeNetwork();

		Vector3 angles = transform.eulerAngles;
		x = angles.y;

        motor = GetComponent <FreeMoementMotor>();

	}

    protected override void SynchronizeState()
    {
    }

    float fMouseXAxis = 0.0f;
    float v = 0.0f;
    float h = 0.0f;
    bool bMouseButtonUp = false;

    public void ClientToServerPlayerInput(float _fMouseXAxis, float _v, float _h, bool _bMouseButtonUp)
    {
        fMouseXAxis = _fMouseXAxis;
        v = _v;
        h = _h;
        bMouseButtonUp = _bMouseButtonUp;
    }

	void Update () {
       
        // jeszcze sie nie polaczylismy
        if (Network.connections.Length == 0)
        {
            fMouseXAxis = Input.GetAxis("Mouse X");
            v = Input.GetAxis("Vertical");
            h = Input.GetAxis("Horizontal");
            bMouseButtonUp = Input.GetMouseButtonUp(0);
        }

        if (IsClient())
        {
            fMouseXAxis = Input.GetAxis("Mouse X");
            v = Input.GetAxis("Vertical");
            h = Input.GetAxis("Horizontal");
            bMouseButtonUp = Input.GetMouseButtonUp(0);
            SendRPC("ClientToServerPlayerInput", RPCMode.Others, fMouseXAxis, v, h, bMouseButtonUp);

            return;
        }

		screenMovementSpace = Quaternion.Euler (0.0f, mainCameraTransform.eulerAngles.y, 0.0f);
		screenMovementForward = screenMovementSpace * Vector3.forward;
		screenMovementRight = screenMovementSpace * Vector3.right;

        x += fMouseXAxis * xSpeed * 0.02f;

		Quaternion rotation = Quaternion.Euler(0.0f, x, 0.0f);
		
		
		isMoving = Mathf.Abs (h) > 0.1f || Mathf.Abs (v) > 0.1f;
		
		motor.movementDirection = h * screenMovementRight + v * screenMovementForward; 
		
		if (motor.movementDirection.sqrMagnitude > 1.0f)
			motor.movementDirection.Normalize();
		
		motor.facingDirection = rotation * new Vector3(1.0f,0.0f,0.0f);
	
		if (v < -0.2)
			movingBack = true;
		else
			movingBack = false;

		float curSmooth = speedSmoothing * Time.deltaTime;
		float targetSpeed = Mathf.Min( motor.movementDirection.magnitude, 1.0f);
		
		_characterState = CharacterState.Idle;
	
		if(Mathf.Abs (h) > 0.1f){


			if(_characterState != CharacterState.StrafingLeft || _characterState != CharacterState.StrafingRight){
                anim.SetTrigger("strafe");
                SendRPC("SetPlayerTriggerAnimState", RPCMode.Others, "strafe");
			}

			if(h < -0.2f) {
				_characterState = CharacterState.StrafingLeft;
			}
			else if(h > 0.2f){
				_characterState = CharacterState.StrafingRight;
			}		

		
			speedMultipler = 1.5f;
			targetSpeed *= walkSpeed;
		
		}
		else{

			if(_characterState != CharacterState.Trotting) {

                anim.SetTrigger("walk");
                SendRPC("SetPlayerTriggerAnimState", RPCMode.Others, "walk");
			}
	
			if (Time.time - trotAfterSeconds > walkTimeStart)
			{
				targetSpeed *= trotSpeed;
				_characterState = CharacterState.Trotting;
			}
			else
			{
				targetSpeed *= walkSpeed;
				_characterState = CharacterState.Walking;
			}
			
		}

		if(!isMoving) {

            //change anim to idle
            //idle SendRPC("SetPlayerTriggerAnimState", RPCMode.Others, "attack");
		}
	
		moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
		
		// Reset walk time start when we slow down
		if (moveSpeed < walkSpeed * 0.3f)
			walkTimeStart = Time.time;

		motor.walkingSpeed = moveSpeed;

		if(bMouseButtonUp)
        {
            float fClosestDistance = 9999.0f;
			anim.SetTrigger("attack");
            SendRPC("SetPlayerTriggerAnimState", RPCMode.Others, "attack");

            AttackFireAnimator.Play("FireAnim4");

            Object[] objectsFound = FindObjectsOfType(typeof(GameObject));
            GameObject closestGameObj = null;
            
            foreach (GameObject objFound in objectsFound)
            {
                GameObject gameObjFound = (GameObject)objFound;
                
                if(gameObjFound == transform.gameObject)
                    continue;

                if (gameObjFound.GetComponent<Entity>())
                {
                    float fDistance = Vector3.Distance(gameObjFound.transform.position, transform.position);
                    if (fDistance < fClosestDistance)
                    {
                        fClosestDistance = fDistance;
                        closestGameObj = gameObjFound;

                    } 
                }
            }

            if (closestGameObj != null && fClosestDistance < 3.0f)
            {
                GetComponent<Entity>().AttackOnce(closestGameObj);
            }
		}
	}

   public void SetPlayerAnimStateTrigger(string strValueName)
    {
        if (IsServer())
        {
            Debug.LogError("This function should not be called from server!");
            return;
        }
        anim.SetTrigger(strValueName);
    }

	static float ClampAngle (float angle, float min, float max) {

		if (angle < -360.0f)
			angle += 360.0f;

		if (angle > 360.0f)
			angle -= 360.0f;

		return Mathf.Clamp (angle, min, max);
	}
}


