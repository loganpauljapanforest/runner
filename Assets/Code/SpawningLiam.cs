//------------------------------------------------------------------------------
//
// File Name:	SpawningLiam.cs
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

public class SpawningLiam : MonoBehaviour
{
    //public variables
    public GameObject[] Obstacles;
    public Vector2 SpawnInterval = new Vector2(0, 1);
    public Vector3 SpawnPosition = new Vector3();

    //private variables
    private float timeTilNextSpawn = 0.0f;
    private int selected = 0;
    private Vector3 rotation = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeTilNextSpawn -= Time.deltaTime;

        if (timeTilNextSpawn <= 0.0f)
        {
            timeTilNextSpawn += Random.Range(SpawnInterval.x, SpawnInterval.y);
            SpawnRandomObject();
        }
    }

    void SpawnRandomObject()
    {
        selected = Random.Range(0, Obstacles.Length);
        Instantiate(Obstacles[selected], SpawnPosition, Quaternion.Euler(rotation));
    }
}
