using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager : MonoBehaviour {

	//private variables
	private ArrayList _allBullets;
	private Bullet bulletPointer;
	private int _lifeSpan, _pointAtLocation;
	private float _shellSpeed, _shellPower;
	private GameObject _owner;

	//accessors and getters
	public GameObject Owner{
		set { _owner = value; }
	}

	// Use this for initialization
	void Start () {
		_allBullets = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
		//creating a temp variable to manipulate our bullets
		Bullet aBullet;
		//iterating over all of our active bullets
	}

	void initialize(int droneType, int amountOfPooledBullets = 80, float aSpeed = 20, float aPower = .01f, int aLifeTime = 200)
	{
		int numberOfPooledBullets = amountOfPooledBullets;
		
		for(int i = 0; i < numberOfPooledBullets; i++)
		{
			Bullet bullet = new Bullet();
			//TODO: check drone type and set bullet speed and power here
			_shellSpeed = 20.0f;
			_shellPower = .01f;
			_lifeSpan = aLifeTime;
			Network.Instantiate(bullet, bullet.Position, bullet.transform.rotation, 10);
			_allBullets.Add(bullet);
		}
		_pointAtLocation = 0;
		bulletPointer = (Bullet)_allBullets[_pointAtLocation];

	}

	bool fireBullet(Vector3 startingPoint, Quaternion startingRotation)
	{
		//if the bullet that power 
		if(bulletPointer.IsActive)
		{
			//fire the bullet
			bulletPointer.fire(_owner.transform.position, _owner.transform.rotation, _shellSpeed, _shellPower);
			//set the bullet to be active
			bulletPointer.IsActive = true;
			//set the bulletPointer to point at the next bullet in line
			if(_pointAtLocation < _allBullets.Count - 1)
			{
				_pointAtLocation++;
				bulletPointer = (Bullet)_allBullets[_pointAtLocation];
			}
			else if(_pointAtLocation >= _allBullets.Count - 1)
			{
				_pointAtLocation = 0;
				bulletPointer = (Bullet)_allBullets[_pointAtLocation];
			}
			//move the pointer to the next object in our all bullets array

		}
		Debug.Log("Reloading more ammo");
		return false;
	}

	/**
	bool fireBullet(Vector3 startingPoint, Quaternion startingRotation)
	{
		//check to see if we have any bullets in our pool of bullets available
		if(_pooledBullets.Count > 0)
		{
			Bullet firedShell = _pooledBullets.Pop();

			//have the bullet fire
			firedShell.fire(owner.transform.position, owner.transform.rotation, _shellSpeed, _shellPower);

			//add this bullet to the list of active bullets
			_activeBullets.Push(firedShell);

			return true;
		}
		Debug.Log("Reloading more ammo");
		return false;
	}
	**/



}
