using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfCrystals { get; private set; }

    public UnityEvent<PlayerInventory> OnCrystalCollected;

    public void CrystalsCollected()
    {
        NumberOfCrystals++;
        OnCrystalCollected.Invoke(this);
    }





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
