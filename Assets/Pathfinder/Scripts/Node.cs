using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Node : MonoBehaviour {
    Pathfinder Pathfinder => Pathfinder.Instance;
    Player Player => Player.Instance;

    const float NODE_SIZE = 1;

    float gCost = 0;  // distance from starting node
    float hCost = 0;  // distance from end node
    float fCost = 0;  // g_cost + f_cost

    [HideInInspector] public Collider col;

    [SerializeField] LayerMask layerObstacles;
    [SerializeField] LayerMask layerNode;
    [SerializeField] TextMeshPro debugText;

    public Vector3 Pos => transform.position;

    void Start() {
        col = GetComponent<Collider>();

        Pathfollower.OnArrive += SetDefault;
    }

    void OnDisable() {
        Pathfollower.OnArrive -= SetDefault;
    }

    public bool IsWalkable {
        get {
            const float MAX_DIS = 1f;
            const bool DEBUG = true;
            var hit = RaycastHitX.Cast(transform.position, Vector3.up, layerObstacles, MAX_DIS, DEBUG);
            var res = hit.collider == null;
            Debug.Log("This node is " + (res ? "walkable" : "NOT walk able"), gameObject);
            return res;
        }
    }

    public void Calc() {
        var nodesPos = (startNode: Pathfinder.StartNode.Pos,
                        endNode: Pathfinder.EndNode.Pos);

        gCost = Vector3.Distance(nodesPos.startNode, transform.position);
        hCost = Vector3.Distance(nodesPos.endNode, transform.position);
        fCost = gCost + hCost;

        if (debugText) {
            debugText.text = $"{gCost.ToString("0.0")} | {hCost.ToString("0.0")}\n<size=3>{fCost.ToString("0.0")}</size>";
            debugText.color = Color.green;
        }
    }

    public Node FindNearestNode() {
        var neighbors = FindNeighbors();
        var nearest = neighbors[0];     // default node value

        // Calc neighbor nodes
        foreach (var el in neighbors) {
            el.Calc();
        }

        // Find nearest neighbor node
        foreach (var el in neighbors) {
            if (nearest.fCost == el.fCost) {
                if (nearest.hCost > el.hCost)
                    nearest = el;
            } else if (nearest.fCost > el.fCost) {
                nearest = el;
            }
        }

        Debug.Log($"Nearest node is {(nearest == null ? "Not found!" : "found")} ", nearest.gameObject);

        return nearest;
    }

    Node[] FindNeighbors() {
        const bool DEBUG = true;

        Vector3[] nodeVectors = new Vector3[] {
            Vector3.forward,
            Vector3.back,
            Vector3.right,
            Vector3.left
        };

        List<RaycastHit> nodeHits = new List<RaycastHit>();
        List<Node> nodes = new List<Node>();

        foreach (var el in nodeVectors) {
            var nodeHit = RaycastHitX.Cast(transform.position, el, Player.layers.Node, NODE_SIZE, DEBUG);

            if (nodeHit.collider) {
                var node = nodeHit.collider.GetComponent<Node>();

                if (node.IsWalkable) {
                    nodeHits.Add(nodeHit);
                    nodeHit.collider.enabled = false;
                    nodes.Add(node);
                }
            }
        }

        foreach (var el in nodeHits) {
            el.collider.enabled = true;
            Debug.Log("Turned back on collider", el.collider.gameObject);
        }

        Debug.Log($"This node has {nodes.Count} neighbors", this);

        return nodes.ToArray();
    }

    public void MarkAsVisited() {
        if (debugText)
            debugText.color = Color.red;

        col.enabled = false;
    }

    void SetDefault() {
        if (debugText) {
            debugText.text = "";
            debugText.color = Color.white;
        }
    }
}