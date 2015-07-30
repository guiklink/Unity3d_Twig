using UnityEngine;
using System.Collections;

public enum WalkState : short {STAND, CROUCH_TO_WALK, HIPS_STEP_HEIGHT, CALCULATE_LEFT_STEP, LEFT_STEP, CALCULATE_RIGHT_STEP, RIGHT_STEP};

public class StateMachine_Twick : MonoBehaviour {

	public static WalkState state;

	// Use this for initialization
	void Start () {
		state = WalkState.STAND;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//print (state);
		float force = Input.GetAxisRaw ("Vertical");
		
		if (force == 0) {
				state = WalkState.STAND;
			} 
		else if(state == WalkState.STAND)
				state = WalkState.CROUCH_TO_WALK;
	}

	void crouchedToStep(){
		state = WalkState.HIPS_STEP_HEIGHT;
		print (state);
	}

	void leftStep(){
		state = WalkState.LEFT_STEP;
		print (state);
	}

	void rightStep(){
		state = WalkState.RIGHT_STEP;
		print (state);
	}

	void calculateLeftStep(){
		state = WalkState.CALCULATE_LEFT_STEP;
		print (state);
	}
	
	void calculateRightStep(){
		state = WalkState.CALCULATE_RIGHT_STEP;
		print (state);
	}
}
