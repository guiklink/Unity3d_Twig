using UnityEngine;
using System.Collections;

public enum ControllerType : short {P, PI, PID};

public class ControllerPID {

	ControllerType type;
	float kp;
	float ki;
	float kd;


	public ControllerPID(ControllerType type, float kp, float ki, float kd){
		this.type = type;
		this.kp = kp;
		this.ki = ki;
		this.kd = kd;
	}


}
