using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaughSFX : MonoBehaviour
{
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.deathCount != 3) return;
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered laugh area, play SFX");
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }
}
