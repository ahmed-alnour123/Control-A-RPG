using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class TradeItem {
    public InventoryItemSO item;

    public List<ItemRequirement> requirements;
}

[Serializable]
public class ItemRequirement {
    public InventoryItemSO item;
    public int count;
}
