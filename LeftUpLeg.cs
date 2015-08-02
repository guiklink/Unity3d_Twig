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
		if (StateMachine_Twick.state == WalkState.LEFT_CROUCH || StateMachine_Twick.state == WalkState.LEFT_STEP) {
			step();
		}
	}
	
	void step(){
		//leftUpLeg.AddTorque (leftUpLeg.transform.right * -500);
		//leftUpLeg.AddForce (leftUpLeg.transform.up * 500);
	}
}
