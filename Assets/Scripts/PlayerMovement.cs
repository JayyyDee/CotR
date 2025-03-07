using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public float speed { get; private set; }
    [SerializeField] private float movementSpeed = 1500;
    [SerializeField] private float maxSpeed = 1500;
    [SerializeField] private Transform spawnPoint;
    private float moveHorizontal;
    private float moveVertical;

    private List<GameObject> inventory = new List<GameObject>();

    private int gemCounter = 0;

    public HealthBarManager healthBar;
    public int maxHealth = 1000;
    public int currentHealth;

    private Rigidbody2D rb;

   
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        //Healthbar
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        gameObject.transform.position = spawnPoint.position;
      
    }

    void Update() {
        //Get the value (1 or -1) for the movement
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        //For animation, the animation will start on the front.
        speed = moveVertical;

        if (IsOwner)
        {
            //To test for damage, press O
            if (Input.GetKeyDown(KeyCode.O))
            {
                TakeDamage(100);
            }

            //To test for healing, press P
            if (Input.GetKeyDown(KeyCode.P))
            {
                TakeHealing(100);
            }
        }

    }

    private void FixedUpdate() //FixedUpdate for physics
    {
        //If the player is not the owner of the playable, can't access the movement.
        if (!IsOwner)
        {
            return;
        }

        PlayerMov();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Move in GemManager, when colliding with the gem, set inactive and plya sound.
        if (collision.CompareTag("Gemme"))
        {
            collision.gameObject.SetActive(false);
            gemCounter = 1;
        }

        if (collision.CompareTag("Ring"))
        {
            inventory.Add(collision.gameObject);
            collision.gameObject.SetActive(false);
            string fullInv = "";
            foreach(GameObject gameObject in inventory)
            {
                fullInv += (gameObject.name+" ");
                
            }
            Debug.Log(fullInv);


        }
    }
    private void PlayerMov()
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

    void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            Invoke("Death", 2);

        }
    }
    void TakeHealing(int heal)
    {
        if ( currentHealth < maxHealth)
        {
            currentHealth += heal;
            healthBar.SetHealth(currentHealth);
        }

    }

    void Death ()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = spawnPoint.position;
    }
}


//Old movement code
//transform.position += movement * Time.deltaTime * 3;
