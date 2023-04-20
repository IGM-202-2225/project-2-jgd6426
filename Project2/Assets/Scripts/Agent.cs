using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsObject))]

public abstract class Agent : MonoBehaviour
{
    public PhysicsObject physicsObject;

    public float maxSpeed = 5f;
    public float maxForce = 5f;

    protected Vector3 totalForce = Vector3.zero;

    private float wanderAngle = 0f;

    public float maxWanderAngle = 45f;

    public float maxWanderChangePerSecond = 10f;

    [SerializeField]
    private Vector2 boxSize;

    private Bounds bounds;

    public float personalSpace = 1f;

    public float visionRange = 2f;

    private void Awake()
    {
        if (physicsObject == null)
        {
            physicsObject = GetComponent<PhysicsObject>();
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        bounds = new Bounds(transform.position, boxSize);

        CalculateSteeringForces();

        totalForce = Vector3.ClampMagnitude(totalForce, maxForce);
        physicsObject.ApplyForce(totalForce);

        totalForce = Vector3.zero;
    }

    protected abstract void CalculateSteeringForces();

    protected Vector3 Seek(Vector3 targetPos, float weight = 1f)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = targetPos - physicsObject.Position;

        // Set desired velocity magnitude to max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate the seek steering force
        Vector3 seekingForce = desiredVelocity - physicsObject.Velocity;

        // Apply the seek steering force
        totalForce += seekingForce * weight;
        return totalForce;
    }

    protected Vector3 Flee(Vector3 targetPos, float weight = 1f)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = physicsObject.Position - targetPos;

        // Set desired velocity magnitude to max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate the flee steering force
        Vector3 fleeingForce = desiredVelocity - physicsObject.Velocity;

        // Apply the flee steering force
        totalForce += fleeingForce * weight;
        return totalForce;
    }

    protected void Wander(float weight = 1f)
    {
        // Update the angle of our current wander
        float maxWanderChange = maxWanderChangePerSecond * Time.deltaTime;
        wanderAngle += Random.Range(-maxWanderChange, maxWanderChange);

        wanderAngle = Mathf.Clamp(wanderAngle, -maxWanderAngle, maxWanderAngle);

        // Get a position that is defined by that wander angle
        Vector3 wanderTarget = Quaternion.Euler(0, 0, wanderAngle) * physicsObject.Direction.normalized + physicsObject.Position;

        // Seek towards our wander position
        Seek(wanderTarget, weight);
    }

    protected void StayInBounds(float weight = 1f)
    {
        Vector3 futurePosition = GetFuturePosition();

        if (futurePosition.x > AgentManager.Instance.maxPosition.x || 
            futurePosition.x < AgentManager.Instance.minPosition.x ||
            futurePosition.y > AgentManager.Instance.maxPosition.y || 
            futurePosition.y < AgentManager.Instance.minPosition.y)
        {
            Seek(Vector3.zero, weight);
        }
    }

    public Vector3 GetFuturePosition(float timeToLookAhead = 1f)
    {
        return physicsObject.Position + physicsObject.Velocity * timeToLookAhead;
    }

    protected void Separate<T>(List<T> agents) where T : Agent
    {
        float sqrPersonalSpace = Mathf.Pow(personalSpace, 2);

        // loop through all the other agents
        foreach (T other in agents)
        {
            // Find the square distance between the two agents
            float sqrDist = Vector3.SqrMagnitude(other.physicsObject.Position - physicsObject.Position);

            if (sqrDist < float.Epsilon)
            {
                continue;
            }

            if (sqrDist < sqrPersonalSpace)
            {
                float weight = sqrPersonalSpace / (sqrDist + 0.1f);
                Flee(other.physicsObject.Position, weight);
            }
        }
    }

    protected void AvoidObstacle(Obstacle obstacle)
    {
        // Get a vector from this agent, to the obstacle
        Vector3 toObstacle = obstacle.Position - physicsObject.Position;

        // Check if the obstacle is behind this agent
        float fwdToObstacleDot = Vector3.Dot(physicsObject.Direction, toObstacle);

        if (fwdToObstacleDot < 0)
        {
            return;
        }

        // Check if the obstacle is too far to the left or right
        float rightToObstacleDot = Vector3.Dot(physicsObject.Right, toObstacle);

        if (Mathf.Abs(rightToObstacleDot) > physicsObject.radius + obstacle.radius)
        {
            return;
        }

        // Check if the obstacle is within our vision range4
        if (fwdToObstacleDot > visionRange)
        {
            return;
        }

        // We've passed all the checks, avoid the obstacle
        Vector3 desiredVelocity;

        if (rightToObstacleDot > 0)
        {
            // If the obstacle is on the right, steer left
            desiredVelocity = physicsObject.Right * -maxSpeed;
        }
        else
        {
            // If the obstacle is on the left, steer right
            desiredVelocity = physicsObject.Right * maxSpeed;
        }

        // Create a weight based on how close we are to the obstacle
        float weight = visionRange / (fwdToObstacleDot + 0.1f);

        // Calculate the steering force from the desired velocity
        Vector3 steeringForce = (desiredVelocity - physicsObject.Velocity) * weight;

        // Apply the steering force to the total force
        totalForce += steeringForce;
    }

    protected void AvoidAllObstacles()
    {
        foreach (Obstacle obstacle in ObstacleManager.Instance.obstacles)
        {
            AvoidObstacle(obstacle);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, physicsObject.radius);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, personalSpace);
    }
}
