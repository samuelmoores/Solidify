using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource BGSound;
    public AudioSource BGMusic;

    public int numOfCoins;
    public int numOfEngineParts;
    public int numOfCrystals;
    public int numOfStones;

    // Start is called before the first frame update
    void Start()
    {
        BGSound.Play();
        BGMusic.Play();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameObject.Find("AimCamera").GetComponent<Camera>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
