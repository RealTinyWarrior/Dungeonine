using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public float effectSpeed = 0.2f;
    public Color color;
    bool doEffect = false;
    float timer = 0f;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void CreateHitffect() => doEffect = true;

    void Update()
    {
        if (doEffect)
        {
            timer += Time.deltaTime;

            if (timer >= effectSpeed)
            {
                doEffect = false;
                timer = 0f;
            }

            if (timer < effectSpeed / 2)
            {
                float opacity = 1 - (((effectSpeed / 2) - timer) / (effectSpeed / 2));
                spriteRenderer.color = Color.Lerp(Color.white, color, opacity);
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);

                Debug.Log(opacity);
            }

            else
            {
                float opacity = 1 - ((timer - (effectSpeed / 2)) / (effectSpeed / 2));
                spriteRenderer.color = Color.Lerp(Color.white, color, opacity);
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);

                Debug.Log(opacity);
            }
        }
    }
}
