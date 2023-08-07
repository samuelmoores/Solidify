using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystals : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) //if anything collids with crystal. this may need to be updated to character collision later.
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>(); //player inventory script

        if (playerInventory != null)
        {
            playerInventory.CrystalsCollected(); //adds one and updates text box
            gameObject.SetActive(false); // deletes the crystal
        }
    }
}
