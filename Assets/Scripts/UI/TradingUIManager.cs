// Fix: button is not working, make it work or make a child button to appear on top of 'em all
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TradingUIManager : MonoBehaviour {

    public static TradingUIManager instance;

    [Header("Trading")]
    public GameObject listItemTemplate;
    public GameObject requiredItemTemplate;

    public Transform selectedView;
    public Transform itemsListView;

    public Button OKButton;
    public Text playerGold;

    private TradingManager tradingManager;
    private GameManager gameManager;
    private PlayerXP playerXP;
    private Transform requirementsParent;
    private bool isBuying;
    private Inventory buyerInventory;
    private Inventory sellerInventory;
    private TradeItem lastItem;

    private void Awake() {
        instance = this;
    }

    void Start() {
        tradingManager = TradingManager.instance;
        gameManager = GameManager.instance;
        playerXP = FindObjectOfType<PlayerXP>();
        requirementsParent = selectedView.Find("Bounds/Requirements/List");
    }

    public void CreateItemsListView() {
        if (sellerInventory.items.Keys.Where((e) => e != gameManager.goldInventoryItem.name).Count() == 0) tradingManager.EndTrade();

        // TODO: change to i=0, it's like this because template is the first child
        // destroy existing list items
        for (int i = 1; i < itemsListView.childCount; i++) {
            Destroy(itemsListView.GetChild(i).gameObject);
        }

        // this is for lastItem
        var foundItem = false;

        playerGold.text = buyerInventory.Gold + "";

        // I used Values becauese Keys are just strings
        foreach (var item in sellerInventory.items.Values) {
            // don't show gold on the list
            if (item.item.type == CollectableType.Gold) continue;

            var menu = Instantiate(listItemTemplate.transform);
            var canBuy = sellerInventory.CanBuy(item, buyerInventory);

            menu.gameObject.SetActive(true); // TODO: remove and replace with prefab

            var name = menu.Find("ItemSlot/Name").GetComponent<Text>();
            var icon = menu.Find("ItemSlot/Icon/Icon").GetComponent<Image>();
            var count = menu.Find("ItemSlot/Count").GetComponent<Text>();
            var playerItemCount = menu.Find("ItemSlot/Icon/Count").GetComponent<Text>();
            var data = menu.GetComponent<UIButtonData>();

            name.text = item.name;
            icon.sprite = item.item.icon;
            count.text = item.count + "";
            playerItemCount.text = buyerInventory.TryGetCountStr(item);
            data.item = item;
            data.canBuy = canBuy;

            menu.SetParent(itemsListView);
            // menu.color = (canBuy)? white:red;

            // if last bought item is still exist, show it
            if (lastItem != null && item.name == lastItem.name) {
                SetSelectedView(item, canBuy);
                foundItem = true;
            } else if (!foundItem) {
                selectedView.gameObject.SetActive(false);
            }
        }

        if (!foundItem) {
            selectedView.gameObject.SetActive(false);
            OKButton.interactable = false;
        }
    }

    public void SetSelectedView(TradeItem item, bool canBuy) {
        selectedView.gameObject.SetActive(true);

        var name = selectedView.Find("Name").GetComponent<Text>();
        var icon = selectedView.Find("Icon/Icon").GetComponent<Image>();
        var count = selectedView.Find("Count").GetComponent<Text>();
        var level = selectedView.Find("Level").GetComponent<Text>();

        name.text = item.name;
        icon.sprite = item.item.icon;
        count.text = item.count + "";
        level.text = playerXP.level < item.requiredLevel ? "Required Level: " + item.requiredLevel : "";

        OKButton.GetComponentInChildren<Text>().text = isBuying ? "Sell" : "Buy";
        OKButton.interactable = canBuy;
        OKButton.onClick.RemoveAllListeners();
        OKButton.onClick.AddListener(() => {
            Debug.Log("I am a button");
            // I made a mistake it should be isSelling
            lastItem = item;
            if (isBuying)
                tradingManager.Sell(item);
            else
                tradingManager.Buy(item);
            UpdateData();
        });

        // TODO: change to i=0, it's like this because template is the first child
        // remove existing list items
        for (int i = 1; i < requirementsParent.childCount; i++) {
            Destroy(requirementsParent.GetChild(i).gameObject);
        }

        foreach (var req in item.requirements) {
            var menu = Instantiate(requiredItemTemplate.transform);

            menu.gameObject.SetActive(true); // TODO: remove and replace with prefab

            var reqName = menu.Find("Name").GetComponent<Text>();
            var reqIcon = menu.Find("Icon/Icon").GetComponent<Image>();
            var reqCount = menu.Find("Count").GetComponent<Text>();
            var reqPlayerItemCount = menu.Find("Icon/Icon/Count").GetComponent<Text>();

            reqName.text = req.name;
            reqIcon.sprite = req.item.icon;
            reqCount.text = "" + req.count;
            // reqPlayerItemCount.text = buyerInventory.TryGetCountStr(req);
            reqPlayerItemCount.text = req.item.type == CollectableType.Gold ? "" : buyerInventory.TryGetCountStr(req);

            menu.SetParent(requirementsParent);
        }
    }

    void UpdateData() {
        CreateItemsListView();
    }

    public void CreateUI(Inventory buyerInventory, Inventory sellerInventory, bool isBuying = true) {
        this.buyerInventory = buyerInventory;
        this.sellerInventory = sellerInventory;
        this.isBuying = isBuying;

        CreateItemsListView();
    }
}

/*
the workflow
player start trading
read two inventories data
set ui to the data

what to do?
extract data
make Set*View() assume that there is already objects
make UpdateData()
*/
