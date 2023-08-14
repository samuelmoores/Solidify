using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowmobile : MonoBehaviour
{
    public GameObject Player;
    PlayerController controller;
    public GameObject message;
    public GameObject fixedMessage;
    GameManager gameManager;
    bool canRide;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        controller = Player.GetComponent<PlayerController>();

    }

    private void Update()
    {

        
        if (controller.onSnowmobile && controller.interacting)
        {
            transform.SetParent(null);
            controller.onSnowmobile = false;
        }

        if (controller.interacting && canRide && gameManager.numOfEngineParts == 4)
        {
            controller.onSnowmobile = true;
            transform.SetParent(Player.transform, false);
            transform.localPosition = Vector3.zero + new Vector3(0, 0.59f, 0);
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;
            
        }

        if(controller.onSnowmobile)
        {
            controller.movementSpeed = 30f;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canRide = true;
            if (gameManager.numOfEngineParts == 4)
            {
                fixedMessage.SetActive(true);
            }
            else
            {
                message.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canRide = false;

            message.SetActive(false);
            fixedMessage.SetActive(false);

        }
    }
}
