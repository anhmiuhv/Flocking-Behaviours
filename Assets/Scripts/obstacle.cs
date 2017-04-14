/*
 * Define the behaviour for the obstacle
 */ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle {

	float radius;
	public Vector3 position;
	float aoe {
		get {
			return (radius + 1) * (radius + 1);
		}
	}
	GameObject g;
	public obstacle(GameObject g){
		radius = 1;
		position = new Vector3 ();
		this.g = g;
		g.transform.position = position;
	}
	//Generate force field
	public void push(Fish f) {
		Vector3 force = f.position - position;
		float distance = force.sqrMagnitude;
		if (distance < aoe){
			f.AddForce (force.normalized * (1 + aoe / Mathf.Abs(distance - (radius*radius))));
		}
	}
}
