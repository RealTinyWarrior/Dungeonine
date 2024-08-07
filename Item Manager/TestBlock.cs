using UnityEngine;

public class TestBlock : MonoBehaviour
{
    Item item;
    SpriteRenderer spriteRl;

    void Start()
    {
        item = GetComponent<ItemParam>().item;
        spriteRl = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRl.color = item.mouseKey == 0 ? Color.red : Color.blue;
    }
}