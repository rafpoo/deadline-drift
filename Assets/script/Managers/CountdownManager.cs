using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownManager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public GameObject player;    // Player atau script movement
    public float countdownTime = 3f;

    void Start()
    {
        // Matikan player di awal
        player.SetActive(false);
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        float time = countdownTime;

        while (time > 0)
        {
            countdownText.text = Mathf.Ceil(time).ToString();
            yield return new WaitForSeconds(1f);
            time -= 1f;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);

        // Aktifkan player setelah countdown selesai
        player.SetActive(true);
    }
}
