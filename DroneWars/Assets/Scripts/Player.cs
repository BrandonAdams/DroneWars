using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed = 0.0f;
	public float strafeSpeed = 0.0f;
	public float liftSpeed = 0.0f;
	public float declSpeed = 0.95f;
	public float speedMod = 0.05f;
	
	public float rotationSpeed = 5.0f;
	public float maxSpeed = 2.0f;
	public float maxLift = 3.0f;
	public float tiltSpeed = 1.0f;
	
	
	private float timer = 0;
	
	public GameObject cam;
	
	private bool isGravOn = false;
	private bool canFly = true;
	
	// The initial orientation.
	private Quaternion initialOrientation;
	
	// The cummulative rotation about the y and x-Axis.
	private float cummulativeRotationYAxis;
	private float cummulativeRotationXAxis;	
	
	// The rotation factor, this will control the speed we rotate at.
	public float rotationSensitvity = 500.0f;
	private float maxUpRollRotation = -300;
	private float maxDownRollRotation = -430;

	public GameObject nameDisplay;
	private string playerName;
	
	private int _bulletFiringTime, _bulletFiringTimer, _bulletCounter, _missleFiringTime, _missleFiringTimer, _missleCounter;
	private GameObject _gameCamera, whirringBladesObject1, whirringBladesObject2, shootingSoundObject;	
	private ArrayList _bullets;
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

	// Use this for initialization
	void Start () {

		playerName = PlayerPrefs.GetString("playerName");

		//save the quaternion representing our initial orientation from the transform
		initialOrientation = transform.rotation;

		//set the cummulativeRotation to zero.
		cummulativeRotationYAxis = 0.0f;
		cummulativeRotationXAxis = 0.0f;

		_myID = this.networkView.viewID;
		_myView = NetworkView.Find(_myID);

		_bullets = new ArrayList();
		_bulletFiringTimer = _missleFiringTimer = 1000;
		_bulletCounter = _missleCounter = 0;
		_bulletFiringTime = 5;
		_missleFiringTime = 300;
		whirringBladesObject1 = this.transform.FindChild("AudioBlades1").gameObject;
		whirringBladesObject2 = this.transform.FindChild("AudioBlades2").gameObject;
		shootingSoundObject = this.transform.FindChild("AudioShootGunSound").gameObject;
	}
	
	// Update is called once per frame
	void Update () {

		nameDisplay.GetComponent<TextMesh>().text = playerName;
		nameDisplay.transform.position = new Vector3(transform.position.x, transform.position.y - 40, transform.position.z);

		if(this.GetComponent<NetworkView>().viewID.isMine)
		{

			/*if(_isGameOver)
		{
			
		}*/
			if(_isGameStarted)
			{ 		
				//players = GameObject.FindGameObjectsWithTag("Drone");
				checkKeyDown();
				checkKeyUp();
				
				if(_isFiringPrimary)
				{
					
					_bulletFiringTimer++;
					if(_bulletFiringTimer > _bulletFiringTime)
					{
						//create a bullet and place it just in front of the player
						GameObject myPlayer = GameObject.Find(_myView.observed.name);
						//Vector3 bulletSpawnPosition = myPlayer.transform.position; 
						//bulletSpawnPosition += myPlayer.transform.forward * 8;
						//bullet.initialize(4.0f, 3.0f, _myView.observed.name, _bulletCounter);
						//bullet.Speed = 4.0f;
						//Network.Instantiate(bullet, bulletSpawnPosition, myPlayer.transform.rotation, 1);
						
						Physics.Raycast(myPlayer.transform.position, myPlayer.transform.forward, out hit, 1000.0f);
						Debug.DrawRay(myPlayer.transform.position, transform.TransformDirection(myPlayer.transform.forward) * 1000.0f, Color.white);
						Debug.Log (hit.collider.gameObject);
						
						if(hit.collider.gameObject.tag == "Drone") {
							
							Debug.Log ("Hit Enemy Drone");
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

			SteerWithMouse();
			
			updateMovement();
		}

	}

	void SteerWithMouse ()
	{


		//Get the left/right Input from the Mouse and use time along with a scaling factor 
		// to add a controlled amount to our cummulative rotation about the y-Axis.
		cummulativeRotationYAxis += Input.GetAxis ("Mouse X") * Time.deltaTime * rotationSensitvity;
		cummulativeRotationXAxis -= Input.GetAxis ("Mouse Y") * Time.deltaTime * rotationSensitvity;	
		
		clampXRotation ();
		
		//Create a Quaternion representing our current cummulative rotation around the y-axis. 
		Quaternion currentRotation = Quaternion.Euler (cummulativeRotationXAxis, cummulativeRotationYAxis, 0.0f);
		
		// Use the quaternion to update the transform of the vehicle's Game Object based on 
		// initial orientation and the accumulated rotation since the original orientation. 
		transform.rotation = initialOrientation * currentRotation;
		
	}

	void checkKeyDown()
	{
		if(Input.GetKeyDown (KeyCode.Mouse0))
		{
			_isFiringPrimary = true;
			whirringBladesObject1.audio.mute = true;
			whirringBladesObject2.audio.mute = true;
			
		}
		
		if(Input.GetKeyDown (KeyCode.Mouse1))
		{
			
			Transform target = this.transform;
			//Raycast -should be what we use or some other construct
			
			//For now ill just take the first player that connects to you
			if(this.GetComponent<PlayerManager>().Players.Count > 0)
			{
				target = this.GetComponent<PlayerManager>().Players[0].transform;
			}
			
			
			
			//Fire secondary weapon here 
			if(_missleFiringTimer > _missleFiringTime)
			{
				//create a bullet and place it just in front of the player
				GameObject myPlayer = GameObject.Find(_myView.observed.name);
				Vector3 missleSpawnPosition = myPlayer.transform.position; 
				missleSpawnPosition += myPlayer.transform.forward * 8;
				Missle r = (Missle)Network.Instantiate(missle, missleSpawnPosition, myPlayer.transform.rotation, 1);
				networkView.RPC("initiateMissile", RPCMode.AllBuffered, r.networkView.viewID, target.gameObject.networkView.viewID);
				//r.Initialize(.001f, 50, target.gameObject.networkView.viewID, _myView.observed.name, 1);
				//reset the firing timer
				_missleFiringTimer = 0;
				_missleCounter++;				
			}
		}
		
	}
	
	[RPC]
	void initiateMissile(NetworkViewID missile, NetworkViewID target) {
		
		GameObject m = NetworkView.Find(missile).observed.gameObject;
		m.GetComponent<Missle>().Initialize(.001f, 50, target, _myView.observed.name, 1);
		
	}
	
	
	void checkKeyUp()
	{
		if(Input.GetKeyUp (KeyCode.Mouse0))
		{
			_isFiringPrimary = false;
			whirringBladesObject1.audio.mute = false;
			
		}
	}
	
	void checkForWinningPlayer(GameObject[] playerList)
	{		
		
		
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

	/*void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject != this)
			Debug.Log("hit something:" + collision.gameObject.name);// modify this to stop movement
	}*/

	void clampXRotation()
	{
		if(cummulativeRotationXAxis > maxUpRollRotation)
		{
			cummulativeRotationXAxis = maxUpRollRotation;
		}		
		else if(cummulativeRotationXAxis < maxDownRollRotation)
		{
			cummulativeRotationXAxis = maxDownRollRotation;
		} 
		
	}



	void updateMovement()
	{
		
		if(canFly == true)
		{
			#region arrows
			#endregion
			
			#region Movement
			
			
			#region W
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
			#endregion W
			
			#region S
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
			#endregion S
			
			
			#region A&D
			
			#endregion
			#region D
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
			#endregion
			#region A
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
			#endregion
			
			
			#region Space and Shift
			#endregion
			#region Space
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
			
			#endregion
			#region Shift
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
			#endregion
			
			
			
			#endregion Movement
			
			
		}
		
		#region checkGrav
		if(Input.GetKeyDown (KeyCode.F))
		{ 
			if(isGravOn == false)
			{
				isGravOn = true;
				canFly = false;
				
				Debug.Log (isGravOn);
			}
			else if(isGravOn == true)
			{
				isGravOn = false;
				Debug.Log (isGravOn);
			}
		}
		#endregion
		#region gravOn
		if(isGravOn == true)
		{
			if(speed < 0)
			{
				speed += speedMod;
				
			}
			else if (speed > 0)
			{
				speed -= speedMod;
				
			}
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
			
			liftSpeed += ((timer/60) * -0.05f);
			
			timer++;
		}
		#endregion
		#region gravOff
		if(isGravOn == false)
		{
			if(timer >= 0)
			{
				timer -= 2;
				liftSpeed += ((timer/60) * -0.05f);
			}
			else{
				canFly = true;
			}
		}
		#endregion
		
		
		//Vector3 moveForward = new Vector3();
		//moveForward.x = transform.forward.x;
		//moveForward.z = transform.forward.z;
		//transform.forward *= speed;
		transform.position += transform.forward * speed;
		//this.rigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);// if not kinematic (kinematic will tell you collisions not act on them)

		//Vector3 strafe = new Vector3();
		//strafe.x = transform.right.x;
		//strafe.z = transform.right.z;
		//strafe.Normalize();
		transform.position += transform.right *strafeSpeed;
		
		//Vector3 lift = new Vector3();
		//lift.y = transform.up.y;
		//lift.Normalize();
		transform.position += transform.up * liftSpeed;
	}
}
