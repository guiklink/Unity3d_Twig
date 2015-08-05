﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeftFoot : MonoBehaviour {

	bool isFirstStep;

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

	Matrix4x4 matrizReference;	// Matrix reference to the hips
	Vector3 footTraj; // Variable to store the path of the foot during the step

	// Use this for initialization
	void Start () {
		leftFoot = GetComponent<Rigidbody>();
		lockPosition = leftFoot.transform.position;

		ragdoll = GameObject.Find("/swat");
		hips = GameObject.Find("/swat/Hips");
		footBase = GameObject.Find("/swat/Hips/LeftUpLeg/LeftLeg/LeftFoot/LeftToeBase");

		// Calculate distance to the hips
		distanceFootToHip = Vector3.Distance(hips.transform.position, leftFoot.transform.position);
		distanceToeBaseToHip = Vector3.Distance(hips.transform.position, footBase.transform.position);

		isFirstStep = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// STEP BLOCKS
		if (StateMachine_Twick.state == WalkState.CALCULATE_LEFT_STEP) {
			footTraj = calculateDesiredPosition ();
			ragdoll.SendMessage ("leftStep");

		} else if (StateMachine_Twick.state == WalkState.LEFT_STEP) {
			leftFoot.isKinematic = true;
			if (moveUntilDesired (footTraj)) {
				leftFoot.isKinematic = false;
				ragdoll.SendMessage ("rightStepCalculate");
			}
		
			// HOLD FOOT IN PLACE
		} else if (StateMachine_Twick.state == WalkState.CALCULATE_RIGHT_STEP || StateMachine_Twick.state == WalkState.RIGHT_STEP) {
			leftFoot.transform.position.Set (lockPosition.x, leftFoot.position.y, lockPosition.z);
			leftFoot.MoveRotation (new Quaternion (0, 0, 0, 1));

		} else if (StateMachine_Twick.state != WalkState.CALCULATE_RIGHT_STEP && StateMachine_Twick.state != WalkState.RIGHT_STEP) {
			lockPosition = leftFoot.transform.position;
		}

		// STAND POSITION
		else if (StateMachine_Twick.state == WalkState.STAND) {
			leftFoot.MoveRotation (new Quaternion (0, 0, 0, 1));
		}

	}

	// Calculate desired position in hips-coordinates and store the hips transformation matrix
	Vector3 calculateDesiredPosition(){
		matrizReference = hips.transform.worldToLocalMatrix;
		Vector3 calculatePos = new Vector3 (calculateFootDisplacement_X(), calculateFootDisplacement_Y(), calculateFootDisplacement_Z());
		calculatePos = leftFoot.transform.localToWorldMatrix.MultiplyPoint (calculatePos);		// Transform the maximum distance the foot can achieve to world coordinates
		calculatePos = matrizReference.MultiplyPoint (calculatePos);
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
		if (isFirstStep) {
			isFirstStep = false;
			return legDisplacementInZ;
		}
		else
			return 2 * legDisplacementInZ;
	}

	// Returns TRUE when the desired position is achieved
	bool moveUntilDesired(Vector3 desiredPos){
		//Transform the foot position to hips coordinates to compare the foot
		Vector3 footInHipsCoord = matrizReference.MultiplyPoint (leftFoot.transform.position);
		
		if (footInHipsCoord.z > desiredPos.z)
			return true;
		else {
			leftFoot.MovePosition (transform.position + transform.forward * Time.deltaTime);
			leftFoot.MoveRotation(new Quaternion(0, 0, 0, 1));
			return false;
		}
	}
}
