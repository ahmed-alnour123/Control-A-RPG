[System.Serializable]
public class InventoryItem {
    public InventoryItemSO item;

    [UnityEngine.SerializeField]
    private int count;
    public int Count { get { return count; } set { count = value; } }

    public string name { get { return item.name; } }

    public InventoryItem Copy() {
        return new InventoryItem {
            item = this.item,
            count = this.count
        };
    }
}

public enum CollectableType { Gold, Wood, Steel, HealthPotion, StaminaPotion }
