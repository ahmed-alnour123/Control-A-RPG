using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class CraftItem {
    public CollectableSO result;

    public List<CraftingIngredient> ingredients;
}

[Serializable]
public class CraftingIngredient {
    public CollectableSO item;
    public int count;
}
