using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class TradeItem {
    public InventoryItemSO item;
    public int count;
    public int requiredLevel;
    public int xp;
    public int effectAmount;
    public List<InventoryItem> requirements;

    public string name { get { return item.name; } } // TODO: find better way

    public TradeItem Copy() {
        return new TradeItem {
            item = this.item,
            count = this.count,
            requiredLevel = this.requiredLevel,
            xp = this.xp,
            effectAmount = this.effectAmount,
            requirements = requirements.ToList()
        };
    }
}
