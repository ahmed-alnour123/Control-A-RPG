using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    /// <summary>this is for assigning data in inspector</summary>
    public List<InventoryItem> startItems;

    public Dictionary<string, InventoryItem> items = new Dictionary<string, InventoryItem>();

    private void Start() {
        foreach (var item in startItems) {
            AddItem(item, item.Count);
        };
    }

    public void AddItem(InventoryItem item, int count) {
        if (!ContainsItem(item)) {
            var newItem = item.Copy();
            newItem.Count = count;
            items.Add(item.name, newItem);
        } else {
            SetCount(item, count);
        }
    }

    public void RemoveItem(InventoryItem item, int count) {
        SetCount(item, -count);

        if (GetCount(item) <= 0) {
            items.Remove(item.name);
        }
    }

    /// <summary>
    /// Note: it add or decrases but doesn't set count
    /// </summary>
    public void SetCount(InventoryItem item, int count) {
        items[item.name].Count += count;
    }

    public int GetCount(InventoryItem item) {
        return GetItem(item).Count;
    }

    private InventoryItem GetItem(InventoryItem item) {
        return items[item.name];
    }

    public bool ContainsItem(InventoryItem item) {
        return items.ContainsKey(item.name);
    }

    public void PrintInventory() {
        var s = "";
        foreach (var item in items.Keys) {
            s += $"{gameObject.name}--> {item}: {GetCount(items[item])}\n";
        }
        Debug.Log(s);
    }
}
