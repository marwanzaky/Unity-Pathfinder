using UnityEngine;

public static class Vector3X {
    public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max) {
        var result = Vector3.zero;

        result.x = Mathf.Clamp(value.x, min.x, max.x);
        result.y = Mathf.Clamp(value.y, min.y, max.y);
        result.z = Mathf.Clamp(value.z, min.z, max.z);

        return result;
    }

    public static Vector3 IgnoreY(Vector3 value, float y = 0) {
        return new Vector3(value.x, y, value.z);
    }

    public static Vector3 IgnoreX(Vector3 value, float y = 0) {
        return new Vector3(value.x, y, value.z);
    }
}