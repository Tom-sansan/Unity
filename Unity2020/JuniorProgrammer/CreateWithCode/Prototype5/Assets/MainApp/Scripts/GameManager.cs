using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject pauseScreen;
    public Button restartButton;
    public List<GameObject> targets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;
    public Slider soundSlider;
    public bool isGameActive;
    public int lives = 5;
    private AudioSource audioSource;
    private bool isPaused;
    private const string strSCORE = "Score: ";
    private const string strLIVES = "Lives: ";
    private float spawnRate = 1.0f;
    private float soundVolume = 50.0f;
    private int score = 0;

    private void Update()
    {
        // Check if the user has pressed the P key
        if (Input.GetKeyDown(KeyCode.P)) ChangePaused();
    }
    public void StartGame(int difficulty)
    {
        scoreText.text = strSCORE + score;
        livesText.text = strLIVES + lives;
        isGameActive = true;
        StartCoroutine(SpawnTarget());
        titleScreen.gameObject.SetActive(false);
        spawnRate /= difficulty;
        // PlaySound();
    }
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = strSCORE + score;
    }

    public void UpdateLives()
    {
        livesText.text = strLIVES + --lives;
        if (lives == 0) GameOver();
    }
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    private void ChangePaused()
    {
        var _isPaused = isPaused;
        isPaused = !_isPaused;
        pauseScreen.SetActive(isPaused);
        Time.timeScale = _isPaused ? 1 : 0;
    }
    //private void PlaySound()
    //{
    //    var sound = GetComponent<AudioSource>();
    //    sound.loop = true;
    //    sound.volume = soundSlider.value;
    //    sound.Play();
    //    Debug.Log("Playing Vol: " + soundSlider.value);
    //}
}
