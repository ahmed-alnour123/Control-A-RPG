using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;



public class SaveManager : MonoBehaviour {
    public GameObject placablesParent;

    public void Save() {
    }

    public void Load() {

    }
}

class SaveData {
    // player status

    // inventories
    List<Inventory> inventory;

    // placables
    List<Transform> placables;
    // Plant and growing
}

class PlayerData {
    public int health;
    public int energy;
    public int xp;
}
