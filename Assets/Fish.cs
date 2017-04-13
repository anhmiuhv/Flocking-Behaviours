using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish
{
    float mass = 1;
	float maxSpeed = 3;
    float fov = Constants.fov;
    CircleCollide collide;
    EulerSolver solver;
    Vector3 position;
	Quaternion orientation;
    Vector3 acceleration;
    Vector3 velocity;
	Vector3 oldPosition;
    GameObject go;
    public Rule r;
    public Fish(GameObject g, Rule r)
    {
        position = new Vector3(Random.Range(-5 ,5), Random.Range(-5, 5), 0);
		orientation = g.transform.rotation;
		oldPosition = new Vector3 ();
        acceleration = new Vector3();
		velocity = Vector3.Normalize(new Vector3(Random.Range(-5, 5) * 0.5F, Random.Range(-5, 5) * 0.5F, 0)) * 2;
        collide = new CircleCollide(position);
        go = g;
		solver = new EulerSolver ();
        this.r = r;
    }

    public bool seeThat(Fish f)
    {
        Vector3 length = f.position - position;
        if (length.sqrMagnitude < fov * fov)
        {
            return true;
        }
        return false;
    }

    public void Update(ref List<Fish> f, float delta)
    {
        navigationModule(ref f);
		detectCollide (ref f);
		solver.Solve (ref oldPosition, ref position, ref velocity, ref acceleration, delta);
		UpdatePosition ();
		UpdateOrientation ();
		collide.position = position;
		go.transform.rotation = orientation;
		go.transform.position = position;


    }
	void detectCollide(ref List<Fish> f) {
		
		foreach (Fish d in f) {
			if (d != this) {
				if (this.collide.collided(d.collide)) {
					velocity += this.Flee (d.position);
				}
			}
		}
	}
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

	void UpdateOrientation(){
		float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90;
		orientation = Quaternion.AngleAxis (angle, Vector3.forward);
	}

	Vector3 Seek(Vector3 targetPosition){
		Vector3 desired = Vector3.Normalize(targetPosition - position) * maxSpeed;
		return desired- velocity;
	}

	Vector3 Flee(Vector3 target) {
		Vector3 desired = Vector3.Normalize (position - target) * maxSpeed;
		return desired - velocity;
	}

	public void AddForce(Vector3 force) {
		acceleration = force / mass;
	}

     Vector3 computeAlignment(ref List<Fish> f) 
    {
        Vector3 v = new Vector3();
        int neighborCount = 0;
        foreach (Fish h in f)
        {
            if (h != this)
            {
                if (seeThat(h))
                {
                    v += h.velocity;
                    neighborCount++;
                }
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
        foreach (Fish h in f)
        {
            if (h != this)
            {
                if (seeThat(h))
                {
                    v += h.position;
                    neighborCount++;
                }
            }
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
        foreach (Fish h in f)
        {
            if (h != this)
            {
                if (seeThat(h))
                {
                    v += h.position - position;
                    neighborCount++;
                }
            }
        }
        if (neighborCount == 0) return v;
        v /= neighborCount;
        v *= -1;
        v.Normalize();
        return v;
    }

    void navigationModule(ref List<Fish> f)
    {
        var alignment = computeAlignment(ref f);
        var cohesion = computeCohesion(ref f);
        var separation = computeSeparation(ref f);
        velocity += alignment + cohesion + separation;
        velocity.Normalize();
    }
}
