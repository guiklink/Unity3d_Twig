using UnityEngine;
using System.Collections;

public class RightUpLeg : MonoBehaviour {
	
	Rigidbody upperLeg;
	
	// Use this for initialization
	void Start () {
		upperLeg = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (StateMachine_Twick.state == WalkState.RIGHT_STEP) 
			upperLeg.constraints = RigidbodyConstraints.FreezeRotationZ;
		else
			upperLeg.constraints = RigidbodyConstraints.None;
	}
}
