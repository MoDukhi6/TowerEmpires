using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashCounter : MonoBehaviour
{
    [SerializeField] private int cash = 0;
    [SerializeField] private Text cashcounterTxt;
    private bool canJoin = false;
    private int amountToAdd;

    private void Start()
    {
        UpdateCashText();
    }

    private void Update()
    {
        if (canJoin)
        {
            OnLevelComplete();
        }
    }

    public void AddCash(int amount)
    {
        cash += amount;
        amountToAdd = amount; // Set amountToAdd when adding cash
        Debug.Log("amountToAdd " + amountToAdd);
        UpdateCashText();
    }

    public int GetCashAmount()
    {
        return cash;
    }

    private void UpdateCashText()
    {
        cashcounterTxt.text = cash.ToString();
    }

    private void OnLevelComplete()
    {
        int prevCash = PlayerPrefs.GetInt("Cash", 0);
        int newCash = prevCash + cash; // Use amountToAdd instead of cash

        Debug.Log("newCash " + newCash + " prevCash " + prevCash + " cash " + cash);
        PlayerPrefs.SetInt("Cash", newCash);
        PlayerPrefs.Save();
        canJoin = false; // Reset the flag after updating PlayerPrefs
    }

    public void GoToMethod(int x)
    {
        if (x == 1)
        {
            canJoin = true; // Set the flag after updating PlayerPrefs
        }
    }
}
