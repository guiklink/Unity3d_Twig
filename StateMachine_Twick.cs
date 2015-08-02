using UnityEngine;
using System.Collections;

public enum WalkState : short {STAND, PICK_LEG, LEFT_CROUCH, RIGHT_CROUCH, LEFT_STEP, RIGHT_STEP};

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
				state = WalkState.PICK_LEG;
	}

	void leftCrouch(){
		state = WalkState.LEFT_CROUCH;
		print (state);
	}

	void rightCrouch(){
		state = WalkState.RIGHT_STEP;
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

}
