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
	GameObject leftFoot;

	bool isLastStep; // This variable tells if it is the last step before stopping movement, if yes a different state of the state machine will need to be called

	Matrix4x4 matrizReference;	// Matrix reference to the hips
	Vector3 footTraj; 			// Variable to store the path of the foot during the step
	
	// Use this for initialization
	void Start () {
		rightFoot = GetComponent<Rigidbody>();
		lockPosition = rightFoot.transform.position;
		
		ragdoll = GameObject.Find("/swat");
		hips = GameObject.Find("/swat/Hips");
		footBase = GameObject.Find("/swat/Hips/RightUpLeg/RightLeg/RightFoot/RightToeBase");
		leftFoot = GameObject.Find("/swat/Hips/LeftUpLeg/LeftLeg/LeftFoot");
		
		// Calculate distance to the hips
		distanceFootToHip = Vector3.Distance(hips.transform.position, rightFoot.transform.position);
		distanceToeBaseToHip = Vector3.Distance(hips.transform.position, footBase.transform.position);

		isLastStep = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//////////////////////// CONDITIONAL STATES (Multiple of these IFs make occur during the same fixed update)

		//////////////////////////////////////////////////////////////////////////////////////////////////////////


		//////////////////// UNIQUE STATES (Only one of these ELSE IF blocks will be evaluated at it fixed update)
		if (StateMachine_Twick.state == WalkState.CALCULATE_RIGHT_STEP) {
			footTraj = calculateDesiredPosition();
			ragdoll.SendMessage("rightStep");
			
		} else if (StateMachine_Twick.state == WalkState.RIGHT_STEP) {
			rightFoot.isKinematic = true;
			if(moveUntilDesired(footTraj)){
				rightFoot.isKinematic = false;
				if(isLastStep){					// is this was the last step a new state to return to the initial position must be called instead
					isLastStep = false;			// restart the variable
					ragdoll.SendMessage("liftingToStand");	
				}
				else
					ragdoll.SendMessage("leftStepCalculate");
			}
			
			// HOLD FOOT IN PLACE
		} else if(StateMachine_Twick.state == WalkState.CALCULATE_LEFT_STEP || StateMachine_Twick.state == WalkState.LEFT_STEP){
			rightFoot.transform.position.Set(lockPosition.x, rightFoot.position.y, lockPosition.z);
			//rightFoot.MoveRotation(new Quaternion(0, 0, 0, 1));
			//rightFoot.MoveRotation(Quaternion.Euler(0,rightFoot.transform.rotation.eulerAngles.y,0));
			rightFoot.MoveRotation(StateMachine_Twick.orientation);
			
		} else if(StateMachine_Twick.state != WalkState.CALCULATE_LEFT_STEP && StateMachine_Twick.state != WalkState.LEFT_STEP && StateMachine_Twick.state != WalkState.STAND){
			lockPosition = rightFoot.transform.position;
		}

		// STAND POSITION
		else if ((StateMachine_Twick.state == WalkState.STAND || StateMachine_Twick.state == WalkState.CROUCH_TO_WALK || StateMachine_Twick.state == WalkState.RISE_TO_STAND) && !StateMachine_Twick.isTurning) {
			lockPosition = rightFoot.transform.position;
			//rightFoot.MoveRotation (new Quaternion (0, 0, 0, 1));
			rightFoot.MoveRotation (Quaternion.Euler(0, hips.transform.rotation.eulerAngles.y,0));
			rightFoot.angularVelocity.Set(0, 0, 0);
			rightFoot.velocity.Set(0,0,0);
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////////////
	}
	
	// Calculate desired position in foot-coordinates and store the foot transformation matrix
	Vector3 calculateDesiredPosition(){
		//rightFoot.MoveRotation (new Quaternion (0, 0, 0, 1));
		//rightFoot.MoveRotation(Quaternion.Euler(0,rightFoot.transform.rotation.eulerAngles.y,0));
		rightFoot.MoveRotation(StateMachine_Twick.orientation);
		matrizReference = rightFoot.transform.worldToLocalMatrix;
		Vector3 calculatePos = new Vector3 (calculateFootDisplacement_X(), calculateFootDisplacement_Y(), calculateFootDisplacement_Z());
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
			ragdoll.SendMessage("firstStepGiven");
			return legDisplacementInZ;
		} else if (StateMachine_Twick.finalizingMovement) {
			isLastStep = true;	// This is the calculation for the last step
			return (calculateFootDisplacementAccordingToOtherFoot_Z() > legDisplacementInZ) ? calculateFootDisplacementAccordingToOtherFoot_Z() : legDisplacementInZ; // the distance to move enought to allign with the other foot
		}
		else
			return (calculateFootDisplacementAccordingToOtherFoot_Z() * 2 > legDisplacementInZ * 2) ? calculateFootDisplacementAccordingToOtherFoot_Z() * 2 : legDisplacementInZ * 2; // the distance is doubled because otherwise it will only move enought to allign with the other foot
	}

	float calculateFootDisplacementAccordingToOtherFoot_Z(){
		Vector3 leftFootPos = matrizReference.MultiplyPoint (leftFoot.transform.position); 	// calculate the left foot position using the rightfoot frame (matrixReference was updated in calculateDesiredPosition())
		return leftFootPos.z;	// return the coordinates in Z from the left foot in rightfoot coordinates
	}
	
	// Returns TRUE when the desired position is achieved
	bool moveUntilDesired(Vector3 desiredPos){
		//Transform the foot position to hips coordinates to compare the foot
		Vector3 footInFootCoord = matrizReference.MultiplyPoint (rightFoot.transform.position);

		if (footInFootCoord.z > desiredPos.z)
			return true;
		else {
			rightFoot.MovePosition (rightFoot.transform.position + rightFoot.transform.forward * Time.deltaTime);
			//rightFoot.MoveRotation(new Quaternion(0, 0, 0, 1));
			//rightFoot.MoveRotation(Quaternion.Euler(0,rightFoot.transform.rotation.eulerAngles.y,0));
			rightFoot.MoveRotation(StateMachine_Twick.orientation);
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

	void printFeetRelation(){
		print("Printing Right Foot Relation:\n" +
		"RIGHT foot position: " + rightFoot.transform.position.ToString () + "\n" +
		"RIGHT target position: " + rightFoot.transform.localToWorldMatrix.MultiplyPoint(footTraj).ToString() + "\n" +
		"LEFT foot position: " + leftFoot.transform.position.ToString());
	}
}
