using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    PlayerController controller;
    [SerializeField] private Transform RespawnPoint;
    bool hitDeadZone;

    private void Start()
    {
        controller = Player.GetComponent<PlayerController>();

    }

    private void Update()
    {
        if(!controller.isFrozen && hitDeadZone)
        {
            RespawnPlayer();
        }
    }

    public void RespawnPlayer()
    {
        controller.transform.position = RespawnPoint.position; 
        controller.currentFreezeMeter = 0f;
        hitDeadZone = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            hitDeadZone = true;
            controller.Freeze();
        }
    }
}
