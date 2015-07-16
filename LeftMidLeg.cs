using UnityEngine;
using System.Collections;

public class LeftMidLeg : MonoBehaviour {

	Rigidbody midLeftLeg;
	Vector3 lockPosition;

	// Use this for initialization
	void Start () {

		midLeftLeg = GetComponent<Rigidbody>();
		lockPosition = midLeftLeg.transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (StateMachine.state == Walk.RIGHT_STEP_UP_LEG || StateMachine.state == Walk.RIGHT_STEP_MID_LEG || StateMachine.state == Walk.RIGHT_LEG_DOWN) {
			//leftFoot.transform.position = lockFootPos;
			midLeftLeg.transform.position = lockPosition;
		} else {
			lockPosition = midLeftLeg.transform.position;
		}
	}
}
