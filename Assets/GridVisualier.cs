using UnityEngine;

public class GridVisualier : MonoBehaviour {

    public int width, height;

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue * 0.4f;
        // Gizmos.DrawCube(transform.position, new Vector3(width, 0, height));
    }
}
