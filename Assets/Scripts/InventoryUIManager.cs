using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour {
    public static InventoryUIManager instance;

    public Text goldText;
    public Transform uiParent;
    public GameObject template;
    public GameObject currentItemButton;

    InventoryUIButton selectedBtn;
    Inventory playerInventory;
    GameManager gameManager;
    PlayerXP playerXP;
    TradeItem currentItem;

    private void Awake() {
        instance = this;
        playerXP = FindObjectOfType<PlayerXP>();
        playerXP.testFunc = UseItem;
        playerInventory = playerXP.GetComponent<Inventory>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.O)) {
            UseItem();
        }
    }

    public void SelectItem() {
        if (selectedBtn == null) return;
        SetCurrentItem(selectedBtn.item);
    }

    public void UseItem() {
        switch (currentItem.item.type) {
            case CollectableType.Food:
            case CollectableType.Plant:
            case CollectableType.HealthPotion:
                DropItem(1);
                playerXP.ChangeHealth(currentItem.effectAmount);
                break;
            case CollectableType.EnergyPotion:
                DropItem(1);
                playerXP.ChangeEnergy(currentItem.effectAmount);
                break;
            case CollectableType.Trap:
                break;
            default:
                return;
        }
    }

    public void DropItem(int count) {
        if (selectedBtn == null) return;
        playerInventory.RemoveItem(selectedBtn.item, count);
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void DropItem() {
        DropItem(selectedBtn.item.count);
    }

    public void SetSelectedButton(InventoryUIButton btn) {
        selectedBtn = btn;
    }

    private void SetCurrentItem(TradeItem item) {
        currentItem = item;
        currentItemButton.transform.Find("Icon").GetComponent<Image>().sprite = item.item.icon;
    }

    private void OnEnable() {
        goldText.text = "" + playerInventory.Gold;

        //  it's like this because template is the first child
        // destroy existing list items
        for (int i = 1; i < uiParent.childCount; i++) {
            Destroy(uiParent.GetChild(i).gameObject);
        }

        foreach (var item in playerInventory.items.Values) {
            if (item.item.type == CollectableType.Gold) continue;
            var menu = Instantiate(template.transform, uiParent);
            menu.gameObject.SetActive(true);
            var name = menu.Find("ItemSlot/Name").GetComponent<Text>();
            var icon = menu.Find("ItemSlot/Icon/Icon").GetComponent<Image>();
            var playerItemCount = menu.Find("ItemSlot/Icon/Count").GetComponent<Text>();
            var btn = menu.GetComponent<InventoryUIButton>();

            name.text = item.name;
            icon.sprite = item.item.icon;
            playerItemCount.text = playerInventory.TryGetCountStr(item);
            btn.item = item;
            btn.Init(() => { selectedBtn = btn; });

            menu.SetParent(uiParent);
        }
    }
}
