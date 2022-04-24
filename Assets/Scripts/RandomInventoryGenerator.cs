using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomInventoryGenerator : MonoBehaviour {

    public GameObject merchantsParent;
    public InventoryItem globalGold;
    public List<InventoryItemSO> allItemsSO;
    public List<InventoryItem> allInventoryItems;
    public List<TradeItem> allTradeItems;

    private void OnEnable() {
        allItemsSO.ForEach(item => {
            // generate inventory items
            var newItem = new InventoryItem();
            newItem.item = item;
            newItem.count = Random.Range(1, 20);
            newItem.xp = Random.Range(1, 20);
            newItem.requiredLevel = Random.Range(0, 5);
            newItem.effectAmount = Random.Range(5, 20);
            allInventoryItems.Add(newItem);
        });

        allItemsSO.ForEach(item => {
            // generate inventory items
            // var newItem = new InventoryItem();
            // newItem.item = item;
            // newItem.count = Random.Range(1, 20);
            // newItem.xp = Random.Range(1, 20);
            // allInventoryItems.Add(newItem);
            var newItem = new TradeItem();
            newItem.item = item;
            newItem.count = Random.Range(1, 10);
            newItem.requiredLevel = Random.Range(0, 5);
            newItem.xp = Random.Range(5, 10);
            newItem.effectAmount = Random.Range(5, 20);
            newItem.requirements = new List<InventoryItem>();
            var gold = globalGold.Copy();
            gold.count = newItem.item.price;
            newItem.requirements.Add(gold);

            int reqCount = Random.Range(1, 4);
            for (int i = 0; i < 2 /*reqCount*/; i++) {
                var newReq = allInventoryItems[Random.Range(0, allInventoryItems.Count)].Copy();
                newReq.count = Random.Range(1, 10);
                newItem.requirements.Add(newReq);
            }
            allTradeItems.Add(newItem);
        });

        merchantsParent.GetComponentsInChildren<Inventory>().ToList().ForEach(i => {
            for (int j = 0; j < Random.Range(5, 10); j++) {
                var randIndex = Mathf.FloorToInt(Random.value * allTradeItems.Count);
                i.startItems.Add(allTradeItems[randIndex].Copy());
            }
        });
    }
}
