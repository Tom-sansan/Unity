using UnityEngine;
using UnityEngine.UI;

public class GuiSceneScript : MonoBehaviour
{
    public Canvas canvas;
    public Text text;
    public Toggle toggle;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
        text.text = "ready...";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) canvas.enabled = true;
    }

    public void OnButtonClick()
    {
        text.text = "ok!!";
    }

    public void OnToggleChanged()
    {
        text.text = toggle.isOn ? "ON" : "OFF";
    }

    public void OnSliderChanged()
    {
        text.text = "value = " + slider.value;
    }

    private void OnGUI()
    {
        // OnGUI5_9();
        // OnGUI5_10();
        // OnGUI5_11();
        // OnGUI5_12();
        // OnGUI5_13();
        OnGUI5_14();
    }

    private void OnGUI5_9()
    {
        CreateGUIBox(10, 10, 350, 100, "Hello");
        CreateGUILabel(35, 60, 300, 50, "This is IMGUI!");
    }

    int msg_x = 0;
    int dx = 1;
    private void OnGUI5_10()
    {
        this.msg_x += this.dx;
        if (this.msg_x > 200 || this.msg_x < 0) dx *= -1;
        CreateGUIBox(this.msg_x, 10, 300, 100, "Hello");
        CreateGUILabel(this.msg_x + 20, 60, 300, 50, "This is IMGUI!");
    }

    int count = 0;
    /// <summary>
    /// IMGUI for Button
    /// </summary>
    void OnGUI5_11()
    {
        CreateGUIBox(10, 10, 300, 150, "Hello");
        if (GUI.Button(new Rect(100, 100, 100, 25), "Add Count")) this.count++;
        CreateGUILabel(35, 60, 300, 50, "count: " + this.count);
    }

    bool flg = false;
    /// <summary>
    /// IMGUI for Toggle
    /// </summary>
    void OnGUI5_12()
    {
        CreateGUIBox(10, 10, 300, 150, "Hello");
        this.flg = GUI.Toggle(new Rect(100, 100, 100, 25), this.flg, "Toggle");
        CreateGUILabel(35, 60, 300, 50, "Toggle: " + this.flg);
    }

    float slide_value = 0;

    /// <summary>
    /// IMGUI for Slider
    /// </summary>
    void OnGUI5_13()
    {
        CreateGUIBox(10, 10, 300, 150, "Hello");
        this.slide_value = GUI.HorizontalSlider(new Rect(100, 100, 100, 25), this.slide_value, 0, 100);
        CreateGUILabel(35,60, 300, 50, "Value: " + this.slide_value);
    }

    string msg = string.Empty;

    /// <summary>
    /// IMGUI for Text
    /// </summary>
    void OnGUI5_14()
    {
        CreateGUILabel(10, 10, 300, 150, "Hello");
        this.msg = GUI.TextField(new Rect(100, 100, 100, 25), this.msg);
        CreateGUILabel(35, 60, 300, 50, "You're writing..." + this.msg);
    }

    /// <summary>
    /// Create IMGUI for Box
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="text"></param>
    private void CreateGUIBox(float x, float y, float width, float height, string text)
    {
        GUI.Box(new Rect(x, y, width, height), text);
    }

    /// <summary>
    /// Create IMGUI for Label
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="text"></param>
    private void CreateGUILabel(float x, float y, float width, float height, string text)
    {
        GUI.Label(new Rect(x, y, width, height), text);
    }
}
