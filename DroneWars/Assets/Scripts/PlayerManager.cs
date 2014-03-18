using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {	
	
	private GameObject gameCamera;
	
	private  GameObject[] players;
	public GameObject[] Players {get{return players;}}	
	private bool gameOver = false;
	private NetworkViewID myID;
	private NetworkView myView;

	private int[] spawnPointNumbers;
	public int[] SpawnPointNumbers{
		get { return spawnPointNumbers; }
		set { spawnPointNumbers = value; }
	}

	private bool isTheHost;
	public bool IsTheHost{
		get { return isTheHost; }	
		set { isTheHost = value; }
	}

	// Use this for initialization
	void Start () 
	{
		myID = this.networkView.viewID;
		myView = NetworkView.Find(myID);
		//making a check to see if the player network id is the same when created and instantiated
		Debug.Log(myID + " : This code is located in PlayerManager in the start method.");
	}
	
	// Update is called once per frame
	void Update () 
	{        
		if(gameOver)
		{
			
		}
		else
		{ 		
			players = GameObject.FindGameObjectsWithTag("Drone");
		}
	}
	
	void OnGUI()
	{
		//DISPLAY PLAYER HUD HERE			
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
			GameObject thePlayer = playerList[i];
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
