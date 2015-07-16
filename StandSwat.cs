using UnityEngine;
using System.Collections;

public class StandSwat : MonoBehaviour {
	
	public float kpX = 100f;
	public float kdX = 100f;
	float ePrevX = 0;
	public float kpY = 10000f;
	public float kpZ = 100f;
	public float kdZ = 100f;
	float ePrevZ = 0;
	public float walkingSpeed = 1000f;

	float standError;
	Vector3 alignError;
	Vector3 standingPosition;
	float balancingHeight;
	Rigidbody rb;
	Vector3 midPointPrev;

	GameObject leftFoot;
	GameObject rightFoot;
	GameObject hips;
	Vector3 midPoint;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();

		// Store Position of the relevant body parts (this must be made generic eventually)
		leftFoot = GameObject.Find("/swat/Hips/LeftUpLeg/LeftLeg");
		rightFoot = GameObject.Find("/swat/Hips/RightUpLeg/RightLeg");
		hips = GameObject.Find("/swat/Hips");

		InitPD ();
	}

	void InitPD()
	{
		standError = 0f;

		// Use this variable to store the Y reference of standing
		standingPosition = transform.position;
		balancingHeight = standingPosition.y;

		//This must be removed once the doll starts moving
		midPoint = (rightFoot.transform.position + leftFoot.transform.position) / 2;
		midPointPrev = (rightFoot.transform.position + leftFoot.transform.position)/2;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//print ("State from STAND: " + StateMachine.state);

		if (StateMachine.state == Walk.STAND)
			//midPoint = (rightFoot.transform.position + leftFoot.transform.position) / 2;
			midPoint = midPoint;
		else
			midPoint = new Vector3 (midPoint.x, midPoint.y, (rightFoot.transform.position.z + leftFoot.transform.position.z) / 2);
			//midPoint = midPoint;
		//print ("Midpoint: " + midPoint);

		if (StateMachine.state == Walk.RIGHT_LEG_DOWN) {
			standPID (balancingHeight);
			balancingHeight -= 0.001f;
		} else {
			standPID (standingPosition.y);
		}
	}	
	
	// PID Loop Function

	void standPID(float heightReference){
		// Calculate Errors
		standError = heightReference - transform.position.y;
		alignError = midPoint - transform.position;
		
		// Calculate Edot 
		float eDotX = alignError.x - ePrevX;
		float eDotZ = alignError.z - ePrevZ;
		
		// P control on the Axes
		//Vector3 forceUpdate = new Vector3 (kpX * alignError.x ,standError.y * kpY, kpZ * alignError.z);
		//force *= walkingSpeed;
		//print ("Force: " + force);
		//Vector3 forceUpdate = new Vector3 (kpX * alignError.x + kdX * eDotX, standError.y * kpY, kpZ * alignError.z + kdZ * eDotZ + force);
		Vector3 forceUpdate = new Vector3 (kpX * alignError.x + kdX * eDotX, standError * kpY, kpZ * alignError.z + kdZ * eDotZ);
		ePrevX = alignError.x;
		ePrevZ = alignError.z;
		rb.AddForce (forceUpdate);
	}

	void printFeetPosition(){
		print ("Right foot: " + rightFoot.transform.position);
		print ("Left foot: " + leftFoot.transform.position);
	}
}
