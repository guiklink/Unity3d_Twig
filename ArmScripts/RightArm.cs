﻿using UnityEngine;
using System.Collections;

public class RightArm : MonoBehaviour {

	bool goingFront;	// variable tells when the arm is about to start swinging so the swing variables can be reset
	
	Matrix4x4 refMatrix; // This matrix is used to stored the T matrix in order to draw lines just before the arm starts moving

	Rigidbody rightArm; // rigid body of the component

	// Variables to control the arm velocity and forces
	public float frontStartingSwingSpeed;
	public float backStartingSwingSpeed;
	float swingSpeed;
	float currentVelocityTemp;		// Variable to store current velocity for damping
	public float frontDampingDeltaTime; 	// Interval of time that the damping will happen entered by the user
	public float backDampingDeltaTime; 	// Interval of time that the damping will happen entered by the user
	float damper;					// Interval of time that the damping will happen (goes in the damping function)

	Vector3 axis;	// axis of torque atuation
	

	// Use this for initialization
	void Start () {
		rightArm = GetComponent<Rigidbody> ();
		axis = new Vector3 (1,0,0);
	}

	void Update(){
		drawAngleLine (rightArm.transform.localToWorldMatrix, Color.red);
	}

	// Update is called once per frame
	void FixedUpdate () {
		float force = Input.GetAxisRaw ("Vertical");
		
		//print ("Position Y: " + rightArm.transform.rotation.y);
		
		if (force == 1) {
			if(StateMachine_Twick.state == WalkState.LEFT_STEP)
				moveFront();

			else if(StateMachine_Twick.state == WalkState.RIGHT_STEP)
				moveBack();

		} else {
			rightArm.constraints = RigidbodyConstraints.FreezeRotationX;
			refMatrix = rightArm.transform.localToWorldMatrix;
		}
		
	}

	void moveFront(){
		//print ("Upper Right Arm: Moving front ...");
		if(goingFront){
			goingFront = false;
			// restart damp variables
			currentVelocityTemp = 0;
			swingSpeed = frontStartingSwingSpeed;
			damper = frontDampingDeltaTime;
		}
		else{
			rightArm.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			swingSpeed = Mathf.SmoothDampAngle(swingSpeed, 0, ref currentVelocityTemp, damper);	// Damp the swing speed to make it looks more real
			//print ("Swing speed = " + swingSpeed);
			rightArm.transform.Rotate(0, -swingSpeed * Time.deltaTime, 0);
		}
	}

	void moveBack(){
		//print ("Upper Right Arm: Moving BACK ...");
		if(!goingFront){
			goingFront = true;

			// restart damp variables
			currentVelocityTemp = 0;
			swingSpeed = backStartingSwingSpeed;
			damper = backDampingDeltaTime;
		}
		else{
			rightArm.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			swingSpeed = Mathf.SmoothDampAngle(swingSpeed, 0, ref currentVelocityTemp, damper);	// Damp the swing speed to make it looks more real
			rightArm.transform.Rotate(0, swingSpeed * Time.deltaTime, 0);
		}
	}

	void drawAngleLine(Matrix4x4 refMatrix, Color color){
		Vector3 start = Vector3.zero;
		Vector3 end = new Vector3 (5, 0, 0);

		Vector3 startInWorldCoord = refMatrix.MultiplyPoint (start);
		Vector3 endInWorldCoord = refMatrix.MultiplyPoint (end);

		Debug.DrawLine (startInWorldCoord, endInWorldCoord, color);
	}

	void setGoingFront(bool b){
		this.goingFront = b;
	}
}
