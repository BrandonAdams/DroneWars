using UnityEngine;
using System.Collections;

public class aStationManager : MonoBehaviour {

	private Vector3[] spawnPointLocations;
	private Vector3[] availableSpawnPoints;
	private GameObject[] players;
	private int positionsTaken;
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

	//create enums to access the different available game states
	public enum GameModes { FreeForAll, Idle };
	public enum GameMaps { AbandonedStation, None };
	//variables to hold the current game states
	public GameModes gameMode = GameModes.Idle;
	public GameMaps gameMap = GameMaps.None;

	// Use this for initialization
	void Start () {
		spawnPointLocations = new Vector3[6]; 
		gameStarted = false;
		lobbyCameraPosition = myCamera.transform.position;
		lobbyCameraRotation = myCamera.transform.rotation;
		positionsTaken = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(gameStarted)
		{
			gameStarted = false;
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

		int spawnPoint; 
		for(int i = 0; i < players.Length; i++)
		{
			spawnPoint = Random.Range(0, (availableSpawnPoints.Length - 1));
			Debug.Log(spawnPointLocations.Length);
			Vector3 newPosition = (Vector3)availableSpawnPoints[spawnPoint];
			Debug.Log(newPosition);
			//players[i].transform.position = newPosition;
			positionsTaken++;

			//availableSpawnPoints = new Vector3[6 - positionsTaken];
			//for(int i = 0; i <
		}

		networkView.RPC("placePlayers", RPCMode.Others, players);

	}



	/**
	 * A method for checking the distance players are from each other
	 **/
	void checkPlayerDistances(GameObject thePlayer, GameObject[] listOfPlayers)
	{
		for(int i = 0; i < listOfPlayers.Length ; i++)
		{
			//action to take if players are close enough
		}
	}

}
