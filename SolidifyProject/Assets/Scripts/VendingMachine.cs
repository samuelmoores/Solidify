using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    PlayerController controller;
    public GameObject Player;
    public GameObject interactMessage;
    public GameObject HandWarmer;
    public Transform dispenseLocation;
    GameManager gameManager;
    bool canInteract = false;
    bool dispensed = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = Player.GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canInteract)
        {
            if (controller.interacting && !dispensed && gameManager.numOfCoins >= 3)
            {
                gameManager.numOfCoins -= 3;
                DispenseHandWarmer();
            }

            if(!controller.interacting)
            {
                dispensed = false;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            interactMessage.SetActive(true);
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactMessage.SetActive(false);
            canInteract = false;
        }
    }

    void DispenseHandWarmer()
    {
        GameObject HandWarmerRef = Instantiate(HandWarmer, dispenseLocation.position, Quaternion.identity);
        Rigidbody rb = HandWarmerRef.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0, 0, -8), ForceMode.Impulse);
        dispensed = true;
    }

}
