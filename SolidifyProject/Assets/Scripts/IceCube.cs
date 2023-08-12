using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCube : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Environment"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Destroy(rb);
        }
    }
}
