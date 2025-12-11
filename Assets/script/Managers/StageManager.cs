using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("BGM")]
    public AudioSource bgmSource;
    public AudioClip bgmStage0_Normal;
    public AudioClip bgmStage1_Intense;
    public AudioClip bgmStage2_MoreIntense;
    public AudioClip bgmStage3_Horror;

    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip thunderSFX;
    public AudioClip flashSFX;
    public AudioClip jumpscareSFX;

    [Header("Effects")]
    public GameObject horrorDarkOverlay;
    public GameObject lightningFlash;
    public GameObject jumpScareScreen;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Stage 0: musik normal
        if (bgmSource != null && bgmStage0_Normal != null)
        {
            bgmSource.clip = bgmStage0_Normal;
            bgmSource.Play();
        }

        if (jumpScareScreen != null)
            jumpScareScreen.SetActive(false);

        if (horrorDarkOverlay != null)
            horrorDarkOverlay.SetActive(false);

        if (lightningFlash != null)
            lightningFlash.SetActive(false);
    }

    /// <summary>
    /// Dipanggil dari Character.cs ketika kena obstacle
    /// </summary>
    public void OnPlayerHit(int hitCount)
    {
        if (GameManager.Instance.IsGameOver)
            return;
        // === STAGE 1 ===
        if (hitCount == 1)
        {
            bgmSource.clip = bgmStage1_Intense;
            bgmSource.Play();

            StartCoroutine(FlashLightning(false));

            TileManager.Instance.moveSpeed += 3f;
        }

        // === STAGE 2 ===
        else if (hitCount == 2)
        {
            bgmSource.clip = bgmStage2_MoreIntense;
            bgmSource.Play();

            StartCoroutine(FlashLightning(false));

            TileManager.Instance.moveSpeed += 3f;
        }

        // === STAGE 3 ===
        else if (hitCount == 3)
        {
            bgmSource.clip = bgmStage3_Horror;
            bgmSource.Play();

            if (horrorDarkOverlay != null)
                horrorDarkOverlay.SetActive(true);


            // Efek kilat
            StartCoroutine(FlashLightning(true));

            TileManager.Instance.moveSpeed = 5f;
        }

        // === STAGE 4 === (Game Over → jumpscare)
        else if (hitCount == 4)
        {
            if (jumpScareScreen != null)
                jumpScareScreen.SetActive(true);

            if (sfxSource != null && jumpscareSFX != null)
                sfxSource.PlayOneShot(jumpscareSFX);

            StartCoroutine(FlashLightning(true));

            // Tidak perlu ganti musik lagi karena game over

            StartCoroutine(TriggerGameOverDelay());
        }
    }

    IEnumerator TriggerGameOverDelay()
    {
        yield return new WaitForSecondsRealtime(2f); // KING bisa atur ini

        jumpScareScreen.SetActive(false);

        GameManager.Instance.GameOver();
    }


    IEnumerator FlashLightning(bool isThunder)
    {
        lightningFlash.SetActive(true);

        Animator anim = lightningFlash.GetComponent<Animator>();
        anim.Play("LightningFlash", -1, 0f);

        if (isThunder)
        {
            if (sfxSource != null && thunderSFX != null)
                sfxSource.PlayOneShot(thunderSFX);
        }
        else
        {
            if (sfxSource != null && flashSFX != null)
                sfxSource.PlayOneShot(flashSFX);
        }


        yield return new WaitForSeconds(0.3f);

        lightningFlash.SetActive(false);
    }

}