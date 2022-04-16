// TODO: close UI when finishing
using UnityEngine;

public class TradingManager : MonoBehaviour {
    public static TradingManager instance;

    private Inventory playerInventory;
    private Inventory otherInventory;

    private GameManager gameManager;
    private TradingUIManager tradingUIManager;
    private PlayerXP playerXP;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        gameManager = GameManager.instance;
        tradingUIManager = TradingUIManager.instance;
        playerInventory = FindObjectOfType<PlayerInteraction>().GetComponent<Inventory>();
        playerXP = FindObjectOfType<PlayerXP>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            Print();
        }
    }

    public void StartTrade(Inventory inventory, bool buy = true) {
        if (gameManager.isTrading || gameManager.isPlacing) return;

        otherInventory = inventory;
        gameManager.SetTrading(true);
        // set up UI
        tradingUIManager.CreateUI(playerInventory, inventory, buy);
    }

    public void EndTrade() {

        if (!gameManager.isTrading) return;

        otherInventory = null;
        gameManager.SetTrading(false); // gamemanager will close the UI
    }

    public void Buy(TradeItem item) {
        foreach (var reqItem in item.requirements) {
            TradeItem req = reqItem;
            playerInventory.RemoveItem(req, req.count);
            otherInventory.AddItem(req, req.count);
        }
        playerInventory.AddItem(item, 1);
        otherInventory.RemoveItem(item, 1);
        Debug.Log(item.xp + " " + item.name);
        playerXP.AddXP(item.xp);
        // UpdateUI()
        Debug.Log("Buying Done");
        Print();
    }

    public void Sell(TradeItem item) {
        foreach (var reqItem in item.requirements) {
            TradeItem req = reqItem;
            playerInventory.AddItem(req, req.count);
            otherInventory.RemoveItem(req, req.count);
        }
        playerInventory.RemoveItem(item, 1);
        otherInventory.AddItem(item, 1);
        Debug.Log("Selling Done");
        Print();
        // UpdateUI()
    }

    void Print() {
        playerInventory.PrintInventory();
        // otherInventory.PrintInventory();
    }
}
