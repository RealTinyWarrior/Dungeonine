using UnityEngine;

public class GameItems : MonoBehaviour
{
    public Item[] items;

    public Item GetItem(int id) => items[id].Clone();
}
