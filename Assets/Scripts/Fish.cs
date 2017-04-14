/*
 * this module handles the fish behaviour
 */ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish
{
    float mass = 1;
	float maxSpeed = 20;
    float fov;
    CircleCollide collide;
    EulerSolver solver;
    public Vector3 position;
	Quaternion orientation;
    Vector3 acceleration;
    public Vector3 velocity;
	Vector3 oldPosition;
    GameObject go;
	public List<Fish> perception =  new List<Fish>();
    public Rule r;
    public Fish(GameObject g, int r)
    {
		maxSpeed = Constants.maxspeed [r];
		fov = Constants.fov [r];
        position = new Vector3(Random.Range(-5 ,5) , Random.Range(-5, 5), 0);
		//Avoid hitting obstacle
		if (Mathf.Abs (position.x) < 1) {
			position.x += 5;
		}
		if (Mathf.Abs (position.y) < 1) {
			position.y += 5;
		}
		orientation = g.transform.rotation;
		oldPosition = new Vector3 ();
        acceleration = new Vector3();
		velocity = Vector3.Normalize(new Vector3(Random.Range(-5, 5) * 0.5F, Random.Range(-5, 5) * 0.5F, 0)) * maxSpeed;
        collide = new CircleCollide(position);
        go = g;
		solver = new EulerSolver ();
		this.r = new Rule(r, this);
		g.transform.position = position;
    }
	//perception field
    public bool seeThat(Fish f)
    {
        Vector3 length = f.position - position;
        if (length.sqrMagnitude < fov * fov)
        {
            return true;
        }
        return false;
    }
	//Update cycle
	public void Update(ref List<Fish> f, ref List<obstacle> obs, float delta)
    {
			
		UpdatePerception (f);
        navigationModule(ref f);
		r.execute (ref f);
		solver.Solve (ref oldPosition, ref position, ref velocity, ref acceleration, delta);
		acceleration = new Vector3 ();
		detectCollide (ref f);
		UpdatePosition ();
		UpdateOrientation ();
		noOverlap (ref f,ref obs);
		collide.position = position;
		go.transform.rotation = orientation;
		go.transform.position = position;


    }
	//want to catch that fish
	bool caught(Fish f) {
		float mag = (position - f.position).sqrMagnitude;
		return mag <= 1f;
	}
	//steering
	void detectCollide(ref List<Fish> f) {
		
		foreach (Fish d in f) {
			if (d != this) {
				if (this.collide.collided(d.collide)) {
					acceleration += this.Flee (d.position) * 2;

				}

					if (this.caught (d)) {
					if (d.r.i == (r.i + 1) % 3) {
						d.convert (this);
					}
					}

			}
		}
	}
	//Warp position
    void UpdatePosition()
    {
        if (position.x < Constants.domains.xMin)
        {
            position.x = Constants.domains.xMax;
        }
        if (position.x > Constants.domains.xMax)
        {
            position.x = Constants.domains.xMin;
        }
        if (position.y < Constants.domains.yMin)
        {
            position.y = Constants.domains.yMax;
        }
        if (position.y > Constants.domains.yMax)
        {
            position.y = Constants.domains.yMin;
        }
    }
	//Update percption list
	void UpdatePerception(List<Fish> f) {
		perception.Clear ();
		foreach (Fish h in f) {
			if (h != this) {
				if (seeThat (h)) {
					perception.Add (h);
				}
			}
		}
	}
	//Update orientation
	void UpdateOrientation(){
		float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90;
		orientation = Quaternion.AngleAxis (angle, Vector3.forward);
	}
	//Helper function to seek that place
	public Vector3 Seek(Vector3 targetPosition){
		Vector3 desired = Vector3.Normalize(targetPosition - position) * maxSpeed;
		return desired- velocity;
	}
	//Helper function to flee
	public Vector3 Flee(Vector3 target) {
		Vector3 desired = Vector3.Normalize (position - target) * maxSpeed;
		return desired - velocity;
	}
	//add force to the object
	public void AddForce(Vector3 force) {
		acceleration = force / mass;
	}

     Vector3 computeAlignment(ref List<Fish> f) 
    {
        Vector3 v = new Vector3();
        int neighborCount = 0;
		foreach (Fish h in perception)
        {
            if (h != this)
            {
                
                    v += h.velocity;
                    neighborCount++;
            }
        }
        if (neighborCount == 0) return v;
        v /= neighborCount;
        v.Normalize();
        return v;
    }

     Vector3 computeCohesion(ref List<Fish> f)
    {
        Vector3 v = new Vector3();
        int neighborCount = 0;
		foreach (Fish h in perception)
        {
            
                    v += h.position;
                    neighborCount++;
                
            
        }
        if (neighborCount == 0) return v;
        v /= neighborCount;
        v = new Vector3(v.x - position.x, v.y - position.y);
        v.Normalize();
        return v;
    }

     Vector3 computeSeparation(ref List<Fish> f)
    {
        Vector3 v = new Vector3();
        int neighborCount = 0;
		foreach (Fish h in perception)
        {
            
                    v += h.position - position;
                    neighborCount++;
          
        }
        if (neighborCount == 0) return v;
        v /= neighborCount;
        v *= -1;
        v.Normalize();
        return v;
    }
	//Naviagation module
    void navigationModule(ref List<Fish> f)
    {
        var alignment = computeAlignment(ref f);
        var cohesion = computeCohesion(ref f);
        var separation = computeSeparation(ref f);
		velocity += alignment + cohesion + 1.2f*separation;
        velocity.Normalize();
    }
	//Enforce no overlap
	void noOverlap(ref List<Fish> f, ref List<obstacle> listObs) {
		foreach (Fish d in f) {
			if (d != this) {
				Vector3 ve = (this.position - d.position);
				float distance = ve.magnitude;
				if (distance == 0)
					distance = 0.01f;
				float overlap = Constants.Radius * 2 - distance;
				if (overlap >= 0) {
					position += ve / distance * overlap;
				}
			}
		}
		foreach (obstacle o in listObs) {
			Vector3 ve = (this.position - o.position);
			float distance = ve.magnitude;
			if (distance == 0)
				distance = 0.01f;
			float overlap = Constants.Radius * 2 - distance;
			if (overlap >= 0) {
				position += ve / distance * overlap;
			}
		}
	}

	void convert(Fish g){
		SpriteRenderer s = go.GetComponent<SpriteRenderer> ();
		SpriteRenderer d = g.go.GetComponent<SpriteRenderer> ();
		r.i = g.r.i;
		maxSpeed = Constants.maxspeed [r.i];
		fov = Constants.fov [r.i];
		s.sprite = d.sprite;
	}
}
