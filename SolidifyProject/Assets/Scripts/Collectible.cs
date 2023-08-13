using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Detects player collision with item, and adds corresponding item to inventory.
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (this.CompareTag("Coin"))
            {
                gameManager.numOfCoins++;
                GameObject.Destroy(gameObject);
                Debug.Log("Coins: " + gameManager.numOfCoins);/////
            }

            if (this.CompareTag("EnginePart"))
            {
                gameManager.numOfEngineParts++;
                GameObject.Destroy(gameObject);
                Debug.Log("Parts: " + gameManager.numOfEngineParts);/////
            }

            if (this.CompareTag("MagicCrystal"))
            {
                gameManager.numOfCrystals++;
                GameObject.Destroy(gameObject);
                Debug.Log("Crystals: " + gameManager.numOfCrystals);/////
            }

            if (this.CompareTag("HandWarmer"))
            {
                gameManager.numOfWarmers++;
                GameObject.Destroy(gameObject);
                Debug.Log("HandWarmer: " + gameManager.numOfWarmers); /////
            }
        }
    }
}
