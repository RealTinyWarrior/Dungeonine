using System.Collections;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    public float offset;
    bool isShaking = true;
    float defaultXPosition;
    int hasShakedTimes = 0;
    Health health;

    void Start()
    {
        health = GetComponent<Health>();
        defaultXPosition = transform.position.x;
    }

    public void TakeDamage(float damage)
    {
        health.Damange(damage);
        hasShakedTimes = 0;
        isShaking = true;

        StartCoroutine(ShakeObject());

        if (health.health <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ShakeObject()
    {
        while (isShaking)
        {
            hasShakedTimes++;

            if (hasShakedTimes == 7)
            {
                isShaking = false;
                hasShakedTimes = 0;
            }

            transform.position = new Vector2(defaultXPosition + Random.Range(-offset, offset), transform.position.y);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
