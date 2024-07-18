using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CurencyScript : MonoBehaviour
{

    [SerializeField] private Text currencyInText;
    private int currStringToInt;
    public int Currency;
    [SerializeField] EnemySpawner enemySpawner;
    private int toAddToCurr;

    void Update()
    {
        Debug.Log(Currency);
        enemySpawner = FindObjectOfType<EnemySpawner>();
        if (enemySpawner.checkFlagToTake() == true)
        {
            
        }

    }
    private void Start()
    {
        //string currTxtToString = currencyInText.text;
        //currStringToInt = int.Parse(currTxtToString);

        //Currency = 270; // Initial currency value
        UpdateCurrencyText();
        //currStringToInt = Currency;
    }


    private void UpdateCurrencyText()
    {
        //Debug.Log("Updating currency text...");

        if (currencyInText != null)
        {
            currencyInText.text = Currency.ToString();
            //Debug.Log("__Currency: " + Currency);
            //Debug.Log("__currencyInText: " + currencyInText.text);
        }
        else
        {
            //Debug.Log("currencyInText is null.");
        }
        //GetCurrency();
    }
    public int GetCurrency()
    {
        return Currency;
    }

    public void ChangeCurrency(int amount)
    {
        
        Currency = amount;
        //Debug.Log("Currency is::: " + Currency);
        UpdateCurrencyText();
    }

    public void AddEnemyKillCurr(int amount)
    {
        toAddToCurr = enemySpawner.ToSendToCurr();
        //Debug.Log("toAddToCurr val isus; " + toAddToCurr);
        Currency += amount;
        //Debug.Log("Added Currency");
        UpdateCurrencyText();
        //Debug.Log("Updated CurrencyTxt");
    }

    //public void Update()
    //{
        // if enemiesAlive-- worked in DecreaseEnemiesAlive function inside EnemySpawner script -> deadFlag = true in that script and turn on deadFlag in this script
    //    if (deadFlag)
    //    {
    //        deadFlag = false;
    //        Currency += killCoins;
    //        UpdateCurrencyText();
    //        //turn off deadFlag in EnemySpawner script
    //    }
    //}
}
