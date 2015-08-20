using UnityEngine;
using System.Collections;

public class RightArm : MonoBehaviour {

	public float frontTurn = 0f;
	public float backTurn = 0f;
	public float forwardBending = -0.3f;
	public float backwardBending = 0.3f;
	public float forwardBendingDegrees;
	public bool goingFront;

	// PID constants
	float kpY = 0.01f;
	float kiY = 0.5f;
	float kdY = 100f;

	// Controller Variables
	float ePrevY;
	float eintY;

	Matrix4x4 refMatrix; // This matrix is used to stored the T matrix in order to draw lines just before the arm starts moving

	Rigidbody rightArm; // rigid body of the component

	// Variables to control the arm velocity and forces
	public float startingSwingSpeed;
	float swingSpeed;
	float currentVelocityTemp;		// Variable to store current velocity for damping
	public float dampingDeltaTime; 	// Interval of time that the damping will happen entered by the user
	float damper;					// Interval of time that the damping will happen (goes in the damping function)

	Vector3 axis;	// axis of torque atuation



	// Use this for initialization
	void Start () {
		rightArm = GetComponent<Rigidbody> ();
		axis = new Vector3 (1,0,0);

		// Initialize PID variables
		ePrevY = 0;
		eintY = 0;
		goingFront = true;

		// Initialize arm swing variables
		swingSpeed = startingSwingSpeed;
		damper = dampingDeltaTime;

		print ("Turns: Front = " + frontTurn + " | Back = " + backTurn + "\nStarting swing speed = " + startingSwingSpeed + "\nDamping delta time = " + dampingDeltaTime);
	}

	void Update(){
		drawAngleLine (rightArm.transform.localToWorldMatrix, Color.red);
		drawGoalLine ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		float force = Input.GetAxisRaw ("Vertical");
		
		//print ("Position Y: " + rightArm.transform.rotation.eulerAngles.y);
		
		if (force == 1) {
			if(goingFront)
			//if(StateMachine_Twick.state == WalkState.LEFT_STEP)
				moveFront();

			else if(!goingFront)
			//if(StateMachine_Twick.state == WalkState.RIGHT_STEP)
				moveBack();

		} else {
			rightArm.constraints = RigidbodyConstraints.None;
			refMatrix = rightArm.transform.localToWorldMatrix;
		}
		
	}

	void moveFront(){
		print ("Moving front ...");
		if(rightArm.transform.rotation.y <= Quaternion.Euler(0, forwardBendingDegrees, 0).y){
			print ("Limit front");
			rightArm.constraints = RigidbodyConstraints.FreezeRotation;
			goingFront = false;

			// restart damp variables
			currentVelocityTemp = 0;
			swingSpeed = startingSwingSpeed;
			damper = dampingDeltaTime * 2;
		}
		else{
			rightArm.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			swingSpeed = Mathf.SmoothDampAngle(swingSpeed, 0, ref currentVelocityTemp, damper);	// Damp the swing speed to make it looks more real
			print ("Swing speed = " + swingSpeed);
			rightArm.transform.Rotate(0, -swingSpeed * Time.deltaTime, 0);

			//rightArm.AddTorque (axis * frontTurn);
			//print ("Position: " + (axis * Time.deltaTime * frontTurn).x + "," + (axis * Time.deltaTime * frontTurn).y + "," + (axis * Time.deltaTime * frontTurn).z);
			//Quaternion deltaRotation = Quaternion.Euler(axis * Time.deltaTime * frontTurn);
			//rightArm.MoveRotation(rightArm.rotation * deltaRotation);
		}
	}

	void moveBack(){
		print ("Moving BACK ...");
		if(rightArm.transform.rotation.y >= backwardBending){
			print ("Limit back");
			rightArm.constraints = RigidbodyConstraints.FreezeRotation;
			goingFront = true;

			// restart damp variables
			currentVelocityTemp = 0;
			swingSpeed = startingSwingSpeed;
			damper = dampingDeltaTime * 2;
		}
		else{
			rightArm.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			swingSpeed = Mathf.SmoothDampAngle(swingSpeed, 0, ref currentVelocityTemp, damper);	// Damp the swing speed to make it looks more real
			rightArm.transform.Rotate(0, swingSpeed * Time.deltaTime, 0);

			//rightArm.AddTorque (axis * backTurn);
			//print ("Position: " + (axis * Time.deltaTime * backTurn).x + "," + (axis * Time.deltaTime * backTurn).y + "," + (axis * Time.deltaTime * backTurn).z);
			//Quaternion deltaRotation = Quaternion.Euler(axis * Time.deltaTime * backTurn);
			//rightArm.MoveRotation(rightArm.rotation * deltaRotation);
		}
	}

	void drawAngleLine(Matrix4x4 refMatrix, Color color){
		Vector3 start = Vector3.zero;
		Vector3 end = new Vector3 (5, 0, 0);

		Vector3 startInWorldCoord = refMatrix.MultiplyPoint (start);
		Vector3 endInWorldCoord = refMatrix.MultiplyPoint (end);

		Debug.DrawLine (startInWorldCoord, endInWorldCoord, color);
	}

	void drawGoalLine(){
		Matrix4x4 rotatedMatrix = refMatrix * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, forwardBendingDegrees, 0), new Vector3(1,1,1));

		drawAngleLine (rotatedMatrix, Color.green);
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
