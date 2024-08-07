using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DynamicShadowCaster : MonoBehaviour
{
    public float offsetY;
    public float range;
    ShadowCaster2D shadowCaster2D;
    Vector2 gizmos;

    void Start()
    {
        shadowCaster2D = GetComponent<ShadowCaster2D>();
    }

    void Update()
    {
        RaycastHit2D[] ray = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y - offsetY), new Vector2(0, -1), range);
        if (!shadowCaster2D.castsShadows) shadowCaster2D.castsShadows = true;

        for (int i = 0; i < ray.Length; i++)
        {
            if (ray[i].collider.CompareTag("Wall"))
            {
                gizmos = ray[i].point;

                if (shadowCaster2D.castsShadows) shadowCaster2D.castsShadows = false;
            }
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, gizmos);
    }
}