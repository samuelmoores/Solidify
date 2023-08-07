using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


// WIP // 

public class NewPlayerController : MonoBehaviour
{
    //Animator animator;
    //Camera cam;
    //CharacterController characterController;
    //float movementSpeed;
    //float rotationSpeed;
    //Vector3 camOffset;
    //Vector3 direction;
    //Quaternion rotation;
    //float horizontalMovement;
    //float forwardMovement;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    animator = GetComponent<Animator>();
    //    movementSpeed = 4.0f;
    //    rotationSpeed = 500.0f;
    //    cam = GameObject.Find("MainCamera").GetComponent<Camera>();
    //    characterController = GetComponent<CharacterController>();

    //    //set the camera behind the player
    //    //x is left/right, y is up/down, z is forward/backward
    //    camOffset = new Vector3(0.25f, 3, -7);
    //    cam.transform.position = transform.position + camOffset;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    //**locked off camera**//
    //    //camera follows the player's position
    //    cam.transform.position = transform.position + camOffset;

    //    //**dynamic camera**//
    //    //Get movement input from user and set the players direction
    //    horizontalMovement = Input.GetAxis("Horizontal");
    //    forwardMovement = Input.GetAxis("Vertical");
    //    direction = new Vector3(horizontalMovement, 0.0f, forwardMovement);

    //    //Normalizing the direction vector keeps the values at 1 so that if
    //    //the player is going sideways and forward/backward, their speed doesn't change
    //    direction = direction.normalized;

    //    //Get the rotation based on the players direction
    //    rotation = Quaternion.LookRotation(direction, Vector3.up);

    //    //Move player with CharacterController Component
    //    characterController.Move(direction * movementSpeed * Time.deltaTime);

    //    //Rotate the player from their current rotation to the updated rotation
    //    if (rotation != Quaternion.identity)
    //    {
    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    //    }

    //    //Play the animation when the player is moving
    //    if (characterController.velocity != Vector3.zero)
    //    {
    //        animator.SetBool("isMoving", true);
    //    }
    //    else
    //    {
    //        animator.SetBool("isMoving", false);
    //    }

    //    //Basic player movement with Input class
    //    /*
    //    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
    //    {
    //        transform.position += direction * movementSpeed * Time.deltaTime;
    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    //        animator.SetBool("isMoving", true);
    //    }
      
    //    if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
    //    {
    //        animator.SetBool("isMoving", false);
    //    }
    //    */
    //}
}
