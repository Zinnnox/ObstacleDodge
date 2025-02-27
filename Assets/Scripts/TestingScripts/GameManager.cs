// File: GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    // Singleton pattern for easy access to GameManager
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager is missing in the scene!");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Keep GameManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Reset the current scene
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Load the next scene in the build order
    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
    
        }
        else
        {
            Debug.Log("You've reached the last level! Resetting to the first level.");
            SceneManager.LoadScene(0); // Restart the game
        }
    }

    // Quit the game (works only in a built version)
    public void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }
    
    public int score = 0; // The player's score
    public TMP_Text scoreText; // Reference to the UI Text element for score display

    // Call this method to add to the player's score
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    // Reset the score (e.g., at the start of a new level)
    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    // Update the score UI
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
