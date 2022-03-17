using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager instance;

    public UnityEvent InventoryChanged;

    void ChangeInventory() {
        InventoryChanged?.Invoke();
    }

    void Awake() {
        instance = this;
    }

    public Dictionary<string, Collectable> collectables = new Dictionary<string, Collectable>();

    private void Start() {
        // InventoryChanged.AddListener(PrintInventory);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Semicolon)) PrintInventory();
    }

    public void AddToInventory(Collectable newItem) {
        if (!collectables.ContainsKey(newItem.name)) {
            collectables.Add(newItem.name, newItem);
        } else {
            collectables[newItem.name].count += newItem.count;
        }
        ChangeInventory();
    }

    public void RemoveFromInventory(Collectable item) {
        collectables.Remove(item.name);
        ChangeInventory();
    }

    public void PrintInventory() {
        foreach (var item in collectables.Keys) {
            Debug.Log($"{item}: {collectables[item].count}");
        }
    }
}
