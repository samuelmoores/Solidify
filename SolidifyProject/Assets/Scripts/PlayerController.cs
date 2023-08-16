using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

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
    GameManager gameManager;
    public GameObject IceCubeMesh;

    //-------------Movement---------------------
    Vector3 movement;
    Vector3 velocity;
    private Vector3 slideRateVector;
    public float slideRate;
    float horizontalInput;
    float verticalInput;
    bool jumping = false;
    public float jumpSpeed;
    bool dodging = false;
    bool usingMouse = true;
    [HideInInspector] public float movementSpeed = 6f;
    float rotateSpeed = 600f;
    float ySpeed;
    [HideInInspector] public int iceCubes = 0;
    float stepOffset;
    bool canJump = true;
    public bool onSnowmobile = false;
    public float jumpSlope;

    //-----------------Attacking----------------------------
    bool aim;
    bool shooting = false;
    [HideInInspector] public bool hasGun = false;
    public float dodgeSpeed;
    float dodgeCooldown = 0;
    bool hasFrozen = false;

    //-------------Health-------------------------------------
    [HideInInspector] public bool isFrozen = false;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public float currentFreezeMeter;
    public List<Transform> SpawnPoints;
    float SolidifiedTimer = 1;
    int spawnIndex = 0;

    //--------------Interacting-----------------------------
    [HideInInspector] public bool interacting = false;
    bool youWin = false;
    public GameObject youWinMessage;
    float youWinTimer = 4;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        stepOffset = characterController.stepOffset;
        slideRateVector.y = -1 * slideRate;

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();

        if (!pauseMenu.gameIsPaused)
        {
            if(!onSnowmobile)
            {
                if (!isFrozen)
                {
                    TakeDamage(Time.deltaTime / 75);
                }
                else
                {
                    TakeDamage(Time.deltaTime / 5);
                }
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

        if(onSnowmobile)
        {
            animator.SetBool("onSnowmobile", true);
            if(interacting)
            {
                animator.SetBool("onSnowmobile", false);
            }
        }

        if(youWin)
        {
            youWinTimer -= Time.deltaTime;

            if(youWinTimer <= 0)
            {
                SceneManager.LoadScene(0);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        //terrain sliding
        if (!canJump)
        {
            Debug.Log("RATE: " + slideRateVector.x + ", " + slideRateVector.y + ", " + slideRateVector.z);
            characterController.Move(slideRateVector * Time.deltaTime * slideRate);
        }

    }

    void GetInput()
    {
        //Gather input from input manager
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        dodgeCooldown -= Time.deltaTime;

        if (dodging)
        {
            movementSpeed = 15;
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
        if (!characterController.isGrounded)
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        //Check for jump
        if (characterController.isGrounded)
        {
            characterController.stepOffset = stepOffset;
            ySpeed = -3.5f;

            if (Input.GetButtonDown("Jump") && !isFrozen && !pauseMenu.gameIsPaused && canJump)
            {
                ySpeed = jumpSpeed;
                jumping = true;
            }else
            {
                jumping = false;
            }
        }else
        {
            characterController.stepOffset = 0f;
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

        //Interacting
        if(Input.GetButton("Interact"))
        {
            interacting = true;
        }

        if(Input.GetButtonUp("Interact"))
        {
            interacting = false;
        }

        //Warming
        if(Input.GetButtonDown("Warm"))
        {
            if(gameManager.numOfWarmers > 0)
            {
                gameManager.numOfWarmers--;
                currentFreezeMeter = 0;
            }

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

            currentFreezeMeter -= damage;

            if(!hasFrozen && gameManager.numOfWarmers > 0)
            {
                gameManager.numOfWarmers--;
                hasFrozen = true;
            }
            
            if (SolidifiedTimer < 0)
            {
                //Unfreeze and respawn at closest spawn point
                Unfreeze();

                GetComponent<CapsuleCollider>().enabled = false;
                GetComponent<CharacterController>().enabled = false;

                transform.position = SpawnPoints[spawnIndex].transform.position;

                GetComponent<CapsuleCollider>().enabled = true;
                GetComponent<CharacterController>().enabled = true;

                currentFreezeMeter = 0;
                SolidifiedTimer = 1;
                DeathCamera.SetActive(false);
                MainCamera.SetActive(true);

            }
            else
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
            iceGun.hasShot = false;

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

    void Dodge()
    {
        if (!dodging && !jumping && !aim)
        {
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
        hasFrozen = false;

    }



    private void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.CompareTag("Snowball"))
        {
            if(!isFrozen && !dodging)
            {
                Freeze();
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnZone02"))
        {
            spawnIndex = 1;
        }else if (other.CompareTag("SpawnZone03"))
        {
            spawnIndex = 2;
        }
        else if (other.CompareTag("SpawnZone04"))
        {
            spawnIndex = 3;
        }
        else if (other.CompareTag("SpawnZone05"))
        {
            spawnIndex = 4;
        }
        else if (other.CompareTag("SpawnZone06"))
        {
            spawnIndex = 5;
        }
        else if (other.CompareTag("SpawnZone07"))
        {
            spawnIndex = 6;
        }
        else if (other.CompareTag("SpawnZone08"))
        {
            spawnIndex = 7;
        }
        else if (other.CompareTag("SpawnZone09"))
        {
            spawnIndex = 8;
        }else if(other.CompareTag("YouWin"))
        {
            youWinMessage.SetActive(true);

            youWin = true;

        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y <= jumpSlope)
        {
            canJump = false;
            slideRateVector.x = hit.normal.x;
            slideRateVector.y = hit.normal.y * -1;
            slideRateVector.z = hit.normal.z;

        }
        else
        {
            canJump = true;
        }
    }

}
