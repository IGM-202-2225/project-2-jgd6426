using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagPlayer : Agent
{
    public enum TagState
    {
        It,
        NotIt,
        Counting
    }

    private TagState currentState = TagState.NotIt;

    public TagState CurrentState => currentState;

    private float countdownTimer = 0f;

    public float visionDistance = 4f;

    public SpriteRenderer spriteRenderer;

    public Sprite itSprite;
    public Sprite notItSprite;
    public Sprite countingSprite;

    protected override void CalculateSteeringForces()
    {
        switch (currentState)
        {
            case TagState.It:
                {
                    // Chase the closest not-it agent
                    TagPlayer targetPlayer = AgentManager.Instance.GetClosestTagPlayer(this);

                    if (IsTouching(targetPlayer))
                    {
                        // Tag the other target
                        targetPlayer.Tag();

                        // Become not-it
                        StateTransition(TagState.NotIt);
                    }
                    else
                    {
                        Seek(targetPlayer.transform.position);
                    }

                    break;
                }
            case TagState.Counting:
                {
                    // Count down to 0, then become it
                    countdownTimer -= Time.deltaTime;

                    if (countdownTimer <= 0f)
                    {
                        StateTransition(TagState.It);
                    }

                    break;
                }
            case TagState.NotIt:
                {
                    // Wander around, unless we see the "It" player, then run away
                    TagPlayer currentIt = AgentManager.Instance.currentItPlayer;

                    float distToItPlayer = Vector3.SqrMagnitude(physicsObject.Position - currentIt.physicsObject.Position);

                    if (distToItPlayer < Mathf.Pow(visionDistance, 2))
                    {
                        Flee(currentIt.physicsObject.Position);
                    }
                    else
                    {
                        Wander();
                    }

                    Separate(AgentManager.Instance.tagPlayers);

                    break;
                }
        }

        StayInBounds(4f);
    }

    private void StateTransition(TagState newTagState)
    {
        currentState = newTagState;

        switch (newTagState)
        {
            case TagState.It:
                {
                    // Do logic for becoming it
                    spriteRenderer.sprite = itSprite;

                    physicsObject.useFriction = false;

                    break;
                }
            case TagState.Counting:
                {
                    // Do logic for becoming a counter
                    countdownTimer = AgentManager.Instance.countdownTime;
                    AgentManager.Instance.currentItPlayer = this;
                    spriteRenderer.sprite = countingSprite;

                    physicsObject.useFriction = true;

                    break;
                }
            case TagState.NotIt:
                {
                    // Transition from being it, to not it
                    spriteRenderer.sprite = notItSprite;

                    physicsObject.useFriction = false;

                    break;
                }
        }
    }

    public void Tag()
    {
        StateTransition(TagState.Counting);
    }

    private bool IsTouching(TagPlayer otherPlayer)
    {
        float sqrDistance = Vector3.SqrMagnitude(physicsObject.Position - otherPlayer.physicsObject.Position);

        float sqrRadii = Mathf.Pow(physicsObject.radius, 2) + Mathf.Pow(otherPlayer.physicsObject.radius, 2);

        return sqrDistance < sqrRadii;
    }
}
