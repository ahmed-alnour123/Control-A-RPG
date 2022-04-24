using System.Collections.Generic;


[System.Serializable]
public class InventoryItem {
    public InventoryItemSO item;
    public int count;
    public int requiredLevel;
    public int xp;
    public int effectAmount;


    public string name { get { return item.name; } }

    private int price { get { return item.price; } }

    public InventoryItem Copy() {
        return new InventoryItem {
            item = this.item,
            count = this.count,
            // price = this.price,
            effectAmount = this.effectAmount,
            requiredLevel = this.requiredLevel,
            xp = this.xp
        };
    }

    public static implicit operator TradeItem(InventoryItem inventoryItem) {
        var goldInventoryItem = GameManager.instance.goldInventoryItem.Copy();
        goldInventoryItem.count = inventoryItem.price;
        var reqList = new List<InventoryItem> { goldInventoryItem };

        return new TradeItem {
            item = inventoryItem.item,
            count = inventoryItem.count,
            requiredLevel = inventoryItem.requiredLevel,
            xp = inventoryItem.xp,
            effectAmount = inventoryItem.effectAmount,
            requirements = reqList
        };
    }
}

public enum CollectableType {
    None,
    Gold,
    HealthPotion,
    EnergyPotion,
    Food,
    Weapon,
    Building,
    Plant,
    Trap
}
