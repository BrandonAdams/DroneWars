using UnityEngine;
using System.Collections;

public class MiniMap : MonoBehaviour {
	
	
	private GameObject toFollow;

	
	public GameObject ToFollow {
		get {return toFollow;}
		set {toFollow = value; }
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		this.transform.position = new Vector3(toFollow.transform.position.x,350,toFollow.transform.position.z);
		
		
	}
}