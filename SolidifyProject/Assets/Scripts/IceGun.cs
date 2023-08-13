using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGun : MonoBehaviour
{
    public GameObject Player;
    PlayerController controller;
    public Transform gunContainer;
    public GameObject IceCubeRef;
    public GameObject StoneRef;
    public AudioSource shootingSound;
    public Transform IceCubeShootPosition;
    public Transform StoneShootPosition;
    bool canPickup;
    public GameObject interactMessage;

    GameObject IceCube;
    GameObject Stone;

    Vector3 shotDirection;
    public float gunStrength;

    private void Start()
    {
        controller = Player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(canPickup)
        {
            if(controller.hasGun)
            {
                interactMessage.SetActive(false);
            }else
            {
                interactMessage.SetActive(true);
            }

            if(controller.interacting && !controller.hasGun)
            {
                Pickup();
            }
        }else
        {
            interactMessage.SetActive(false);

        }



    }

    public void Shoot()
    {
        shootingSound.Play();
        IceCube = Instantiate(IceCubeRef, IceCubeShootPosition.position, Quaternion.Euler(0, 0, -150));
        //Destroy(IceCube, 7f);
        shotDirection = new Vector3(0f, -IceCubeShootPosition.position.y, 0f);
        if (IceCube != null)
        {
            IceCube.GetComponent<Rigidbody>().AddForce(IceCubeShootPosition.transform.forward * gunStrength, ForceMode.Impulse);
        }

    }

    public void ShootStone()
    {
        shootingSound.Play();

        Stone = Instantiate(StoneRef, StoneShootPosition.position, Quaternion.Euler(0, 0, -150));
        Destroy(Stone, 2f);
        shotDirection = new Vector3(0f, -StoneShootPosition.position.y, 0f);
        if (Stone != null)
        {
            Stone.GetComponent<Rigidbody>().AddForce(StoneShootPosition.transform.forward * gunStrength * 3, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;

        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = false;
        }
    }

    void Pickup()
    {
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
        controller.hasGun = true;
    }
}
