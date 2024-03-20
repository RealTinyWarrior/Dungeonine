using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float start;
    float startTimer;

    void Update()
    {
        if (startTimer < start) startTimer += Time.deltaTime;
        else Destroy(gameObject);
    }
}