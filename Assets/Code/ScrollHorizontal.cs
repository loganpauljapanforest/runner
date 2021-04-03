//------------------------------------------------------------------------------
//
// File Name:	ScrollHorizontal.cs
// Author(s):	Jeremy Kings (j.kings) - Unity Project
//              Nathan Mueller - original Zero Engine project
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//Modified by Liam Binford
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollHorizontal : MonoBehaviour
{
    public bool FlipDirection = false;
    public float MoveSpeed = 10.0f;
    public float WrapZoneLeft = -18.0f;
    public float WrapZoneRight = 56.0f;
    public bool cluster = false;

    // Update is called once per frame
    void Update()
    {
        // Store current position
        Vector3 position = transform.position;

        // Left --> Right, Reset
        if(FlipDirection)
        {
            if (transform.position.x >= WrapZoneRight && !cluster)
            {
                position.x = WrapZoneLeft - transform.position.x - WrapZoneRight;
            }
        }
        // Left <-- Right, Reset
        else
        {
            if (transform.position.x <= WrapZoneLeft && !cluster)
            {
                position.x = WrapZoneRight + transform.position.x - WrapZoneLeft;
            }

            if(cluster && transform.position.x <= WrapZoneLeft)
            {
                Destroy(gameObject);
            }
        }


        // Move
        if(FlipDirection)
        {
            position.x += MoveSpeed * Time.deltaTime;
        }
        else
        {
            position.x -= MoveSpeed * Time.deltaTime;
        }

        // Set new position
        transform.position = position;
    }
}
