using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public int minDamage = 10;
    public int maxDamage = 25;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Destructable"))
        {
            DestructableObject destructableObject = col.GetComponent<DestructableObject>();
            destructableObject.TakeDamage(RandomDamage());
        }
    }

    int RandomDamage() => Random.Range(minDamage, maxDamage);
}