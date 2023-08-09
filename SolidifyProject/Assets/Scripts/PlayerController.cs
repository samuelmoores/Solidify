using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 movement;
    public Camera MainCamera;
    public Camera AimCamera;
    public Animator animator;
    public IceGun iceGun;
    public float movementSpeed = 6f;
    public float rotateSpeed = 600f;
    float horizontalInput;
    float verticalInput;
    bool aim;
    bool usingMouse = true;
    public bool hasGun = false;

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();

        if(aim && hasGun)
        {
            Aim();
        }
        else
        {
            MainCamera.enabled = true;
            AimCamera.enabled = false;
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

        GetAimInput();

    }

    void Move()
    {
        //Move the character
        if (aim && hasGun)
        {
            movementSpeed = 1f;
            Quaternion currentRotation = transform.rotation;
            Quaternion newRotation = Quaternion.Euler(MainCamera.transform.eulerAngles.x, MainCamera.transform.eulerAngles.y, currentRotation.eulerAngles.z);
            transform.rotation = newRotation;
            transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
            animator.SetBool("isAiming", true);

        }
        else
        {
            movementSpeed = 6f;
            transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
            animator.SetBool("isAiming", false);
        }

        //Check if the character is moving
        if (movement != Vector3.zero)
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

        if(Input.GetButtonDown("Fire1"))
        {
            animator.SetBool("isShooting", true);
            iceGun.Shoot();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            animator.SetBool("isShooting", false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gun"))
            hasGun = true;
    }

}
