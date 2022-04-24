using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Inventory : MonoBehaviour {

    [HideInInspector]
    public int id;
    public bool forPlacables;

    /// <summary>this is for assigning data in inspector</summary>
    public List<TradeItem> startItems;

    public Dictionary<string, TradeItem> items = new Dictionary<string, TradeItem>();

    private PlayerXP playerXP;

    public int Gold {
        get {
            var goldName = GameManager.instance.goldInventoryItem.name;
            return items.ContainsKey(goldName) ? items[goldName].count : 0;
        }
    }

    private void Awake() {
        foreach (var item in startItems) {
            AddItem(item, item.count);
        }
    }

    private void Start() {
        playerXP = FindObjectOfType<PlayerXP>();
    }

    public void AddItem(TradeItem item, int count) {
        if (!ContainsItem(item)) {
            var newItem = item.Copy();
            newItem.count = count;
            items.Add(newItem.name, newItem);
        } else {
            SetCount(item, count);
        }
    }

    public void RemoveItem(TradeItem item, int count) {
        SetCount(item, -count);

        if (GetCount(item) <= 0) {
            items.Remove(item.name);
        }
    }

    /// <summary>
    /// Note: it add or decrases but doesn't set count
    /// </summary>
    public void SetCount(TradeItem item, int count) {
        if (!ContainsItem(item)) {
            Debug.LogError($"Can't find {item.name} in {name}");
            PrintInventory();
        }
        items[item.name].count += count;
    }

    public int GetCount(TradeItem item) {
        return GetItem(item).count;
    }

    /// <summary>return item count in the inventory or empty string if it doesn't exist</summary>
    public string TryGetCountStr(TradeItem item) {
        if (!ContainsItem(item)) return "";
        return GetItem(item).count + "";
    }

    private TradeItem GetItem(TradeItem item) {
        return items[item.name];
    }

    public bool ContainsItem(TradeItem item) {
        return items.ContainsKey(item.name);
    }

    public bool CanBuy(TradeItem item, Inventory other) {
        var theItem = GetItem(item);
        var canbuy = true;
        foreach (var reqItem in theItem.requirements) {
            // I'm using InventoryItem's type casting here for req
            TradeItem req = reqItem;
            if (!(other.ContainsItem(req) && other.GetItem(req).count >= req.count)) {
                canbuy = false;
                break;
            }
        }
        return canbuy && playerXP.level >= item.requiredLevel;
    }

    public void PrintInventory() {
        var s = "";
        foreach (var item in items.Keys) {
            s += $"{gameObject.name}--> {item}: {GetCount(items[item])}\n";
        }
        Debug.Log(s);
    }
}
