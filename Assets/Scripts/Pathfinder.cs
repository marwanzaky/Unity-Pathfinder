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

    void Start() {
        startNode.Calc();
    }

    public Node[] FindPath(Node endNode) {
        const int MAX_TRIES = 1000;

        var path = new List<Node>();
        var nearest = startNode;

        this.endNode = endNode;
        AddPath(startNode);

        for (int i = 0; i < MAX_TRIES; i++) {
            if (nearest == endNode) {
                Debug.Log($"Pathfinder found the path node 'tries:{i + 1}'", nearest);
                return path.ToArray();
            }

            nearest = nearest.FindNearestNode();
            AddPath(nearest);
        }

        void AddPath(Node node) {
            path.Add(node);
            node.MarkAsVisited();
        }

        return null;
    }

}