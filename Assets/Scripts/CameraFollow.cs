using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform followObject;

    private Vector3 offset;

    void Start() {
        offset = followObject.position - transform.position;
    }


    void LateUpdate() {
        transform.position = followObject.position - offset;
    }
}
