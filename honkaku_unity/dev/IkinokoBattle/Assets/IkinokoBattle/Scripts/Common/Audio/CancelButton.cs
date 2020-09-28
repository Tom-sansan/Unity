using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sound of cancel button clicked
/// </summary>
[RequireComponent(typeof(Button))]
public class CancelButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Make sound of cancel when button is clicked
        GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.Instance.Play("cancel");
        });
    }
}
