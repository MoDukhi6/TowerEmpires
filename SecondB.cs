using UnityEngine;
using UnityEngine.UI;

public class SecondButtonScript : MonoBehaviour
{
    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(SecondButtonClicked);
    }

    // Function to handle the second button's click event
    private void SecondButtonClicked()
    {
        Debug.Log("Second button was clicked!");
        // You can add your specific actions here
    }
}
