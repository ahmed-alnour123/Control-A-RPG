using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public Inventory inventory1;
    public Inventory inventory2;
    public InventoryItem collectable;

    void SStart() {
        Debug.Log("Starting");
        inventory1.PrintInventory();
        inventory2.PrintInventory();
        Debug.Log("");

        Debug.Log("Adding");
        inventory1.AddItem(collectable, 30);
        inventory2.AddItem(collectable, 30);

        inventory1.PrintInventory();
        inventory2.PrintInventory();
        Debug.Log("");

        Debug.Log("Removing");
        inventory1.RemoveItem(collectable, 1);

        inventory1.PrintInventory();
        inventory2.PrintInventory();

        Debug.Log("Removing 38");
        inventory1.RemoveItem(collectable, 38);

        inventory1.PrintInventory();
        inventory2.PrintInventory();
    }


    void Update() {

    }
}
