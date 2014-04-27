using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public float speed = 0.0f;
	public float strafeSpeed = 0.0f;
	public float liftSpeed = 0.0f;
	public float declSpeed = 0.95f;
	public float speedMod = 0.05f;
	
	public float rotationSpeed = 5.0f;
	public float maxSpeed = 40.0f;
	public float maxLift = 3.0f;
	public float tiltSpeed = 1.0f;
	
	
	private float timer = 0;
	//private float rotateCounterA = 0;
	//private float rotateCounterD = 0;
	//private float rotateCounter = 0;
	
	
	public GameObject cam;
	
	//private float moveUp = 0.0f;
	
	private bool isGravOn = false;
	private bool canFly = true;
	//private bool isRotate = false;
	
	// The initial orientation.
	private Quaternion initialOrientation;
	
	// The cummulative rotation about the y and x-Axis.
	private float cummulativeRotationYAxis;
	private float cummulativeRotationXAxis;	
	
	// The rotation factor, this will control the speed we rotate at.
	public float rotationSensitvity = 500.0f;
	private float maxUpRollRotation = -300;
	private float maxDownRollRotation = -430;
	
	//private Vector3 position;
	
	//private Vector3 newForward;
	
	//private float vx = 0.0f;
	//private float vy = 0.0f;
	//private float vz = 0.0f;

	// Use this for initialization
	void Start () 
	{
		//save the quaternion representing our initial orientation from the transform
		initialOrientation = transform.rotation;

		Screen.lockCursor = true;
		Screen.showCursor = false;

		//set the cummulativeRotation to zero.
		cummulativeRotationYAxis = 0.0f;
		cummulativeRotationXAxis = 0.0f;
		//position = new Vector3(0,0,0);
		//newForward = new Vector3(0,0,0);
       
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
		cummulativeRotationXAxis -= Input.GetAxis ("Mouse Y") * Time.deltaTime * rotationSensitvity;	
		
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
				if(speed < maxSpeed)
				{
					speed += speedMod;
				}
				else 
				{
					speed = maxSpeed;
					//transform.Translate (-Vector3.forward * vx * Time.deltaTime);
					//position.x = position.x + (vx * Time.deltaTime);
				}
			}
			else if(!(Input.GetKey (KeyCode.W)) && speed> 0)
			{
				speed *= declSpeed;
				if(speed <=1)
				{
					speed = 0;
				}
				//transform.Translate (-Vector3.forward * vx * Time.deltaTime);
				//position.x = position.x + (vx * Time.deltaTime);
			}
			#endregion W
			
			#region S
			if(Input.GetKey (KeyCode.S) && !(Input.GetKey (KeyCode.W)))
			{
				if(speed > -maxSpeed)
				{
					speed -= speedMod;;
					//transform.Translate(-Vector3.forward * vx * Time.deltaTime);
					//position.x = position.x + (vx * Time.deltaTime);
				}
				else
				{
					speed = -maxSpeed;
					//transform.Translate(-Vector3.forward * vx * Time.deltaTime);
					//position.x = position.x + (vx * Time.deltaTime);
				}
				//another way to move forward is
				//transform.Translate(0.0, speed * Time.deltaTime, Space.Self);
				//vz = -10;
			}
			else if(!(Input.GetKey (KeyCode.S)) && speed < 0)
			{
				speed *= declSpeed;
				if(speed > -1)
				{
					speed = 0;
				}
				//transform.Translate (-Vector3.forward * vx * Time.deltaTime);
				//position.x = position.x + (vx * Time.deltaTime);
			}
			#endregion S
			
			/*if(Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.S))
			{
				if(vx > 0)
				{
					vx--;
					//transform.Translate(-Vector3.forward * vx * Time.deltaTime);
				}
				else if(vx < 0)
				{
					vx ++;
					//transform.Translate(-Vector3.forward * vx * Time.deltaTime);
					
				}
			}*/
			#region A&D
			/*if(Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.D))
			{

			}*/
			#endregion
			#region D
			if((Input.GetKey (KeyCode.D)) && !(Input.GetKey (KeyCode.A)))
			{
				if(strafeSpeed < maxSpeed)
				{
					strafeSpeed += speedMod;
				}
				else 
				{
					strafeSpeed = maxSpeed;
					//transform.Translate (-Vector3.forward * vx * Time.deltaTime);
					//position.x = position.x + (vx * Time.deltaTime);
				}
				//vx = 10;
				/*if(rotateCounter < 30)
				{
					rotateCounter += tiltSpeed;
					transform.Rotate(Vector3.forward,-rotateCounter);
				}
				else
				{
					transform.Rotate(Vector3.forward,-rotateCounter);
				}*/
				
				//transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
				//another way to move forward is
				//transform.Translate(0.0, speed * Time.deltaTime, Space.Self);
			}
			else if(!(Input.GetKey (KeyCode.D)) && strafeSpeed> 0)
			{
				strafeSpeed *= declSpeed;
				if(strafeSpeed <=1)
				{
					strafeSpeed = 0;
				}
			}
			#endregion
			#region A
			if((Input.GetKey (KeyCode.A)) && !(Input.GetKey (KeyCode.D)))
			{
				if(speed > -maxSpeed)
				{
					strafeSpeed -= speedMod;
					
				}
				else
				{
					strafeSpeed = -maxSpeed;
					
				}
				
			}
			else if(!(Input.GetKey (KeyCode.A)) && strafeSpeed < 0)
			{
				strafeSpeed *= declSpeed;
				if(strafeSpeed >= -1)
				{
					strafeSpeed = 0;
				}
			}
			/*else if(!(Input.GetKey (KeyCode.D)) && rotateCounterD > 0)
			{
				rotateCounterD -= 2;
				transform.Rotate (Vector3.forward, rotateCounterD);
			}*/
			#endregion
			
			
			#region Space and Shift
			#endregion
			#region Space
			if(Input.GetKey( KeyCode.Space) && !(Input.GetKey (KeyCode.LeftShift)))
			{
				
				if(liftSpeed < maxLift)
				{
					liftSpeed += speedMod;
				}
				else 
				{
					liftSpeed = maxLift;
					//transform.Translate (-Vector3.forward * vx * Time.deltaTime);
					//position.x = position.x + (vx * Time.deltaTime);
				}
			}
			else if(!(Input.GetKey (KeyCode.Space)) && liftSpeed > 0)
			{
				liftSpeed *= declSpeed;
				if(liftSpeed <= .5)
				{
					liftSpeed = 0;
				}
			}
			
			#endregion
			#region Shift
			if(Input.GetKey(KeyCode.LeftShift) && !(Input.GetKey (KeyCode.Space)))
			{
				if(liftSpeed > -maxLift)
				{
					liftSpeed -= speedMod;
				}
				else 
				{
					liftSpeed = -maxLift;
					//transform.Translate (-Vector3.forward * vx * Time.deltaTime);
					//position.x = position.x + (vx * Time.deltaTime);
				}
			}
			else if(!(Input.GetKey (KeyCode.LeftShift)) && liftSpeed < 0)
			{
				liftSpeed *= declSpeed;
				if(liftSpeed >= -.5)
				{
					liftSpeed = 0;
				}
			}
			
			if(!(Input.GetKey( KeyCode.Space)) && !(Input.GetKey (KeyCode.LeftShift)) || Input.GetKey( KeyCode.Space) && (Input.GetKey (KeyCode.LeftShift)))
			{
				if(liftSpeed > 0)
				{
					if(liftSpeed <= .5)
					{
						liftSpeed = 0;
					}
				}
				else if(liftSpeed < 0)
				{
					if(liftSpeed >= -.5)
					{
						liftSpeed = 0;
					}
				}
				else
					liftSpeed *= declSpeed;
			}
			#endregion
			
			
			
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
			if(speed < 0)
			{
				speed += speedMod;
				
			}
			else if (speed > 0)
			{
				speed -= speedMod;
				
			}
			if(liftSpeed < 0)
			{
				liftSpeed += speedMod;
			}
			else if (liftSpeed > 0)
			{
				liftSpeed -= speedMod;
				
			}
			if(strafeSpeed < 0)
			{
				strafeSpeed += speedMod;
				
			}
			else if (strafeSpeed > 0)
			{
				strafeSpeed -= speedMod;
				
			}
			
			liftSpeed += ((timer/60) * -0.05f);
			
			timer++;
		}
		#endregion
		#region gravOff
		if(isGravOn == false)
		{
			if(timer >= 0)
			{
				timer -= 2;
				liftSpeed += ((timer/60) * -0.05f);
			}
			else{
				canFly = true;
			}
		}
		#endregion
		
		Vector3 moveForward = new Vector3();
		moveForward.x = transform.forward.x;
		moveForward.z = transform.forward.z;
		transform.position += moveForward*speed;
		
		Vector3 strafe = new Vector3();
		strafe.x = transform.right.x;
		strafe.z = transform.right.z;
		transform.position += strafe*strafeSpeed;
		
		Vector3 lift = new Vector3();
		lift.y = transform.up.y;
		transform.position += lift*liftSpeed;
	}
}
