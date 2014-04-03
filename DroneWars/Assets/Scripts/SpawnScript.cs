using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {
	
	/**
	public GameObject polarityPrefab;

	public GameObject snidenPrefab;
	public GameObject vistigePrefab;  
	**/
	public GameObject tridentPrefab;
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
			//playerPrefab.GetComponent<SphereCollider>().enabled = false;
			//playerPrefab = polarityPrefab; //uncomment when polarity drone implemented
				break;
			case 2:
			//playerPrefab = tempDrone;
			//playerPrefab.GetComponent<SphereCollider>().enabled = false;
			playerPrefab = tridentPrefab; //uncomment when trident drone implemented
			//playerPrefab.GetComponent<MeshCollider>().enabled = false;
				break;
			case 3:
			playerPrefab = tempDrone;
			//playerPrefab.GetComponent<SphereCollider>().enabled = false;
			//playerPrefab = vistigePrefab; //uncomment when vistige drone implemented
				break;
			case 4:
			playerPrefab = tempDrone;
			//playerPrefab.GetComponent<SphereCollider>().enabled = false;
			//playerPrefab = snidenPrefab; //uncomment when sniden drone implemented
				break;
			default:
				break;
		}		 

		//Network.Instantiate(PlayerPrefab, sPoint.transform.position, Quaternion.identity, 0);
		GameObject gameObject = (GameObject)Network.Instantiate(playerPrefab, sPoint, Quaternion.identity, 0);
		return gameObject;
	}

}
