using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    private Vector3 velocity;
    private Vector3 acceleration;
    private Vector3 direction;

    public float mass = 2f;

    public bool bounceOffWalls = false;
    
    public bool useFriction = false;

    public bool rotate = true;

    public float frictionCoeff = 0.2f;

    public Vector3 Velocity => velocity;
    public Vector3 Direction => direction;
    public Vector3 Position => transform.position;
    public Vector3 Right => transform.right;

    public float radius = 1f;

    // Start is called before the first frame update
    void Start()
    {
        direction = Random.insideUnitCircle.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (useFriction)
        {
            ApplyFriction(frictionCoeff);
        }

        // Calculate a new velocity based on the current acceleration of this object
        velocity += acceleration * Time.deltaTime;

        // Calculate the new position based on the velocity for this frame
        transform.position += velocity * Time.deltaTime;

        if (velocity.sqrMagnitude > Mathf.Epsilon)
        {
            // Store the direction that the object is moving in
            direction = velocity.normalized;
        }

        // Zero out the acceleration for the next frame
        acceleration = Vector3.zero;

        // look in the direction object is going
        if (rotate)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.back, direction);
        }

        if (bounceOffWalls)
        {
            BounceOffWalls();
        }
    }

    // Applies a force to this object following Newton's second law of motion
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }
    
    // Applies a friction force to this object
    private void ApplyFriction(float coeff)
    {
        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * coeff;

        ApplyForce(friction);
    }

    private void BounceOffWalls()
    {
        // If we're off screen, and we're still moving off screen, change the direction we're moving in
        if (transform.position.x > AgentManager.Instance.maxPosition.x && velocity.x > 0)
        {
            velocity.x *= -1f;
            
        }

        if (transform.position.x < -AgentManager.Instance.minPosition.x && velocity.x < 0)
        {
            velocity.x *= -1f;
        }

        if (transform.position.y > AgentManager.Instance.maxPosition.y && velocity.y > 0)
        {
            velocity.y *= -1f;
        }

        if (transform.position.y < -AgentManager.Instance.minPosition.y && velocity.y < 0)
        {
            velocity.y *= -1f;
        }
    }
}
