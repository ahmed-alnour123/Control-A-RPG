using UnityEngine;

public class Grid : MonoBehaviour {

    public int size;

    public Vector3 GetPoint(Vector3 position) {
        if (size == 0) size = 1;
        position -= transform.position;
        var result = new Vector3(
            Mathf.RoundToInt(position.x / size) * size,
            Mathf.RoundToInt(position.y / size) * size,
            Mathf.RoundToInt(position.z / size) * size
        );
        result += transform.position;
        return result;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        for (var i = -10f; i < 10; i += size) {
            for (var j = -10f; j < 10; j += size) {
                Gizmos.DrawSphere(GetPoint(new Vector3(i, 0, j)), 0.1f);
            }
        }
    }
}
