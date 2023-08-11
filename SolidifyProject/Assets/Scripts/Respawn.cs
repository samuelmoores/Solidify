using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    PlayerController controller;
    [SerializeField] private Transform RespawnPoint;
    float respawnTimer = 4f;
    bool playerSpawned = false;

    private void Start()
    {
        controller = Player.GetComponent<PlayerController>();
          
    }

    private void Update()
    {


        if (controller.isDead)
        {
            respawnTimer -= Time.deltaTime;
           
            if (respawnTimer < 1 && !playerSpawned)
                RespawnPlayer();

            if (respawnTimer < 0)
            {
                controller.isDead = false;
            }

        }
    }

    private void RespawnPlayer()
    {
        controller.Unfreeze();
        Player.transform.position = RespawnPoint.transform.position;
        playerSpawned = true;
        controller.currentFreezeMeter = 0f;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            controller.isDead = true;
            controller.Freeze();
        }
    }
}
