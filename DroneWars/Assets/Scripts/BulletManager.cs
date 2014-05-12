using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager : MonoBehaviour {

	private Stack<Bullet> _allBullets;
	private Stack<Bullet> _activeBullets;
	private Stack<Bullet> _pooledBullets;
	private int _lifeSpan;
	private GameObject owner;

	public Stack<Bullet> ActiveBullets
	{
		get {return _activeBullets;}
	}

	// Use this for initialization
	void Start () {
		_allBullets = new Stack<Bullet>();
		_pooledBullets = new Stack<Bullet>();
		_activeBullets = new Stack<Bullet>();
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
			//TODO: check drone type and set bullet speed here
			_lifeSpan = aLifeTime;
			Network.Instantiate(bullet, bullet.Position, bullet.transform.rotation, 10);
			_allBullets.Push(bullet);
		}
		_pooledBullets = _allBullets;
	}

	bool fireBullet(Vector3 startingPoint, Quaternion startingRotation)
	{
		//check to see if we have any bullets in our pool of bullets available
		if(_pooledBullets.Count > 0)
		{
			Bullet firedShell = _pooledBullets.Pop();

			//have the bullet fire
			float shellSpeed = 20.0f;
			float shellPower = .01f;
			firedShell.fire(owner.transform.position, owner.transform.rotation, shellSpeed, shellPower);

			//add this bullet to the list of active bullets
			_activeBullets.Push(firedShell);

			return true;
		}
		Debug.Log("Reloading more ammo");
		return false;
	}



}
