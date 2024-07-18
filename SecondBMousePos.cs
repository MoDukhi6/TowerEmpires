using System.Collections;
using UnityEngine;

public class SecondBMousePos : MonoBehaviour
{
    public ShieldTower shieldTower;
    public GameObject gameObjectToOpen;
    private bool isFirstClick = true;
    private bool alreadyMoved = false;
    private Vector3 firstClickPosition;

    // Cooldown variables
    public float cooldownDuration; // 2 seconds cooldown
    private float cooldownTimer = 0.0f;


    void Update()
    {
        // Check if the cooldown is over
        if (cooldownTimer <= 0.0f)
        {
            if (Input.GetMouseButtonDown(0) && isFirstClick && !alreadyMoved)
            {
                firstClickPosition = GetMouseWorldPosition();

                if (IsMousePositionInsideButton(firstClickPosition))
                {
                    Debug.Log("Mouse Click Position: " + firstClickPosition + " is inside the button.");
                    MoveDefendersToTargetPosition();
                }
                else
                {
                    Debug.Log("Mouse Click Position: " + firstClickPosition + " is outside the button.");
                }
            }
        }
        else
        {
            // Reduce the cooldown timer
            cooldownTimer -= Time.deltaTime;
        }
    }

    bool IsMousePositionInsideButton(Vector3 position)
    {
        // Assuming your button has a 2D circle collider, you can use Physics2D.OverlapPoint
        Collider2D buttonCollider2D = GetComponent<Collider2D>();
        if (buttonCollider2D != null)
        {
            // Set the desired center of the 2D collider
            Vector3 boundsCenter = buttonCollider2D.bounds.center;

            // Check if the position is inside the bounds
            return buttonCollider2D.OverlapPoint(position);
        }

        // If the button doesn't have a collider, return false
        return false;
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
    }

    void MoveDefendersToTargetPosition()
    {
        if (shieldTower != null)
        {
            GameObject[] defenders = shieldTower.GetDefendersArray();

            // Ensure the z component of firstClickPosition is 0
            firstClickPosition.z = 0;

            StartCoroutine(MoveDefendersWithAnimation(defenders));
            isFirstClick = true;
            alreadyMoved = false;

            // Start the cooldown timer
            cooldownTimer = cooldownDuration;
        }
    }

    IEnumerator MoveDefendersWithAnimation(GameObject[] defenders)
    {
        float animationDuration = 1.0f;
        Vector3 offset = firstClickPosition - GetAverageDefenderPosition(defenders);

        // Record initial positions
        Vector3[] initialPositions = new Vector3[defenders.Length];
        for (int i = 0; i < defenders.Length; i++)
        {
            if (defenders[i] != null)
            {
                initialPositions[i] = defenders[i].transform.position;
                Animator defenderAnimator = defenders[i].GetComponent<Animator>();
                if (defenderAnimator != null)
                {
                    defenderAnimator.SetTrigger("Run");
                }
            }
        }

        float elapsedTime = 0f;

        // While loop
        while (elapsedTime < animationDuration)
        {
            for (int i = 0; i < defenders.Length; i++)
            {
                if (defenders[i] != null)
                {
                    Vector3 initialPosition = initialPositions[i];
                    Vector3 targetPosition = initialPosition + offset;

                    defenders[i].transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / animationDuration);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }


        // Set all defenders to the final position
        for (int i = 0; i < defenders.Length; i++)
        {
            if (i < defenders.Length && defenders[i] != null)
            {
                Vector3 initialPosition = initialPositions[i];
                Vector3 targetPosition = initialPosition + offset;

                if (i < defenders.Length && defenders[i] != null)  // Additional check
                {
                    defenders[i].transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / animationDuration);

                    Animator defenderAnimator = defenders[i].GetComponent<Animator>();
                    if (defenderAnimator != null)
                    {
                        defenderAnimator.SetTrigger("Idle");
                    }
                }
            }
        }


        // Close the game object (you can replace "gameObjectToClose" with the actual game object you want to close)
        GameObject gameObjectToClose = gameObject;
        if (gameObjectToClose != null)
        {
            gameObjectToClose.SetActive(false);
        }

        if (gameObjectToOpen != null)
        {
            gameObjectToOpen.SetActive(true);
        }

        yield return new WaitForSeconds(animationDuration);

        firstClickPosition = Vector3.zero;

        // If needed, reset the "Run" trigger
        foreach (GameObject defender in defenders)
        {
            Animator defenderAnimator = defender.GetComponent<Animator>();
            if (defenderAnimator != null)
            {
                defenderAnimator.ResetTrigger("Run");
            }
        }
    }

    Vector3 GetAverageDefenderPosition(GameObject[] defenders)
    {
        Vector3 averagePosition = Vector3.zero;
        int validDefenders = 0;

        foreach (GameObject defender in defenders)
        {
            if (defender != null)
            {
                averagePosition += defender.transform.position;
                validDefenders++;
            }
        }

        if (validDefenders > 0)
        {
            averagePosition /= validDefenders;
        }

        return averagePosition;
    }
}
