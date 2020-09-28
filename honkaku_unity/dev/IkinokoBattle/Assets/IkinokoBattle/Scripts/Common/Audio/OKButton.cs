using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sound of OK Button clicked
/// </summary>
[RequireComponent(typeof(Button))]
public class OKButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.Instance.Play("ok");
        });
    }
}
