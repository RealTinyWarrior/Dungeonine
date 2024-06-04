using System.Collections;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    public int minDamage = 3;
    public int maxDamage = 8;
    public float damageDelay = 0.1f;
    public AudioSource hitAudio;
    BonineHealth bonineHealth;
    bool runCoroutine = false;

    void Start()
    {
        bonineHealth = GameObject.FindGameObjectWithTag("Bonine").GetComponent<BonineHealth>();
        StartCoroutine(DealDamage());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bonine"))
        {
            runCoroutine = true;
            hitAudio.Play();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Bonine"))
        {
            runCoroutine = false;
            hitAudio.Stop();
        }
    }

    IEnumerator DealDamage()
    {
        while (true)
        {
            if (runCoroutine) bonineHealth.Damage(Random.Range(minDamage, maxDamage));

            yield return new WaitForSeconds(damageDelay);
        }
    }
}