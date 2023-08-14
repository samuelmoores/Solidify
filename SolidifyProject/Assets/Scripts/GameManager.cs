using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioSource BGSound;
    public AudioSource BGMusic;
    public Text enginePartsText;
    public Text coinsText;
    public Text heatingPadsText;
    public Text crystalsText;

    public int numOfCoins;
    public int numOfEngineParts;
    public int numOfCrystals;
    public int numOfWarmers;

    // Start is called before the first frame update
    void Start()
    {
        BGSound.Play();
        BGMusic.Play();

        numOfCrystals = 4; numOfWarmers = 4;
        numOfEngineParts = 4;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        enginePartsText.text = numOfEngineParts.ToString();
        coinsText.text = numOfCoins.ToString();
        heatingPadsText.text = numOfWarmers.ToString();
        crystalsText.text = numOfCrystals.ToString();


    }
}
