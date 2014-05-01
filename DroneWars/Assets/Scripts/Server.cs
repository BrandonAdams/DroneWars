 using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour {
	#region Instance Variables
	//enums to access the current the server state
	public enum ServerState { notInitialized, Initialized };
	//create enums to access the different available game states
	public enum GameModes { FreeForAll, Lobby };
	
	//variables to hold the current game and server states
	public GameModes gameMode = GameModes.Lobby;
	public ServerState currentState = ServerState.notInitialized; 
	//network information
	private string connectionIP = "129.21.28.5";
	public int connectPort = 25002;

	private NetworkViewID ID;
	public NetworkViewID NetworkID{
		get { return ID; }	
	}

	public GameObject gameCamera;
	
	public bool isPlaying = false;
	//private float currentTimer  = 0;
	//private float gameTimer = 60; // total amount of game time 	
	private string playerName = "";
	//private string endingMessage = "";
	
	/*********    Lobby Window Variables**********/
	public Font gameFont; //adding our fonts
	private Rect lobbyWindow; //our window
	private bool inHost, isHost, inClient, inJoinGame; //variables to change window contents
	public bool IsHost{
		get { return isHost; }
	}
	private int _selectedDrone = 1/*, _totalNumberOfPlayers = 0*/; 
	private float screenHalfWidth, lobbyXPosition, lobbyYPosition, lobbyWindowWidth, lobbyWindowHeight;
	private float selectingButtonHeight = 60.0f;
	private float updatingButtonHeight = 40.0f;
	public Texture polarityImage, tridentImage, snidenImage, vistigeImage;
	private Texture currentDisplayedImage;
	private string _polarityName = "Polarity", _tridentName = "Trident", _snidenName = "Sniden", _vistigeName = "Vistige";
	private string tempName = "Guest";



	#endregion

	// Use this for initialization
	void Start () 
	{
		screenHalfWidth = Screen.width / 2;
		lobbyXPosition = (screenHalfWidth / 2) / 2;
		lobbyYPosition = 30;
		lobbyWindowWidth = screenHalfWidth + (screenHalfWidth / 2);
		lobbyWindowHeight = Screen.height - 60;
		lobbyWindow = new Rect(lobbyXPosition,lobbyYPosition, lobbyWindowWidth, lobbyWindowHeight);
		audio.volume = 3;
	}
	#region OnGUI and Update Method
	void OnGUI()
	{		
		if (currentState == ServerState.notInitialized)
		{
			lobbyWindow = GUILayout.Window (1, lobbyWindow, CreateLobbyWindow, "LOBBY");
		}
	}
	
	// Update is called once per frame
	void Update () 
	{

	
	}
	#endregion
	#region Lobby Window content
	/** 
	 * 	Creating the lobby window
	 **/
	void CreateLobbyWindow (int windowID)
	{
		GUI.backgroundColor = Color.red;
		//setting our font size
		GUI.skin.textField.fontSize = 20;
		GUI.skin.textArea.fontSize = 24;	
		GUI.skin.button.fontSize = 24;
		GUI.skin.label.fontSize = 24;
		//setting our font
		GUI.skin.font = gameFont;
		//leave a gap after the header
		GUILayout.Space(30);
		//Display the information requested by user
		if(inHost)
		{
			//Entering the IP address
			GUI.Label(new Rect(70, 50, 240.0f, 30.0f), "Enter your IP: ");
			connectionIP = GUI.TextField(new Rect(280, 50, 130.0f, 30.0f), connectionIP);
			//Displaying the Port number to connect to
			GUI.Label(new Rect(70, 90, 200.0f, 30.0f), "Port Number: ");
			GUI.Label(new Rect(280, 90, 100.0f, 30.0f), connectPort + "");
			
			//display the current pilot name
			GUI.Label(new Rect(70, 150, 200.0f, 30.0f), "Pilot Name: ");
			GUI.Label(new Rect(280, 150, 450.0f, 30.0f), playerName);
			
			//display the current selected drone
			string pickedDrone;
			switch(_selectedDrone){
			case 1:
				pickedDrone = _polarityName;
				break;
			case 2:
				pickedDrone = _tridentName;
				break;
			case 3:
				pickedDrone = _vistigeName;
				break;
			case 4:
				pickedDrone = _snidenName;
				break;
			default:
				pickedDrone = "none";
				break;
			}				
			GUI.Label(new Rect(70, 180, 200.0f, 30.0f), "Drone: ");
			GUI.Label(new Rect(280, 180, 450.0f, 30.0f), pickedDrone);
			
			//button to join the game
			if(GUI.Button(new Rect(100, 250, 250.0f, selectingButtonHeight), "Create Game"))
			{
				isHost = true;
				startServer(connectPort);
				inJoinGame = true;
				inHost = false;
			}
			if(GUI.Button (new Rect(lobbyWindowWidth - 370.0f, (lobbyWindowHeight - updatingButtonHeight) - 20, 350.0f, updatingButtonHeight), "Return to Main Lobby"))
			{
				inHost = false;
			}
		}
		else if(inClient)
		{
			//Entering the IP address
			GUI.Label(new Rect(70, 50, 350.0f, 30.0f), "Enter Host IP: ");
			connectionIP = GUI.TextField(new Rect(280, 50, 130.0f, 30.0f), connectionIP);
			//Entering the Port number
			GUI.Label(new Rect(70, 90, 200.0f, 30.0f), "Port Number: ");
			GUI.Label(new Rect(280, 90, 100.0f, 30.0f), connectPort + "");
			//connectPort = int.Parse(GUI.TextField(new Rect(280, 90, 100.0f, 30.0f), connectPort.ToString()).ToString()); //This is just in case we want to allow players to change the port number

			//display the current pilot name
			GUI.Label(new Rect(70, 150, 200.0f, 30.0f), "Pilot Name: ");
			GUI.Label(new Rect(280, 150, 450.0f, 30.0f), playerName);

			//display the current selected drone
			string pickedDrone;
			switch(_selectedDrone){
			case 1:
				pickedDrone = _polarityName;
				break;
			case 2:
				pickedDrone = _tridentName;
				break;
			case 3:
				pickedDrone = _vistigeName;
				break;
			case 4:
				pickedDrone = _snidenName;
				break;
			default:
				pickedDrone = "none";
				break;
			}				
			GUI.Label(new Rect(70, 180, 200.0f, 30.0f), "Drone: ");
			GUI.Label(new Rect(280, 180, 450.0f, 30.0f), pickedDrone);

			//button to join the game
			if(GUI.Button(new Rect(100, 250, 250.0f, selectingButtonHeight), "Join Game"))
			{
				//JOIN THE SELECTED SERVER HERE
				inJoinGame = true;
				isHost = false;
				inClient = false;
				connectToServer(connectionIP, connectPort);
			}

			if(GUI.Button (new Rect(lobbyWindowWidth - 370.0f, (lobbyWindowHeight - updatingButtonHeight) - 20, 350.0f, updatingButtonHeight), "Return to Main Lobby"))
			{
				inClient = false;
			}
		}
		else if(inJoinGame)
		{
			GUI.Label(new Rect((lobbyWindowWidth / 2) - 90.0f, 20, 190.0f, 30.0f), "GAME LOBBY");

			if(isHost)
			{
				if(GUI.Button (new Rect(lobbyWindowWidth - 370.0f, (lobbyWindowHeight - updatingButtonHeight) - 20, 350.0f, updatingButtonHeight), "Disconnect Server"))
				{
					inClient = inHost = isHost = inJoinGame = false;				
					//DISCONNECT THE SERVER HERE
					DisconnectNetwork();
				}

				if(GUI.Button (new Rect(30, (lobbyWindowHeight - updatingButtonHeight) - 50, 200.0f, 47), "Deploy"))
				{
					//launch the game here
					//GameObject gameManager = GameObject.Find("GameManager_GO");
					currentState = ServerState.Initialized;
					//gameManager.GetComponent<aStationManager>().GameStarted = true; 
					networkView.RPC("startUpGame", RPCMode.All);  //start the game
					Screen.lockCursor = true;
					Screen.showCursor = false;
				}
			}
			else
			{
				if(GUI.Button (new Rect(lobbyWindowWidth - 370.0f, (lobbyWindowHeight - updatingButtonHeight) - 20, 350.0f, updatingButtonHeight), "Disconnect"))
				{
					inClient = inHost = isHost = inJoinGame = false;
					//DISCONNECT FROM SERVER HERE
					DisconnectNetwork();
				}


			}

			GameObject[] listOfPlayers = GameObject.FindGameObjectsWithTag("Drone");
			float labelYPosition = 110;
			if(listOfPlayers.Length > 0)
			{
				for(int i = 0; i < listOfPlayers.Length; i++)
				{
					//Debug.Log (listOfPlayers[i]);
					string pName = listOfPlayers[i].GetComponent<PlayerLabel>().PlayerName;
					GUI.Label (new Rect(90, labelYPosition, 450.0f, 40.0f), pName);
					labelYPosition += 40;
				}
			}

		}
		else
		{
			GUI.Label(new Rect((lobbyWindowWidth / 2) - 90.0f, 20, 180.0f, 30.0f), "MAIN LOBBY");
			GUI.Label(new Rect(70, 70, 200.0f, 30.0f), "Select a Drone");
			if(GUI.Button (new Rect(90, 110, 150.0f, updatingButtonHeight), "Polarity"))
			{
				//make playerPrefab this prefab
				_selectedDrone = 1;
				audio.Play();
			}
			if(GUI.Button (new Rect(90, 160, 150.0f, updatingButtonHeight), "Trident"))
			{
				//make playerPrefab this prefab
				_selectedDrone = 2;
			}
			if(GUI.Button (new Rect(90, 210, 150.0f, updatingButtonHeight), "Vistige"))
			{
				//make playerPrefab this prefab
				_selectedDrone = 3;
			}
			if(GUI.Button (new Rect(90, 260, 150.0f, updatingButtonHeight), "Sniden"))
			{
				//make playerPrefab this prefab
				_selectedDrone = 4;
			}			

			//PLACE IMAGE OF DRONE HERE		
			switch(_selectedDrone){
			case 1:
				currentDisplayedImage = polarityImage;
				break;
			case 2:
				currentDisplayedImage = tridentImage;
				break;
			case 3:
				currentDisplayedImage = vistigeImage;
				break;
			case 4:
				currentDisplayedImage = snidenImage;
				break;
			default:
				currentDisplayedImage = polarityImage;
				break;
			}
			GUI.DrawTexture(new Rect(430, 80, 250, 250), currentDisplayedImage, ScaleMode.ScaleToFit, true, 0);	

			if(GUI.Button(new Rect(40, 310, 250.0f, selectingButtonHeight), "Join a Game"))
			{
				//check to make sure playing does not enter a blank name
				if(tempName.Length > 15)
				{
					tempName = tempName.Substring(0, 15);
				}
				playerName = tempName;
				if(playerName != "")
				{				
					//store the player name locally
					PlayerPrefs.SetString ("playerName", playerName);
					inClient = true;
				}
			}
			GUI.Label(new Rect(120, 375, 100.0f, 30.0f), "- OR -");

			if(GUI.Button(new Rect(40, 410, 250.0f, selectingButtonHeight), "Host a Game"))
			{

				//check to make sure playing does not enter a blank name
				//tempName = GUI.TextField(new Rect(410, 370, 300.0f, 30.0f), tempName);
				if(tempName.Length > 15)
				{
					tempName = tempName.Substring(0, 15);
				}
				playerName = tempName;
				if(playerName != "")
				{				
					//store the player name locally
					PlayerPrefs.SetString ("playerName", playerName);
					inHost = true;
				}
			}
			tempName = GUI.TextField(new Rect(410, 370, 300.0f, 30.0f), tempName);
			GUI.Label(new Rect(490, 405, 200.0f, 30.0f), "Pilot Name");
			GUI.Label(new Rect(430, 440, 300.0f, 30.0f), "(max 15 characters)");
		}
	}//END CREATELOBBYWINDOW
	#endregion
	#region Network Methods

	void startServer (int theConnectionPort, int theTotalNumberOfConnections = 6)
	{
		Network.InitializeServer(theTotalNumberOfConnections, theConnectionPort, Network.HavePublicAddress());
	}

	void connectToServer(string theIPAddress, int theConnectionPort)
	{
		Network.Connect(theIPAddress, theConnectionPort);
		//Debug.Log(playerName + " just connected");
	}
	

	//we are informed that we were successful in initializing the server
	void OnServerInitialized ()
	{
		//Running as Server
		createMyPlayer();
	}
	
	// we are informed that a player has just connected to us (the server)
	void OnPlayerConnected (NetworkPlayer player)
	{
		//Debug.Log ("Player " + player + " connected from " + player.ipAddress + ":" + player.port);
	}

	/**
	 * If you are a client this is where you will create and intantiate your object
	 **/
	void OnConnectedToServer ()
	{
		//Running as Client
		createMyPlayer();
	}
	
	public void DisconnectNetwork()
	{
		Network.Disconnect();
	}

	//called on both client AND server
	void OnDisconnectedFromServer (NetworkDisconnection info)
	{
		//reload the application so we can start over
		Application.LoadLevel (Application.loadedLevel);	
	}
	
	//some player has disconnected. 
	//We'd better clean up their stuff
	void OnPlayerDisconnected (NetworkPlayer player)
	{
		//Debug.Log ("Clean up after player " + player);
		Network.RemoveRPCs (player);
		Network.DestroyPlayerObjects (player);
	}

	void OnFailedToConnect(NetworkConnectionError error) 
	{
		Debug.Log("Could not connect to server: " + error);
	}

	#endregion
	#region Class Methods
	void createMyPlayer()
	{
		//Find the game object in charge of creating a new player and create the selected drone
		GameObject spawnManager = GameObject.Find("SpawnManager_GO");
		GameObject newPlayer = spawnManager.GetComponent<SpawnScript>().SpawnPlayer(2); //TODO fix this hack and make it dynamic
		newPlayer.GetComponent<PlayerManager>().IsTheHost = isHost;
		//change the name of the new player in the heirarchy to the player's name
		GameObject.Find("tridentDrone(Clone)").name = playerName; //TODO Fix this hack and make it dynamic
		//change the playerName variable in the PlayerLabel class to display the players name in game
		newPlayer.GetComponent<PlayerLabel>().PlayerName = playerName;
		//get the network view id of the new player
		ID = newPlayer.networkView.viewID;		
		//Debug.Log(ID + " : This code is located in the createMyPlayer method in the Server class.");

		//send out an RPC to everyone that we have a new player using the network view ID we just obtained
		networkView.RPC ("addPlayer", RPCMode.AllBuffered, playerName, ID);
		networkView.RPC ("SendToServer", RPCMode.All, playerName + " just connected");
	}

	#endregion
	#region RPC Methods
	[RPC]
	void SendToServer(string msg)
	{
		Debug.Log(msg);
	}

	[RPC]
	void addPlayer (string theName, NetworkViewID theID)
	{			



		//get the NetworkView component of the new gameobject using the ID sent over the RPC
		NetworkView view = NetworkView.Find(theID);

		if(!view.isMine)
		{			
			//using the NetworkView of that object, get the gameobject in question and save it to a variable
			GameObject newPlayer = view.observed.gameObject;
			//set the name of the new player in the heiarchy to that players name
			newPlayer.name = theName;
			//change the playerName of the object using the gameObject to reference that variable
			newPlayer.GetComponent<PlayerLabel>().PlayerName = theName;	

			//GameObject player = GameObject.Find(theName);
		    GameObject.Find(playerName).GetComponent<PlayerManager>().Players.Add(newPlayer);
			//Debug.Log(player.GetComponent<PlayerManager>().Players.Count);

		}		
	}

	[RPC]
	void startUpGame()
	{
		//get the game manager object and change the gameStarted variable to true to initialize camera settings and player placement
		GameObject gameManager = GameObject.Find("GameManager_GO");
		currentState = ServerState.Initialized;
		gameManager.GetComponent<aStationManager>().GameStarted = true;
	}

	#endregion


}//END SERVER CLASS
