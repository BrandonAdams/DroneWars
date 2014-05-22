using UnityEngine;
using System.Collections;

public class aStationManager : MonoBehaviour {

	//private variables
	private GameObject[] players;
	private Vector3 droneCameraPosition, lobbyCameraPosition;
	private Quaternion lobbyCameraRotation;
	private bool gameStarted, gameActive, gameOver;
	public GameObject _player;

	//variables related to damage/ warnings
	private bool takingDamage = false;
	private bool lockedOn = false;
	private string altReady = "Health";
	private float _amountHealthNewValue = 500.0f;
	private float _totalHealth = 500.0f;


	//Public variables
	// source images
	public Texture2D map;
	public Texture2D healthBack;
	public Texture2D cDownMissle;
	public Texture2D engine;
	public Texture healthBar;
	public Texture2D healthTop;
	public Texture2D hurtLeft;
	public Texture2D hurtRight;
	public Texture2D aim;
	public Texture2D hitmarker;
	public Texture2D lockOn;
	public int healthDisplay = 100;
	public Camera myCamera;
	public GameObject AbandonedStation;
	public GameObject[] stationSpawnPointLocations;


	//public accessor and getters
	public bool GameStarted{
		get { return gameStarted; }
		set { gameStarted = value; }
	}
	public bool GameActive{
		get { return gameActive; }
		set { gameActive = value; }
	}
	public bool GameOver{
		get { return gameOver; }
		set { gameOver = value; }
	}
	public float HealthNewValue {
		get {return _amountHealthNewValue;}
		set {_amountHealthNewValue = value; }
	}
	public float TotalHealth {
		get {return _totalHealth;}
		set {_totalHealth = value; }
	}

	//create enums to access the different available game states
	public enum GameModes { FreeForAll, Idle };
	public enum GameMaps { AbandonedStation, None };
	//variables to hold the current game states
	public GameModes gameMode = GameModes.Idle;
	public GameMaps gameMap = GameMaps.None;

	public GameObject miniMap;
	public GameObject enemyMarker;

	// Use this for initialization
	void Start () {		 
		gameStarted = false;
		//lobbyCameraPosition = myCamera.transform.position;
		//lobbyCameraRotation = myCamera.transform.rotation;

	}
	
	// Update is called once per frame
	void Update () {
		if(gameStarted)
		{

			gameStarted = false;
			gameMap = GameMaps.AbandonedStation;  //THIS NEEDS TO CHANGE ********** NOT PERMANENT!!!!!!!!!!!!!!!!!!!!!!!!!
			//have the host collect all the players playing
			players = GameObject.FindGameObjectsWithTag("Drone");
			//have the host place all the players around the map
			for(int i = 0; i < players.Length; i++)
			{
				NetworkView view = players[i].networkView;
				GameObject thePlayer = view.observed.gameObject;
				
				if(thePlayer.GetComponent<Player>().IsTheHost)
				{
					spawnPlayers ();
				}
			}
			//get the starting position for our camera
			for(int i = 0; i < players.Length; i++)
			{
				NetworkView view = players[i].networkView;
				if(view.isMine)
				{
					droneCameraPosition = players[i].transform.position;
					droneCameraPosition.z += 5;
					myCamera.transform.position = droneCameraPosition;
					Quaternion playerRotation = players[i].transform.rotation;
					myCamera.transform.rotation = playerRotation;
					myCamera.transform.parent = players[i].transform;
					players[i].AddComponent<CameraScript>();
					players[i].GetComponent<Player>().IsGameStarted = true;
					networkView.RPC("sendMessage", RPCMode.All, view.observed.name + " camera moved");
					_player = players[i];
				}
			}
			//start updating gameplay material
			gameActive = true;


		}
		else if(gameActive)
		{			 

		}
		else if(gameOver)
		{

		}
	}

	//Display the HUD for the player
	void OnGUI()
	{

       	if(gameActive)
		{
			/**
			GUI.skin.label.fontSize = 24;
			

			
			// Place the lower two dials (cooldown - left & health - right)
			GUI.Label (new Rect (0,Screen.height - cDown.height, cDown.width,cDown.height), cDown);
			GUI.Label (new Rect (Screen.width - health.width,Screen.height - health.height,health.width,health.height), health);
			
			//put alt text lable relative to cDown
			GUI.Label(new Rect(cDown.width / 2 - cDown.width / 6, Screen.height - cDown.height / 2 - cDown.height / 6, cDown.width, cDown.height), altReady);
			
			//output health to player
			//GUI.Label(new Rect(Screen.width - cDown.width / 2 - cDown.width / 6, Screen.height - cDown.height / 2 - cDown.height / 7, cDown.width, cDown.height), "" + healthDisplay);
			**/
			if(_player.GetComponent<Player>().TakingDamage){
				
				// Place the hurt makers (can't be seen unless hit)
				GUI.Label (new Rect (0,0 , hurtLeft.width, hurtLeft.height), hurtLeft);
				GUI.Label (new Rect (Screen.width-hurtRight.width, 0, hurtRight.width, hurtRight.height), hurtRight);
			}
			// Place the 'locked-on' warning
			/*if(lockedOn){
				GUI.Label (new Rect (Screen.width/2 - lockOn.width, Screen.height - lockOn.height*2, lockOn.width , lockOn.height), lockOn);
			}
			*/
			// Place the Map on screen
			GUI.Label (new Rect(Screen.width - (map.width - 40), 0, map.width, map.height * .7f), map); //400
			//Place the crosshairs
			GUI.Label (new Rect (Screen.width/ 2 - aim.width/2 + 15, Screen.height/ 2 - aim.height/2 + 15, aim.width * .75f, aim.height * .75f), aim);

			if(_player.GetComponent<Player>().HitEnemy) {
				GUI.Label (new Rect (Screen.width/ 2 - aim.width/2, Screen.height/ 2 - aim.height/2, aim.width * .75f, aim.height * .75f), hitmarker);
			}

			//health bar back
			GUI.DrawTexture(new Rect(30, Screen.height - 130, healthBack.width * .80f, healthBack.height * .80f ),healthBack);

			if(_player.GetComponent<Player>().Engines) {
				GUI.DrawTexture(new Rect(30, Screen.height - 130, engine.width * .80f, engine.height * .80f ), engine);
			}

			if(_player.GetComponent<Player>().MissleTimer >= 300) {
				GUI.DrawTexture(new Rect(30, Screen.height - 130, cDownMissle.width * .80f, cDownMissle.height * .80f ), cDownMissle);
			}

			//Place the health bar
			GUI.DrawTexture(new Rect(30 + (80 * (1 - (_player.GetComponent<Player>().Health *_player.GetComponent<Player>().HealthPercentage/100))), Screen.height - 131, (healthBar.width * .80f) * (_player.GetComponent<Player>().Health * _player.GetComponent<Player>().HealthPercentage/100), 100), healthBar);

			GUI.DrawTexture(new Rect(30, Screen.height - 130, healthTop.width * .80f, healthTop.height * .80f ),healthTop);


		}
	}

	public void StartGame(string map)
	{			
		
		switch (map)
		{
		case "AbandenedStation":			
			//Start the GameMode arena + move all players			
			ChangeArena((int)gameMap);
			networkView.RPC("ChangeArena", RPCMode.Others, (int)gameMap);						
			break;
		default:
			break;
		}		
	}

	/**
	 * A method to reset all player scores to 0
	 **/
	[RPC]
	public void ResetScores()
	{

	}

	[RPC]
	public void ChangeArena(int selectedMap)
	{
		GameMaps prev = gameMap;
		gameMap = (GameMaps)selectedMap;
		//Debug.Log(gameMap);
		//Debug.Log(prev);
		switch (gameMap)
		{
		case GameMaps.AbandonedStation:
			AbandonedStation.SetActive(true);
			break;		
		case GameMaps.None:
			switch (prev)
			{
			case GameMaps.AbandonedStation:
				AbandonedStation.SetActive(false);
				break;			
			default:
				break;
			}
			break;
		default:
			break;
		}
		//Debug.Log(AbandonedStation.activeInHierarchy);		
	}

	/**
	 * Selecting a spawn point for the player
	 **/
	void spawnPlayers()
	{

		//move players to the correct spawnpoint locations via which map is being played
		switch(gameMap)
		{
		case GameMaps.AbandonedStation:
			int count = stationSpawnPointLocations.Length;
			Vector3 playerPosition;

			for(int i = 0; i < players.Length; i++)
			{
				GameObject currentPlayer = players[i];

				//Debug.Log("moved player " + currentPlayer.name);
				if(players[i].networkView.viewID.isMine) {
					miniMap = (GameObject)GameObject.Instantiate(miniMap, this.transform.position, this.transform.rotation);
					miniMap.GetComponent<MiniMap>().ToFollow = players[i];
				}else {
					enemyMarker = (GameObject)GameObject.Instantiate(enemyMarker, this.transform.position, this.transform.rotation);
					enemyMarker.GetComponent<MiniMap>().ToFollow = players[i];
				}
				
				int randomPoint = Random.Range(0, count);
				playerPosition = stationSpawnPointLocations[randomPoint].transform.position;
				NetworkViewID currentPlayerViewID = currentPlayer.networkView.viewID;
				if(players.Length > 1)
				{
					networkView.RPC("placePlayer", RPCMode.All, currentPlayerViewID, playerPosition);
				}
				else{
					currentPlayer.transform.position = playerPosition;
					//GameObject cam = GameObject.Find("Main Camera");
				}					
			}			
			break;
		default:
			break;
		}

	}

	/**
	 * A method for checking the distance players are from each other
	 **/
	float checkPlayerDistances(GameObject thePlayer)
	{
		float distance = 5;
		for(int i = 0; i < players.Length ; i++)
		{
			//action to take if players are close enough
			if(players[i] == thePlayer)
				continue;
			else{
				float calcDistance = Vector3.Distance(players[i].transform.position, thePlayer.transform.position);
				if(calcDistance <= distance)
				{
					distance = calcDistance;
				}
			}
		}
		return distance;
	}

	[RPC]
	void sendMessage(string msg)
	{
		Debug.Log(msg);
	}

	[RPC]
	void placePlayer(NetworkViewID playerNetworkID, Vector3 position)
	{
		NetworkView playerView = NetworkView.Find(playerNetworkID);
		GameObject playerToPlace = playerView.observed.gameObject;
		playerToPlace.transform.position = position;

		if(playerNetworkID.isMine) {
			miniMap = (GameObject)GameObject.Instantiate(miniMap, this.transform.position, this.transform.rotation);
			miniMap.GetComponent<MiniMap>().ToFollow = NetworkView.Find(playerNetworkID).observed.gameObject;
		}else {
			enemyMarker = (GameObject)GameObject.Instantiate(enemyMarker, this.transform.position, this.transform.rotation);
			enemyMarker.GetComponent<MiniMap>().ToFollow = NetworkView.Find(playerNetworkID).observed.gameObject;
		}
	}

	[RPC]
	void updatePlayerHealth(NetworkViewID playerID)
	{
		GameObject player = NetworkView.Find(playerID).observed.gameObject;
		GameObject stationManager = GameObject.Find("GameManager_GO");
		stationManager.GetComponent<aStationManager>().HealthNewValue = 500.0f;
		stationManager.GetComponent<aStationManager>().HealthNewValue *= player.GetComponent<Player>().HealthPercentage;
		stationManager.GetComponent<aStationManager>().TotalHealth = stationManager.GetComponent<aStationManager>().HealthNewValue;
	}
}
