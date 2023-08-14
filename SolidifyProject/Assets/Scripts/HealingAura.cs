using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAura : MonoBehaviour
{
    //public GameObject Player;
    public float healRate;
    public PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider collision)
    {
  
        if (controller.currentFreezeMeter > 0)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                controller.currentFreezeMeter -= healRate *Time.deltaTime;
            }
        }
    }
}
