using UnityEngine;
using System.Collections;

public class LeftFootToeBase_End : MonoBehaviour {

	Rigidbody leftToe;
	Vector3 lockPosition;

	// Use this for initialization
	void Start () {
		leftToe = GetComponent<Rigidbody>();
		lockPosition = leftToe.transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (StateMachine.state == Walk.RIGHT_STEP_UP_LEG || StateMachine.state == Walk.RIGHT_STEP_MID_LEG || StateMachine.state == Walk.RIGHT_LEG_DOWN) {
			//leftFoot.transform.position = lockFootPos;
			leftToe.transform.position = lockPosition;
		} else {
			lockPosition = leftToe.transform.position;
		}
	}
}
