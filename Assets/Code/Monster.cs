//------------------------------------------------------------------------------
//
// File Name:	Monster.cs
// Author(s):	Liam Binford
// Project:		Endless Runner
// Course:		WANIC VGP2
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovementController>().currentHealth -= 100;
            Debug.Log(collision.GetComponent<PlayerMovementController>().currentHealth);
        }
    }
}
