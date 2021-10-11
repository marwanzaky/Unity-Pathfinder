using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class Node : MonoBehaviour {
    Pathfinder Pathfinder => Pathfinder.Instance;

    const float NODE_SIZE = 1;

    float gCost = 0;  // distance from starting node
    float hCost = 0;  // distance from end node
    float fCost = 0;  // g_cost + f_cost

    [SerializeField] LayerMask nodeMask;
    [SerializeField] TextMeshPro debugText;

    public Vector3 Pos => transform.position;

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
        var nearest = null as Node;
        var neighbors = FindNeighbors();

        // Calc neighbor nodes
        foreach (var el in neighbors)
            el.Calc();

        // Find nearest neighbor node
        foreach (var el in neighbors)
            if (nearest == null || nearest.hCost > el.hCost)
                nearest = el;

        return nearest;
    }

    Node[] FindNeighbors() {
        const bool DEBUG = true;

        var nodeVectors = new Vector3[] {
            Vector3.forward,
            Vector3.back,
            Vector3.right,
            Vector3.left
        };

        var nodes = new List<Node>();

        foreach (var el in nodeVectors) {
            var nodeHit = RaycastHitX.Cast(transform.position, el, nodeMask, NODE_SIZE, DEBUG);

            if (nodeHit.collider) {
                var node = nodeHit.collider.GetComponent<Node>();
                nodeHit.collider.enabled = false;
                nodes.Add(node);
            }
        }

        Debug.Log($"This node has {nodes.Count} neighbors", this);

        return nodes.ToArray();
    }
}