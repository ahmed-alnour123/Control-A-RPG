// to use this script just add the ground to Level Layer and assign selectionArea
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggingManager : MonoBehaviour {
    public static DraggingManager instance;

    [SerializeField]
    private LayerMask dragLayer = default;

    private bool isDragging; // TODO: add to gameManager
    private Camera mainCamera;
    private Vector3 startDrag;
    private Vector3 endDrag;
    private Vector3 dragCenter;
    private Vector3 dragSize;
    private Transform selectionArea = default;
    private Grid grid;
    private PlacingManager placingManager;

    private void Awake() {
        instance = this;
    }

    void Start() {
        mainCamera = Camera.main;
        grid = FindObjectOfType<Grid>();
        placingManager = FindObjectOfType<PlacingManager>();

        selectionArea = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        selectionArea.name = "selectionArea";
        selectionArea.gameObject.SetActive(false);
    }

    void Update() {
        if (placingManager.isPlacing) return;

        if (Input.GetMouseButtonDown(0)) {
            startDrag = Utility.MouseToTerrainPosition();
            endDrag = startDrag;
        } else if (Input.GetMouseButton(0)) {
            endDrag = Utility.MouseToTerrainPosition();

            if (Vector3.Distance(startDrag, endDrag) > 1) {
                selectionArea.gameObject.SetActive(true);
                isDragging = true;
                startDrag = grid.GetPoint(startDrag);
                endDrag = grid.GetPoint(endDrag);
                dragCenter = ((startDrag + endDrag) / 2);
                dragSize = (endDrag - startDrag);
                selectionArea.transform.position = dragCenter;
                selectionArea.transform.localScale = dragSize + Vector3.up;
            }
        } else if (Input.GetMouseButtonUp(0)) {
            if (isDragging) {
                HandleSelectedObjects(InteractionType.Destroy);
                isDragging = false;
                selectionArea.gameObject.SetActive(false);
            } else {
            }
        }
    }

    void HandleSelectedObjects(InteractionType interactionType) {
        dragSize.Set(Mathf.Abs(dragSize.x / 2), 1, Mathf.Abs(dragSize.z / 2));
        RaycastHit[] hits = Physics.BoxCastAll(dragCenter, dragSize, Vector3.up, Quaternion.identity, 0);
        Debug.LogWarning(hits.Length);
        switch (interactionType) {
            case InteractionType.Plant:
                for (int i = -((int)dragSize.x); i < dragSize.x; i++) {
                    for (int j = -((int)dragSize.z); j < dragSize.z; j++) {
                        // instantiate plants or anything
                    }
                }
                break;
            case InteractionType.Destroy:
                foreach (RaycastHit hit in hits) {
                    if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Level")) continue;
                    Destroy(hit.collider.transform.parent.gameObject);
                }
                break;
        }
    }
}

public enum InteractionType {
    Plant, // check for rect size != 0
    Destroy
}

