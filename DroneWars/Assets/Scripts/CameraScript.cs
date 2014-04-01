using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	
	public float speed = 7.0f;
	public float rotationSpeed = 5.0f;
	public float maxSpeed = 40.0f;
	public float tiltSpeed = 1.0f;
	

	private float timer = 0;
	private float rotateCounterA = 0;
	private float rotateCounterD = 0;
	private float rotateCounter = 0;


	public GameObject cam;

	private bool isGravOn = false;
	private bool canFly = true;
	private bool isRotate = false;
	
	// The initial orientation.
	private Quaternion initialOrientation;

	// The cummulative rotation about the y and x-Axis.
	private float cummulativeRotationYAxis;
	private float cummulativeRotationXAxis;	

	// The rotation factor, this will control the speed we rotate at.
	public float rotationSensitvity = 500.0f;
	private float maxUpRollRotation = -300;
	private float maxDownRollRotation = -430;

	private float vx = 0.0f;
	private float vy = 0.0f;
	private float vz = 0.0f;
	// Use this for initialization
	void Start () 
	{
		//save the quaternion representing our initial orientation from the transform
		initialOrientation = transform.rotation;
		
		//set the cummulativeRotation to zero.
		cummulativeRotationYAxis = 0.0f;
		cummulativeRotationXAxis = 0.0f;
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		SteerWithMouse();
		
		updateMovement();
		
	}
	
	void SteerWithMouse ()
	{
		//Get the left/right Input from the Mouse and use time along with a scaling factor 
		// to add a controlled amount to our cummulative rotation about the y-Axis.
		cummulativeRotationYAxis += Input.GetAxis ("Mouse X") * Time.deltaTime * rotationSensitvity;
		cummulativeRotationXAxis += Input.GetAxis ("Mouse Y") * Time.deltaTime * rotationSensitvity;	
		
		clampXRotation ();
		
		//Create a Quaternion representing our current cummulative rotation around the y-axis. 
		Quaternion currentRotation = Quaternion.Euler (cummulativeRotationXAxis, cummulativeRotationYAxis, 0.0f);
		
		// Use the quaternion to update the transform of the vehicle's Game Object based on 
		// initial orientation and the accumulated rotation since the original orientation. 
		transform.rotation = initialOrientation * currentRotation;

	}
	
	void clampXRotation()
	{
		if(cummulativeRotationXAxis > maxUpRollRotation)
		{
			cummulativeRotationXAxis = maxUpRollRotation;
		}		
		else if(cummulativeRotationXAxis < maxDownRollRotation)
		{
			cummulativeRotationXAxis = maxDownRollRotation;
		}
		
	}
	
	void updateMovement()
	{
		if(canFly == true)
		{
			#region arrows
			/*if(Input.GetKey (KeyCode.UpArrow))
			{
				transform.Translate(Vector3.forward * speed * Time.deltaTime);
				//another way to move forward is
				//transform.Translate(0.0, speed * Time.deltaTime, Space.Self);
				//vz = 10;
			}
			if(Input.GetKey (KeyCode.DownArrow))
			{
				transform.Translate(Vector3.back * speed * Time.deltaTime);
				//another way to move forward is
				//transform.Translate(0.0, speed * Time.deltaTime, Space.Self);
				//vz = -10;
			}
			if(Input.GetKey (KeyCode.LeftArrow))
			{
				transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
				//transform.Translate (Vector3.left * speed * Time.deltaTime);
				//vx = 10;
				//another way to move forward is
				//transform.Translate(0.0, speed * Time.deltaTime, Space.Self);
			}
			if(Input.GetKey (KeyCode.RightArrow))
			{
				transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
					//transform.Translate(Vector3.right * speed * Time.deltaTime);
				//another way to move forward is
				//transform.Translate(0.0, speed * Time.deltaTime, Space.Self);
				//vx = -10;
			}*/
			#endregion

			#region Movement


			#region W
			if(Input.GetKey (KeyCode.W) && !(Input.GetKey (KeyCode.S)))
			{
				if(vx < maxSpeed)
				{
					vx ++;
					transform.Translate(-Vector3.forward * vx * Time.deltaTime);

				}
				else 
				{
					vx = maxSpeed;
					transform.Translate (-Vector3.forward * vx * Time.deltaTime);
				}
			}
			else if(!(Input.GetKey (KeyCode.W)) && vx > 0)
			{
				vx--;
				transform.Translate (-Vector3.forward * vx * Time.deltaTime);
			}
			#endregion W

			#region S
			if(Input.GetKey (KeyCode.S) && !(Input.GetKey (KeyCode.W)))
			{
				if(vx > -maxSpeed)
				{
					vx--;
					transform.Translate(-Vector3.forward * vx * Time.deltaTime);
				}
				else
				{
					vx = -maxSpeed;
					transform.Translate(-Vector3.forward * vx * Time.deltaTime);
				}
				//another way to move forward is
				//transform.Translate(0.0, speed * Time.deltaTime, Space.Self);
				//vz = -10;
			}
			else if(!(Input.GetKey (KeyCode.S)) && vx < 0)
			{
				vx++;
				transform.Translate (-Vector3.forward * vx * Time.deltaTime);
			}
			#endregion S

			if(Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.S))
			{
				if(vx > 0)
				{
					vx--;
					transform.Translate(-Vector3.forward * vx * Time.deltaTime);
				}
				else if(vx < 0)
				{
					vx ++;
					transform.Translate(-Vector3.forward * vx * Time.deltaTime);
					
				}
			}
			#region A&D
			/*if(Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.D))
			{

			}*/
			#endregion
			#region A
			if((Input.GetKey (KeyCode.A)) && !(Input.GetKey (KeyCode.D)))
			{
				isRotate = true;
				if(vz < maxSpeed)
				{
					vz++;
					transform.Translate (-Vector3.left * speed * Time.deltaTime);
				}
				else
				{
					vz = maxSpeed;
					transform.Translate(-Vector3.left * vz * Time.deltaTime);
				}
				//vx = 10;
				if(rotateCounter < 30)
				{
					rotateCounter += tiltSpeed;
					transform.Rotate(Vector3.forward,-rotateCounter);
				}
				else
				{
					transform.Rotate(Vector3.forward,-rotateCounter);
				}

				//transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
				//another way to move forward is
				//transform.Translate(0.0, speed * Time.deltaTime, Space.Self);
			}
			else if(!(Input.GetKey (KeyCode.A)) && vz > 0)
			{
				vz--;
				transform.Translate(-Vector3.left * vz * Time.deltaTime);
				if(rotateCounter > 0)
				{
					rotateCounter -= tiltSpeed;
					transform.Rotate (Vector3.forward,-rotateCounter);
				}
			}
			#endregion
			#region D
			if((Input.GetKey (KeyCode.D)) && !(Input.GetKey (KeyCode.A)))
			{
				isRotate = true;
				if(vz > -maxSpeed)
				{
					vz--;
					transform.Translate (-Vector3.left * vz * Time.deltaTime);
				}
				else
				{
					vz = -maxSpeed;
					transform.Translate(-Vector3.left * vz * Time.deltaTime);
				}

				if(rotateCounter > -30)
				{
					rotateCounter -= tiltSpeed;
					transform.Rotate (Vector3.forward, -rotateCounter);
				}
				else
					transform.Rotate (Vector3.forward, -rotateCounter);
				//transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
				//another way to move forward is
				//transform.Translate(0.0, speed * Time.deltaTime, Space.Self);
			}
			else if(!(Input.GetKey(KeyCode.D)) && vz < 0)
			{
				vz++;
				transform.Translate(-Vector3.left * vz * Time.deltaTime);
				if(rotateCounter < 0)
				{
					rotateCounter += tiltSpeed;
					transform.Rotate (Vector3.forward, -rotateCounter);
				}
			}
			/*else if(!(Input.GetKey (KeyCode.D)) && rotateCounterD > 0)
			{
				rotateCounterD -= 2;
				transform.Rotate (Vector3.forward, rotateCounterD);
			}*/
			#endregion

			if(Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.D) )
			{
				if(vz > 0)
				{
					vz--;
					transform.Translate(-Vector3.left * vz * Time.deltaTime);
				}
				else if(vz < 0)
				{
					vz ++;
					transform.Translate(-Vector3.left * vz * Time.deltaTime);
				}
			}

			if(isRotate == false)
			{
				if(rotateCounter > 0)
				{
					rotateCounter -= tiltSpeed;
					transform.Rotate (Vector3.forward, -rotateCounter);
				}
				else if(rotateCounter < 0)
				{
					rotateCounter += tiltSpeed;
					transform.Rotate (Vector3.forward, -rotateCounter);
				}
			}

			#region Space and Shift
			/*if(Input.GetKey (KeyCode.Space) && Input.GetKey (KeyCode.LeftShift))
			{

			}*/
			#endregion
			#region Space
			 if(Input.GetKey( KeyCode.Space) && !(Input.GetKey (KeyCode.LeftShift)))
			{
				if(vy < maxSpeed)
				{
					vy++;
					transform.Translate (Vector3.up * vy * Time.deltaTime);
				}
				else
				{
					vy = maxSpeed;
					transform.Translate (Vector3.up * vy * Time.deltaTime);
				}
			}
			if(!(Input.GetKey (KeyCode.Space)) && vy > 0)
			{
				vy--;
				transform.Translate (Vector3.up * vy * Time.deltaTime);
			}
			#endregion
			#region Shift
			if(Input.GetKey(KeyCode.LeftShift) && !(Input.GetKey (KeyCode.Space)))
			{
				if(vy > -maxSpeed)
				{
					vy--;
					transform.Translate (Vector3.up * vy * Time.deltaTime);
				}
				else
				{
					vy = -maxSpeed;
					transform.Translate(Vector3.up * vy * Time.deltaTime);
				}
			}
			else if(!(Input.GetKey (KeyCode.LeftShift)) && vy < 0)
			{
				vy++;
				transform.Translate (Vector3.up * vy * Time.deltaTime);
			}
			#endregion

			if(Input.GetKey (KeyCode.Space) && Input.GetKey (KeyCode.LeftShift))
			{
				if(vy > 0)
				{
					vy--;
					transform.Translate(Vector3.up * vy * Time.deltaTime);
				}
				else if(vy < 0)
				{
					vy++;
					transform.Translate(Vector3.up * vy * Time.deltaTime);
					
				}
			}

			#endregion Movement

		}

		#region checkGrav
		if(Input.GetKeyDown (KeyCode.F))
		{ 
			if(isGravOn == false)
			{
				isGravOn = true;
				canFly = false;

				Debug.Log (isGravOn);
			}
			else if(isGravOn == true)
			{
				isGravOn = false;
				Debug.Log (isGravOn);
			}
		}
		#endregion
		#region gravOn
		if(isGravOn == true)
		{
			if(vx < 0)
			{
				vx++;
				transform.Translate (-Vector3.forward * vx * Time.deltaTime);
			}
			else if (vx > 0)
			{
				vx--;
				transform.Translate (-Vector3.forward * vx * Time.deltaTime);
			}
			if(vy < 0)
			{
				vy++;
				transform.Translate (Vector3.up * vy * Time.deltaTime);
			}
			else if (vy > 0)
			{
				vy--;
				transform.Translate (Vector3.up * vy * Time.deltaTime);
			}
			if(vz < 0)
			{
				vz++;
				transform.Translate (-Vector3.left * vz * Time.deltaTime);
			}
			else if (vz > 0)
			{
				vz--;
				transform.Translate (-Vector3.left * vz * Time.deltaTime);
			}
			transform.Translate (-Vector3.down * (timer/60) * -0.5f,Space.World);

			timer++;
			//Debug.Log (timer);
		}
		#endregion
		#region gravOff
		if(isGravOn == false)
		{
			if(timer >= 0)
			{
				timer -= 2;
				transform.Translate (-Vector3.down * (timer/60) * -0.5f, Space.World);
			}
			else{
				canFly = true;
			}
		}
		#endregion

		Vector3 Velocity = new Vector3(vx,vy,vz);
	}
}
