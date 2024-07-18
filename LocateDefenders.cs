using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocateDefenders : MonoBehaviour
{
    public ShieldTower shieldTower; // Reference to the ShieldTower script

    private Vector3 targetPosition; // Store the target position for defenders
    private bool isFirstClick = true; // Flag to track the first mouse click
    private int countClicks = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
        {
            countClicks++;
            targetPosition = Input.mousePosition;
            targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(targetPosition.x, targetPosition.y, Camera.main.nearClipPlane));

            if (isFirstClick)
            {
                Debug.Log("First Click Position: " + targetPosition);
            }
            else
            {
                Debug.Log("Second Click Position: " + targetPosition);
                
            }
        }

        // Reset the click count after processing
        if (countClicks >= 2)
        {
            countClicks = 0;
        }
    }




}
