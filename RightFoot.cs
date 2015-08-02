using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RightFoot : MonoBehaviour {
	
	// Public variables to control the step speed
	float stepSpeed_x = 0.05f;
	float stepSpeed_y = 0.05f;
	float stepSpeed_z = 0.05f;
	
	// Foot variables
	Rigidbody rightFoot;
	Vector3 lockPosition;
	float distanceFootToHip;
	float distanceToeBaseToHip;
	
	// References for other body parts
	GameObject hips;
	GameObject footBase;
	GameObject ragdoll;
	
	Vector3 footTraj; // Variable to store the path of the foot during the step
	
	// Use this for initialization
	void Start () {
		rightFoot = GetComponent<Rigidbody>();
		lockPosition = rightFoot.transform.position;
		
		ragdoll = GameObject.Find("/swat");
		hips = GameObject.Find("/swat/Hips");
		footBase = GameObject.Find("/swat/Hips/LeftUpLeg/LeftLeg/LeftFoot/LeftToeBase");
		
		// Calculate distance to the hips
		distanceFootToHip = Vector3.Distance(hips.transform.position, rightFoot.transform.position);
		distanceToeBaseToHip = Vector3.Distance(hips.transform.position, footBase.transform.position);
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// STEP BLOCKS
		if (StateMachine_Twick.state == WalkState.RIGHT_CROUCH) {
			footTraj = calculateDesiredPosition();
			print ("TRAJECTORY CALCULATED!");
			
		} else if (StateMachine_Twick.state == WalkState.RIGHT_STEP) {
			rightFoot.isKinematic = true;
			print("Target: " + footTraj);
			if(moveUntilDesired(footTraj)){
				rightFoot.isKinematic = false;
				//ragdoll.SendMessage("leftCrouch");
			}
			
			// HOLD FOOT IN PLACE
		} else if(StateMachine_Twick.state == WalkState.LEFT_CROUCH || StateMachine_Twick.state == WalkState.LEFT_STEP){
			rightFoot.transform.position.Set(lockPosition.x, rightFoot.position.y, lockPosition.z);
			rightFoot.MoveRotation(new Quaternion(0, 0, 0, 1));
			
		} else if(StateMachine_Twick.state != WalkState.LEFT_CROUCH && StateMachine_Twick.state != WalkState.LEFT_STEP){
			lockPosition = rightFoot.transform.position;
		}
		
	}
	
	// Calculate desired position in world-coordinates
	Vector3 calculateDesiredPosition(){
		Vector3 calculatePos = new Vector3 (calculateFootDisplacement_X(), calculateFootDisplacement_Y(), calculateFootDisplacement_Z());
		calculatePos = rightFoot.transform.localToWorldMatrix.MultiplyPoint (calculatePos);
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
	
	// Returns TRUE when the desired position is achieved
	bool moveUntilDesired(Vector3 desiredPos){
		if (rightFoot.transform.position.z > desiredPos.z)
			return true;
		else {
			rightFoot.MovePosition (transform.position + transform.forward * Time.deltaTime);
			rightFoot.MoveRotation(new Quaternion(0, 0, 0, 1));
			return false;
		}
	}
}
