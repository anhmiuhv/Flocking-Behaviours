using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EulerSolver {

    public EulerSolver()
    {

    }
    public void Solve(ref Vector3 oldPosition, ref Vector3 position, ref Vector3 velocity, ref Vector3 acceleration, float delta)
    {
		oldPosition = position;
		int steps = 8;
        delta = delta / 8;
        for (int i = 0; i < steps; i++)
        {
            velocity = velocity + acceleration * delta;
            position = position + velocity * delta;
        }
    }
}
