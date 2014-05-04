using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	/****   VARIABLES FOR MOVEMENT  ******/
	public float strafeSpeed = 0.0f;
	public float liftSpeed = 0.0f;
	public float declSpeed = 0.95f;
	private float speedMod = .05f;
	
	public float rotationSpeed = 5.0f;
	public float maxLift = 1.0f;
	public float tiltSpeed = 1.0f;
	private bool isFalling = false;
	private bool canFly = true;	
	//private float timer = 0;

	//The Character Controller attached to this gameObject
	private CharacterController characterController;	
	// The linear gravity factor. Made available in the Editor.
	private float gravity = 100.0f;	
	// mass of vehicle
	private float mass = 33.7f;	
	// The cummulative rotation about the y and x-Axis.
	private float cummulativeRotationYAxis;
	private float cummulativeRotationXAxis;	
	// The rotation factor, this will control the speed we rotate at.
	private float rotationSensitivity = 70.0f;
	private float maxUpRollRotation = 20;
	private float maxDownRollRotation = -20;
	
	//movement variables - exposed in inspector panel
	public float maxSpeed = 2.0f; 		//maximum speed of vehicle
	public float maxForce = 15.0f; 		// maximimum force allowed
	public float friction = 0.997f; 	// multiplier decreases speed
	
	//movement variables - updated by this component
	private float speed = 0.0f;  		//current speed of vehicle
	private Vector3 steeringForce; 		// force that accelerates the vehicle
	private Vector3 velocity; 			//change in position per second
	/**************************************/

	public GameObject cam;	
	// The initial orientation.
	private Quaternion initialOrientation;

	//Individual Player Variables
	public GameObject nameDisplay;
	private string playerName;
	private float _health;
	
	private int _bulletFiringTime, _bulletFiringTimer, /*_bulletCounter*/_missleFiringTime, _missleFiringTimer, _missleCounter;
	private GameObject _gameCamera, /*whirringBladesObject1, whirringBladesObject2, */shootingSoundObject;	
	//private ArrayList _bullets;
	private bool /*_isGameOver, */_isGameStarted, _isFiringPrimary/*, _isFiringSecondary*/ = false;
	private RaycastHit hit;

	//Networking Variables
	private bool _isTheHost;
	private NetworkViewID _myID;
	private NetworkView _myView;

	//public variables
	public Bullet bullet;
	public Missle missle;

	public bool IsTheHost{
		get { return _isTheHost; }	
		set { _isTheHost = value; }
	}

	public bool IsGameStarted
	{
		get { return _isGameStarted;}
		set { _isGameStarted = value; }
	}

	public float Health {
		get {return _health;}
	}

	// Use this for initialization
	void Start () {

		playerName = PlayerPrefs.GetString("playerName");
		_health = 400.0f;

		//save the quaternion representing our initial orientation from the transform
		initialOrientation = transform.rotation;

		//set the cummulativeRotation to zero.
		cummulativeRotationYAxis = 0.0f;
		cummulativeRotationXAxis = 0.0f;

		_myID = this.networkView.viewID;
		_myView = NetworkView.Find(_myID);

		//_bullets = new ArrayList();
		_bulletFiringTimer = _missleFiringTimer = 1000;
		/*_bulletCounter =*/ _missleCounter = 0;
		_bulletFiringTime = 5;
		_missleFiringTime = 300;
		//whirringBladesObject1 = this.transform.FindChild("AudioBlades1").gameObject;
		//whirringBladesObject2 = this.transform.FindChild("AudioBlades2").gameObject;
		shootingSoundObject = this.transform.FindChild("AudioShootGunSound").gameObject;
	}
	
	// Update is called once per frame
	void Update () {

		//DISPLAYING THE PLAYERS NAME ON THE SCREEN
		nameDisplay.GetComponent<TextMesh>().text = playerName;
		nameDisplay.transform.position = new Vector3(transform.position.x, transform.position.y - 40, transform.position.z);

		if(this.GetComponent<NetworkView>().viewID.isMine)
		{
			characterController = this.GetComponent<CharacterController>();
			/*if(_isGameOver)
			{
				
			}*/
			if(_isGameStarted)
			{ 		
				//players = GameObject.FindGameObjectsWithTag("Drone");
				checkKeyDown();
				checkKeyUp();

				SteerWithMouse();
				// calculate steering forces that will change our position in space
				//transform.position += CalcForces();
				characterController.Move(CalcForces());
				if(_isFiringPrimary)
				{
					
					_bulletFiringTimer++;
					if(_bulletFiringTimer > _bulletFiringTime)
					{
						//create a bullet and place it just in front of the player
						//myPlayer = GameObject.Find(_myView.observed.name);
						//Vector3 bulletSpawnPosition = myPlayer.transform.position; 
						//bulletSpawnPosition += myPlayer.transform.forward * 8;
						//bullet.initialize(4.0f, 3.0f, _myView.observed.name, _bulletCounter);
						//bullet.Speed = 4.0f;
						//Network.Instantiate(bullet, bulletSpawnPosition, myPlayer.transform.rotation, 1);

						GameObject myPlayer = GameObject.Find(_myView.observed.name);
						Physics.Raycast(myPlayer.transform.position, myPlayer.transform.forward, out hit, 500.0f);
						Debug.Log (hit.collider.gameObject);
						
						if(hit.collider.gameObject.tag == "Drone") {
							
							Debug.Log ("Hit Enemy Drone");
							hit.collider.gameObject.GetComponent<Player>().updateHealth(-20.0f);
						}
						
						shootingSoundObject.audio.Play();
						//_bullets.Add(bullet);
						//reset the firing timer
						//_bulletFiringTimer = 0;
						//_bulletCounter++;
						
					}
				}
				//Increase the timer for the missle
				_missleFiringTimer++;

								
			}


		}

	}

	public void updateHealth(float change) {

		_health += change;

	}

	private Vector3 KeyboardAcceleration ()
	{					
		//dv is desired velocity
		Vector3 dv = Vector3.zero;

		if(canFly == true)
		{
			if(Input.GetKey (KeyCode.W) && !(Input.GetKey (KeyCode.S)))
			{
				if(speed < maxSpeed)
				{
					speed += speedMod;
				}
				else 
				{
					speed = maxSpeed;					
				}
			}
			else if(!(Input.GetKey (KeyCode.W)) && speed> 0)
			{
				speed *= declSpeed;
				if(speed <=1)
				{
					speed = 0;
				}
			}			
			
			if(Input.GetKey (KeyCode.S) && !(Input.GetKey (KeyCode.W)))
			{
				if(speed > -maxSpeed)
				{
					speed -= speedMod;;
				}
				else
				{
					speed = -maxSpeed;
				}
			}
			else if(!(Input.GetKey (KeyCode.S)) && speed < 0)
			{
				speed *= declSpeed;
				if(speed > -1)
				{
					speed = 0;
				}
			}
			
			if((Input.GetKey (KeyCode.D)) && !(Input.GetKey (KeyCode.A)))
			{
				if(strafeSpeed < maxSpeed)
				{
					strafeSpeed += speedMod;
				}
				else 
				{
					strafeSpeed = maxSpeed;
				}
				
			}
			else if(!(Input.GetKey (KeyCode.D)) && strafeSpeed> 0)
			{
				strafeSpeed *= declSpeed;
				if(strafeSpeed <=1)
				{
					strafeSpeed = 0;
				}
			}
			
			
			if((Input.GetKey (KeyCode.A)) && !(Input.GetKey (KeyCode.D)))
			{
				if(strafeSpeed > -maxSpeed)
				{
					strafeSpeed -= speedMod;
					
				}
				else
				{
					strafeSpeed = -maxSpeed;
					
				}				
			}
			else if(!(Input.GetKey (KeyCode.A)) && strafeSpeed < 0)
			{
				strafeSpeed *= declSpeed;
				if(strafeSpeed >= -1)
				{
					strafeSpeed = 0;
				}
			}
			
			if(Input.GetKey( KeyCode.Space) && !(Input.GetKey (KeyCode.LeftShift)))
			{
				
				if(liftSpeed < maxLift)
				{
					liftSpeed += speedMod;
				}
				else 
				{
					liftSpeed = maxLift;
				}
			}
			else if(!(Input.GetKey (KeyCode.Space)) && liftSpeed > 0)
			{
				liftSpeed *= declSpeed;
				if(liftSpeed <= .5)
				{
					liftSpeed = 0;
				}
			}			
			
			if(Input.GetKey(KeyCode.LeftShift) && !(Input.GetKey (KeyCode.Space)))
			{
				if(liftSpeed > -maxLift)
				{
					liftSpeed -= speedMod;
				}
				else 
				{
					liftSpeed = -maxLift;
				}
			}
			else if(!(Input.GetKey (KeyCode.LeftShift)) && liftSpeed < 0)
			{
				liftSpeed *= declSpeed;
				if(liftSpeed >= -.5)
				{
					liftSpeed = 0;
				}
			}
			
			if(!(Input.GetKey( KeyCode.Space)) && !(Input.GetKey (KeyCode.LeftShift)) || Input.GetKey( KeyCode.Space) && (Input.GetKey (KeyCode.LeftShift)))
			{
				if(liftSpeed > 0)
				{
					if(liftSpeed <= .5)
					{
						liftSpeed = 0;
					}
				}
				else if(liftSpeed < 0)
				{
					if(liftSpeed >= -.5)
					{
						liftSpeed = 0;
					}
				}
				else
					liftSpeed *= declSpeed;
			}			
		}

		#region Drone shut down parameters
		/***** AUGMENTING OUR SPEED VARIABLES IF THE DRONE IS SHUT DOWN ******/
		/**
		if(isFalling)
		{
			//Debug.Log("Player is Falling");
			if(liftSpeed < 0)
			{
				liftSpeed += speedMod;
			}
			else if (liftSpeed > 0)
			{
				liftSpeed -= speedMod;
				
			}
			if(strafeSpeed < 0)
			{
				strafeSpeed += speedMod;
				
			}
			else if (strafeSpeed > 0)
			{
				strafeSpeed -= speedMod;
				
			}
			
			liftSpeed -= ((timer/60) * 1.0f);

			if(liftSpeed < -maxLift) 
				liftSpeed = -maxLift;

			timer += 1.0f;


		}
		else if(isFalling == false)
		{
			//Debug.Log("Player is not Falling");
			if(timer > 0)
			{
				timer -= 4;
				liftSpeed += ((timer/60) * -.05f);

				if(liftSpeed < -maxSpeed)
					liftSpeed = -maxSpeed;

				Debug.Log(liftSpeed);

			}
			else{
				canFly = true;
				timer = 0;
			}
		}
**/
#endregion Drone shut down parameters

		Vector3 planeVector = Vector3.zero;
		planeVector.x = this.transform.forward.x;
		planeVector.z = this.transform.forward.z;
		planeVector *= speed;
		dv += planeVector;
		
		Vector3 strafeVector = new Vector3();
		strafeVector.x = transform.right.x;
		strafeVector.z = transform.right.z;
		strafeVector.Normalize();
		strafeVector *= strafeSpeed;
		dv += strafeVector;
		
		Vector3 liftVector = new Vector3();
		liftVector.y = transform.up.y;
		liftVector.Normalize();
		liftVector *= liftSpeed;

		dv += liftVector; 
				
		return dv;
	}
	
	// Calculate the forces that alter velocity
	private Vector3 CalcForces ()
	{
		steeringForce = Vector3.zero;
		if(!canFly)
		{
			Vector3 gravityVector = Vector3.down;
			gravityVector += new Vector3() * gravity;
			steeringForce += (gravityVector * mass);
		}
		else
			steeringForce += KeyboardAcceleration();


		return steeringForce;		
	}
	
	// if steering forces exceed maxForce they are set to maxForce
	private void ClampForces ()
	{
		if (steeringForce.magnitude > maxForce) {
			steeringForce.Normalize ();
			steeringForce *= maxForce;
		}
	}

	void SteerWithMouse ()
	{
		if(canFly)
		{
			//Get the left/right Input from the Mouse and use time along with a scaling factor 
			// to add a controlled amount to our cummulative rotation about the axis'.
			cummulativeRotationYAxis += Input.GetAxis ("Mouse X") * Time.deltaTime * rotationSensitivity;
			cummulativeRotationXAxis -= Input.GetAxis ("Mouse Y") * Time.deltaTime * rotationSensitivity;
			
			//CLAMP THE X ROTATION TO PREVENT IT FROM LOOKING TOO FAR
			if(cummulativeRotationXAxis > maxUpRollRotation)
			{
				cummulativeRotationXAxis = maxUpRollRotation;
			}		
			else if(cummulativeRotationXAxis < maxDownRollRotation)
			{
				cummulativeRotationXAxis = maxDownRollRotation;
			}
			
			
			//Create a Quaternion representing our current cummulative rotation around the y-axis. 
			Quaternion currentRotation = Quaternion.Euler (cummulativeRotationXAxis, cummulativeRotationYAxis, 0.0f);
			
			/** Use the quaternion to update the transform of the vehicle's Game Object based on 
			 initial orientation and the accumulated rotation since the original orientation. **/
			transform.rotation = initialOrientation * currentRotation;
		}		
	}

	void checkKeyDown()
	{
		if(Input.GetKeyDown (KeyCode.Mouse0))
		{
			_isFiringPrimary = true;		
		}
		
		if(Input.GetKeyDown (KeyCode.Mouse1))
		{
			
			Transform target = this.transform;
			//Raycast -should be what we use or some other construct
			GameObject myPlayer = GameObject.Find(_myView.observed.name);
			Physics.Raycast(myPlayer.transform.position, myPlayer.transform.forward, out hit, 500.0f);
			Debug.Log (hit.collider.gameObject);

			
			//For now ill just take the first player that connects to you
			if(hit.collider.gameObject.tag == "Drone")
			{
				target = hit.collider.gameObject.transform;
			}

			
			//Fire secondary weapon here 
			if(_missleFiringTimer > _missleFiringTime)
			{
				//create a bullet and place it just in front of the player
				Vector3 missleSpawnPosition = myPlayer.transform.position; 
				missleSpawnPosition += myPlayer.transform.forward * 8;
				Debug.Log(missle);

				Missle r = (Missle)Network.Instantiate(missle, missleSpawnPosition, myPlayer.transform.rotation, 1);

				if(target != this.transform)
					networkView.RPC("initiateMissile", RPCMode.AllBuffered, r.networkView.viewID, target.gameObject.networkView.viewID);

				//reset the firing timer
				_missleFiringTimer = 0;
				_missleCounter++;				
			}
		}

		if(Input.GetKeyDown (KeyCode.F))
		{ 
			isFalling = !isFalling;
			if(isFalling)
				canFly = false;
			else
				canFly = true;  //TODO: Need to change this to a different variable that will allow the drone to cycle on before giving control back to player
		}
		
	}

	void checkKeyUp()
	{
		if(Input.GetKeyUp (KeyCode.Mouse0))
		{
			_isFiringPrimary = false;
			
		}
	}	

	
	/**
     * A method to check if a non it player is close to a player that is it
     **/
	void checkPlayerDistances(GameObject[] playerList)
	{
		//go through the list of players and check to see which player is me
		for(int i=0; i < playerList.Length; i++)
		{
			//GameObject thePlayer = playerList[i];
			NetworkViewID theID = playerList[i].networkView.viewID;
			NetworkView theView = NetworkView.Find(theID);
			
			//If the network view is mine then check myself against all other player in vicinity
			if(theView.isMine)
			{
				//go through the list of players and check my distance from them
				for(int j=0; j < playerList.Length; j++)
				{
					NetworkViewID playerID = playerList[i].networkView.viewID;
					NetworkView playerView = NetworkView.Find(playerID);
					if (playerView.isMine)
						continue; //don't check the distance against myself
					else{
						
					}                   
				}
				break; //don't continue to search for me in the list since I have been found.
			}		
		}
	}//END checkPlayerDistances

	[RPC]
	void initiateMissile(NetworkViewID missile, NetworkViewID target) {
		
		GameObject m = NetworkView.Find(missile).observed.gameObject;
		m.GetComponent<Missle>().Initialize(.001f, 50, target, _myView.observed.name, 1);
		
	}

}
