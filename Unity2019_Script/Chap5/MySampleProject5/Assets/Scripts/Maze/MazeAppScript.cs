using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class MazeAppScript : MonoBehaviour
{
    // GUI parts
    public Text messageText;
    public GameObject panel;
    public Slider sizeSlider;
    public Slider levelSlider;

    public int gameTime = 300;                  // Play time
    public int power = 100;                     // Initial value for Power
    public int mazeSize = 100;                  // Maze size
    public float mazeLevel = 1;                 // Maze level
    private System.Random rnd;
    private bool endFlg = false;                // End flag
    private int playTime = 300;                 // Current play time
    private int endTime = 300;                  // End time
    private int hiScore = 0;                    // High score
    private bool toolFlg = false;               // Flag for displaying tool
    private string mazeSizeStr = "mazeSize";
    private string mazeLevelStr = "mazeLevel";
    private string hiScoreStr = "hiScore";

    /// <summary>
    /// Initialize Maze
    /// </summary>
    void Start()
    {
        rnd = new System.Random(System.Environment.TickCount);
        LoadPref();
        ReSet();
    }

    // Update is called once per frame
    /// <summary>
    /// Make setting window ON/OFF by space bar
    /// End check, time update and end time check
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            toolFlg = !toolFlg;
            panel.GetComponent<CanvasGroup>().alpha = toolFlg ? 1f : 0f;
            if (endFlg) SavePref();
        }
        if (endFlg) return;
        playTime = endTime - (int)Time.time;
        CheckTime();
    }

    /// <summary>
    /// Load settings
    /// </summary>
    private void LoadPref()
    {
        mazeSize = PlayerPrefs.GetInt(mazeSizeStr);
        mazeLevel = PlayerPrefs.GetFloat(mazeLevelStr);
        hiScore = PlayerPrefs.GetInt(hiScoreStr);
        if (mazeSize < 4) mazeSize = 4;
        if (mazeLevel < 1) mazeLevel = 1;
        sizeSlider.value = mazeSize;
        levelSlider.value = mazeLevel;
    }

    /// <summary>
    /// Save settings
    /// </summary>
    private void SavePref()
    {
        PlayerPrefs.SetInt(mazeSizeStr, mazeSize);
        PlayerPrefs.SetFloat(mazeLevelStr, mazeLevel);
    }

    /// <summary>
    /// Initialize Maze. Delete the current maze and create a new one.
    /// </summary>
    private void ReSet()
    {
        SavePref();
        panel.GetComponent<CanvasGroup>().alpha = 0f;
        messageText.text = string.Empty;
        power = 100;
        endTime = gameTime + (int)Time.time;
        toolFlg = false;
        endFlg = false;

        // Get all walls
        GameObject[] walls = GameObject.FindGameObjectsWithTag("ob_wall");
        // Destroy walls
        foreach (var wall in walls)
        {
            GameObject.Destroy(wall);
        }

        // Get all enemy balls
        GameObject[] sps = GameObject.FindGameObjectsWithTag("sphere");
        // Destroy enemy balls
        foreach (var sp in sps)
        {
            GameObject.Destroy(sp);
        }

        CreateMazeData();
        CreateSphere();
        GameObject.Find("unitychan").GetComponent<MazeAvatarColliderScript>().collisionFlg = false;
        GameObject.Find("Plane").GetComponent<Renderer>().material.color = Color.white;
    }

    /// <summary>
    /// Create a new Maze data. This is not a actual Maze creation but a blueprint with two-dimensional array
    /// </summary>
    private void CreateMazeData()
    {
        // Maze size with width and height.
        // For example, if mazeSize is 1 then maze size is 6 x 6 (= width x height).
        int mazeW = mazeSize * 4 + 2;
        bool[,] fdata = new bool[mazeW, mazeW];
        // Create surrounding walls
        for (int i = 0; i < mazeW; i++)
        {
            for (int j = 0; j < mazeW; j++)
            {
                if (i == 0 || i == (mazeW - 1) ||
                    j == 0 || j == (mazeW - 1))
                    fdata[i, j] = true;     // wall
                else
                    fdata[i, j] = false;    // no wall
            }
        }


        int[,] arw = new int[,]
        {
            { 0, -1 }, { 0, 1 }, { -1, 0 }, { 1, 0 }
        };
        // Create inside walls
        for (int i = 0; i < (mazeSize / 2) * (mazeSize / 2); i++)
        {
            while (true)
            {
                int x = rnd.Next(1, mazeSize) * 4;
                int y = rnd.Next(1, mazeSize) * 4;
                if (fdata[x, y]) continue;
                int n = i % 4;  // Pick up a arw array randomly
                fdata[x, y] = true;
                while (true)
                {
                    // Create a wall until the wall collides with other walls
                    x += arw[n, 0];
                    y += arw[n, 1];
                    if (fdata[x, y]) break;
                    else fdata[x, y] = true;
                }
                break;
            }
        }

        // Locate unitychan
        int cp = mazeW / 2;
        fdata[cp, cp] = false;
        GameObject.Find("unitychan").transform.position = new Vector3(cp, 0, cp);

        // Create actual walls
        CreateMaze(fdata);

        // Create goal
        int[,] gdatas = new int[,]
        {
            {1, 1}, {1, mazeW - 2}, {mazeW - 2, 1}, {mazeW - 2, mazeW - 2}
        };
        int gn = rnd.Next(4);
        Vector3 goalpos = new Vector3(gdatas[gn, 0], 1.5f, gdatas[gn, 1]);
        GameObject.Find("Goal").transform.position = goalpos;
    }

    /// <summary>
    /// Create a new Maze object with MazeData
    /// </summary>
    /// <param name="data"></param>
    private void CreateMaze(bool[,] data)
    {
        int mazeW = mazeSize * 4 + 2;
        Texture texture = Resources.Load<Texture>("CliffAlbedoSpecular");
        for (int i = 0; i < mazeW; i++)
        {
            for (int j = 0; j < mazeW; j++)
            {
                if (data[i, j])
                {
                    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.tag = "ob_wall";
                    obj.transform.localScale = new Vector3(1, 2, 1);
                    obj.transform.position = new Vector3(i, 1, j);
                    obj.GetComponent<Collider>().isTrigger = false;
                    obj.GetComponent<Renderer>().material.mainTexture = texture;
                }
            }
        }
    }

    /// <summary>
    /// Create enamy object
    /// </summary>
    private void CreateSphere()
    {
        for (int i = 0; i < (mazeSize / 2); i++)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.tag = "sphere";
            Renderer renderer = obj.GetComponent<Renderer>();
            renderer.material.color = new Color(1, 0, 0, 0.5f);
            // Set material
            renderer.material.SetFloat("_Mode", 3f);
            renderer.material.SetInt("_SrcBlend", (int)BlendMode.One);
            renderer.material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            renderer.material.SetInt("_Zwrite", 0);
            renderer.material.DisableKeyword("_ALPHATEST_ON");
            renderer.material.DisableKeyword("_ALPHABLEND_ON");
            renderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            renderer.material.renderQueue = 3000;

            // Set location
            obj.AddComponent<Rigidbody>();
            obj.transform.position = new Vector3(rnd.Next(mazeSize) * 4 + 2, 0, rnd.Next(mazeSize) * 4 + 1);
            // Add script
            obj.AddComponent<MazeSphereScript>();
        }
    }

    /// <summary>
    /// Return end flag
    /// </summary>
    /// <returns></returns>
    public bool IsEnd() => endFlg;

    /// <summary>
    /// Power down
    /// </summary>
    /// <param name="n"></param>
    public void LossPower(int n)
    {
        power -= n;
        if (power <= 0)
        {
            power = 0;
            BadEnd();
        }
    }

    /// <summary>
    /// Check end time
    /// </summary>
    public void CheckTime()
    {
        if (playTime <= 0) BadEnd();
    }

    /// <summary>
    /// Process of game over
    /// </summary>
    public void BadEnd()
    {
        endFlg = true;
        int score = (int)(power * mazeLevel + playTime * mazeSize);
        messageText.color = Color.blue;
        messageText.text = "GAMEOVER";
        PlayerPrefs.SetInt(hiScoreStr, 1);  // delete
    }

    /// <summary>
    /// Process of good end
    /// </summary>
    public void GoodEnd()
    {
        endFlg = true;
        int score = (int)(power * mazeLevel * 2 + playTime * mazeSize * 2);
        string msg = "CLEAR!";
        messageText.color = Color.yellow;
        if (score > hiScore)
        {
            hiScore = score;
            msg = "Hi-Score!";
            PlayerPrefs.SetInt(hiScoreStr, hiScore);
            messageText.color = Color.red;
        }
        msg += " [" + score + "]";
        messageText.text = msg;
    }
    /// <summary>
    /// Set value of SizeSlider
    /// </summary>
    public void SetSizeSlider() => mazeSize = (int)sizeSlider.value;

    /// <summary>
    /// Set value of LevelSlider
    /// </summary>
    public void SetLevelSlider() => mazeLevel = (int)levelSlider.value;

    /// <summary>
    /// Reset by ResetButton
    /// </summary>
    public void DoReset() => ReSet();

    /// <summary>
    /// GUI to show SCORE, TIME and POWER
    /// </summary>
    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 150, 80), "SCORE");
        GUI.Label(new Rect(35, 40, 100, 50), "TIME: " + playTime);
        GUI.Label(new Rect(35, 60, 100, 50), "POWER: " + power);
    }
}
