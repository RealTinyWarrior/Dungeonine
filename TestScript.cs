using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InventoryManager inventory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryManager>();
        inventory.AddItemOnBonine(ItemCode.TheSlasher, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
