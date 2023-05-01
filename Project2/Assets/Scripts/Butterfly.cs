using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : Agent
{
    protected override void CalculateSteeringForces()
    {
        Wander();

        Flock(AgentManager.Instance.butterflies);

        StayInBounds(3f);

        AvoidAllObstacles();
    }
}
