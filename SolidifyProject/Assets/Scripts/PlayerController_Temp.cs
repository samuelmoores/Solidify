using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController_Temp : MonoBehaviour
{
    Vector3 movement;
    public Transform Camera;
    public Animator animator;
    public float movementSpeed;
    public float rotateSpeed;
    float horizontalInput;
    float verticalInput;

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        movement = Quaternion.AngleAxis(Camera.eulerAngles.y, Vector3.up) * movement;

        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
        
        if(movement != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotateSpeed * Time.deltaTime);
        }
        else animator.SetBool("isMoving", false);

    }
}
