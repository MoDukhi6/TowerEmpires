using UnityEngine;
using UnityEngine.UI;

public class IntToText : MonoBehaviour
{
    [SerializeField]
    private Text textComponent;

    private int intValue = 42; // Your int variable

    void Start()
    {
        // Ensure that the Text component is assigned in the Unity Editor
        if (textComponent != null)
        {
            // Convert int to string and assign it to the Text component
            textComponent.text = intValue.ToString();
        }
        else
        {
            Debug.LogError("Text component is not assigned to IntToText script on GameObject: " + gameObject.name);
        }
    }
}
