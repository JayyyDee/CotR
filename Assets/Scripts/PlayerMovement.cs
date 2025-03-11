using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

public class PlayerMovement : NetworkBehaviour
{
    public float speed { get; private set; }
    [SerializeField] private float movementSpeed = 1500;
    [SerializeField] private float maxSpeed = 1500;
    [SerializeField] private Transform spawnPoint;
    private float moveHorizontal;
    private float moveVertical;

    //Camera
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject playerCamera;


    private bool playingFootsteps = false; // To check Are we playing the sound of footsteps currently
    public float footstepsSpeed = 0.2f; //BASE Time between playing each Footsteps sound //How fast we walk //will have to modif to match selon le speed animation

    private List<GameObject> inventory = new List<GameObject>();

    private int gemCounter = 0;

    public HealthBarManager healthBar;
    public int maxHealth = 1000;
    public int currentHealth;

    private Rigidbody2D rb;

    public Ring ring;
    private Vector2 mousePosition; 
   
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        //Healthbar
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        gameObject.transform.position = spawnPoint.position;
      
    }

    void Update() 
    {
        //Get the value (1 or -1) for the movement
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        //StartFootsteps AKA the condition to enable or disable the sounds of walking
        if (moveVertical > 0 && !playingFootsteps || moveVertical < 0 && !playingFootsteps || moveHorizontal > 0 && !playingFootsteps || moveHorizontal < 0 && !playingFootsteps) 
        {
            StartFootsteps();
        }
        else if (moveVertical == 0 && moveHorizontal == 0) 
        {
            StopFootsteps();
        }

        //For animation, the animation will start on the front.
        speed = moveVertical;

        //Test for aiming with mouse 
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


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
            //To shoot a bullet
            if (Input.GetKeyDown(KeyCode.E))
            {
                ring.Shoot(transform);
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

        ////Testing the aiming (again)
        //Vector2 aimDirection = mousePosition - rb.position;
        //float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        //rb.rotation = aimAngle;

        PlayerMov();

    }

    public override void OnNetworkSpawn()
    {

        if (IsOwner) {

            playerCamera.gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(false);
            playerCamera.GetComponent<CameraMovement>().player = this.transform;
        }


        base.OnNetworkSpawn();
    }

    //private void OnEnable()
    //{
 
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Move in GemManager, when colliding with the gem, set inactive and plya sound.
        if (collision.CompareTag("Gemme"))
        {
            collision.gameObject.SetActive(false);
            gemCounter = 1;
            AkUnitySoundEngine.PostEvent("Event_Jadeide_Slow__Pickup", this.gameObject); // The Event to play sounds of collecting the Jadeide
        }

        if (collision.CompareTag("Ring"))
        {
            
            inventory.Add(collision.gameObject);
            ring = collision.gameObject.GetComponent<Ring>();
            collision.gameObject.SetActive(false);
            string fullInv = "";
            foreach(GameObject gameObject in inventory)
            {
                fullInv += (gameObject.name + " ");
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

    // Walking Sounds
    void StartFootsteps()  //Start walking
    {
        playingFootsteps = true;
        InvokeRepeating(nameof(PlayFootsteps), 0f, footstepsSpeed);
    }
    void StopFootsteps() //Stop walking
    {
        playingFootsteps = false;
        CancelInvoke(nameof(PlayFootsteps));
    }

    void PlayFootsteps()  //The Event to play sounds of footsteps
    {
        AkUnitySoundEngine.PostEvent("Event_Footstep", this.gameObject);
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
        mainCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
        gameObject.SetActive(false);
        gameObject.transform.position = spawnPoint.position;
    }
}


//Old movement code
//transform.position += movement * Time.deltaTime * 3;
