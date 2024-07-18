using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Part2SetTower : MonoBehaviour
{
    public GameObject imageObject;
    private Image imageComponent;

    
    [SerializeField] private Text TPriceInText;
    public int TPriceInInt;
    private bool canBuy = false;
    private int currencyInInt;

    public GameObject openImage1;
    public GameObject closeImage1;
    public GameObject openImage2;
    public GameObject closeImage2;

    private CurencyScript currencyScript;

    private void Start()
    {
        currencyScript = FindObjectOfType<CurencyScript>();
        if (currencyScript == null)
        {
            Debug.LogError("CurencyScript not found!");
            return;
        }

        string stextString = TPriceInText.text;
        TPriceInInt = int.Parse(stextString);

        imageComponent = imageObject.GetComponent<Image>();
        imageObject.SetActive(false);


        // //Take currency val from Currency script and put it into currfromScript.
        //resultIntCurrency = currfromScript; // currfromScript is the value of the updated Currency from CurrencyScript.
        //currencyInText.text = Currency.ToString();
        int currfromScript = currencyScript.GetCurrency();
        currencyInInt = int.Parse(currfromScript.ToString());
    }

    public void ToggleImage()
    {
        int currfromScript = currencyScript.GetCurrency();
        Debug.Log("currfromScript: " + currfromScript + "TPriceInInt: " + TPriceInInt);
        if (currfromScript >= TPriceInInt)
        {
            canBuy = true;
        }

        if (canBuy)
        {// You can buy this tower.
            imageObject.SetActive(!imageObject.activeSelf);
            openImage1.SetActive(true);
            closeImage1.SetActive(false);
            openImage2.SetActive(true);
            closeImage2.SetActive(false);
            //Debug.Log("you're in 1: currfromScript is " + currfromScript + "TPriceInInt is " + TPriceInInt);
            if (currfromScript - TPriceInInt < 0)
            {
                currfromScript = 0;
                //Update currency in Currency script to be currfromScript
                currencyScript.ChangeCurrency(currfromScript);
            }
            else
            {
                //Debug.Log("you're in 2: currfromScript is " + currfromScript + "TPriceInInt is " + TPriceInInt);

                //Update currency in Currency script to be currfromScript -= TPriceInInt
                currencyScript.ChangeCurrency(currfromScript - TPriceInInt);
            }
            canBuy = false;
        }
        else
        {
            Debug.Log("You don't have enough coins!");
        }
    }

}
