using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIButtonPop : MonoBehaviour
{
    [SerializeField] private float popDuration = 0.25f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero; // mulai dari kecil
    }

    private void OnEnable()
    {
        StartCoroutine(PopupAnimation());
    }

    private IEnumerator PopupAnimation()
    {
        float t = 0f;
        float overshoot = 1.15f; // scale membesar sedikit dulu

        // Naik ke 1.15x
        while (t < popDuration)
        {
            t += Time.deltaTime;
            float p = t / popDuration;
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale * overshoot, p);
            yield return null;
        }

        // Turun balik ke 1x
        t = 0f;
        while (t < popDuration * 0.5f)
        {
            t += Time.deltaTime;
            float p = t / (popDuration * 0.5f);
            transform.localScale = Vector3.Lerp(originalScale * overshoot, originalScale, p);
            yield return null;
        }
    }
}
