using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGun : MonoBehaviour
{
    public Transform gunContainer;
    public GameObject IceCubeRef;
    public GameObject StoneRef;
    public AudioSource shootingSound;
    public Transform IceCubeShootPosition;
    public Transform StoneShootPosition;


    GameObject IceCube;
    GameObject Stone;

    Vector3 shotDirection;
    public float gunStrength;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        shootingSound.Play();
        IceCube = Instantiate(IceCubeRef, IceCubeShootPosition.position, Quaternion.Euler(0, 0, -150));
        Destroy(IceCube, 7f);
        shotDirection = new Vector3(0f, -IceCubeShootPosition.position.y, 0f);
        if (IceCube != null)
        {
            IceCube.GetComponent<Rigidbody>().AddForce(IceCubeShootPosition.transform.forward * gunStrength, ForceMode.Impulse);
        }

    }

    public void ShootStone()
    {
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
        Pickup(other);
    }

    void Pickup(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.SetParent(gunContainer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;
        }
    }
}
