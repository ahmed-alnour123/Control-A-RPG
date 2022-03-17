using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootItem", menuName = "ScriptableObject/Loot Object")]
public class LootableObjectSO : ScriptableObject {
    public new string name;
    public int animationId;
    public float timeToLoot;
    public bool regenratable;
    public GameObject model;
    // public Texture icon;
}

