using UnityEngine;
using UnityEngine.UI;

public class UIButtonData : MonoBehaviour {

    public TradeItem item;
    public bool canBuy;

    private Button button;
    private TradingUIManager uiManager;

    private void Start() {
        uiManager = TradingUIManager.instance;
        button = GetComponentInChildren<Button>();

        button.onClick.AddListener(() => {
            uiManager.SetSelectedView(item, canBuy);
        });
    }
}
