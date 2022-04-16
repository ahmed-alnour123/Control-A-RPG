using System.Collections.Generic;
using System.Linq;


[System.Serializable]
public class InventoryItem {
    public InventoryItemSO item;
    public int count;
    public int price;
    public int requiredLevel;
    public int xp;

    public string name { get { return item.name; } }

    public InventoryItem Copy() {
        return new InventoryItem {
            item = this.item,
            count = this.count,
            price = this.price,
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
            requirements = reqList
        };
    }
}

public enum CollectableType { Gold, Wood, Steel, HealthPotion, StaminaPotion }
