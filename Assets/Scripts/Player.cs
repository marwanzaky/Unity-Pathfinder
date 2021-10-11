using UnityEngine;

public class Player : MonoBehaviour {
    Camera cam;

    [SerializeField] LayerMask nodeMask;
    [SerializeField] PathFollower follower;

    void Start() {
        cam = Camera.main;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0))
            SelectEndNode();
    }

    void SelectEndNode() {
        const float MAX_DIS = 1000f;
        const bool DEBUG = true;

        var ray = cam.ScreenPointToRay(Input.mousePosition);
        var hit = RaycastHitX.Cast(ray.origin, ray.direction, nodeMask, MAX_DIS, DEBUG);

        if (hit.collider) {
            var node = hit.collider.GetComponent<Node>();
            var path = Pathfinder.Instance.FindPath(node);
            follower.FollowPath(path);
        } else { Debug.LogWarning("We didn't find any node at this mouse position"); }
    }
}