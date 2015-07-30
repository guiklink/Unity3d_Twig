using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeftFoot : MonoBehaviour {

	float stepSpeed_x = 0.5f;
	float stepSpeed_y = 0.5f;
	float stepSpeed_z = 0.5f;

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
		//print (distanceFootToHip);
		
		if (StateMachine_Twick.state == WalkState.RIGHT_STEP || StateMachine_Twick.state == WalkState.CALCULATE_RIGHT_STEP)
			leftFoot.transform.position = lockPosition;
		else if (StateMachine_Twick.state == WalkState.CALCULATE_LEFT_STEP) {
			//print("Current left foot pos: " + leftFoot.transform.position);
			//print("Left foot position to update: " + calculateFootTrajectory());
			footTraj = calculateFootTrajectory ();
			ragdoll.SendMessage ("leftStep");
		} else if (StateMachine_Twick.state == WalkState.LEFT_STEP) {
			leftFoot.isKinematic = true;
			if(moveFoot(footTraj))
				print("In position.");
		}
		else
			lockPosition = leftFoot.transform.position;
	}

	// Returned stored positions for feet trajectories in world coordinates
	Vector3 calculateFootTrajectory(){
		float h = StandSwat.walkingHeight;
		float H = distanceToeBaseToHip; // Only use 90% of the distance between hip and foot
		//print ("H = " + H);
		//print ("h = " + h);
		float legDisplacementInZ = Mathf.Sqrt (Mathf.Pow(H,2) - Mathf.Pow(h,2));
		//print ("Leg displacement: " + legDisplacementInZ);

		Vector3 newPosition = leftFoot.transform.localToWorldMatrix.MultiplyPoint (new Vector3(0, 0, legDisplacementInZ));
		return newPosition;
	}

	// Move foot at internal clock rate (give the desired world coordinates)
	bool moveFoot(Vector3 desiredPos){
		print ("---------------------------------------------------------------------------------");
		print("Left foot position ( X: " + leftFoot.transform.position.x + " | Y: " + leftFoot.transform.position.y + " | Z: " + leftFoot.transform.position.z + " )");
		print("DESIRED Left foot position ( X: " + desiredPos.x + " | Y: " + desiredPos.y + " | Z: " + desiredPos.z + " )");
		if (leftFoot.transform.position.x == desiredPos.x && leftFoot.transform.position.y == desiredPos.y && leftFoot.transform.position.z == desiredPos.z)
			return true;
		else {
			// Increment the foot position by a factor of time
			Vector3 tmpPos = new Vector3(Time.deltaTime * stepSpeed_x, Time.deltaTime * stepSpeed_y, Time.deltaTime * stepSpeed_z);

			// Converts to reference to foot coordinates to check if the position is not being overlapped
			Vector3 desiredPosInFootCoord = leftFoot.transform.worldToLocalMatrix.MultiplyPoint(desiredPos);

			print("\nTMP position ( X: " + tmpPos.x + " | Y: " + tmpPos.y + " | Z: " + tmpPos.z + " )");
			print("DESIRED converted foot position ( X: " + desiredPosInFootCoord.x + " | Y: " + desiredPosInFootCoord.y + " | Z: " + desiredPosInFootCoord.z + " )");


			//Make sure that the position is not overlapped
			if(tmpPos.x > desiredPosInFootCoord.x)
				tmpPos.x = desiredPosInFootCoord.x;
			if(tmpPos.y > desiredPosInFootCoord.y)
				tmpPos.y = desiredPosInFootCoord.y;
			if(tmpPos.z > desiredPosInFootCoord.z)
				tmpPos.z = desiredPosInFootCoord.z;

			print("\nTMP CALCULATED ( X: " + tmpPos.x + " | Y: " + tmpPos.y + " | Z: " + tmpPos.z + " )");

			// Trasnform to the world frame and move to it
			tmpPos = leftFoot.transform.localToWorldMatrix.MultiplyPoint(tmpPos);

			print("\nTMP UPDATE ( X: " + tmpPos.x + " | Y: " + tmpPos.y + " | Z: " + tmpPos.z + " )");
			print ("---------------------------------------------------------------------------------");

			leftFoot.MovePosition(tmpPos);
			//leftFoot.transform.position = tmpPos;
			return false;
		}
	}
}
