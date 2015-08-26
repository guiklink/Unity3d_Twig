using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	GameObject swat;

	// Use this for initialization
	void Start () {
		swat = GameObject.Find("/swat");
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x, transform.position.y, swat.transform.position.z);
	}
}
