using UnityEngine;
using System.Collections;

public class RightMidLeg : MonoBehaviour {

	Rigidbody midLegRB;
	Rigidbody rightLegRB;
	Rigidbody rightFootRB;
	
	GameObject leftLeg;

	// Use this for initialization
	void Start () {
		// MOVING right leg
		midLegRB = GetComponent<Rigidbody>();

		// STEADY left leg
		leftLeg = GameObject.Find("/swat/Hips/LeftUpLeg/LeftLeg");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float force = Input.GetAxisRaw ("Vertical");
		
		if (force != 0) {
			print ("Force: " + force);
			midLegRB.AddTorque(midLegRB.transform.right * -200 * force);
		}

		print ("Upper Leg Angle: " + midLegRB.transform.rotation.eulerAngles);
		print ("Upper Knee Angle: " + leftLeg.transform.rotation.eulerAngles);
	}
}
