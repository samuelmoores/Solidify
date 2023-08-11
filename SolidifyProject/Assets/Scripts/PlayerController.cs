using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 movement;
    public Camera MainCamera;
    public Camera AimCamera;
    public Camera DeathCamera;
    public Animator animator;
    public IceGun iceGun;
    public GameObject IceCubeMesh;
    public GameObject pauseMenuRef;
    PauseMenu pauseMenu;
    public float movementSpeed = 6f;
    public float rotateSpeed = 600f;
    float horizontalInput;
    float verticalInput;
    bool aim;
    bool usingMouse = true;
    public bool hasGun = false;
    bool jumping = false;
    bool shooting = false;
    bool dodging = false;
    public bool isFrozen = false;
    public bool isDead;

    private void Start()
    {
        pauseMenu = pauseMenuRef.GetComponent<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();

        if(aim && hasGun && !jumping)
        {
            Aim();
        }
        else
        {
            MainCamera.enabled = true;
            AimCamera.enabled = false;
        }

        if(isDead)
        {
            MainCamera.enabled = false;
            DeathCamera.enabled = true;
            DeathCamera.transform.parent = null;
        }
        else
        {
            MainCamera.enabled = true;
            DeathCamera.enabled = false;
            DeathCamera.transform.parent = transform;

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
        AimCamera.enabled = true;
        MainCamera.enabled = false;

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
