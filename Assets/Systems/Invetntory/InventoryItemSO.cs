using UnityEngine;


[CreateAssetMenu(fileName = "Collectable Item", menuName = "ScriptableObject/Collectable Item")]
public class InventoryItemSO : ScriptableObject {
    public new string name;
    public string description;
    public CollectableType type;
    public Texture icon;
    // public GameObject model;
}
