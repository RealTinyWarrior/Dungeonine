using System.Collections;
using UnityEngine;

public class Obsticle : MonoBehaviour
{
    public float delay;
    public Rigidbody2D rb;

    void Start() => StartCoroutine(DoObsticle());

    // * Adds friction to the obsticle based on the delay
    IEnumerator DoObsticle()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            rb.velocity = Vector2.zero;
        }
    }
}