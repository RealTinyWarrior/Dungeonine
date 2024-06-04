using UnityEngine;

public class IgnoreCollisiton : MonoBehaviour
{
    void Start() => Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), GameObject.FindGameObjectWithTag("Bonine").GetComponent<BoxCollider2D>());
}