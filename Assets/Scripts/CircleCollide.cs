/*
 * circle behaviours
 */ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollide{

    public float radius = Constants.Radius;
    public Vector3 position;
    public CircleCollide(Vector3 position)
    {
        this.position = position;
    }

    public bool collided(CircleCollide o)
    {
        Vector3 y = position - o.position;
        float length = y.sqrMagnitude;
        if (length < (radius * 2) * (radius * 2)) return true;
        return false;

    }
}
