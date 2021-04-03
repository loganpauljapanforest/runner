using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    float timer;
    bool startTimer = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 0.2 && startTimer)
        {
            gameObject.GetComponentInParent<PlayerMovementController>().grounded = false;
            timer = 0;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            startTimer = false;
            gameObject.GetComponentInParent<PlayerMovementController>().grounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            startTimer = true;
        }
    }
}
