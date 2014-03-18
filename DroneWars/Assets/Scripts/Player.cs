using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public GameObject nameDisplay;
	private string playerName;

	// Use this for initialization
	void Start () {
		playerName = PlayerPrefs.GetString("playerName");
	}
	
	// Update is called once per frame
	void Update () {
		nameDisplay.GetComponent<TextMesh>().text = playerName;
		nameDisplay.transform.position = new Vector3(transform.position.x, transform.position.y - 40, transform.position.z);
	}
}
