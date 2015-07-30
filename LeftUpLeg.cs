using UnityEngine;
using System.Collections;

public class LeftUpLeg : MonoBehaviour {
	
	Rigidbody leftUpLeg;
	GameObject ragdoll;
	
	// Use this for initialization
	void Start () {
		leftUpLeg = GetComponent<Rigidbody>();
		
		ragdoll = GameObject.Find("/swat");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//print ("RightUpLeg Euler Angle: " + (360 - leftUpLeg.transform.eulerAngles.x));
		if (StateMachine.state == Walk.RIGHT_STAND) {
			toBackAngle();
		}
	}
	
	void toBackAngle(){
		leftUpLeg.AddForce (leftUpLeg.transform.forward * -100);
	}
}
