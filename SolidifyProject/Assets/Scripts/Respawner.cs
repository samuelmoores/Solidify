using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public GameObject Player;
    public GameObject SpawnPoint;
    PlayerController controller;
    CharacterController characterController;
    bool hitDeadZone = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = Player.GetComponent<PlayerController>();
        characterController = Player.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!controller.isFrozen && hitDeadZone)
        {
            Respawn();
            hitDeadZone = false;
        }
    }

    void Respawn()
    {
        Player.GetComponent<CapsuleCollider>().enabled = false;
        Player.GetComponent<CharacterController>().enabled = false;

        Player.transform.position = SpawnPoint.transform.position;
        Player.GetComponent<CapsuleCollider>().enabled = true;
        Player.GetComponent<CharacterController>().enabled = true;


    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            controller.Freeze();
            controller.currentFreezeMeter = 1;
            hitDeadZone = true;

        }
    }
}

