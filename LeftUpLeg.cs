using UnityEngine;
using System.Collections;

public class LeftUpLeg : MonoBehaviour {

	Rigidbody leftUpLeg;

	// Use this for initialization
	void Start () {
		leftUpLeg = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (StateMachine.state == Walk.RIGHT_STEP_UP_LEG) {
			
		}
	}
}
