using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlaceObject {
    public int width;
    public int height;

    [Space()]
    public GameObject prefab;
    public Sprite icon;

    [HideInInspector]
    public Transform transform { get { return prefab.transform; } }
    [HideInInspector]
    public FacingDirection facingDirection { get { return transform.rotation.eulerAngles.y % 180 == 0 ? FacingDirection.Horizontal : FacingDirection.Vertical; } }

    private GameObject _visual;

    public GameObject visual {
        get {
            if (_visual == null) {
                _visual = GameObject.Instantiate(prefab);
                _visual.name = "Visual " + _visual.name;
                _visual.GetComponents<Collider>().ToList().ForEach(c => c.enabled = false);
                _visual.GetComponentsInChildren<Collider>().ToList().ForEach(c => c.enabled = false);
                // _visual.GetComponent<Renderer>().material.color = Color.blue * 0.5f;
                // TODO: change _visual material
                _visual.SetActive(false);
            }
            return _visual;
        }
    }
}

public enum FacingDirection { Horizontal, Vertical }

