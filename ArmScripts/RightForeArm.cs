using UnityEngine;
using System.Collections;

public class RightForeArm : MonoBehaviour {

	public float turn;
	Rigidbody foreArm;

	// Use this for initialization
	void Start () {
		foreArm = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//foreArm.AddTorque (transform.right * turn);
		//foreArm.AddForce (foreArm.transform.forward * turn);
		float force = Input.GetAxisRaw ("Vertical");
		
		if (force == 1) {
			//foreArm.MovePosition (transform.position + transform.forward * Time.deltaTime);
			foreArm.AddForce(transform.forward * turn);
		}
	}
}
