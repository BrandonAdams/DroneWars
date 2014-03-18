using UnityEngine;
using System.Collections;

public class CharacterCollider : MonoBehaviour {
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		

	}
	
	void OnControllerColliderHit(ControllerColliderHit hit) {
	
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null)
            return; 		
		
		if(body.tag == "")
		{

		}	
		
    }//end OnControllerColliderHit	
	
	void ResetMoveDirection()
	{

	}
}
