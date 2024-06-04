using UnityEngine;

public class GlowManager : MonoBehaviour
{
    public Texture2D[] glowTextures;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ApplyChangeInGlow(int index)
    {
        spriteRenderer.material.SetTexture("_GlowTex", glowTextures[index]);
    }
}
