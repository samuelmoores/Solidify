using System;
using UnityEngine;
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
    Animator animator;
    IceGun iceGun;
    public GameObject IceCubeMesh;

    //-------------Movement---------------------
    Vector3 movement;
    float horizontalInput;
    float verticalInput;
    bool jumping = false;
    bool dodging = false;
    bool usingMouse = true;
    float movementSpeed = 6f;
    float rotateSpeed = 600f;

    //-----------------Attacking----------------------------
    bool aim;
    bool shooting = false;
    [HideInInspector] public bool hasGun = false;

    //-------------Health-------------------------------------
    [HideInInspector] public bool isFrozen = false;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public float currentFreezeMeter;

    private void Start()
    {
        MainCamera = GameObject.Find("Main Camera");
        AimCamera = GameObject.Find("AimCamera");
        DeathCamera = transform.Find("DeathCam").gameObject;
        animator = GetComponent<Animator>();
        HUD = GameObject.Find("HUD");
        pauseMenu = HUD.GetComponent<PauseMenu>();
        iceGun = GameObject.Find("IceGun").GetComponent<IceGun>();
        healthBar = HUD.transform.GetChild(2).GetComponent<HealthBar>();
        healthBar.SetHealth(0);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();

        if (!pauseMenu.gameIsPaused)
            TakeDamage(Time.deltaTime / 100);

        if(aim && hasGun && !jumping)
        {
            Aim();
        }
        else
        {
            MainCamera.SetActive(true);
            AimCamera.SetActive(false);
        }

        if (isDead)
        {
            MainCamera.SetActive(false);
            DeathCamera.SetActive(true);
        }
        else
        {
            MainCamera.SetActive(true);
            DeathCamera.SetActive(false);
        }

        

    }

    void GetInput()
    {
        //Gather input from input manager
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //set the direction for the movement based on where the player is going and where the camer is looking
        movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        movement = Quaternion.AngleAxis(MainCamera.transform.eulerAngles.y, Vector3.up) * movement;

        if (Input.GetButtonUp("Jump") && !jumping && !isFrozen && !pauseMenu.gameIsPaused)
        {
            Jump();
        }

        if (!jumping && !isFrozen)
            GetAimInput();

        if(Input.GetButtonDown("Dodge") || Input.GetKeyDown(KeyCode.F))
        {
            Dodge();
        }

        if(Input.GetButtonUp("Dodge") || Input.GetKeyUp(KeyCode.F))
        {
            if(!isFrozen)
            {
                animator.SetBool("isDodging", false);

                dodging = false;
            }

        }

        if(isFrozen)
        {
            IceCubeMesh.GetComponent<MeshRenderer>().enabled = true;
        }else
        {
            IceCubeMesh.GetComponent<MeshRenderer>().enabled = false;
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
            transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
            animator.SetBool("isAiming", true);

        }
        else if(!isFrozen)
        {
            movementSpeed = 6f;
            transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
            animator.SetBool("isAiming", false);
        }

        //Check if the character is moving
        if (movement != Vector3.zero && !isFrozen)
        {
            //Start animation
            animator.SetBool("isMoving", true);

            //Only rotate the character is they are not aiming
            if(!aim)
            {
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
            }

        }
        else animator.SetBool("isMoving", false);
    }    

    void TakeDamage(float damage)
    {
        if(currentFreezeMeter < 1)
        {
            currentFreezeMeter += damage;
            healthBar.SetHealth(currentFreezeMeter);
        }
        else
        {
            isDead = true;
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
        MainCamera.SetActive(false);

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
        GetComponent<Rigidbody>().AddForce(Vector3.up * 600, ForceMode.Impulse);
        animator.SetBool("isJumping", true);
        jumping = true;
    }

    void Dodge()
    {
        if (!dodging && !jumping)
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * 900, ForceMode.Impulse);
            animator.SetBool("isDodging", true);
            dodging = true;
        }

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
        if(!isDead)
        {

        }
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
            animator.SetBool("isJumping", false);
            jumping = false;

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
