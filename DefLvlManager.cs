using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefLvlManager : MonoBehaviour
{
    public static DefLvlManager main;

    public Transform DefStart1;

    public Transform DefStart2;

    public Transform DefStart3;


    public Transform endpoint1; // Add endpoint for Defender1
    public Transform endpoint2; // Add endpoint for Defender2
    public Transform endpoint3; // Add endpoint for Defender3

    private void Awake()
    {
        main = this;
    }

    public Transform GetDefStart(int defenderIndex)
    {
        switch (defenderIndex)
        {
            case 0:
                return DefStart1;
            case 1:
                return DefStart2;
            case 2:
                return DefStart3;
            default:
                Debug.LogError("Invalid defender index");
                return null;
        }
    }


    public Transform GetDefEndpoint(string defenderName)
    {
        switch (defenderName)
        {
            case "Defender1":
                return endpoint1;
            case "Defender2":
                return endpoint2;
            case "Defender3":
                return endpoint3;
            default:
                Debug.LogError("Invalid defender name");
                return null;
        }
    }
}
