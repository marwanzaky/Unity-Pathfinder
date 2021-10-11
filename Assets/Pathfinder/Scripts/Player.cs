using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    const float MAX_DIS = 1000f;
    const bool DEBUG = true;

    Camera cam;

    [SerializeField] LayerMask nodeMask;
    [SerializeField] Pathfollower pathfollower;

    void Start() {
        cam = Camera.main;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0))
            SelectEndNode();

        if (Input.GetKeyDown(KeyCode.R))
            Restart();
    }

    void SelectEndNode() {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        var hit = RaycastHitX.Cast(ray.origin, ray.direction, nodeMask, MAX_DIS, DEBUG);

        if (hit.collider) {
            var node = hit.collider.GetComponent<Node>();
            var path = Pathfinder.Instance.FindPath(node);
            pathfollower.FollowPath(path);
        } else { Debug.LogWarning("We didn't find any node at this mouse position"); }
    }

    void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}