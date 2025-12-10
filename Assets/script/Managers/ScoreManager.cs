using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text finalScoreText; // muncul di GameOver panel

    [Header("Score Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float scoreMultiplier = 5f; // makin besar, makin cepat naik skor

    private float score = 0f;
    private float startZ;
    private bool isScoring = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (player != null)
            startZ = player.position.z;

        UpdateScoreUI();
        if (finalScoreText != null)
            finalScoreText.text = "";
    }

    void Update()
    {
        if (!isScoring) return;
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            StopScoring();
            return;
        }

        if (player != null)
        {
            float distance = player.position.z - startZ;
            Debug.Log("Distance: " + distance);
            score = Mathf.Max(0, distance * scoreMultiplier);
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }

    public void StopScoring()
    {
        isScoring = false;

        // 🔥 sembunyikan score di layar
        if (scoreText != null)
            scoreText.gameObject.SetActive(false);

        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + Mathf.FloorToInt(score);
    }

    public void ResetScore()
    {
        isScoring = true;
        score = 0f;

        // 🔥 tampilkan lagi score di layar
        if (scoreText != null)
            scoreText.gameObject.SetActive(true);

        UpdateScoreUI();

        if (finalScoreText != null)
            finalScoreText.text = "";
    }



    public int GetFinalScore()
    {
        return Mathf.FloorToInt(score);
    }
}
