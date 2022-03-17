using UnityEngine;


[CreateAssetMenu(fileName = "Collectable Item", menuName = "ScriptableObject/Collectable Item")]
public class CollectableSO : ScriptableObject {
    public new string name;
    public CollectableType type;
    public Texture icon;
    // public GameObject model;
}
