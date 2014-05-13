using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	//private variables
	private float _speed;
	private float _power;
	private int _bulletAge;
	private Vector3 _magLocation;

	//public accessors and getters
	public float Speed{
		get { return _speed; }
		set { _speed = value; }
	}
	public float Power{
		get { return _power; }
		set { _power = value; }
	}
	public int BulletAge{
		get { return _bulletAge; }
		set { _bulletAge = value; }
	}
	public Vector3 Position{
		get { return transform.position; }
	}
	public Vector3 MagLocation{
		get { return _magLocation; }
	}

	// Use this for initialization
	void Start () {
		_speed = 0;
		_magLocation = new Vector3(-500, 0 , 0);
		transform.position = _magLocation;
	}

	// Update is called once per frame
	void Update () {
		//move our bullet 
		transform.position += transform.forward * _speed;
	}

	public void fire(Vector3 startingPosition, Quaternion startingRotation, float bulletSpeed, float bulletPower)
	{
		_bulletAge = 0;
		transform.position = startingPosition;
		transform.rotation = startingRotation;
		_speed = bulletSpeed;
		_power = bulletPower;
	}
}
