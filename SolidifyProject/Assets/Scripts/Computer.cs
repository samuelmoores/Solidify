using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    public GameObject message;
    // Start is called before the first frame update
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            message.SetActive(true);
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
