using UnityEngine;
using System.Collections;

public class StandSwat : MonoBehaviour {
	
	public float kpX = 100f;
	public float kdX = 100f;
	float ePrevX = 0;
	public float kpY = 10000f;
	public float kpZ = 100f;
	public float kdZ = 100f;
	float ePrevZ = 0;
	public float walkingSpeed = 1000f;

	Vector3 standError;
	Vector3 alignError;
	Vector3 standingPosition;
	Rigidbody rb;
	Vector3 midPointPrev;

	GameObject leftFoot;
	GameObject rightFoot;
	GameObject hips;
	Vector3 midPoint;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();

		// Store Position of the relevant body parts (this must be made generic eventually)
		leftFoot = GameObject.Find("/swat/Hips/LeftUpLeg/LeftLeg");
		rightFoot = GameObject.Find("/swat/Hips/RightUpLeg/RightLeg");
		hips = GameObject.Find("/swat/Hips");


		InitPD ();
	}

	void InitPD()
	{
		standError = new Vector3(0,0,0);

		// Use this variable to store the Y reference of standing
		standingPosition = transform.position;

		//This must be removed once the doll starts moving
		midPoint = (rightFoot.transform.position + leftFoot.transform.position) / 2;
		midPointPrev = (rightFoot.transform.position + leftFoot.transform.position)/2;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float force = Input.GetAxisRaw ("Vertical");

		// Update within the line of movement
		float rotationOnY = hips.transform.rotation.eulerAngles.y;
		//print ("Rotation: " + hips.transform.rotation.eulerAngles);
		//midPoint = CalculateFootAlignment(rotationOnY);

		// Ensure that the puppet walks in a straigh line
		if (force == 0)
			//midPoint = (rightFoot.transform.position + leftFoot.transform.position) / 2;
			midPoint = midPoint;
		else
			//midPoint = new Vector3 (midPoint.x, midPoint.y, (rightFoot.transform.position.z + leftFoot.transform.position.z) / 2);
			midPoint = midPoint;
		print ("Midpoint: " + midPoint);

		// Calculate Errors
		standError = standingPosition - transform.position;
		alignError = midPoint - transform.position;

		// Calculate Edot 
		float eDotX = alignError.x - ePrevX;
		float eDotZ = alignError.z - ePrevZ;

		// P control on the Axes
		//Vector3 forceUpdate = new Vector3 (kpX * alignError.x ,standError.y * kpY, kpZ * alignError.z);
		force *= walkingSpeed;
		//print ("Force: " + force);
		Vector3 forceUpdate = new Vector3 (kpX * alignError.x + kdX * eDotX, standError.y * kpY, kpZ * alignError.z + kdZ * eDotZ + force);
		ePrevX = alignError.x;
		ePrevZ = alignError.z;
		rb.AddForce (forceUpdate);
	}

	// Implement something to lock the puppet allignment
	Vector3 CalculateFootAlignment(float hipsRot){
		Vector3 rawAllignment = (rightFoot.transform.position + leftFoot.transform.position)/2;

		float x = (rawAllignment.x - midPointPrev.x) * Mathf.Cos (hipsRot);
		float y = rawAllignment.y - midPointPrev.y;
		float z = (rawAllignment.z - midPointPrev.z) * Mathf.Sin (hipsRot);

		print ("X: " + Mathf.Cos (hipsRot));
		print ("Z: " + Mathf.Sin (hipsRot));
		Vector3 update = new Vector3 (midPointPrev.x + x, midPointPrev.y + y, midPointPrev.z + z);

		midPointPrev = rawAllignment;
		return update;
		//return rawAllignment;
	}
}
