using UnityEngine;
using System.Collections;

public class RightMidLeg : MonoBehaviour {

	Rigidbody midRightLegRB;
	Rigidbody leftFootRB;
	Rigidbody leftMidLegRB;
	
	GameObject leftFoot;
	GameObject leftMidLeg;
	GameObject hips;
	GameObject rightFoot;

	//Variable to know if the foot is ahead or behind
	bool isAhead;

	// Use this for initialization
	void Start () {
		// MOVING right leg
		midRightLegRB = GetComponent<Rigidbody>();
		rightFoot = GameObject.Find("/swat/Hips/RightUpLeg/RightLeg");


		// STEADY left leg
		leftFoot = GameObject.Find("/swat/Hips/LeftUpLeg/LeftLeg/LeftFoot");
		leftFootRB = leftFoot.GetComponent<Rigidbody>();
		leftMidLeg = GameObject.Find("/swat/Hips/LeftUpLeg/LeftLeg");
		leftMidLegRB = leftMidLeg.GetComponent<Rigidbody>();

		// Reference for the hips
		hips = GameObject.Find("/swat/Hips");

		//Foot doesnt start ahead
		isAhead = false;
	}
	
	// Update is called once per frame
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
	}
}
