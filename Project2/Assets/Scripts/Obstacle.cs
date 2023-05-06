using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Agent
{
    public float radius = 1f;

    public Vector3 Position => transform.position;

    protected override void CalculateSteeringForces()
    {
        Wander();

        StayInBounds(3f);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Position, radius);
    }
}
