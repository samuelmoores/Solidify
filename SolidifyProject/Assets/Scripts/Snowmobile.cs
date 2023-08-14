using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowmobile : MonoBehaviour
{
    public GameObject Player;
    public GameObject message;
    public GameObject winMessage;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //transform.SetParent(Player.transform, false);
            //transform.localPosition = Vector3.zero + new Vector3(0, 0.59f, 0);
            //transform.localRotation = Quaternion.Euler(Vector3.zero);
            //transform.localScale = Vector3.one;
            message.SetActive(true);
        }

        if(gameManager.numOfEngineParts == 4)
        {
            Debug.Log("You Win");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            message.SetActive(false);
        }
    }
}
