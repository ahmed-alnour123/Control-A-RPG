using UnityEngine;


[CreateAssetMenu(fileName = "Collectable Item", menuName = "ScriptableObject/Collectable Item")]
public class InventoryItemSO : ScriptableObject {
    public new string name;
    // [Multiline]
    // public string description;
    public CollectableType type;
    public Sprite icon;
    // public GameObject model;
}
