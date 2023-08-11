using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private Transform RespawnPoint;
    bool playerDied = false;
    float respawnTimer = 3f;

    private void Update()
    {
        if(playerDied)
        {
            respawnTimer -= Time.deltaTime;
            if(respawnTimer < 0)
                RespawnPlayer();

        }
    }

    private void RespawnPlayer()
    {
        Player.transform.position = RespawnPoint.transform.position;
        playerDied = false;
        respawnTimer = 3;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerDied = true;

        }
    }
}
