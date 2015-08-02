using UnityEngine;
using System.Collections;

public class StandSwat : MonoBehaviour {

	// Height the doll should crouch before walking
	public static float walkingHeight = 0.9f;

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
	GameObject ragdoll;
	Vector3 midPoint;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();

		// Store Position of the relevant body parts (this must be made generic eventually)
		leftFoot = GameObject.Find("/swat/Hips/LeftUpLeg/LeftLeg/LeftFoot");
		rightFoot = GameObject.Find("/swat/Hips/RightUpLeg/RightLeg/RightFoot");
		hips = GameObject.Find("/swat/Hips");
		ragdoll = GameObject.Find("/swat");

		InitPD ();
	}

	void InitPD()
	{
		standError = 0f;

		// Use this variable to store the Y reference of standing
		standingPosition = transform.position;

		// THIS VARIABLE NEEDS TO BE ACCOUNTED FOR CASES THAT THE DOLL DOESNT START IN Y = 0
		balancingHeight = standingPosition.y;

		//This must be removed once the doll starts moving
		midPoint = (rightFoot.transform.position + leftFoot.transform.position) / 2;
		midPointPrev = (rightFoot.transform.position + leftFoot.transform.position)/2;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//print (balancingHeight);

		// The mid point controls the X-axis and Z-axis of the ragdoll alligned according to its feet
		if (StateMachine_Twick.state == WalkState.STAND)
			midPoint = midPoint;

		else if (StateMachine_Twick.state == WalkState.PICK_LEG){
			//if(shouldRightLegStep)
			//	ragdoll.SendMessage("rightCrouch");
			//else
			ragdoll.SendMessage("leftCrouch");
			//ragdoll.SendMessage("rightCrouch");
		}

		else if (StateMachine_Twick.state == WalkState.LEFT_CROUCH) {
			balancingHeight -= 0.003f;
			if (balancingHeight <= walkingHeight)
				ragdoll.SendMessage ("leftStep");
		}

		else if (StateMachine_Twick.state == WalkState.RIGHT_CROUCH) {
			balancingHeight -= 0.003f;
			if (balancingHeight <= walkingHeight)
				ragdoll.SendMessage ("rightStep");
		} 

		if (StateMachine_Twick.state != WalkState.STAND)
			midPoint = new Vector3 (midPoint.x, midPoint.y, (rightFoot.transform.position.z + leftFoot.transform.position.z) / 2);

		standPID (midPoint.x, balancingHeight, midPoint.z);
	}	
	
	// PID Loop Function get the coordinates that the body will be maintained

	void standPID(float x_reference, float y_reference, float z_Reference){
		// Calculate Errors
		standError = y_reference - transform.position.y;

		Vector3 x_z_reference = new Vector3 (x_reference, 0, z_Reference);
		alignError = x_z_reference - transform.position;
		
		// Calculate Edot 
		float eDotX = alignError.x - ePrevX;
		float eDotZ = alignError.z - ePrevZ;
		
		// P control on the Axes
		//Vector3 forceUpdate = new Vector3 (kpX * alignError.x ,standError.y * kpY, kpZ * alignError.z);
		//force *= walkingSpeed;
		//print ("Force: " + force);
		//if(force != null)
		//Vector3 forceUpdate = new Vector3 (kpX * alignError.x + kdX * eDotX, standError * kpY, kpZ * alignError.z + kdZ * eDotZ + force);
		//else
		Vector3 forceUpdate = new Vector3 (kpX * alignError.x + kdX * eDotX, standError * kpY, kpZ * alignError.z + kdZ * eDotZ);
		ePrevX = alignError.x;
		ePrevZ = alignError.z;
		rb.AddForce (forceUpdate);
	}

	// Function returns true if right leg should give the next step and false if left leg should step
	bool shouldRightLegStep(){
		Vector3 leftFootCoordInHipCoord = rb.transform.localToWorldMatrix.MultiplyPoint (leftFoot.transform.position);
		Vector3 rightFootCoordInHipCoord = rb.transform.localToWorldMatrix.MultiplyPoint (rightFoot.transform.position);

		if (rightFootCoordInHipCoord.z >= leftFootCoordInHipCoord.z)
			return true;
		else
			return false;
	}

	void printFeetPosition(){
		print ("Right foot: " + rightFoot.transform.position);
		print ("Left foot: " + leftFoot.transform.position);
	}
}
