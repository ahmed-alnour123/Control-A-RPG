using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour {
    public static Vector3 MouseToTerrainPosition() {
        Vector3 position = Vector3.zero;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit info, 100, LayerMask.GetMask("Ground")))
            position = info.point;
        return position;
    }
    public static RaycastHit CameraRay() {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit info, 100))
            return info;
        return new RaycastHit();
    }
}

public static class Extenstions {
    public static Vector3 ToInt(this Vector3 v) {
        return new Vector3(
        Mathf.Round(v.x),
        Mathf.Round(v.y),
        Mathf.Round(v.z)
        );
    }
}
