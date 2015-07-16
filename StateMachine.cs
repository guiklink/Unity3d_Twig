using UnityEngine;
using System.Collections;

public enum Walk : short {STAND ,RIGHT_STEP_UP_LEG, RIGHT_STEP_MID_LEG, RIGHT_LEG_DOWN};

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
		state = Walk.RIGHT_STEP_MID_LEG;
	}

	void rightMidLegOnPosition(){
		state = Walk.RIGHT_LEG_DOWN;
	}
}
