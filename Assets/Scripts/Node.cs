using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class Node : MonoBehaviour {
    [SerializeField] float gCost = 0;  // distance from starting node
    [SerializeField] float hCost = 0;  // distance from end node
    [SerializeField] float fCost = 0;  // g_cost + f_cost
    [SerializeField] float nodeSize = 1;
    [SerializeField] LayerMask nodeMask;
    [SerializeField] TextMeshPro debugText;

    public void Calc() {
        var node = (Pathfinder.Instance.StartNode, Pathfinder.Instance.EndNode);

        gCost = Vector3.Distance(node.StartNode.transform.position, transform.position);
        hCost = Vector3.Distance(node.EndNode.transform.position, transform.position);
        fCost = gCost + hCost;

        if (debugText)
            debugText.text = $"{gCost.ToString("0.0")} | {hCost.ToString("0.0")}\n<size=3>{fCost.ToString("0.0")}</size>";
    }

    public Node FindNearestNode() {
        var nearest = null as Node;
        var neighbors = FindNeighbors();

        foreach (var el in neighbors)
            el.Calc();

        foreach (var el in neighbors)
            if (nearest == null ||
            nearest.hCost > el.hCost)
                nearest = el;

        nearest.debugText.color = Color.green;

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
            var nodeHit = RaycastHitX.Cast(transform.position, el, nodeMask, nodeSize, DEBUG);

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