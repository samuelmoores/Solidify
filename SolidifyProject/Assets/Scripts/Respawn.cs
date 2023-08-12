using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    GameObject Player;
    PlayerController controller;
    [SerializeField] private Transform RespawnPoint;
    bool hitDeadZone;

    private void Start()
    {
        Player = GameObject.Find("Sal");
        controller = Player.GetComponent<PlayerController>();
        
    }

    private void Update()
    {
        if(!controller.isFrozen && hitDeadZone)
        {
            Debug.Log("controller frozen + hitdeadzone");/////////////
            RespawnPlayer();
        }
    }

    public void RespawnPlayer()
    {
        Debug.Log("respawned");/////////////
        controller.transform.position = RespawnPoint.position;  //     <!>   player doesnt teleport.
        controller.currentFreezeMeter = 0f; //   <!>   players camera gets locked backwards when unfreezing.
        hitDeadZone = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("collided");//////////
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("collided-PLAYER");/////////////
            hitDeadZone = true;
            controller.Freeze();
        }
    }
}
