using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController_Temp : MonoBehaviour
{
    Animator animator;
    Camera cam;
    float movementSpeed;
    Vector3 camOffset;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movementSpeed = 4.0f;
        cam = GameObject.Find("MainCamera").GetComponent<Camera>();

        //set the camera behind the player
        camOffset = new Vector3(1, 2, -3);
        cam.transform.position = transform.position + camOffset;
    }

    // Update is called once per frame
    void Update()
    {
        //camera follows the player's position
        cam.transform.position = transform.position + camOffset;

        //Player moves forward in world space when W is pressed or stops when W is let go
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * movementSpeed * Time.deltaTime;
            animator.SetBool("isMoving", true);
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("isMoving", false);
        }
    }
}
