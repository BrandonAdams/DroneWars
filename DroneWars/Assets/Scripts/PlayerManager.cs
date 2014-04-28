using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {	

	//private variables
	private GameObject _gameCamera, whirringBladesObject1, whirringBladesObject2, shootingSoundObject;	
	private List<GameObject> _players;
	private ArrayList _bullets;
	private bool /*_isGameOver, */_isGameStarted, _isFiringPrimary/*, _isFiringSecondary*/ = false;
	private bool _isTheHost;
	private NetworkViewID _myID;
	private NetworkView _myView;
	private int[] _spawnPointNumbers;
	private int _bulletFiringTime, _bulletFiringTimer, _bulletCounter, _missleFiringTime, _missleFiringTimer, _missleCounter;


	//public variables
	public Bullet bullet;
	public Missle missle;

	//public accessor and getters
	public bool IsTheHost{
		get { return _isTheHost; }	
		set { _isTheHost = value; }
	}
	public int[] SpawnPointNumbers{
		get { return _spawnPointNumbers; }
		set { _spawnPointNumbers = value; }
	}
	public List<GameObject> Players {
		get{return _players;}
	}
	public bool IsGameStarted
	{
		get { return _isGameStarted;}
		set { _isGameStarted = value; }
	}

	void Awake() {
		_players = new List<GameObject>();
	}

	// Use this for initialization
	void Start () 
	{
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
	void Update () 
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
					Vector3 bulletSpawnPosition = myPlayer.transform.position; 
					bulletSpawnPosition += myPlayer.transform.forward * 8;
					//bullet.initialize(4.0f, 3.0f, _myView.observed.name, _bulletCounter);
					bullet.Speed = 4.0f;
					Network.Instantiate(bullet, bulletSpawnPosition, myPlayer.transform.rotation, 1);
					shootingSoundObject.audio.Play();
					_bullets.Add(bullet);
					//reset the firing timer
					_bulletFiringTimer = 0;
					_bulletCounter++;

				}
			}
			//Increase the timer for the missle
			_missleFiringTimer++;

		}
	}
	
	void OnGUI()
	{
		//DISPLAY PLAYER HUD HERE			
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
				if(_players.Count > 0)
				{
					target = _players[0].transform;
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

	
}
