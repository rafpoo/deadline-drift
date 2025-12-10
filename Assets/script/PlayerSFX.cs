using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public AudioSource footstepSource;
    public AudioSource sfxSource;     // Tambahan untuk loncat
    public AudioClip jumpClip;        // Suara loncat

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // --- FOOTSTEP ---
        bool isRunning = anim.GetCurrentAnimatorStateInfo(0).IsName("Run");

        if (isRunning && !footstepSource.isPlaying)
            footstepSource.Play();
        else if (!isRunning && footstepSource.isPlaying)
            footstepSource.Stop();

        // --- JUMP SOUND ---
        if (Input.GetKeyDown(KeyCode.Space))
            PlayJump();
    }

    void PlayJump()
    {
        sfxSource.PlayOneShot(jumpClip);
    }
}
