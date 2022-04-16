using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlacingManager : MonoBehaviour {

    public Transform UIButtonsParent;
    public GameObject buttonTemplate;
    public GameObject placingButton;
    public Sprite placingButtonSprite; // sprite when in placing mode
    public Sprite notPlacingButtonSprite; // sprite when not in placing mode
    public List<PlaceObject> placeObjects;
    public LayerMask placeLayer;

    private PlaceObject currentObject;
    private Grid grid;
    [HideInInspector]
    public bool isPlacing = false; // TODO: Add to gameManager


    private void Start() {
        grid = FindObjectOfType<Grid>();

        placeObjects.ForEach(p => CreateButton(p));
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            currentObject = null;
            isPlacing = false;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            UpdateRotation();
        }

        UpdateObjectVisuals();
    }

    /*
    1- instantaite Object
    2- move object with mouse
    3- make it trigger only
    */

    void UpdateObjectVisuals() {
        if (currentObject == null) return;
        // ...
        var pos = grid.GetPoint(Utility.MouseToTerrainPosition());

        currentObject.visual.transform.position = pos;

        var overlapTest = Physics.OverlapBox(
                pos,
                new Vector3(currentObject.width * 0.95f, 1, currentObject.height * 0.95f) / 2,
                Quaternion.Euler((currentObject.facingDirection == FacingDirection.Vertical ? 90 : 0) * Vector3.up),
                placeLayer
            );

        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (overlapTest.Length <= 0) {
                Instantiate(currentObject.prefab, pos, currentObject.transform.rotation);
            } else {
                var s = "";
                overlapTest.ToList().ForEach(c => s += c.name + "\n");
                Debug.Log($"Can't Place an Object Here there is {s}");
            }
        }
    }

    void UpdateRotation() {
        if (currentObject == null) return;
        // ...
        currentObject.prefab.transform.rotation *= Quaternion.Euler(0, 90, 0); // TODO: replace with global variable for instead of rotating the prefab
        currentObject.visual.transform.rotation = currentObject.prefab.transform.rotation;
    }

    public void CreateButton(PlaceObject placeObject) {
        var newButton = Instantiate(buttonTemplate);
        newButton.SetActive(true);
        newButton.transform.parent = UIButtonsParent;
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

    public void ChangeButtonSprite(bool placing) {
        if (placing) {
            placingButton.transform.Find("Icon").GetComponent<Image>().sprite = placingButtonSprite;
        } else {
            placingButton.transform.Find("Icon").GetComponent<Image>().sprite = notPlacingButtonSprite;
        }
    }

    private void OnDisable() {
        if (currentObject != null)
            currentObject.visual.SetActive(false);
        currentObject = null;
    }
}
