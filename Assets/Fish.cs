using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish
{
    float mass = 1;
    float fov = Constants.fov;
    CircleCollide collide;
    EulerSolver solver;
    Vector3 position;
    Vector3 orientation;
    Vector3 acceleration;
    Vector3 velocity;
	Vector3 oldPosition;
    GameObject go;
    public Rule r;
    public Fish(GameObject g, Rule r)
    {
        position = new Vector3(Random.Range(-5 ,5), Random.Range(-5, 5), 0);
        orientation = new Vector3();
		oldPosition = new Vector3 ();
        acceleration = new Vector3();
        velocity = new Vector3(Random.Range(-5, 5) * 0.5F, Random.Range(-5, 5) * 0.5F, 0);
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
		
		solver.Solve (ref oldPosition, ref position, ref velocity, ref acceleration, delta);
		UpdatePosition ();
		float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - 90;
		go.transform.rotation = Quaternion.Slerp(go.transform.rotation ,Quaternion.AngleAxis(angle, Vector3.forward),1);

		go.transform.position = position;

    }
    public void UpdatePosition()
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
}