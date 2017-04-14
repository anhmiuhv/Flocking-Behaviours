/*
 * Implement the evade and pusuit
 */ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rule
{
    public int i;
	Fish g;
	public Rule(int i, Fish g)
    {
        this.i = i;
		this.g = g;
    }
	public void execute(ref List<Fish> f)
    {
		foreach (Fish d in g.perception) {
			if (d.r.i == (i + 1) % 3) {
				pursuit (d);
			}
			if (d.r.i == (i + 2) % 3) {
				evade(d);
			}
		}
    }

	public void pursuit(Fish f){
		Vector3 future = f.position + f.velocity * Time.fixedDeltaTime;
		Vector3 v = g.Seek (future);
		g.velocity += v; 
	}



	public void evade(Fish f){
		Vector3 future = f.position + f.velocity * Time.fixedDeltaTime;
		Vector3 v = g.Flee (future);
		g.velocity += v;
	}
}
