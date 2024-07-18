using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashMenu : MonoBehaviour
{
    public int cash;
    [SerializeField] private Text cashTxt;
    // Use this method to retrieve the cash value in the menu scene
    private void Start()
    {
        // Check if the "Cash" key exists in PlayerPrefs
        if (PlayerPrefs.HasKey("Cash"))
        {
            cash = PlayerPrefs.GetInt("Cash");
            UpdateCashText();
        }
        else
        {
            // If the key doesn't exist, set it to the default value (0)
            PlayerPrefs.SetInt("Cash", 0);
            cash = 0;
            PlayerPrefs.Save();
        }
    }

    private void UpdateCashText()
    {
        cashTxt.text = cash.ToString();
    }
}
