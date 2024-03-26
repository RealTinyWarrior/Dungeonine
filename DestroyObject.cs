using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float timer;
    float countDown;

    void Update()
    {
        if (countDown < timer) countDown += Time.deltaTime;
        else Destroy(gameObject);
    }
}