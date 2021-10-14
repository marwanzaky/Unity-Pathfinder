using UnityEngine;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour
{
    public LayerMask nodeLayer;

    [SerializeField] List<Node> open = new List<Node>();
    [SerializeField] List<Node> closed = new List<Node>();

    public Node[] Calc(Node startNode, Node targetNode)
    {
        ResetDefault();
        open.Add(startNode);

        while (true)
        {
            var cur = LowestFCost();
            open.Remove(cur);
            closed.Add(cur);

            if (cur == targetNode)
                return RetracePath(startNode, targetNode);

            foreach (var neightbour in cur.Neightbours())
            {
                if (!neightbour.IsTraversable || closed.Contains(neightbour))
                    continue;

                if (neightbour.FCost < cur.FCost || !open.Contains(neightbour))
                {
                    neightbour.Calc(startNode, targetNode);
                    neightbour.Parent = cur;

                    if (!open.Contains(neightbour))
                        open.Add(neightbour);
                }
            }
        }
    }

    Node[] RetracePath(Node startNode, Node targetNode)
    {
        List<Node> path = new List<Node>();
        Node cur = targetNode;

        while (cur != startNode)
        {
            path.Add(cur);
            cur = cur.Parent;
        }

        path.Reverse();

        return path.ToArray();
    }

    Node LowestFCost()
    {
        Node res = null;

        foreach (var cur in open)
            if (res == null || res.FCost > cur.FCost)
                res = cur;

        return res;
    }

    void ResetDefault()
    {
        open.Clear();
        closed.Clear();
    }
}