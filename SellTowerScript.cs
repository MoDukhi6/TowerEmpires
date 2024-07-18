using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellTowerScript : MonoBehaviour
{
    [SerializeField] CurencyScript currencyScript;

    public int sellCost;
    private int currencyInInt;

    void Start()
    {
        int currfromScript = currencyScript.GetCurrency();
        currencyInInt = int.Parse(currfromScript.ToString());
    }


    public void Sell()
    {
        int currScript = currencyScript.GetCurrency();
        Debug.Log("currScript in sell script is " + currScript);
        currencyScript.ChangeCurrency(currScript + sellCost);
        //currencyScript.AddEnemyKillCurr(sellCost);
    }

}
