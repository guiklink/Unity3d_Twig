﻿using UnityEngine;
using System.Collections;

public class RightMidLeg : MonoBehaviour {

	Rigidbody rightMidLeg;

	GameObject ragdoll;

	Vector3 lockPosition;
	Transform standingBackup;

	// Use this for initialization
	void Start () {
		rightMidLeg = GetComponent<Rigidbody>();
		ragdoll = GameObject.Find("/swat");

		standingBackup = rightMidLeg.transform;
	}

	// Update is called once per frame
	void FixedUpdate () {
		//print (StateMachine.state);
		if (StateMachine.state == Walk.RIGHT_STEP_UP_LEG) {
			rightMidLeg.AddForce (rightMidLeg.transform.forward * 0);
		}
		if (StateMachine.state == Walk.RIGHT_STEP_MID_LEG) {
			rightMidLeg.isKinematic = true;
			rightMidLeg.isKinematic = false;
			ragdoll.SendMessage("rightMidLegOnPosition");
		}
		if (StateMachine.state == Walk.RIGHT_LEG_DOWN) {
			rightMidLeg.AddForce (rightMidLeg.transform.up * -10);
		}
		if (StateMachine.state == Walk.RIGHT_STAND) {
			rightMidLeg.transform.localPosition = lockPosition;
			rightMidLeg.transform.rotation = standingBackup.rotation;
		}

		if (StateMachine.state != Walk.RIGHT_STAND) {
			lockPosition = rightMidLeg.transform.localPosition;
		}
	}


	// BACKUP

	/*// Update is called once per frame
	void FixedUpdate () {
		float force = Input.GetAxisRaw ("Vertical");
		
		if (force != 0) {
			//print ("Force: " + force);
			//print ("Constrained");
			//leftFootRB.constraints = RigidbodyConstraints.FreezePosition;
			//leftMidLegRB.constraints = RigidbodyConstraints.FreezePosition;
			//print("Left Foot:" + leftFootRB.transform.position);
			midRightLegRB.AddTorque (midRightLegRB.transform.right * -200 * force);

			//float x = midRightLegRB.transform.rotation.eulerAngles.x;
			//print ("x = " + x);
			float footDistance = rightFoot.transform.position.z - leftFoot.transform.position.z;
			//print(footDistance);
			if(!isAhead && footDistance >= 0.29){
				//print("WORK!!!");
				hips.SendMessage("rightLegUpCheck");
				isAhead = true;
			}
		} 
	}*/
}
