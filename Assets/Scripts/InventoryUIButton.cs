using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryUIButton : MonoBehaviour {

    public TradeItem item;

    public void Init(UnityAction action) {
        GetComponentInChildren<Button>().onClick.AddListener(action);
    }


    void Update() {

    }
}
