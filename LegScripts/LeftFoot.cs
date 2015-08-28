using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeftFoot : MonoBehaviour {

	// Public variables to control the step speed
	float stepSpeed_x = 0.05f;
	float stepSpeed_y = 0.05f;
	float stepSpeed_z = 0.05f;

	// Foot variables
	Rigidbody leftFoot;
	Vector3 lockPosition;
	float distanceFootToHip;
	float distanceToeBaseToHip;

	// References for other body parts
	GameObject hips;
	GameObject footBase;
	GameObject ragdoll;
	GameObject rightFoot;

	bool isLastStep; // This variable tells if it is the last step before stopping movement, if yes a different state of the state machine will need to be called

	Matrix4x4 matrizReference;	// Matrix reference to the hips
	Vector3 footTraj; // Variable to store the path of the foot during the step

	// Use this for initialization
	void Start () {
		leftFoot = GetComponent<Rigidbody>();
		lockPosition = leftFoot.transform.position;

		ragdoll = GameObject.Find("/swat");
		hips = GameObject.Find("/swat/Hips");
		footBase = GameObject.Find("/swat/Hips/LeftUpLeg/LeftLeg/LeftFoot/LeftToeBase");
		rightFoot = GameObject.Find("/swat/Hips/RightUpLeg/RightLeg/RightFoot");

		// Calculate distance to the hips
		distanceFootToHip = Vector3.Distance(hips.transform.position, leftFoot.transform.position);
		distanceToeBaseToHip = Vector3.Distance(hips.transform.position, footBase.transform.position);

		isLastStep = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//////////////////////// CONDITIONAL STATES (Multiple of these IFs make occur during the same fixed update)
		
		//////////////////////////////////////////////////////////////////////////////////////////////////////////


		//////////////////// UNIQUE STATES (Only one of these ELSE IF blocks will be evaluated at it fixed update)
		if (StateMachine_Twick.state == WalkState.CALCULATE_LEFT_STEP) {
			footTraj = calculateDesiredPosition ();
			ragdoll.SendMessage ("leftStep");

		} else if (StateMachine_Twick.state == WalkState.LEFT_STEP) {
			leftFoot.isKinematic = true;
			if (moveUntilDesired (footTraj)) {
				leftFoot.isKinematic = false;
				if(isLastStep){					// is this was the last step a new state to return to the initial position must be called instead
					isLastStep = false;			// restart the variable
					ragdoll.SendMessage("liftingToStand");	
				}
				else
					ragdoll.SendMessage ("rightStepCalculate");
			}
		
			// HOLD FOOT IN PLACE
		} else if (StateMachine_Twick.state == WalkState.CALCULATE_RIGHT_STEP || StateMachine_Twick.state == WalkState.RIGHT_STEP) {
			leftFoot.transform.position.Set (lockPosition.x, leftFoot.position.y, lockPosition.z);
			leftFoot.MoveRotation (new Quaternion (0, 0, 0, 1));

		} else if (StateMachine_Twick.state != WalkState.CALCULATE_RIGHT_STEP && StateMachine_Twick.state != WalkState.RIGHT_STEP && StateMachine_Twick.state != WalkState.STAND) {
			lockPosition = leftFoot.transform.position;
		}

		// STAND POSITION
		else if (StateMachine_Twick.state == WalkState.STAND || StateMachine_Twick.state == WalkState.CROUCH_TO_WALK || StateMachine_Twick.state == WalkState.RISE_TO_STAND) {
			lockPosition = rightFoot.transform.position;
			//leftFoot.MoveRotation (new Quaternion (0, 0, 0, 1));
			leftFoot.MoveRotation (Quaternion.Euler(0, hips.transform.rotation.eulerAngles.y,0));
			leftFoot.angularVelocity.Set(0, 0, 0);
			leftFoot.velocity.Set(0,0,0);
		}
		//////////////////////////////////////////////////////////////////////////////////////////////////////////
	}

	// Calculate desired position in foot-coordinates and store the foot transformation matrix
	Vector3 calculateDesiredPosition(){
		leftFoot.MoveRotation(new Quaternion(0, 0, 0, 1));
		matrizReference = leftFoot.transform.worldToLocalMatrix;
		Vector3 calculatePos = new Vector3 (calculateFootDisplacement_X(), calculateFootDisplacement_Y(), calculateFootDisplacement_Z());
		//calculatePos = leftFoot.transform.localToWorldMatrix.MultiplyPoint (calculatePos);		// Transform the maximum distance the foot can achieve to world coordinates
		//calculatePos = matrizReference.MultiplyPoint (calculatePos);
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
		if (StateMachine_Twick.isFirstStep) {
			ragdoll.SendMessage ("firstStepGiven");
			return legDisplacementInZ;
		} else if (StateMachine_Twick.finalizingMovement) {
			isLastStep = true;	// This is the calculation for the last step
			return (calculateFootDisplacementAccordingToOtherFoot_Z() > legDisplacementInZ) ? calculateFootDisplacementAccordingToOtherFoot_Z() : legDisplacementInZ; // the distance to move enought to allign with the other foot
		}
		else
			return (calculateFootDisplacementAccordingToOtherFoot_Z() * 2 > legDisplacementInZ * 2) ? calculateFootDisplacementAccordingToOtherFoot_Z() * 2 : legDisplacementInZ * 2; // the distance is doubled because otherwise it will only move enought to allign with the other foot
	}

	float calculateFootDisplacementAccordingToOtherFoot_Z(){
		Vector3 rightFootPos = matrizReference.MultiplyPoint (rightFoot.transform.position); 	// calculate the right foot position using the left foot frame (matrixReference was updated in calculateDesiredPosition())
		return rightFootPos.z;	// return the coordinates in Z from the left foot in rightfoot coordinates
	}

	// Returns TRUE when the desired position is achieved
	bool moveUntilDesired(Vector3 desiredPos){
		//Transform the foot position to hips coordinates to compare the foot
		Vector3 footInFootCoord = matrizReference.MultiplyPoint (leftFoot.transform.position);
		
		if (footInFootCoord.z > desiredPos.z)
			return true;
		else {
			leftFoot.MovePosition (transform.position + transform.forward * Time.deltaTime);
			leftFoot.MoveRotation(new Quaternion(0, 0, 0, 1));
			return false;
		}
	}

	void printMatrix4x4(Matrix4x4 matrix){
		string space = " "; 
		print ("Printing Matrix:");
		print (matrix.m00 + space + matrix.m01 + space + matrix.m02 + space + matrix.m03);
		print (matrix.m10 + space + matrix.m11 + space + matrix.m12 + space + matrix.m13);
		print (matrix.m20 + space + matrix.m21 + space + matrix.m22 + space + matrix.m23);
		print (matrix.m30 + space + matrix.m31 + space + matrix.m32 + space + matrix.m33);
	}
}
