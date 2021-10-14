using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    const float SIZE_NODE = 1;

    float gCost = 0;  // distance from starting node
    float hCost = 0;  // distance from end node
    float fCost = 0;  // g_cost + f_cost

    bool isTraversable = true;
    Node parent = null;

    [SerializeField] LayerMask obstacleLayer;

    public float FCost { get => fCost; }
    public bool IsTraversable { get => isTraversable; }
    public Node Parent { get => parent; set => parent = value; }

    public void Calc(Node startNode, Node targetNode)
    {
        Pathfinder pathfinder = FindObjectOfType<Pathfinder>();
        gCost = Vector3.Distance(startNode.transform.position, transform.position);
        hCost = Vector3.Distance(targetNode.transform.position, transform.position);
        fCost = gCost + hCost;
    }

    public Node[] Neightbours()
    {
        Vector3[] nodeVectors = new Vector3[] {
            Vector3.forward,
            Vector3.back,
            Vector3.right,
            Vector3.left
        };

        List<RaycastHit> nodeHits = new List<RaycastHit>();
        List<Node> nodes = new List<Node>();

        foreach (var el in nodeVectors)
        {
            var nodeHit = RaycastHitX.Cast(transform.position, el, FindObjectOfType<Pathfinder>().nodeLayer, SIZE_NODE);
            var obstacleHit = RaycastHitX.Cast(transform.position, el, obstacleLayer, SIZE_NODE);

            if (nodeHit.collider != null && obstacleHit.collider == null)
            {
                nodeHit.collider.enabled = false;
                nodeHits.Add(nodeHit);
            }
        }

        foreach (var el in nodeHits)
        {
            el.collider.enabled = true;
            nodes.Add(el.collider.GetComponent<Node>());
        }

        return nodes.ToArray();
    }
}