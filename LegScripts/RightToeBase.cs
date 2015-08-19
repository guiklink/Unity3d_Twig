using UnityEngine;
using System.Collections;

public class RightToeBase : MonoBehaviour {
	
	Transform rightToe;

	GameObject foot;
	GameObject ragdoll;

	static public Vector3 footPosHitFloor;

	// Use this for initialization
	void Start () {
		rightToe = GetComponent<Transform>();
		ragdoll = GameObject.Find("/swat");
		foot = GameObject.Find("/swat/Hips/RightUpLeg/RightLeg/RightFoot");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (StateMachine.state == Walk.RIGHT_LEG_DOWN) {
			//print (foot.transform.position.y);
			if (rightToe.position.y >= 0.09f && rightToe.position.y <= 0.108f){
				//print ("TOUCH FLOOR");
				footPosHitFloor = foot.transform.position;
				//ragdoll.SendMessage("standOnRightLeg");
			}
		}
	}
}
