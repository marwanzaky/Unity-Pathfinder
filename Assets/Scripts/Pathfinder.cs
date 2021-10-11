using UnityEngine;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour {
    #region Singletone

    public static Pathfinder Instance { get; private set; }

    void Awake() {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }

    #endregion

    [SerializeField] Node startNode;
    [SerializeField] Node endNode;

    public Node StartNode => startNode;
    public Node EndNode => endNode;

    public Node[] FindPath(Node endNode) {
        var path = new List<Node>();
        var nearest = startNode;

        this.endNode = endNode;
        path.Add(startNode);

        while (true) {
            if (nearest == endNode) {
                Debug.Log("Pathfinder found the path node!", nearest);
                return path.ToArray();
            }

            nearest = nearest.FindNearestNode();
            path.Add(nearest);
        }
    }
}