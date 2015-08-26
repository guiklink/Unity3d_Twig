using UnityEngine;
using System.Collections;

public enum WalkState : short {STAND, PICK_LEG, CROUCH_TO_WALK, RISE_TO_STAND, CALCULATE_LEFT_STEP, CALCULATE_RIGHT_STEP, LEFT_STEP, RIGHT_STEP};

public class StateMachine_Twick : MonoBehaviour {

	public static WalkState state;	// GAIT state
	public static bool isFirstStep;	// Tells if it is the first step from the STAND state, in this case the leg will move less
	public static bool finalizingMovement; // If this is true after this step the ragdoll should go back to the STAND position
	public bool turned;				// Variable tells if the doll turned it maximum amount on this update

	GameObject hips;

	// Use this for initialization
	void Start () {
		hips = GameObject.Find("/swat/Hips"); 

		state = WalkState.STAND;
		isFirstStep = true;
		finalizingMovement = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//print (state);
		//print (finalizingMovement);
		float force = Input.GetAxisRaw ("Vertical");
		//float force = 0;
		float twist = Input.GetAxisRaw ("Horizontal");

		if (twist == 1) {
			hips.SendMessage("turnRight");
		}
		else if (twist == -1) {
			hips.SendMessage("turnLeft");
		}

		if (force == 0) {
			if(state == WalkState.STAND){
				state = WalkState.STAND;
			}
			else{
				finalizingMovement = true;	// telling that it should give the last step 
			}
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

	void stepped(){
		if (state == WalkState.LEFT_STEP)
			rightStepCalculate ();
		if (state == WalkState.RIGHT_STEP)
			leftStepCalculate ();
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

	void firstStepGiven(){
		isFirstStep = false;
	}

	void liftingToStand(){
		state = WalkState.RISE_TO_STAND;
	}

	void standPosition(){
		finalizingMovement = false;
		isFirstStep = true;
		state = WalkState.STAND;
	}

}
