using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager : MonoBehaviour {

	//public variables
	public GameObject bullet;
	//private variables
	private List<GameObject> _allBullets;
	private GameObject bulletPointer;
	private int _lifeSpan, _pointAtLocation;
	private float _shellSpeed, _shellPower;
	private GameObject _owner;
	private int counter;

	//accessors and getters
	public GameObject Owner{
		set { _owner = value; }
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		/**
		//creating a temp variable to manipulate our bullets
		Bullet aBullet;

		//iterating over all of our active bullets - aging them (if too old then reset the bullet)
		for(int i = 0; i < _allBullets.Count; i++)
		{
			if(_allBullets[i].GetComponent<Bullet>().IsActive)
			{
				_allBullets[i].GetComponent<Bullet>().BulletAge++;
				Debug.Log(_allBullets[i].GetComponent<Bullet>().BulletAge);
				if(_allBullets[i].GetComponent<Bullet>().BulletAge >= _lifeSpan)
				{
					_allBullets[i].GetComponent<Bullet>().IsActive = false;
					Debug.Log("BULLET DEACTIVATED");
					_allBullets[i].GetComponent<Bullet>().transform.position = _allBullets[i].GetComponent<Bullet>().MagLocation;
				}
			}

		}
		**/
	}

	public void initialize(int droneType, float aSpeed, int amountOfPooledBullets = 80, float aPower = .01f, int aLifeTime = 200)
	{
		int numberOfPooledBullets = amountOfPooledBullets;
		bullet.GetComponent<Bullet>().Speed = 0;
		bullet.GetComponent<Bullet>().IsActive = false;
		counter = 0;

		if(_allBullets == null)
		{
			_allBullets = new List<GameObject>();
		}
		for(int i = 0; i < numberOfPooledBullets; i++)
		{

			//TODO: check drone type and set bullet speed and power here
			_shellSpeed = aSpeed;
			_shellPower = aPower;
			_lifeSpan = aLifeTime;
			Network.Instantiate(bullet, bullet.GetComponent<Bullet>().MagLocation, _owner.transform.rotation, 10 + counter);
			Debug.Log(_allBullets);

			_allBullets.Add(bullet);
			counter++;
		}

		_pointAtLocation = 0;
		bulletPointer = _allBullets[_pointAtLocation].gameObject;

	}

	public bool fireBullet(Vector3 startingPoint, Quaternion startingRotation)
	{
		counter++;
		//if the bullet that power 
		Debug.Log ("Bullet IsActive = " + bulletPointer.GetComponent<Bullet>().IsActive);
		if(!bulletPointer.GetComponent<Bullet>().IsActive)
		{
			//fire the bullet
			bulletPointer.GetComponent<Bullet>().fire(_owner.transform.position, _owner.transform.rotation, _shellSpeed, _shellPower);
			//set the bullet to be active
			bulletPointer.GetComponent<Bullet>().IsActive = true;
			Debug.Log ("Bullet IsActive = " + bulletPointer.GetComponent<Bullet>().IsActive);
			//set the bulletPointer to point at the next bullet in line
			if(_pointAtLocation < _allBullets.Count - 1)
			{
				_pointAtLocation++;
				bulletPointer = null;
				bulletPointer = _allBullets[_pointAtLocation].gameObject;
				Debug.Log("Switched Bullet");
				Debug.Log ("Bullet IsActive = " + bulletPointer.GetComponent<Bullet>().IsActive);
			}
			//if we are at the end of our array point at the beginning bullet again
			else if(_pointAtLocation >= _allBullets.Count - 1)
			{
				_pointAtLocation = 0;
				bulletPointer = _allBullets[_pointAtLocation].gameObject;
				Debug.Log("Back to beginning of array");
				Debug.Log ("Bullet IsActive = " + bulletPointer.GetComponent<Bullet>().IsActive);
			}
			Debug.Log("Fired Bullet" + counter);
			return true;
		}
		Debug.Log("Reloading more ammo"+ counter);
		return false;

	}

}
