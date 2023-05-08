using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : Agent
{
    protected override void CalculateSteeringForces()
    {

        if (state)
        {
            Wander();

            Flock(AgentManager.Instance.butterflies);

            Flee(MousePosition);

            Separate(AgentManager.Instance.butterflies);

            StayInBounds(3f);

            AvoidAllObstacles();
        }
        else
        {
            Wander();

            Flock(AgentManager.Instance.butterflies);

            Separate(AgentManager.Instance.butterflies);

            StayInBounds(3f);

            AvoidAllObstacles();
        }
    }
}
