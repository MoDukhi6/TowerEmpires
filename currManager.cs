using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class currManager : MonoBehaviour
{
    public static currManager main;

    public int currency;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = 89;
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int valueFromSetTower)
    {
        if (currency >= valueFromSetTower)
        {
            currency -= valueFromSetTower; // Buy tower.
            return true;
        }
        else
        {
            Debug.Log("You dont have enougth coins!");
            return false;
        }
    }
}
