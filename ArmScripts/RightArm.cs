using UnityEngine;
using System.Collections;

public class RightArm : MonoBehaviour {

	public float frontTurn = 0f;
	public float backTurn = 0f;
	public float forwardBending = -0.3f;
	public float backwardBending = 0.3f;
	public bool goingFront;

	// PID constants
	float kpY = 0.01f;
	float kiY = 0.5f;
	float kdY = 100f;

	// Controller Variables
	float ePrevY;
	float eintY;

	Rigidbody rightArm;

	public Vector3 axis;

	// Use this for initialization
	void Start () {
		rightArm = GetComponent<Rigidbody> ();
		axis = new Vector3 (0,1,0);

		// Initialize PID variables
		ePrevY = 0;
		eintY = 0;
		goingFront = true;

		print ("Turns: Front = " + frontTurn + " | Back = " + backTurn);
	}

	// Update is called once per frame
	void FixedUpdate () {
		float force = Input.GetAxisRaw ("Vertical");
		
		//print ("Position Y: " + rightArm.transform.rotation.y);
		
		if (force == 1) {
			if(goingFront)
			//if(StateMachine_Twick.state == WalkState.LEFT_STEP)
				moveFront();

			else if(!goingFront)
			//if(StateMachine_Twick.state == WalkState.RIGHT_STEP)
				moveBack();

		} else {
			rightArm.constraints = RigidbodyConstraints.None;
		}
		
	}

	void moveFront(){
		print ("Moving front ...");
		if(rightArm.transform.rotation.y <= forwardBending){
			print ("Limit front");
			rightArm.constraints = RigidbodyConstraints.FreezeRotation;
			goingFront = false;
		}
		else{
			rightArm.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			//rightArm.AddTorque (axis * frontTurn);
			//print ("Position: " + (axis * Time.deltaTime * frontTurn).x + "," + (axis * Time.deltaTime * frontTurn).y + "," + (axis * Time.deltaTime * frontTurn).z);
			Quaternion deltaRotation = Quaternion.Euler(axis * Time.deltaTime * frontTurn);
			rightArm.MoveRotation(rightArm.rotation * deltaRotation);
		}
	}

	void moveBack(){
		print ("Moving BACK ...");
		if(rightArm.transform.rotation.y >= backwardBending){
			print ("Limit back");
			rightArm.constraints = RigidbodyConstraints.FreezeRotation;
			goingFront = true;
		}
		else{
			rightArm.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			//rightArm.AddTorque (axis * backTurn);
			print ("Position: " + (axis * Time.deltaTime * backTurn).x + "," + (axis * Time.deltaTime * backTurn).y + "," + (axis * Time.deltaTime * backTurn).z);
			Quaternion deltaRotation = Quaternion.Euler(axis * Time.deltaTime * backTurn);
			rightArm.MoveRotation(rightArm.rotation * deltaRotation);
		}
	}

	// Update is called once per frame
	/*void FixedUpdate () {
		float force = Input.GetAxisRaw ("Vertical");

		print ("Position Y: " + rightArm.transform.rotation.y);

		if (force == 1) {
			rightArm.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			float u = armAngleController(rightArm.transform.rotation, new Quaternion(0, forwardBending, 0, rightArm.transform.rotation.w));
			print ("u = " + u);
			rightArm.AddTorque (axis * u);
		} else {
			rightArm.constraints = RigidbodyConstraints.None;
		}

	}*/

	float armAngleController(Quaternion current, Quaternion reference){
		float eY = reference.y - current.y;

		float edotY = eY - ePrevY;
		eintY += eY;
		ePrevY = eY;

		return (kpY * eY + kiY * eintY + kdY * edotY);
	}
}
