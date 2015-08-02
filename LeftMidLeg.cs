using UnityEngine;
using System.Collections;

public class LeftMidLeg : MonoBehaviour {

	Rigidbody midLeg;
	Vector3 lockPosition;
	Vector3 forceVector;	

	// Use this for initialization
	void Start () {
		midLeg = GetComponent<Rigidbody>();

		forceVector = new Vector3 (0, 0, 100);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}
}
