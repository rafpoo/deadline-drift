using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "SampleScene";

    [Header("Fade System")]
    [SerializeField] private FadeController fadeSystem;

    private void OnEnable()
    {
        StartCoroutine(StaggerButtons());
    }

    private void Start()
    {
        if (playButton != null)
            playButton.onClick.AddListener(StartGame);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    private IEnumerator StaggerButtons()
    {
        // RESET SCALE
        if (playButton != null)
            playButton.transform.localScale = Vector3.zero;

        if (quitButton != null)
            quitButton.transform.localScale = Vector3.zero;

        // MATIKAN dulu biar animasi muncul
        if (playButton != null) playButton.gameObject.SetActive(false);
        if (quitButton != null) quitButton.gameObject.SetActive(false);

        yield return null;

        // 1) Play button ON + animasi pop
        if (playButton != null)
        {
            playButton.gameObject.SetActive(true);
            yield return StartCoroutine(PopIn(playButton.transform));
        }

        // Delay untuk quit
        yield return new WaitForSeconds(0.15f);

        // 3) Quit button pop
        if (quitButton != null)
        {
            quitButton.gameObject.SetActive(true);
            yield return StartCoroutine(PopIn(quitButton.transform));
        }
    }

    private IEnumerator PopIn(Transform target)
    {
        float duration = 0.25f;
        float t = 0f;

        Vector3 start = Vector3.zero;
        Vector3 over = Vector3.one * 1.1f;
        Vector3 end = Vector3.one;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;
            target.localScale = Vector3.Lerp(start, over, p);
            yield return null;
        }

        t = 0f;
        while (t < duration * 0.5f)
        {
            t += Time.deltaTime;
            float p = t / (duration * 0.5f);
            target.localScale = Vector3.Lerp(over, end, p);
            yield return null;
        }
    }

    public void StartGame()
    {
        if (fadeSystem != null)
        {
            fadeSystem.FadeOutAndLoad(gameSceneName);
        }
        else
        {
            Debug.LogWarning("FadeSystem belum di-assign!");
            SceneManager.LoadScene(gameSceneName);
        }
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
