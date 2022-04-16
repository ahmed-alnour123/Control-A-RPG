using UnityEngine;

[RequireComponent(typeof(InteractionEventRegisterer), typeof(Inventory))]
public class Tradable : MonoBehaviour, IInteractable {

    private TradingManager tradingManager;
    private Inventory inventory;
    private GameManager gameManager;

    void Start() {
        tradingManager = TradingManager.instance;
        gameManager = GameManager.instance;
        inventory = GetComponent<Inventory>();
    }


    void Update() {

    }

    public void OnPlayerInteraction() {
        if (gameManager.isTrading) return;
        tradingManager.StartTrade(inventory, false);
    }
}
