using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 movement;
    public Camera MainCamera;
    public Camera AimCamera;
    public Animator animator;
    public float movementSpeed = 6f;
    public float rotateSpeed = 600f;
    float horizontalInput;
    float verticalInput;
    bool aim;
    bool usingMouse = true;

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
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

        Aim();

    }

    void Move()
    {
        //Move the character
        if (aim)
        {
            Quaternion currentRotation = transform.rotation;
            Quaternion newRotation = Quaternion.Euler(MainCamera.transform.eulerAngles.x, MainCamera.transform.eulerAngles.y, currentRotation.eulerAngles.z);
            transform.rotation = newRotation;
            transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

        }
        else
        {
            transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
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

    void Aim()
    {
        if(aim)
        {
            AimCamera.enabled = true;
            MainCamera.enabled = false;
        }else
        {
            MainCamera.enabled = true;
            AimCamera.enabled = false;
        }
    }

    void GetAimInput()
    {
        if (usingMouse)
        {
            if (Input.GetMouseButtonDown(1))
            {
                aim = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                aim = false;
            }
        }

        if (Input.GetAxis("LeftTrigger") == 1f)
        {
            usingMouse = false;
            aim = true;
        }

        if (!usingMouse)
        {
            if (Input.GetAxis("LeftTrigger") == 0f)
            {
                aim = false;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            usingMouse = true;
            aim = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            aim = false;
        }
    }
}
