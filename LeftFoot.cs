using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeftFoot : MonoBehaviour {

	float stepSpeed_x = 0.05f;
	float stepSpeed_y = 0.05f;
	float stepSpeed_z = 0.05f;

	Rigidbody leftFoot;
	Vector3 lockPosition;
	float distanceFootToHip;
	float distanceToeBaseToHip;
	
	GameObject hips;
	GameObject footBase;
	GameObject ragdoll;

	Vector3 footTraj; // Variable to store the path of the foot during the step

	// Use this for initialization
	void Start () {
		leftFoot = GetComponent<Rigidbody>();
		lockPosition = leftFoot.transform.position;

		ragdoll = GameObject.Find("/swat");
		hips = GameObject.Find("/swat/Hips");
		footBase = GameObject.Find("/swat/Hips/LeftUpLeg/LeftLeg/LeftFoot/LeftToeBase");
		
		distanceFootToHip = Vector3.Distance(hips.transform.position, leftFoot.transform.position);
		distanceToeBaseToHip = Vector3.Distance(hips.transform.position, footBase.transform.position);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (StateMachine_Twick.state == WalkState.LEFT_CROUCH) {
			footTraj = calculateDesiredPosition();
		} else if (StateMachine_Twick.state == WalkState.LEFT_STEP) {
			leftFoot.isKinematic = true;
			if(moveUntilDesired(footTraj)){
				leftFoot.isKinematic = false;
				ragdoll.SendMessage("rightCrouch");
			}
		} else if(StateMachine_Twick.state == WalkState.RIGHT_CROUCH || StateMachine_Twick.state == WalkState.RIGHT_STEP){
			leftFoot.transform.position.Set(lockPosition.x, leftFoot.position.y, lockPosition.z);
			leftFoot.MoveRotation(new Quaternion(0, 0, 0, 1));
		} else if(StateMachine_Twick.state != WalkState.RIGHT_CROUCH && StateMachine_Twick.state != WalkState.RIGHT_STEP){
			lockPosition = leftFoot.transform.position;
		}
	}

	// Calculate desired position in world-coordinates
	Vector3 calculateDesiredPosition(){
		Vector3 calculatePos = new Vector3 (calculateFootDisplacement_X(), calculateFootDisplacement_Y(), calculateFootDisplacement_Z());
		calculatePos = leftFoot.transform.localToWorldMatrix.MultiplyPoint (calculatePos);
		return calculatePos;
	}

	// Returne X position
	float calculateFootDisplacement_X(){
		return 0f;
	}

	// Returne Y position
	float calculateFootDisplacement_Y(){
		return 0.2f;
	}

	// Returne Z position
	float calculateFootDisplacement_Z(){
		float h = StandSwat.walkingHeight;
		float H = distanceToeBaseToHip; // Only use 90% of the distance between hip and foot
		float legDisplacementInZ = Mathf.Sqrt (Mathf.Pow(H,2) - Mathf.Pow(h,2));
		return legDisplacementInZ;
	}

	bool moveUntilDesired(Vector3 desiredPos){
		if (leftFoot.transform.position.z > desiredPos.z)
			return true;
		else {
			leftFoot.MovePosition (transform.position + transform.forward * Time.deltaTime);
			leftFoot.MoveRotation(new Quaternion(0, 0, 0, 1));
			return false;
		}
	}
}
