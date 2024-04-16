using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorManager : MonoBehaviour
{
    public float animationSpeed;
    public Tilemap tilemap;
    public Vector3Int[] coordinates;
    public TileBase tile01;
    public TileBase tile02;
    public TileBase tile03;
    public TileBase tile04;
    public GameObject doorInteraction;
    public GameObject darkManager;
    public Trigger trigger;
    public bool doorIsOpened = false;
    public bool isFloor2 = false;
    Collider2D tempCol;
    InventoryManager inventory;
    GameObject bonine;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryManager>();
        bonine = GameObject.FindGameObjectWithTag("Bonine");
        tempCol = new();
    }

    public void OpenDoor()
    {
        tilemap.SetTile(coordinates[0], tile01);
        tilemap.SetTile(coordinates[1], tile02);
        tilemap.SetTile(coordinates[2], tile03);
        tilemap.SetTile(coordinates[3], tile04);

        if (!isFloor2) doorIsOpened = true;
        else doorIsOpened = false;
    }

    public void CheckForKey(int itemID)
    {
        int itemIndex = inventory.ItemIndexOnInventory(itemID);
        if (itemIndex == -1) return;

        doorIsOpened = true;
        inventory.RemoveItem(itemIndex);
        if (trigger.isActive) trigger.onTriggerEnter?.Invoke(tempCol);
        //Play audio

        doorInteraction.SetActive(false);
        OpenDoor();
    }

    public void TeleportX(float x)
    {
        if (doorIsOpened)
        {
            bonine.transform.position = new Vector2(x, bonine.transform.position.y);
            darkManager.SetActive(false);
        }
    }

    public void TeleportY(float y)
    {
        if (doorIsOpened)
        {
            bonine.transform.position = new Vector2(bonine.transform.position.x, y);
            darkManager.SetActive(false);
        }
    }
}
