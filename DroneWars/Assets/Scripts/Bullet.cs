using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	//private variables
	private float _speed;
	private float _power;
	private int _bulletTag, _bulletLifePeriod, _bulletCurrentLife;
	private string _bulletName;
	private NetworkViewID _myID;
	private NetworkView _myView;

	//public accessors and getters
	public float Speed{
		get { return _speed; }
		set { _speed = value; }
	}
	public float Power{
		get { return _power; }
		set { _power = value; }
	}
	public int BulletTag{
		get { return _bulletTag; }
	}
	public string BulletName{
		get { return _bulletName; }
	}

	// Use this for initialization
	void Start () {
		_bulletLifePeriod = 200;
		_bulletCurrentLife = 0;
		_myID = this.networkView.viewID;
		_myView = NetworkView.Find(_myID);
	}

	public void initialize(float bulletSpeed, float bulletPower, string playerName, int tagID)
	{
		//instantiating our variables
		_speed = bulletSpeed;
		_power = bulletPower;
		_bulletName = playerName + "Bullet" + tagID;
		_bulletTag = tagID;
	}

	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * 2;
		//make our bullet older
		_bulletCurrentLife++;
		//our bullet has died of old age
		if(_bulletCurrentLife > _bulletLifePeriod)
		{
			GameObject[] liveBullets = GameObject.FindGameObjectsWithTag("Bullet");
			for(int i = 0; i < liveBullets.Length; i++)
			{
				if(liveBullets[i].networkView.isMine)
				{
					Debug.Log("Destroyed bullet");
					Network.Destroy(liveBullets[i]);
					break;
				}
			}

		}

	}
}
