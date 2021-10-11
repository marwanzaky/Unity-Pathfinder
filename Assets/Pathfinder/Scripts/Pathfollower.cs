using UnityEngine;
using System.Collections;
using System;

public class Pathfollower : MonoBehaviour {
    Player Player => Player.Instance;

    public static Action OnArrive;
    public static bool IsTravelling { get; set; }

    [SerializeField] float speed = 3f;

    public IEnumerator FollowPath() {
        IsTravelling = true;

        var i = 0;
        var path = Pathfinder.Instance.FindPath();

        if (path.Length > 0) {
            while (i < path.Length) {
                transform.position = Vector3.MoveTowards(transform.position, NodePos(), speed * Time.deltaTime);

                if (transform.position == NodePos())
                    i++;

                yield return null;
            }

            IsTravelling = false;
            OnArrive?.Invoke();
        } else { Debug.LogError("Error: Cannot follow the path 'null'"); }

        Vector3 NodePos() => Vector3X.IgnoreY(path[i].transform.position, transform.position.y);
    }

    public Node GetCurrentNode() {
        const float MAX_DIS = 1f;
        const bool DEBUG = true;
        var hit = RaycastHitX.Cast(transform.position, Vector3.down, Player.layers.Node, MAX_DIS, DEBUG);

        if (hit.collider) {
            var node = hit.collider.GetComponent<Node>();
            Debug.LogWarning("Get current node", node.gameObject);
            return node;
        }

        Debug.LogWarning("Current node NOT found!");
        return null;
    }
}