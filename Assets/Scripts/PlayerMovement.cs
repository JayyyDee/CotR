using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public float speed { get; private set; }
    [SerializeField] private float movementSpeed = 1500;
    [SerializeField] private float maxSpeed = 1500;
    private float moveHorizontal;
    private float moveVertical;

    private int gemCounter = 0; //Move in GemManager

    private Rigidbody2D rb;

    //Multiplayer


    void Start()
    {
       rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        //Get the value (1 or -1) for the movement
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        //For animation, the animation will start on the front.
        speed = moveVertical;

    }

    private void FixedUpdate() //FixedUpdate for physics
    {
        if (speed == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }

        Vector3 movement = new Vector3(moveHorizontal, moveVertical);
        //Apply force to the rigidbody to move it.
        rb.AddForce(movement * movementSpeed * Time.fixedDeltaTime);

        //If the horizontal velocity is more than the max speed, set velocity to the maxSpeed to not go pass it.
        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }

        //If the vertical velocity is more than the max speed, set velocity to the maxSpeed to not go pass it.
        if (rb.velocity.y > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.x);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Move in GemManager, when colliding with the gem, set inactive and plya sound.
        if (collision.CompareTag("Gemme"))
        {
            collision.gameObject.SetActive(false);
            gemCounter = 1;
        }
    }
}


//Old movement code
//transform.position += movement * Time.deltaTime * 3;
