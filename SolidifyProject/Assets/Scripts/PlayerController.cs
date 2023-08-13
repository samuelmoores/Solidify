using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{
    //----------------Cameras-----------------------
    GameObject MainCamera;
    GameObject AimCamera;
    GameObject DeathCamera;

    //------------HUD-----------------------------
    GameObject HUD;
    PauseMenu pauseMenu;
    HealthBar healthBar;

    //----------------Components--------------------
    CharacterController characterController;
    Animator animator;
    IceGun iceGun;
    public GameObject IceCubeMesh;

    //-------------Movement---------------------
    Vector3 movement;
    Vector3 velocity;
    float horizontalInput;
    float verticalInput;
    bool jumping = false;
    public float jumpSpeed;
    bool dodging = false;
    bool usingMouse = true;
    float movementSpeed = 6f;
    float rotateSpeed = 600f;
    float ySpeed;

    //-----------------Attacking----------------------------
    bool aim;
    bool shooting = false;
    [HideInInspector] public bool hasGun = false;
    public float dodgeSpeed;
    float dodgeCooldown = 0;

    //-------------Health-------------------------------------
    [HideInInspector] public bool isFrozen = false;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public float currentFreezeMeter;
    public List<Transform> SpawnPoints;
    float SolidifiedTimer = 5;

    //--------------Interacting-----------------------------
    [HideInInspector] public bool interacting = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        MainCamera = GameObject.Find("Main Camera");
        AimCamera = GameObject.Find("AimCamera");
        DeathCamera = transform.Find("DeathCam").gameObject;
        animator = GetComponent<Animator>();
        HUD = GameObject.Find("HUD");
        pauseMenu = HUD.GetComponent<PauseMenu>();
        iceGun = GameObject.Find("IceGun").GetComponent<IceGun>();
        healthBar = HUD.transform.GetChild(2).GetComponent<HealthBar>();
        healthBar.SetHealth(0);
        MainCamera.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();

        if (!pauseMenu.gameIsPaused)
        {
            if(!isFrozen)
            {
                TakeDamage(Time.deltaTime / 100);
            }else
            {
                TakeDamage(Time.deltaTime / 5);
            }
        }

        if(aim && hasGun)
        {
            Aim();
        }
        else
        {
            MainCamera.SetActive(true);
            AimCamera.SetActive(false);
        }

    }

    void GetInput()
    {
        //Gather input from input manager
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        Debug.Log(dodgeCooldown);

        dodgeCooldown -= Time.deltaTime;

        if (dodging)
        {
            movementSpeed = 25;
        }
        
        //Check for dodge before calculating movement
        //so the dodge can use the players movement direction
        if ((Input.GetButton("Dodge") || Input.GetKey(KeyCode.F)) && dodgeCooldown < 0)
        {
            dodgeCooldown = 0.95f;
            Dodge();
        }
        
        //set the direction for the movement based on where the player is going and where the camer is looking
        movement = new Vector3(horizontalInput, 0f, verticalInput);
        float magnitude = Mathf.Clamp01(movement.magnitude) * movementSpeed;
        movement.Normalize();
        movement = Quaternion.AngleAxis(MainCamera.transform.eulerAngles.y, Vector3.up) * movement;

        //Apply gravity
        ySpeed += Physics.gravity.y * Time.deltaTime;

        //Check for jump
        if (characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump") && !isFrozen && !pauseMenu.gameIsPaused)
            {
                ySpeed = jumpSpeed;
            }
        }
        
        //Play jump animation
        animator.SetBool("isJumping", !characterController.isGrounded);
        
        //Set value to move the character upwards
        velocity = movement * magnitude;
        velocity.y = ySpeed;

        if (!jumping && !isFrozen)
            GetAimInput();

        //Show the ice cube that the player has solidified in
        if(isFrozen)
        {
            IceCubeMesh.GetComponent<MeshRenderer>().enabled = true;
        }else
        {
            IceCubeMesh.GetComponent<MeshRenderer>().enabled = false;
        }


        if(Input.GetButton("Interact"))
        {
            interacting = true;
        }

        if(Input.GetButtonUp("Interact"))
        {
            interacting = false;
        }
        

    }

    void Move()
    {
        //Move the character
        if (aim && hasGun && !isFrozen)
        {
            movementSpeed = 1f;
            Quaternion currentRotation = transform.rotation;
            Quaternion newRotation = Quaternion.Euler(MainCamera.transform.eulerAngles.x, MainCamera.transform.eulerAngles.y, currentRotation.eulerAngles.z);
            transform.rotation = newRotation;
            characterController.Move(velocity * Time.deltaTime);
            animator.SetBool("isAiming", true);

        }
        else if(!isFrozen)
        {
            //Take away the slow aim movement
            movementSpeed = 6f;
            //Move the character
            characterController.Move(velocity * Time.deltaTime);
            animator.SetBool("isAiming", false);
        }

        //Check if the character is moving
        if (movement != Vector3.zero && !isFrozen)
        {
            //Start animation
            
            if (characterController.isGrounded)
            {
                animator.SetBool("isMoving", true);
            }

            //Only rotate the character is they are not aiming
            if (!aim)
            {
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
            }

        }
        else animator.SetBool("isMoving", false);
    }    

    void TakeDamage(float damage)
    {
        if(currentFreezeMeter <= 1)
        {
            currentFreezeMeter += damage;
            healthBar.SetHealth(currentFreezeMeter);
        }
        else if (isFrozen)
        {
            //Solidified Timer
            SolidifiedTimer -= Time.deltaTime;

            if(SolidifiedTimer < 0)
            {
                //Unfreeze and respawn at closest spawn point
                Unfreeze();
                currentFreezeMeter = 0;
                SolidifiedTimer = 5;
                DeathCamera.SetActive(false);
            }else
            {
                DeathCamera.SetActive(true);
            }

        }
        else
        {
            Freeze();
        }
    }

    void GetAimInput()
    {
        if (usingMouse)
        {
            if (Input.GetMouseButtonDown(1) && hasGun)
            {
                aim = true;
            }
            if (Input.GetMouseButtonUp(1) && hasGun)
            {
                aim = false;
                Quaternion clearAimRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                transform.rotation = clearAimRotation;
            }
        }

        if (Input.GetAxis("LeftTrigger") == 1f && hasGun)
        {
            usingMouse = false;
            aim = true;

        }

        if (!usingMouse)
        {
            if (Input.GetAxis("LeftTrigger") == 0f && hasGun)
            {
                aim = false;
                Quaternion clearAimRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                transform.rotation = clearAimRotation;
            }
        }

        if (Input.GetMouseButtonDown(1) && hasGun)
        {
            usingMouse = true;
            aim = true;
            Quaternion clearAimRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            transform.rotation = clearAimRotation;
        }
        if (Input.GetMouseButtonUp(1) && hasGun)
        {
            aim = false;
            Quaternion clearAimRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            transform.rotation = clearAimRotation;
        }
    }

    void Aim()
    {

        AimCamera.SetActive(true);

        if (Input.GetAxis("RightTrigger") == 1f || Input.GetMouseButtonDown(0))
        {
            animator.SetBool("isShooting", true);
            if(!shooting)
            {
                iceGun.Shoot();
                shooting = true;
            }
        }

        if(Input.GetAxis("RightTrigger") == 0f || Input.GetMouseButtonUp(0))
        {
            animator.SetBool("isShooting", false);
            shooting = false;

        }

        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetBool("isShooting", true);
            iceGun.ShootStone();
        }
        if (Input.GetButtonUp("Fire2"))
        {
            animator.SetBool("isShooting", false);
        }

    }

    void Jump()
    {
        //GetComponent<Rigidbody>().AddForce(Vector3.up * 600, ForceMode.Impulse);
        
        //animator.SetBool("isJumping", true);
    }

    void Dodge()
    {
        if (!dodging && !jumping)
        {
            Debug.Log("dodging");
            animator.SetBool("isDodging", true);
            dodging = true;
        }

    }

    void EndDodge()
    {
        animator.SetBool("isDodging", false);
        dodging = false;
    }

    public void Freeze()
    {
        animator.SetBool("isHit", true);
        isFrozen = true;
        aim = false;
    }

    public void Unfreeze()
    {
        animator.SetBool("isHit", false);
        isFrozen = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gun"))
            hasGun = true;

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Environment"))
        {
            //animator.SetBool("isJumping", false);
            //jumping = false;

        }

        if(collision.gameObject.CompareTag("Snowball"))
        {
            if(!isFrozen && !dodging)
            {
                Freeze();
            }
        }

    }

}
