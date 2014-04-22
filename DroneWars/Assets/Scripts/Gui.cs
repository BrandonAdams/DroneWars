using UnityEngine;
using System.Collections;
//using GUIElement;

public class Gui : MonoBehaviour {

	// source images
    public Texture2D map;
    public Texture2D cDown;
    public Texture2D health;
    public Texture2D hurtLeft;
	public Texture2D hurtRight;
    public Texture2D aim;
	public Texture2D lockOn;

	//variables related to damage/ warnings
	private bool takingDamage = false;
	private bool lockedOn = false;

	//variables related to alt cooldown and health
	public int healthDisplay = 100;
	private string altReady = "ALT\nRDY";
	//Text boxes
	//private GUIText altText;

	void OnGUI () {
        GUI.skin.label.fontSize = 24;

		// Place the Map on screen
		GUI.Label (new Rect(Screen.width- map.width, 0, map.width, map.height), map); //400

        // Place the lower two dials (cooldown - left & health - right)
		GUI.Label (new Rect (0,Screen.height - cDown.height, cDown.width,cDown.height), cDown);
		GUI.Label (new Rect (Screen.width - health.width,Screen.height - health.height,health.width,health.height), health);
       
		//put alt text lable relative to cDown
        GUI.Label(new Rect(cDown.width / 2 - cDown.width / 6, Screen.height - cDown.height / 2 - cDown.height / 6, cDown.width, cDown.height), altReady);

        //output health to player
        GUI.Label(new Rect(Screen.width - cDown.width / 2 - cDown.width / 6, Screen.height - cDown.height / 2 - cDown.height / 7, cDown.width, cDown.height), "" + healthDisplay);

		if(takingDamage){

			// Place the hurt makers (can't be seen unless hit)
			GUI.Label (new Rect (0,0 , hurtLeft.width, hurtLeft.height), hurtLeft);
			GUI.Label (new Rect (Screen.width-hurtRight.width, 0, hurtRight.width, hurtRight.height), hurtRight);
		}
        // Place the 'locked-on' warning
		if(lockedOn){
			GUI.Label (new Rect (Screen.width/2 - lockOn.width, Screen.height - lockOn.height*2, lockOn.width , lockOn.height), lockOn);
		}
		//Place the crosshairs
		GUI.Label (new Rect (Screen.width/ 2, Screen.height/ 2, aim.width, aim.height), aim);

	}
}
