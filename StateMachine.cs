using UnityEngine;
using System.Collections;

public enum Walk : short {STAND ,RIGHT_STEP_UP_LEG, RIGHT_STEP_MID_LEG, RIGHT_LEG_DOWN, RIGHT_STAND};

public class StateMachine : MonoBehaviour {
	
	public static Walk state;

	// Use this for initialization
	void Start () {
		state = Walk.STAND;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//print (state);
		float force = Input.GetAxisRaw ("Vertical");

		if (force == 0) {
			state = Walk.STAND;
		} else {
			if(state == Walk.STAND)
				state = Walk.RIGHT_STEP_UP_LEG;
		}
	}

	void righUpLegLiftedFront(){
		print ("changing to RIGHT_STEP_MID_LEG...");
		state = Walk.RIGHT_STEP_MID_LEG;
	}

	void rightMidLegOnPosition(){
		print ("changing to RIGHT_LEG_DOWN...");
		state = Walk.RIGHT_LEG_DOWN;
	}

	void standOnRightLeg(){
		print ("changing to RIGHT_STAND...");
		state = Walk.RIGHT_STAND;
	}
}
