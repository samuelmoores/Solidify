using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // has to do with text mesh pro

public class InventoryUI : MonoBehaviour
{
    private TextMeshProUGUI crystalText;

    // Start is called before the first frame update
    void Start()
    {
        crystalText = GetComponent<TextMeshProUGUI>(); // text display box
    }

    public void UpdateCrystalText(PlayerInventory playerInventory)
    {
        crystalText.text = playerInventory.NumberOfCrystals.ToString(); //output crystal integer to string text
    }

}
