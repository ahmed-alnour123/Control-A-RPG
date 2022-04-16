using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public InventoryItem goldInventoryItem;

    [Space()]
    public GameObject placingManagerGamaObject;
    public GameObject tradingCanvas;
    public GameObject placingCanvas;
    public GameObject InGameCanvas;

    // Game Status variables
    [HideInInspector]
    public bool isTrading { get; private set; }
    [HideInInspector]
    public bool isSleeping { get; private set; }
    [HideInInspector]
    public bool isPlacing { get; private set; }

    private PlacingManager placingManager;
    private PlayerXP playerXP;

    public void SetTrading(bool trading) {
        isTrading = trading;
        tradingCanvas.SetActive(isTrading);
        InGameCanvas.SetActive(!isTrading);
    }

    public void SetPlacingMode(bool isPlacing) {
        this.isPlacing = isPlacing;
        placingManagerGamaObject.SetActive(isPlacing);
        placingCanvas.SetActive(isPlacing);
        placingManager.ChangeButtonSprite(isPlacing);
    }

    void Awake() {
        instance = this;
        placingCanvas.SetActive(false); // fixes a glitch, if you started and it's not active it'll show nothing when activating it
    }

    void Start() {
        playerXP = FindObjectOfType<PlayerXP>();
        placingManager = FindObjectOfType<PlacingManager>(true);
    }


    void Update() {
        if (Input.GetKeyDown(KeyCode.Y)) {
            SetPlacingMode(!isPlacing);
        }
    }

    public void Sleep() {
        if (isSleeping) return;
        isSleeping = true;
        // change time [day/night]
        playerXP.ChangeEnergy(playerXP.level * 20); // TODO: use a formual to calculate it
        playerXP.transform.position = Vector3.zero;
        isSleeping = false;
    }

    public void TogglePlacing() {
        SetPlacingMode(!isPlacing);
    }
}
