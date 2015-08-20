using UnityEngine;
using System.Collections;

public enum WalkState : short {STAND, PICK_LEG, CROUCH_TO_WALK, CALCULATE_LEFT_STEP, CALCULATE_RIGHT_STEP, LEFT_STEP, RIGHT_STEP};

public class StateMachine_Twick : MonoBehaviour {

	public static WalkState state;

	// Use this for initialization
	void Start () {
		state = WalkState.STAND;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//print (state);
		//float force = Input.GetAxisRaw ("Vertical");
		float force = 0;

		if (force == 0) {
				state = WalkState.STAND;
			} 
		else if(state == WalkState.STAND)
				state = WalkState.CROUCH_TO_WALK;
	}

	void pickLeg(){
		state = WalkState.PICK_LEG;
	}

	void crouchToWalk(){
		state = WalkState.CROUCH_TO_WALK;
		//print (state);
	}

	void leftStepCalculate(){
		state = WalkState.CALCULATE_LEFT_STEP;
		//print (state);
	}
	
	void rightStepCalculate(){
		state = WalkState.CALCULATE_RIGHT_STEP;
		//print (state);
	}

	void leftStep(){
		state = WalkState.LEFT_STEP;
		//print (state);
	}

	void rightStep(){
		state = WalkState.RIGHT_STEP;
		//print (state);
	}

}
