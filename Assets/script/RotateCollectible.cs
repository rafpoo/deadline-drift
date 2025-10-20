using UnityEngine;

public class RotateCollectible : MonoBehaviour
{
    public float rotationSpeed = 90f;
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
