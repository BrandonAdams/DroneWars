using UnityEngine;
using System.Collections;

public class Missle : MonoBehaviour {

	//private variables
	private float _speed;
	private float _power;
	private int _missleTag, _missleLifePeriod, _missleCurrentLife, _missleGuidanceActive;
	private string _missleName;
	private NetworkViewID _myID, _preyID;
	//private bool _fired;
	//private NetworkView _myView;

	//public accessors and getters
	public float Speed{
		get { return _speed; }
		set { _speed = value; }
	}
	public float Power{
		get { return _power; }
		set { _power = value; }
	}
	public int MissleTag{
		get { return _missleTag; }
	}
	public string MissleName{
		get { return _missleName; }
	}
	public NetworkViewID PreyID{
		get { return _preyID; }
		set { _preyID = value; } 
	}

	// Use this for initialization
	void Start () {
		//total life span of a missle
		_missleLifePeriod = 500;
		//time it takes for the missle to start guiding itself toward its target
		//_missleGuidanceActive = 10;
		//missles current life
		_missleCurrentLife = 0;
		//_fired = false;
		//network components
		//_myID = this.networkView.viewID;
		//_myView = NetworkView.Find(_myID);
		_speed = 1.0f;
	
	}

	void OnCollisionEnter(Collision collision) {

		Debug.Log ("COLLLLLLLLLLLLLISIONNNNNNNNNNN");

		if(_preyID != NetworkViewID.unassigned){
			NetworkView targetView = NetworkView.Find(_preyID);

			if(collision.collider.gameObject == targetView.observed.gameObject) { //checks if the collided object is the drone that we are following

				collision.collider.gameObject.GetComponent<Player>().networkHealthUpdate(15.0f);
				_missleCurrentLife = _missleLifePeriod + 1;

			}

		}else if(collision.collider.gameObject.tag == "Drone") { //checks if the collided object is a Drone, even if it isn't one we are seeking

			collision.collider.gameObject.GetComponent<Player>().networkHealthUpdate(15.0f);
			_missleCurrentLife = _missleLifePeriod + 1;

		}else {

			//Sets missle's life over maximum, destorying missle
			_missleCurrentLife = _missleLifePeriod + 1;

		}



	}

	public void Initialize(float missleSpeed, float misslePower, NetworkViewID target, string playerName, int tagID)
	{
		//instantiating our variables
		_speed = missleSpeed;
		_power = misslePower;
		_missleName = playerName + "Missle" + tagID;
		_missleTag = tagID;
		Debug.Log (target);
		Debug.Log (playerName);
		_preyID = target;
		//_fired = true;
		//Debug.Log (_preyID);
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log("PreyID: " + _preyID);
		//Debug.Log ("Speed: " + _speed);
		//Debug.Log("Before: " + transform.forward);

		//Hunting our target
		//Does Not begin searching until fired, which is noted in the initialize function
	
		HuntTarget();
		
		//Debug.Log("After: " + transform.forward);

		//transform.forward.Normalize();
		transform.position += transform.forward * _speed;
		//Debug.Log("My ID  = " + _myID);
		//make our missle older
		_missleCurrentLife++;
		//our missle has died of old age

		if(_missleCurrentLife > _missleLifePeriod)
		{
			GameObject[] liveMissles = GameObject.FindGameObjectsWithTag("Missle");
			for(int i = 0; i < liveMissles.Length; i++)
			{
				if(liveMissles[i].networkView.isMine)
				{
					Debug.Log("Destroyed missle");
					Network.Destroy(liveMissles[i]);
					break;
				}
			}
			
		}

	
	}


	void HuntTarget()
	{

		if(_preyID != NetworkViewID.unassigned) {

		
			//Debug.Log("Hunt PreyID: " + _preyID);
			NetworkView targetView = NetworkView.Find(_preyID);
			//make sure our target is still in the game before steering toward it
			if(targetView)
			{
				GameObject target = targetView.observed.gameObject;
				//get our new vector heading via our targets position and mine
				Vector3 newHeading = target.transform.position - transform.position;
				transform.forward = newHeading - transform.forward * _speed;
				//transform.forward = newHeading.normalized;
			}
		}
	}
}
