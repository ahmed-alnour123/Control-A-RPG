[System.Serializable]
public class Collectable {
    public CollectableSO item;
    public int count;

    public string name { get { return item.name; } }
}

public enum CollectableType { Gold, Wood, Steel, HealthPotion, StaminaPotion }
