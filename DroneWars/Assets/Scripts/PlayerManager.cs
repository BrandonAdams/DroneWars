using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {	

	//private variables

	private List<GameObject> _players;
	private bool /*_isGameOver, */_isGameStarted, _isFiringPrimary/*, _isFiringSecondary*/ = false;
	private NetworkViewID _myID;
	private NetworkView _myView;
	private int[] _spawnPointNumbers;


	//public accessor and getters
	public int[] SpawnPointNumbers{
		get { return _spawnPointNumbers; }
		set { _spawnPointNumbers = value; }
	}
	public List<GameObject> Players {
		get{return _players;}
	}

	void Awake() {
		_players = new List<GameObject>();
	}

	// Use this for initialization
	void Start () 
	{
		_myID = this.networkView.viewID;
		_myView = NetworkView.Find(_myID);
	
	}
	
	// Update is called once per frame
	void Update () 
	{        

	}
	
	void OnGUI()
	{
		//DISPLAY PLAYER HUD HERE			
	}



	
}
