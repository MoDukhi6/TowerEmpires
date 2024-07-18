// DefDes script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefDes : MonoBehaviour
{
    public ShieldTower shieldT;

    private GameObject[] defendersArray;

    void Update()
    {
        // Get the array of defenders from the ShieldTower
        defendersArray = shieldT.GetDefendersArray();
        // Check if the Shield Tower is not active and destroy defenders
        if (!shieldT.isActiveAndEnabled)
        {
            DestroyDefenders();
        }
    }

    // Call this method to destroy all defenders
    private void DestroyDefenders()
    {
        //Debug.Log("defendersArray: " + defendersArray);
        foreach (GameObject defender in defendersArray)
        {
            if (defender != null)
            {
                Destroy(defender);
                shieldT.DecreaseDefAlive();
                shieldT.SetTActiveToFalse();
            }
        }
    }

}
