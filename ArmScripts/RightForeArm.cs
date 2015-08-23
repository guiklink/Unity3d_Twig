using UnityEngine;
using System.Collections;

public class RightForeArm : MonoBehaviour {

	public float turn;
	Rigidbody foreArm;

	// Swing speed variables
	public float swingSpeedInput;

	float force1 = 0f;

	// Use this for initialization
	void Start () {
		foreArm = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {
		drawAngleLine (foreArm.transform.localToWorldMatrix, Color.magenta);
	}

	// Update is called once per frame
	void FixedUpdate () {
		float force = Input.GetAxisRaw ("Vertical");

		if (force == 1)
			force1 = 1;

		if (force1 == 1) {
			moveFront ();
		} else {
			foreArm.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
			foreArm.constraints &= ~RigidbodyConstraints.FreezePositionY;
		}
	}

	void moveFront(){
		print ("Right forearm: Moving front ...");
		foreArm.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		foreArm.constraints &= ~RigidbodyConstraints.FreezePositionY;

		foreArm.transform.Rotate(0, -180 * Time.deltaTime, 0);
	}

	void drawAngleLine(Matrix4x4 refMatrix, Color color){
		Vector3 start = Vector3.zero;
		Vector3 end = new Vector3 (5, 0, 0);
		
		Vector3 startInWorldCoord = refMatrix.MultiplyPoint (start);
		Vector3 endInWorldCoord = refMatrix.MultiplyPoint (end);
		
		Debug.DrawLine (startInWorldCoord, endInWorldCoord, color);
	}
	
	void drawGoalLine(){
		//Matrix4x4 rotatedMatrix = refMatrix * Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, forwardBendingDegrees, 0), new Vector3(1,1,1));
		
		//drawAngleLine (rotatedMatrix, Color.green);
	}
}
