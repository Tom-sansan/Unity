using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    public int difficulty;
    private GameManager gameManager;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        
    }

    private void SetDifficulty()
    {
        Debug.Log(gameObject.name + " was clicked.");
        gameManager.StartGame(difficulty);
    }
}
