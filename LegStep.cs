using UnityEngine;
using System.Collections;

public class LegStep : MonoBehaviour {

	Rigidbody upperLegRB;
	Rigidbody rightLegRB;
	Rigidbody rightFootRB;

	GameObject leg;
	GameObject rightLeg;
	GameObject rightFoot;

	// Use this for initialization
	void Start () {
		upperLegRB = GetComponent<Rigidbody>();

		// Store Position of the relevant body parts (this must be made generic eventually)
		leg = GameObject.Find("/swat/Hips/LeftUpLeg/LeftLeg");
		rightLeg = GameObject.Find("/swat/Hips/RightUpLeg");
		rightLegRB = rightLeg.GetComponent<Rigidbody>();
		rightFoot = GameObject.Find("/swat/Hips/RightUpLeg/RightLeg/RightFoot");
		rightFootRB = rightFoot.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float force = Input.GetAxisRaw ("Vertical");

		if (force != 0) {
			print ("Force: " + force);
			//rightLegRB.constraints = RigidbodyConstraints.FreezeAll;
			//rightFootRB.constraints = RigidbodyConstraints.FreezeAll;
			//Vector3 upperLegForceUpdate = new Vector3 (0, 0, 60);
			//upperLegRB.AddForce(upperLegForceUpdate);
			upperLegRB.AddTorque(transform.forward * 200 * force);
		}

		//rightLegRB.constraints = RigidbodyConstraints.None;
		//rightFootRB.constraints = RigidbodyConstraints.None;
			
		print ("Upper Leg Angle: " + upperLegRB.transform.rotation.eulerAngles);
		print ("Upper Knee Angle: " + leg.transform.rotation.eulerAngles);
	}
}
