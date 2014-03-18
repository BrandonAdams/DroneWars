using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {
	
	/**
	public GameObject polarityPrefab;
	public GameObject tridentPrefab;
	public GameObject snidenPrefab;
	public GameObject vistigePrefab;  
	**/
	public GameObject tempDrone;
	private GameObject playerPrefab;			

	// Use this for initialization
	void Start () {	

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public GameObject SpawnPlayer(int selectedDrone)
	{
	
		//creating our player at a point outside of the game arena for preparation to start the game
		Vector3 sPoint = new Vector3(-25, 30, -560);
		//set the players drone to the selected drone type
		switch(selectedDrone)
		{
			case 1:
			playerPrefab = tempDrone; //DELETE THIS WHEN ACTUAL DRONES ARE IMPLEMENTED
			// playerPrefab = polarityPrefab; //uncomment when polarity drone implemented
				break;
			case 2:
			playerPrefab = tempDrone;
			// playerPrefab = tridentPrefab; //uncomment when polarity drone implemented
				break;
			case 3:
			playerPrefab = tempDrone;
			// playerPrefab = vistigePrefab; //uncomment when polarity drone implemented
				break;
			case 4:
			playerPrefab = tempDrone;
			// playerPrefab = snidenPrefab; //uncomment when polarity drone implemented
				break;
			default:
				break;
		}
		playerPrefab.GetComponent<SphereCollider>().enabled = false; //TODO: CHANGE THIS TO THE COLLIDER OF THE PREFAB WHEN THEY ARE BROUGHT IN

		//Network.Instantiate(PlayerPrefab, sPoint.transform.position, Quaternion.identity, 0);
		GameObject gameObject = (GameObject)Network.Instantiate(playerPrefab, sPoint, Quaternion.identity, 0);
		return gameObject;
	}

}
