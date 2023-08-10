using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGun : MonoBehaviour
{
    public Transform gunContainer;
    public GameObject bulletRef;
    public AudioSource shootingSound;
    GameObject bullet;
    public Transform ShootPosition;
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
        bullet = Instantiate(bulletRef, ShootPosition.position, Quaternion.Euler(0, 0, -150));
        Destroy(bullet, 7f);
        shotDirection = new Vector3(0f, -ShootPosition.position.y, 0f);
        if (bullet != null)
        {
            bullet.GetComponent<Rigidbody>().AddForce(ShootPosition.transform.forward * gunStrength, ForceMode.Impulse);
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
