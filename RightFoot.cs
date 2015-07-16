using UnityEngine;
using System.Collections;

public class RightFoot : MonoBehaviour {


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}

	void OnTriggerEnter(Collider other) {
		print("COLIDED!!!!");
	}
}
