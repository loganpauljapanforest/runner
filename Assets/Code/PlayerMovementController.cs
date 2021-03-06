//------------------------------------------------------------------------------
//
// File Name:	PlayerMovementController.cs
// Author(s):	Jeremy Kings (j.kings) - Unity Project
//              Nathan Mueller - original Zero Engine project
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//Modified by Liam Binford and Charles Sweatt
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float MoveSpeed = 10;
    public int MaxHealth = 3;
    public float JumpHeight = 5;
    public int MaxNumberOfJumps = 2;
    public float deceleration = 4;
    public int dashDistance = 5;
    public float dashSpeed = 15;
    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode SlideKey = KeyCode.LeftShift;
    public KeyCode DashKey = KeyCode.D;
    public KeyCode SlamKey = KeyCode.S;
    public int currentHealth = 0;

    private int jumpsRemaining = 0;
    private string nameOfHealthDisplayObject = "HealthBar";
    private string nameOfDistanceLabelObject = "DistanceLabel";
    private GameObject healthBarObj = null;
    private GameObject distanceObj = null;
    private float startingX = 0;
    private PlayerAnimationManager animationManager;
    private bool dashing = false;
    private Rigidbody2D myrb;
    public bool grounded = true;
    private GameObject below;

    public AudioClip JumpSound;
    public AudioClip DashSound;
    public AudioClip SlamSound;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        healthBarObj = GameObject.Find(nameOfHealthDisplayObject);
        distanceObj = GameObject.Find(nameOfDistanceLabelObject);
        animationManager = GetComponent<PlayerAnimationManager>();
        if (healthBarObj != null)
        {
          healthBarObj.GetComponent<FeedbackBar>().SetMax(MaxHealth);
        }

        // Take the square root of the jump height so that the math for gravity works
        // to make the number the user enters the number of units the player will
        // actually be able to jump
        JumpHeight = Mathf.Sqrt(2.0f * Physics2D.gravity.magnitude * JumpHeight);
        
        currentHealth = MaxHealth;
        startingX = transform.position.x;

        // Reset score
        PlayerSaveData.DistanceRun = 0;

        myrb = gameObject.GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        /*below = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 1.5f), 1).gameObject;

        if(below != null && below.CompareTag("Floor"))
        {
            grounded = true;
            Debug.Log("84");
        }
        else
        {
            Debug.Log("88");
            grounded = false;
        }*/


        if(transform.position.y < -0.5f)
        {
            transform.position = new Vector3(transform.position.x, -0.5f, transform.position.z);
        }

        // Jumping
        if (Input.GetKeyDown(JumpKey))
        {
            if (jumpsRemaining > 0)
            {
                animationManager.SwitchTo(PlayerAnimationStates.Jump);
                var jump_vec = new Vector3(0,JumpHeight,0);
                gameObject.GetComponent<Rigidbody2D>().velocity = jump_vec;
                jumpsRemaining -= 1;

                audioSource.PlayOneShot(JumpSound, 10.0F);
            }
        }
        // Sliding
       /* else if (Input.GetKey(SlideKey) && grounded)
        {
            animationManager.SwitchTo(PlayerAnimationStates.Slide);
        }*/
        // Dashing
        else if(Input.GetKeyDown(DashKey) && !grounded && !dashing)
        {
            dashing = true;

            audioSource.PlayOneShot(DashSound, 10.0F);
        }
        // Slamming
        else if(Input.GetKeyDown(SlamKey) && !grounded)
        {
            myrb.gravityScale = 20;

            animationManager.SwitchTo(PlayerAnimationStates.Slam);

            audioSource.PlayOneShot(SlamSound, 8.0F);
        }
        // Running
        else if (!Input.GetKey(SlideKey) && grounded)
        {
            animationManager.SwitchTo(PlayerAnimationStates.Run);
        }
        // Falling
        else
        {
            animationManager.SwitchTo(PlayerAnimationStates.Jump);
        }

        if(dashing)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(startingX + dashDistance + startingX, transform.position.y, transform.position.z), dashSpeed);
            if(transform.position.x >= dashDistance)
            {
                dashing = false;
            }
        }
        if(grounded)
        {
            myrb.gravityScale = 3;
        }

        // Lock the player to X = StartingX;
        //gameObject.transform.position = new Vector3(startingX, transform.position.y, transform.position.z);
        if (transform.position.x > startingX && !dashing)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(startingX, transform.position.y, transform.position.z), deceleration);
        }

        // Update the Distance travelled
        PlayerSaveData.DistanceRun += MoveSpeed * Time.deltaTime;
        if (distanceObj != null)
        {
            if (distanceObj.GetComponent<TextMeshProUGUI>() != null)
            {
                string distText = string.Format("{0,4:F1}", PlayerSaveData.DistanceRun);
                distanceObj.GetComponent<TextMeshProUGUI>().text = "Distance: " 
                    + distText + " m";
            }
        }

        // Game Over
        if (currentHealth <= 0)
        {
            // Load score level
            UnityEngine.SceneManagement.SceneManager.LoadScene("ScoreScreen");
        }
    }

  
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Hit an Obstacle
        if (collision.collider.gameObject.CompareTag("Obstacle"))
        {
            Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();

            if (obstacle != null)
            {
                currentHealth -= obstacle.Damage;
               
                if (obstacle.DestroyOnPlayerCollision)
                {
                    Destroy(collision.collider.gameObject);
                }
                if (healthBarObj != null)
                {
                    healthBarObj.GetComponent<FeedbackBar>().SetValue(currentHealth);
                    //animationManager.SwitchTo(PlayerAnimationStates.Hurt);
                }
            }
        }
        // Hit the floor
        if (collision.collider.gameObject.CompareTag("Floor"))
        {
            jumpsRemaining = MaxNumberOfJumps;
        }
        
    }
   
}