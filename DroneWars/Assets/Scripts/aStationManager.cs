using UnityEngine;
using System.Collections;

public class aStationManager : MonoBehaviour {


	private GameObject[] players;
	private Vector3 droneCameraPosition, lobbyCameraPosition;
	private Quaternion lobbyCameraRotation;
	private bool gameStarted, gameActive, gameOver;
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

	public Camera myCamera;
	public GameObject AbandonedStation;
	public GameObject[] stationSpawnPointLocations;

	//create enums to access the different available game states
	public enum GameModes { FreeForAll, Idle };
	public enum GameMaps { AbandonedStation, None };
	//variables to hold the current game states
	public GameModes gameMode = GameModes.Idle;
	public GameMaps gameMap = GameMaps.None;

	// Use this for initialization
	void Start () {		 
		gameStarted = false;
		lobbyCameraPosition = myCamera.transform.position;
		lobbyCameraRotation = myCamera.transform.rotation;
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
			spawnPlayers ();
			//get the starting position for our camera
			for(int i = 0; i < players.Length; i++)
			{
				NetworkView view = players[i].networkView;
				if(view.isMine)
				{
					droneCameraPosition = players[i].transform.position;
					droneCameraPosition.y -= 10;
					myCamera.transform.position = droneCameraPosition;
					Quaternion playerRotation = players[i].transform.rotation;
					myCamera.transform.rotation = playerRotation;


				}
			}

			Debug.Log(players.Length);
			Debug.Log(myCamera.transform.position);
		}
		else if(gameActive)
		{

		}
		else if(gameOver)
		{

		}
	}

	//Display the HUD for the player
	void onGUI()
	{

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
		Debug.Log("In spawn Players");
		//move players to the correct spawnpoint locations via which map is being played
		switch(gameMap)
		{
		case GameMaps.AbandonedStation:
			int count = stationSpawnPointLocations.Length;
			Vector3 playerPosition;
			for(int i = 0; i < players.Length; i++)
			{
				GameObject currentPlayer = players[i];

					int randomPoint = Random.Range(0, count);
					playerPosition = stationSpawnPointLocations[randomPoint].transform.position;
					currentPlayer.transform.position = playerPosition;
				GameObject cam = GameObject.Find("Main Camera");
				cam.AddComponent<MouseLook>();
				if(players.Length > 1)
					networkView.RPC("placePlayer", RPCMode.Others, currentPlayer, playerPosition);
			}
			
			break;
		default:
			Debug.Log(gameMap);
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
	void placePlayer(GameObject player, Vector3 position)
	{
		player.transform.position = position;

	}

}
