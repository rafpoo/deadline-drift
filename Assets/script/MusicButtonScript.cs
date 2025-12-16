using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButtonScript : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Image enabledImage;
    [SerializeField] private Image disabledImage;
    private bool isPlaying = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleMusic()
    {
        isPlaying = !isPlaying;
        if (isPlaying)
        {
            musicSource.Play();
            enabledImage.gameObject.SetActive(true);
            disabledImage.gameObject.SetActive(false);
        }
        else
        {
            musicSource.Pause();
            enabledImage.gameObject.SetActive(false);
            disabledImage.gameObject.SetActive(true);
        }
    }
}
