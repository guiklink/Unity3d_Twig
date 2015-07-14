using UnityEngine;
using System.Collections;

public class LeftMidLeg : MonoBehaviour {

	Rigidbody midLeftLegRB;
	GameObject leftFoot;

	Vector3 lockFootPos;

	// Use this for initialization
	void Start () {

		midLeftLegRB = GetComponent<Rigidbody>();
		lockFootPos= midLeftLegRB.transform.position;

		leftFoot = GameObject.Find("/swat/Hips/LeftUpLeg/LeftLeg/LeftFoot");
		//lockFootPos = leftFoot.transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float force = Input.GetAxisRaw ("Vertical");
		
		if (force != 0) {
			//leftFoot.transform.position = lockFootPos;
			midLeftLegRB.transform.position = lockFootPos;
		} else {
			lockFootPos= midLeftLegRB.transform.position;
		}
	}
}
