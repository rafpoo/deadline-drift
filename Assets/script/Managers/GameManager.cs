using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private Button retryButton;

    private bool isGameOver = false;
    private int score;
    public bool IsGameOver => isGameOver;
    public int deathCount = 0;

    public TextMeshProUGUI countdownText;
    public float countdownTime = 3.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateScoreUI();

        if (retryButton != null)
            retryButton.onClick.AddListener(ReturnToMainMenu);

    }


    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (TileManager.Instance != null)
            TileManager.Instance.StopTiles();

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.StopScoring();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverText != null)
            gameOverText.text = "💥 GAME OVER 💥\nScore: " + ScoreManager.Instance.GetFinalScore();

        Debug.Log("Game Over triggered!");
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        Instance.deathCount = 0;

        SceneManager.LoadScene("MainMenu");
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        Instance.deathCount = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
