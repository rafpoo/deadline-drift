using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // buat akses tombol UI

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Elements")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameOverTextObject;
    [SerializeField] private Button retryButton;

    private TMP_Text gameOverText;

    private bool isGameOver = false;
    public bool IsGameOver => isGameOver; // 👈 getter publik

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        gameOverText = gameOverTextObject.GetComponent<TMP_Text>();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (retryButton != null)
            retryButton.onClick.AddListener(RestartGame);
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        // ❌ jangan pakai Time.timeScale = 0
        // biarkan physics tetap jalan

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverText != null)
            gameOverText.text = "💥 GAME OVER 💥";

        Debug.Log("Game Over triggered!");
    }

    public void RestartGame()
    {
        Debug.Log("RestartGame() called!");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
