using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCursor : Agent
{
    protected override void CalculateSteeringForces()
    {
        if (state)
        {
            Cursor.visible = true;
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            Cursor.visible = false;
            GetComponent<SpriteRenderer>().enabled = true;
            transform.position = MousePosition;
        }
    }
}
