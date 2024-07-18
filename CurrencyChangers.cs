/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CurrencyChangers : MonoBehaviour
{
    private int currency;
    private int valueFromSetTower;

    [SerializeField]
    private Text currencyTxt;

    public int Currency
    {
        get
        {
            return currency;
        }
        set
        {
            this.currency = value;
            this.currencyTxt.text = value.ToString();
        }
    }
    
    void Start()
    {
        Currency = 90;
        if (currencyTxt == null)
        {
            Debug.Log("Currency text field is not assigned.");
        }
    }
    public void ToChangeCurr()
    {
        //SetTower SetTowerInstance = new SetTower();
        valueFromSetTower = SetTower.newcur;
        this.currency = valueFromSetTower;
        Debug.Log(SetTower.newcur + " " + valueFromSetTower + " " + Currency + " " + this.currency);
        UpdateCurrencyText();
        //Debug.Log(SetTower.newcur + " " + valueFromSetTower + " " + Currency + " " + this.currency);
    }

    private void UpdateCurrencyText()
    {
        if (Currency != 0)
        {
            //currencyTxt.text = this.currency.ToString();
            //this.currencyTxt.text = this.currency.ToString();
            Debug.Log(Currency);
        }
        else
        {
            Debug.Log("Currency text field is null.");
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyChangers : MonoBehaviour
{
    //private int currency;
    private int valueFromSetTower;

    [SerializeField]
    public Text currencyTxt;
    [SerializeField]
    private int Currency;

    public static CurrencyChangers CurrencyChangersFunc; // Static reference to the CurrencyChangers instance


    void Start()
    {
        CurrencyChangersFunc = this; // Assign the current instance to the static variable
        Currency = 90; // Initial currency value
        UpdateCurrencyText();
    }

    public void ToChangeCurr()
    {
        //SetTower SetTowerInstance = new SetTower();
        valueFromSetTower = SetTower.newcur;
        Currency = valueFromSetTower;
        
        //currencyTxt.text = Currency.ToString();
        Debug.Log(SetTower.newcur + " " + valueFromSetTower + " " + Currency);
        ////////////////////UpdateCurrencyText();
        //Debug.Log(SetTower.newcur + " " + valueFromSetTower + " " + Currency + " " + this.currency);
    }


    private void UpdateCurrencyText()
    {
        Debug.Log("Updating currency text");
        Debug.Log("currencyTxt: " + currencyTxt);

        if (currencyTxt != null)
        {
            currencyTxt.text = Currency.ToString();
            Debug.Log("__Currency: " + Currency);
            Debug.Log("__currencyTxt: " + currencyTxt);
        }
        else
        {
            Debug.Log("currencyTxt is null.");
        }
    }
}

*/