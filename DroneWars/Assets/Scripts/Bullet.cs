using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	//private variables
	private float _speed;
	private float _power;
	private int _bulletAge;
	private Vector3 _magLocation;
	private bool _active;
	private string _bulletName;

	//public accessors and getters
	public bool IsActive{
		get { return _active; }
		set { _active = value; }
	}
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
	public Vector3 MagLocation{
		get { return _magLocation; }
	}
	public string BulletName{
		get { return _bulletName; }
		set { _bulletName = value; }
	}

	// Use this for initialization
	void Start () {
		_magLocation = new Vector3(-500, 0 , 0);
		transform.position = _magLocation;
	}

	// Update is called once per frame
	void Update () {
		//move our bullet 
		transform.position += transform.forward * _speed;

	}

	void reset()
	{
		_speed = 0;
		_active = false;
		transform.position = _magLocation;
	}

	public void fire(Vector3 startingPosition, Quaternion startingRotation, float bulletSpeed, float bulletPower)
	{
		_bulletAge = 0;
		transform.position = startingPosition;
		transform.rotation = startingRotation;
		_speed = bulletSpeed;
		_power = bulletPower;
		Invoke("reset", 5.0f);
	}


}
