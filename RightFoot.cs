using UnityEngine;
using System.Collections;

public class RightFoot : MonoBehaviour {
	
	Rigidbody rightFoot;
	Vector3 lockPosition;
	float distanceToHip;

	GameObject hips;

	//Variable that stores positions for feet trajectories

	// Use this for initialization
	void Start () {
		rightFoot = GetComponent<Rigidbody>();
		lockPosition = rightFoot.transform.position;

		hips = GameObject.Find("/swat/Hips");

		distanceToHip = Vector3.Distance(hips.transform.position, rightFoot.transform.position);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//print (distanceToHip);

		if (StateMachine_Twick.state == WalkState.LEFT_CROUCH || StateMachine_Twick.state == WalkState.LEFT_STEP) {
			rightFoot.transform.position.Set(lockPosition.x, rightFoot.position.y, lockPosition.z);
			rightFoot.MoveRotation(new Quaternion(0, 0, 0, 1));
		}
		else if(StateMachine_Twick.state != WalkState.LEFT_CROUCH && StateMachine_Twick.state != WalkState.LEFT_STEP){
			lockPosition = rightFoot.transform.position;
		}
	}

	bool calculateFootTrajectory(){
		return true;
	}
}
