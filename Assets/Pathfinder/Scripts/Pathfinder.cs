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
    Node endNode;

    public Node StartNode {
        get => startNode;
        set => startNode = value;
    }

    public Node EndNode {
        get => endNode;
        set => endNode = value;
    }

    void Start() {
        startNode.Calc();
    }

    public Node[] FindPath() {
        // Check requirements first before running this algoristh.
        if (StartNode == null || EndNode == null) {
            Debug.LogError("Please select missing nodes before running the pathfinder");
            return null;
        }

        const int MAX_TRIES = 1000;

        var path = new List<Node>();
        var nearest = startNode;

        AddPath(startNode);

        for (int i = 0; i < MAX_TRIES; i++) {
            if (nearest == endNode) {
                Debug.Log($"Pathfinder found the path node 'tries:{i + 1}'", nearest);

                foreach (var el in path)
                    el.col.enabled = true;

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