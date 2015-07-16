using UnityEngine;
using System.Collections;

public class RightUpLeg : MonoBehaviour {

	Rigidbody rightUpLeg;
	GameObject ragdoll;

	// Use this for initialization
	void Start () {
		rightUpLeg = GetComponent<Rigidbody>();

		ragdoll = GameObject.Find("/swat");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//print ("RightUpLeg Euler Angle: " + (360 - rightUpLeg.transform.eulerAngles.x));
		if (StateMachine.state == Walk.RIGHT_STEP_UP_LEG) {
			toAngle();
			//print("RightUpLeg Angle: " + (360 - rightUpLeg.transform.eulerAngles.x));

			if(360 - rightUpLeg.transform.eulerAngles.x >= 50 && 360 - rightUpLeg.transform.eulerAngles.x <= 61){
				ragdoll.SendMessage("righUpLegLiftedFront");
			}
		}
		if(StateMachine.state == Walk.RIGHT_STEP_MID_LEG){
			toAngle();	
		}
	}

	void toAngle(){
		rightUpLeg.AddTorque (rightUpLeg.transform.right * -150);
	}
}
