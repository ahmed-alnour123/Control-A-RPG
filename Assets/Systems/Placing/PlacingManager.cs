using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlacingManager : MonoBehaviour {

    public Transform buildingsUIButtonsParent;
    public Transform plantsUIButtonsParent;
    public GameObject buttonTemplate;
    public GameObject placingButton;
    public Sprite placingButtonSprite; // sprite when in placing mode
    public Sprite notPlacingButtonSprite; // sprite when not in placing mode
    public GameObject buildingsUIButton;
    public GameObject plantsUIButton;
    public GameObject placablesContainer;

    [Space()]
    public List<PlaceObject> startBuildings;
    public List<PlaceObject> startPlants;
    public Inventory buildingsMerchant;
    public Inventory plantsMerchant;
    [HideInInspector]
    public List<PlaceObject> placeObjects;
    public LayerMask placeLayer;

    [HideInInspector]
    public bool lastIsBuilding;

    private PlaceObject currentObject;
    private Grid grid;
    private Dictionary<PlaceObject, int> buildings = new Dictionary<PlaceObject, int>();
    private Dictionary<PlaceObject, int> plants = new Dictionary<PlaceObject, int>();
    [HideInInspector]
    public bool isPlacing = false; // TODO: Add to gameManager
    private Matrix4x4 matrix; // ! DEBUG

    public int buildingsListCount { get { return buildings.Keys.Where(b => buildings[b] > 0).Count(); } }
    public int plantsListCount { get { return plants.Keys.Where(p => plants[p] > 0).Count(); } }

    // bool isInitialized;
    // private void OnEnable() {
    //     if (!isInitialized) {
    //         isInitialized = true;
    //         return;
    //     }
    //     if (!RefreshButtons())
    //         GameManager.instance.SetPlacingMode(false);
    // }

    private void Start() {
        grid = FindObjectOfType<Grid>();

        placeObjects.AddRange(startBuildings);
        placeObjects.AddRange(startPlants);

        startBuildings.ForEach(b => buildingsMerchant.AddItem(b.tradeItem, b.tradeItem.count));
        startPlants.ForEach(p => plantsMerchant.AddItem(p.tradeItem, p.tradeItem.count));

        // placeObjects.ForEach(p => CreateButton(p));
        buildings.Keys.Where(b => buildings[b] > 0).ToList().ForEach(b => CreateButton(b));
        plants.Keys.Where(p => buildings[p] > 0).ToList().ForEach(p => CreateButton(p));

        gameObject.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            currentObject.visual.SetActive(false);
            currentObject = null;
            isPlacing = false;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            UpdateRotation();
        }

        UpdateObjectVisuals();

    }

    void PlacePrefab(Vector3 pos, Collider[] overlapTest) {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (overlapTest.Length <= 0) {
            Instantiate(currentObject.prefab, pos, currentObject.transform.rotation, placablesContainer.transform).GetComponent<PlacableHolder>().placeObject = currentObject;

            switch (currentObject.tradeItem.item.type) {
                case CollectableType.Building:
                    buildings[currentObject] -= 1;
                    if (buildings[currentObject] <= 0) {
                        Debug.Log("Removing " + buildings.Remove(currentObject));
                        HideVisual();
                        currentObject = null;
                    }
                    break;
                case CollectableType.Plant:
                    plants[currentObject] -= 1;
                    if (plants[currentObject] <= 0) {
                        Debug.Log("Removing " + plants.Remove(currentObject));
                        HideVisual();
                        currentObject = null;
                    }
                    break;
            }
            if (!RefreshButtons()) {
                HideVisual();
                GameManager.instance.SetPlacingMode(false);
            }
        } else {
            var s = "";
            overlapTest.ToList().ForEach(c => s += c.name + "\n");
            Debug.Log($"Can't Place an Object Here there is {s}");
        }
    }

    void UpdateObjectVisuals() {
        if (currentObject == null) return;

        var pos = grid.GetPoint(Utility.MouseToTerrainPosition());

        currentObject.visual.transform.position = pos;

        var overlapTest = Physics.OverlapBox(
                pos,
                new Vector3(currentObject.width * 0.95f, 1, currentObject.height * 0.95f) / 2,
                Quaternion.Euler((currentObject.facingDirection == FacingDirection.Vertical ? 90 : 0) * Vector3.up),
                placeLayer
            );

        matrix = Matrix4x4.TRS(pos, currentObject.transform.rotation, new Vector3(currentObject.width, 1, currentObject.height)); // ! DEBUG
        if (Input.GetMouseButtonDown(0) || (currentObject.isContinous && Input.GetMouseButton(0))) {
            PlacePrefab(pos, overlapTest);
        }
    }

    void UpdateRotation() {
        if (currentObject == null) return;
        currentObject.prefab.transform.rotation *= Quaternion.Euler(0, 90, 0); // TODO: replace with global variable for instead of rotating the prefab
        currentObject.visual.transform.rotation = currentObject.prefab.transform.rotation;
    }

    private void CreateButton(PlaceObject placeObject) {
        var newButton = Instantiate(buttonTemplate);
        newButton.SetActive(true);
        newButton.transform.parent = placeObject.tradeItem.item.type == CollectableType.Building ? buildingsUIButtonsParent : plantsUIButtonsParent;
        var image = newButton.transform.Find("Icon").GetComponent<Image>();
        image.sprite = placeObject.icon;

        var btn = newButton.GetComponentInChildren<Button>();
        btn.onClick.AddListener(() => {
            if (currentObject != null) {
                currentObject.visual.SetActive(false);
            }
            currentObject = placeObject;
            currentObject.visual.SetActive(true);
            isPlacing = true;
        });
    }

    private void ClearButtons() {
        for (int i = 1; i < buildingsUIButtonsParent.childCount; i++) {
            Destroy(buildingsUIButtonsParent.GetChild(i).gameObject);
        }

        for (int i = 1; i < plantsUIButtonsParent.childCount; i++) {
            Destroy(plantsUIButtonsParent.GetChild(i).gameObject);
        }
    }

    public bool RefreshButtons() {
        ClearButtons();

        var _buildingsList = buildings.Keys.Where(b => buildings[b] > 0).ToList();
        if (_buildingsList.Count > 0) {
            _buildingsList.ForEach(b => CreateButton(b));
        }

        var _plantsList = plants.Keys.Where(p => plants[p] > 0).ToList();
        if (_plantsList.Count > 0) {
            _plantsList.ForEach(p => CreateButton(p));
        }

        return (lastIsBuilding && _buildingsList.Count > 0) || (!lastIsBuilding && _plantsList.Count > 0);
    }

    public void ChangeButtonSprite(bool placing) {
        return; // TODO: change sprite, because cross is taken
        if (placing) {
            placingButton.transform.Find("Icon").GetComponent<Image>().sprite = placingButtonSprite;
        } else {
            placingButton.transform.Find("Icon").GetComponent<Image>().sprite = notPlacingButtonSprite;
        }
    }

    public void AddPlacable(TradeItem item) {
        PlaceObject placable;
        switch (item.item.type) {
            case CollectableType.Building:
                // TODO: find a way to store data and get the right placable
                placable = startBuildings.Find(b => b.tradeItem.name == item.name);
                if (buildings.ContainsKey(placable)) {
                    buildings[placable] += 1;
                } else {
                    buildings.Add(placable, 1);
                }
                break;
            case CollectableType.Plant:
                placable = startPlants.Find(p => p.tradeItem.name == item.name);
                if (plants.ContainsKey(placable)) {
                    plants[placable] += 1;
                } else {
                    plants.Add(placable, 1);
                }
                break;
            default:
                break;
        }
    }

    // used for UI buttons
    public void SetLastIsBuilding(bool value) {
        lastIsBuilding = value;
    }

    private void OnDisable() {
        HideVisual();
    }

    private void HideVisual() {
        if (currentObject != null)
            currentObject.visual.SetActive(false);
        currentObject = null;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow * 0.2f;
        Gizmos.matrix = matrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
