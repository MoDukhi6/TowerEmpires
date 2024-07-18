using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriceTower : MonoBehaviour
{
    [SerializeField]
    private int price;
    [SerializeField]
    private Text priceTxt;

    public int Price
    {
        get
        {
            return price;
        }
    }

    private void Start()
    {
        priceTxt.text = price+"";
    }
}
