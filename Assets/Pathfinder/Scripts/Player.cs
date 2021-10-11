using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    #region Singletone

    public static Player Instance { get; private set; }

    void Awake() {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }

    #endregion

    const float MAX_DIS = 1000f;
    const bool DEBUG = true;

    Camera cam;

    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask layerNode;
    [SerializeField] LayerMask layerPathFollower;
    [SerializeField] Pathfollower pathfollower;

    public Pathfollower Pathfollower => pathfollower;
    public (LayerMask Node, LayerMask Pathfollower) layers => (layerNode, layerPathFollower);

    void Start() {
        cam = Camera.main;

        Pathfollower.OnArrive += GetCurrentNode;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && !Pathfollower.IsTravelling)
            Select();

        if (Input.GetKeyDown(KeyCode.R))
            Restart();
    }

    void Select() {
        var hit = RaycastHitX.MousePosHit(cam, layerMask, MAX_DIS, DEBUG);

        // Return if null
        if (hit.collider == null) {
            return;
        }

        // Select pathfollower
        if (layerPathFollower == (layerPathFollower | (1 << hit.collider.gameObject.layer))) {
            pathfollower = hit.collider.gameObject.GetComponent<Pathfollower>();
            GetCurrentNode();
            Debug.Log("Pathfollower selected.", pathfollower.gameObject);
        }

        // Select end node
        if (layerNode == (layerNode | (1 << hit.collider.gameObject.layer))) {
            var endNode = hit.collider.gameObject.GetComponent<Node>();

            if (endNode.IsWalkable) {
                Pathfinder.Instance.EndNode = endNode;
                StartCoroutine(pathfollower.FollowPath());
                Debug.Log("End node selected.", endNode.gameObject);
            }
        }
    }

    void GetCurrentNode() {
        if (pathfollower)
            Pathfinder.Instance.StartNode = pathfollower.GetCurrentNode();
    }

    void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}