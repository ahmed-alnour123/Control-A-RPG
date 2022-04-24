using UnityEngine;

public class DeletingManager : MonoBehaviour {

    private PlayerXP playerXP;
    private Inventory playerInventory;

    private void Start() {
        playerXP = FindObjectOfType<PlayerXP>();
        playerInventory = playerXP.GetComponent<Inventory>();
    }

    void Update() {
        if (GameManager.instance.isDeleting)
            CheckDelete();
    }

    private void CheckDelete() {
        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit info, 100, LayerMask.GetMask("Level"))) {
                var placableComponent = info.transform.GetComponentInParent<PlacableHolder>();
                if (placableComponent != null) {
                    Debug.Log(" Hitted: " + placableComponent.placeObject.tradeItem.item.type);
                    var gold = GameManager.instance.goldInventoryItem.Copy();
                    gold.count = Mathf.FloorToInt(placableComponent.placeObject.tradeItem.requirements[0].count * 0.75f);
                    playerInventory.AddItem(gold, gold.count);
                    Destroy(placableComponent.gameObject);
                }
            }
        }
    }

}
